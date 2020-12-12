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
    public class UsagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Usages
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("reader"))
            {
                var applicationDbContext = _context.Usages
                    .Where(u => u.UsageStatusId != 3)
                    .Include(u => u.Reader)
                    .Where(u => u.Reader.User.UserName == User.Identity.Name)
                    .Include(u => u.BookCopy)
                    .Include(u => u.BookCopy.Book)
                    .Include(u => u.UsageStatus); 
                
                return View(await applicationDbContext.ToListAsync());
            }
            if (User.IsInRole("librarian"))
            {
                var applicationDbContext = _context.Usages
                    .Where(u => u.UsageStatusId != 3)
                    .Include(u => u.Reader)
                    .Include(u => u.Reader.User)
                    .Include(u => u.StartLibrarian)
                    .Include(u => u.StartLibrarian.User)
                    .Include(u => u.EndLibrarian)
                    .Include(u => u.EndLibrarian.User)
                    .Include(u => u.BookCopy)
                    .Include(u => u.BookCopy.Book)
                    .Include(u => u.UsageStatus);

                return View(await applicationDbContext.ToListAsync());
            }

            return RedirectToAction("Index", "Home");
        }

        // GET: Usages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usage = await _context.Usages
                .Include(u => u.BookCopy)
                .Include(u => u.EndLibrarian)
                .Include(u => u.Reader)
                .Include(u => u.StartLibrarian)
                .Include(u => u.UsageStatus)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usage == null)
            {
                return NotFound();
            }

            return View(usage);
        }

        // GET: Usages/Create
        public IActionResult Create()
        {
            ViewData["BookCopyId"] = new SelectList(_context.BookCopies, "Id", "Id");
            ViewData["EndLibrarianId"] = new SelectList(_context.Librarians, "Id", "Id");
            ViewData["ReaderId"] = new SelectList(_context.Readers, "Id", "Id");
            ViewData["StartLibrarianId"] = new SelectList(_context.Librarians, "Id", "Id");
            ViewData["UsageStatusId"] = new SelectList(_context.UsageStatuses, "Id", "Id");
            return View();
        }

        // POST: Usages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BookCopyId,StartDate,EndDate,StartLibrarianId,EndLibrarianId,ReaderId,UsageStatusId")] Usage usage)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookCopyId"] = new SelectList(_context.BookCopies, "Id", "Id", usage.BookCopyId);
            ViewData["EndLibrarianId"] = new SelectList(_context.Librarians, "Id", "Id", usage.EndLibrarianId);
            ViewData["ReaderId"] = new SelectList(_context.Readers, "Id", "Id", usage.ReaderId);
            ViewData["StartLibrarianId"] = new SelectList(_context.Librarians, "Id", "Id", usage.StartLibrarianId);
            ViewData["UsageStatusId"] = new SelectList(_context.UsageStatuses, "Id", "Id", usage.UsageStatusId);
            return View(usage);
        }

        // GET: Usages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usage = await _context.Usages.FindAsync(id);
            if (usage == null)
            {
                return NotFound();
            }
            ViewData["BookCopyId"] = new SelectList(_context.BookCopies, "Id", "Id", usage.BookCopyId);
            ViewData["EndLibrarianId"] = new SelectList(_context.Librarians, "Id", "Id", usage.EndLibrarianId);
            ViewData["ReaderId"] = new SelectList(_context.Readers, "Id", "Id", usage.ReaderId);
            ViewData["StartLibrarianId"] = new SelectList(_context.Librarians, "Id", "Id", usage.StartLibrarianId);
            ViewData["UsageStatusId"] = new SelectList(_context.UsageStatuses, "Id", "Id", usage.UsageStatusId);
            return View(usage);
        }

        // POST: Usages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BookCopyId,StartDate,EndDate,StartLibrarianId,EndLibrarianId,ReaderId,UsageStatusId")] Usage usage)
        {
            if (id != usage.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsageExists(usage.Id))
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
            ViewData["BookCopyId"] = new SelectList(_context.BookCopies, "Id", "Id", usage.BookCopyId);
            ViewData["EndLibrarianId"] = new SelectList(_context.Librarians, "Id", "Id", usage.EndLibrarianId);
            ViewData["ReaderId"] = new SelectList(_context.Readers, "Id", "Id", usage.ReaderId);
            ViewData["StartLibrarianId"] = new SelectList(_context.Librarians, "Id", "Id", usage.StartLibrarianId);
            ViewData["UsageStatusId"] = new SelectList(_context.UsageStatuses, "Id", "Id", usage.UsageStatusId);
            return View(usage);
        }

        public async Task<IActionResult> History()
        {
            var applicationDbContext = _context.Usages
                .Where(u => u.UsageStatusId == 3)
                .Include(u => u.Reader)
                .Include(u => u.Reader.User)
                .Include(u => u.StartLibrarian)
                .Include(u => u.StartLibrarian.User)
                .Include(u => u.EndLibrarian)
                .Include(u => u.EndLibrarian.User)
                .Include(u => u.BookCopy)
                .Include(u => u.BookCopy.Book)
                .Include(u => u.UsageStatus);

            return View(await applicationDbContext.ToListAsync());

        }

        public async Task<IActionResult> GetBackBook(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usage = await _context.Usages.FindAsync(id);

            if (usage == null)
            {
                return NotFound();
            }

            usage.UsageStatusId = 3;

            Librarian librarian = _context.Librarians
                .Where(u => u.User.UserName == User.Identity.Name)
                .FirstOrDefault();

            usage.EndLibrarian = librarian;

            _context.Usages.Update(usage);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> GetBook(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usage = await _context.Usages.FindAsync(id);

            if (usage == null)
            {
                return NotFound();
            }

            usage.UsageStatusId = 2;

            Librarian librarian = _context.Librarians
                .Where(u => u.User.UserName == User.Identity.Name)
                .FirstOrDefault();

            usage.StartLibrarian = librarian;

            _context.Usages.Update(usage);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usage = await _context.Usages.FindAsync(id);
            
            if (usage == null)
            {
                return NotFound();
            }

            var book = await _context.BookCopies.FindAsync(usage.BookCopyId);
            book.IsInStock = true;
            _context.BookCopies.Update(book);

            _context.Usages.Remove(usage);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        private bool UsageExists(int id)
        {
            return _context.Usages.Any(e => e.Id == id);
        }
    }
}
