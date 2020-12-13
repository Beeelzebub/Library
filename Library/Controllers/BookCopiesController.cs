using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Library.Data;
using Library.Models;
using Library.ViewModels;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace Library.Controllers
{
    public class BookCopiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookCopiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        
        public async Task<IActionResult> Index(int id)
        {
            var applicationDbContext = _context.BookCopies.Where(b => b.BookId == id).Include(b => b.Book).Include(b => b.Picture);
            ViewData["BookTitle"] = _context.Books.Find(id).Title; 
            return View(await applicationDbContext.ToListAsync());
        }
        [Authorize(Roles = "reader")]
        public async Task<IActionResult> Book(string userName, int bookId)
        {
            var reader = _context.Readers.Where(r => r.User.UserName == userName)
                .Include(r => r.User)
                .FirstOrDefault();

            var bookCopy = _context.BookCopies.Find(bookId);

            if (bookCopy == null)
            {
                return NotFound();
            }

            Usage usage = new Usage
            {
                BookCopyId = bookCopy.Id,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(30),
                ReaderId = reader.Id,
                UsageStatusId = 1
            };

            bookCopy.IsInStock = false;

            _context.BookCopies.Update(bookCopy);
            _context.Usages.Add(usage);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Usages");
        }

        [Authorize(Roles = "librarian")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookCopy = await _context.BookCopies
                .Include(b => b.Book)
                .Include(b => b.Picture)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bookCopy == null)
            {
                return NotFound();
            }

            return View(bookCopy);
        }

        [Authorize(Roles = "librarian")]
        public IActionResult Create()
        {
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Title");
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "librarian")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookCopyViewModel model)
        {
            if (ModelState.IsValid)
            {
                byte[] imageData = null;

                if (model.Image != null)
                {
                    // считываем переданный файл в массив байтов
                    using (var binaryReader = new BinaryReader(model.Image.OpenReadStream()))
                    {
                        imageData = binaryReader.ReadBytes((int)model.Image.Length);
                    }
                }

                Picture picture = new Picture { Image = imageData };
                _context.Pictures.Add(picture);
                await _context.SaveChangesAsync();

                BookCopy bookCopy = new BookCopy
                {
                    BookId = model.BookId,
                    Notes = model.Notes,
                    PictureId = picture.Id,
                    IsInStock = true
                };

                _context.Add(bookCopy);
                await _context.SaveChangesAsync();


                return RedirectToAction("Index", "BookCopies", new { id = bookCopy.BookId });
            }

            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Title");
            return View(model);
        }

        [Authorize(Roles = "librarian")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookCopy = await _context.BookCopies.FindAsync(id);
            if (bookCopy == null)
            {
                return NotFound();
            }
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Id", bookCopy.BookId);
            ViewData["PictureId"] = new SelectList(_context.Pictures, "Id", "Id", bookCopy.PictureId);
            return View(bookCopy);
        }

        [HttpPost]
        [Authorize(Roles = "librarian")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BookId,Notes,IsInStock,PictureId")] BookCopy bookCopy)
        {
            if (id != bookCopy.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bookCopy);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookCopyExists(bookCopy.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "BookCopies", new { id = bookCopy.BookId });
            }
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Id", bookCopy.BookId);
            ViewData["PictureId"] = new SelectList(_context.Pictures, "Id", "Id", bookCopy.PictureId);
            return View(bookCopy);
        }

        [Authorize(Roles = "librarian")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookCopy = await _context.BookCopies.FindAsync(id);

            if (bookCopy == null)
            {
                return NotFound();
            }

            int bookId = bookCopy.BookId;
            var picture = await _context.Pictures.FindAsync(bookCopy.PictureId);
            _context.BookCopies.Remove(bookCopy);
            _context.Pictures.Remove(picture);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "BookCopies", new { id = bookId });

        }

        private bool BookCopyExists(int id)
        {
            return _context.BookCopies.Any(e => e.Id == id);
        }
    }
}
