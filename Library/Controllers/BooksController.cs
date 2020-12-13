using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Library.Data;
using Library.Models;
using Microsoft.AspNetCore.Authorization;

namespace Library.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BooksController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Books.Include(b => b.Author).Include(b => b.BookCopies);
            return View(await applicationDbContext.ToListAsync());
        }

        [Authorize(Roles = "librarian")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        [Authorize(Roles = "librarian")]
        public IActionResult Create()
        {
            var authors = _context.Authors.Select(a => new
            {
                AuthorId = a.Id,
                FullName = a.FirstName + " " + a.SecondName
            }).ToList();

            ViewData["AuthorId"] = new SelectList(authors, "AuthorId", "FullName");
            return View();
        }
        
        [HttpPost]
        [Authorize(Roles = "librarian")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Year,AuthorId")] Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Id", book.AuthorId);
            return View(book);
        }

        [Authorize(Roles = "librarian")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            var authors = _context.Authors.Select(a => new
            {
                AuthorId = a.Id,
                FullName = a.FirstName + " " + a.SecondName
            }).ToList();
            ViewData["AuthorId"] = new SelectList(authors, "AuthorId", "FullName");

            return View(book);
        }

        [HttpPost]
        [Authorize(Roles = "librarian")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Year,AuthorId")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            var authors = _context.Authors.Select(a => new
            {
                AuthorId = a.Id,
                FullName = a.FirstName + " " + a.SecondName
            }).ToList();
            ViewData["AuthorId"] = new SelectList(authors, "AuthorId", "FullName");

            return View(book);
        }

        [Authorize(Roles = "librarian")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Author)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "librarian")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pictures = _context.BookCopies
                .Where(b => b.BookId == id)
                .Include(b => b.Picture)
                .Select(b => b.Picture);

            var book = await _context.Books.FindAsync(id);

            _context.Books.Remove(book);
            _context.Pictures.RemoveRange(pictures);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}
