using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Library.Data;
using Library.Models;

namespace Library.Controllers
{
    public class BookCopiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookCopiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BookCopies
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.BookCopies.Include(b => b.Book).Include(b => b.Picture);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: BookCopies/Details/5
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

        // GET: BookCopies/Create
        public IActionResult Create()
        {
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Id");
            ViewData["PictureId"] = new SelectList(_context.Pictures, "Id", "Id");
            return View();
        }

        // POST: BookCopies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BookId,Notes,IsInStock,PictureId")] BookCopy bookCopy)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bookCopy);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Id", bookCopy.BookId);
            ViewData["PictureId"] = new SelectList(_context.Pictures, "Id", "Id", bookCopy.PictureId);
            return View(bookCopy);
        }

        // GET: BookCopies/Edit/5
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

        // POST: BookCopies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Id", bookCopy.BookId);
            ViewData["PictureId"] = new SelectList(_context.Pictures, "Id", "Id", bookCopy.PictureId);
            return View(bookCopy);
        }

        // GET: BookCopies/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: BookCopies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bookCopy = await _context.BookCopies.FindAsync(id);
            _context.BookCopies.Remove(bookCopy);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookCopyExists(int id)
        {
            return _context.BookCopies.Any(e => e.Id == id);
        }
    }
}
