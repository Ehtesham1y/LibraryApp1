using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class BooksController : Controller
    {
        private readonly DbLibraryContext _context;

        public BooksController(DbLibraryContext context)
        {
            _context = context;
        }

        // GET: Books
        //public async Task<IActionResult> Index()
        //{
        //    var dbLibraryContext = _context.Books.Include(b => b.Shelf).Where(x => x.IsDeleted == false && x.IsAvailable == true);

        //    return View(await dbLibraryContext.ToListAsync());
        //}
        public IActionResult Index(string searchQuery)
        {
            var books = _context.Books.Where(x => x.IsDeleted == false && x.IsAvailable == true).ToList();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                books = books.Where(b => b.BookName.Contains(searchQuery, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return View(books);
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Shelf)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            ViewData["ShelfId"] = new SelectList(_context.Shelves, "ShelfId", "ShelfId");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Code,BookName,Author,IsAvailable,Price,ShelfId,PublishYear,CreatedOn,IsDeleted")] Book book)
        {
            //var parameter = new List<SqlParameter>();
            //parameter.Add(new SqlParameter("@ProductName", book.Code));
            //parameter.Add(new SqlParameter("@ProductDescription", book.BookName));
            //parameter.Add(new SqlParameter("@ProductPrice", book.Author));
            //parameter.Add(new SqlParameter("@ProductStock", book.IsAvailable));
            //parameter.Add(new SqlParameter("@ProductDescription", book.Price));
            //parameter.Add(new SqlParameter("@ProductPrice", book.ShelfId));
            //parameter.Add(new SqlParameter("@ProductStock", book.PublishYear));
            //parameter.Add(new SqlParameter("@ProductPrice", book.CreatedOn));
            //parameter.Add(new SqlParameter("@ProductStock", book.IsDeleted));
            //    var result =
            //      _context.Database.ExecuteSqlRawAsync(@"EXECUTE InsertBook @Code, @BookName, @Author, @IsAvailable, @Price, @ShelfId, ,@PublishYear , @CreatedOn, @IsDeleted", parameter.ToArray());

            //return RedirectToAction(nameof(Index));
           
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            
            ViewData["ShelfId"] = new SelectList(_context.Shelves, "ShelfId", "ShelfId", book.ShelfId);
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["ShelfId"] = new SelectList(_context.Shelves, "ShelfId", "ShelfId", book.ShelfId);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Code,BookName,Author,IsAvailable,Price,ShelfId,PublishYear,CreatedOn,IsDeleted")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (id == book.Id)
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
            ViewData["ShelfId"] = new SelectList(_context.Shelves, "ShelfId", "ShelfId", book.ShelfId);
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Books == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Shelf)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Books == null)
            {
                return Problem("Entity set 'DbLibraryContext.Books'  is null.");
            }
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                book.IsDeleted = true;
              //  _context.Books.Remove(book);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
          return (_context.Books?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
