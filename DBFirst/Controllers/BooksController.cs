using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DBFirst.Models;
using DBFirst.Pagination;
using DBFirst.Services;
using AspNetCoreGeneratedDocument;

namespace DBFirst.Controllers
{
    public class BooksController : Controller
    {
        private readonly IRepository<Book> _bookRepository;

        public BooksController(IRepository<Book> bookRepository)
        {
            _bookRepository = bookRepository;
        }

        private async Task<PaginatedList<Book>> PrepareData(string sortOrder, string searchString, int? pageNumber, int? recordsPerPage)
        {

            int pageSize;
            if (recordsPerPage == null && TempData.Peek("recordsPerPage") == null)
            {
                pageSize = 5;
                TempData["recordsPerPage"] = pageSize;
            }
            else if (recordsPerPage != null)
            {
                pageSize = (int)recordsPerPage;
                TempData["recordsPerPage"] = pageSize;
            }
            else int.TryParse(TempData.Peek("recordsPerPage")?.ToString(), out pageSize);


            ViewData["CurrentSort"] = sortOrder;

            if (sortOrder == null) sortOrder = "bookName_asc";
            ViewData["CurrentSort"] = searchString;

            ViewData["BookNameSort"] = sortOrder == "bookName_asc" ? "bookName_desc" : "bookName_asc";
            ViewData["IzdSort"] = sortOrder == "izd_asc" ? "izd_desc" : "izd_asc";
            ViewData["CategorySort"] = sortOrder == "category_asc" ? "category_desc" : "category_asc";

            return await _bookRepository.GetListDataFilterAsync(searchString, sortOrder, pageNumber ?? 1, pageSize);
        }

        // GET: Books
        public async Task<IActionResult> Index(string sortOrder, string searchString, int? pageNumber, int? recordsPerPage)
        {
            return View(await PrepareData(sortOrder, searchString, pageNumber, recordsPerPage));
        }

        // GET: Books
        public async Task<IActionResult> Indexdata(string sortOrder, string searchString, int? pageNumber, int? recordsPerPage)
        {
            return PartialView(await PrepareData(sortOrder, searchString, pageNumber, recordsPerPage));
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _bookRepository.GetDataAsync((int)id);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("N,Code,New,Name,Price,Izd,Pages,Format,Date,Pressrun,Themes,Category")] Book book)
        {
            if (ModelState.IsValid)
            {
                await _bookRepository.AddDataAsync(book);
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _bookRepository.GetDataAsync((int)id);

            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookToUpdate = await _bookRepository.GetDataAsync((int)id);

            if (await TryUpdateModelAsync<Book>(bookToUpdate, "", b => b.Name, b => b.Izd, b => b.Category))
            {
                try
                {
                    await _bookRepository.SaveDataAsync();
                    return RedirectToAction(nameof(Index));
                } catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "Unable to save changes. " + "Try again, and if the problem persists, " + "see your system administrator.");
                }
            }
            return View(bookToUpdate);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _bookRepository.GetDataAsync((int)id);

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
            var book = await _bookRepository.GetDataAsync(id);

            if (book == null)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                await _bookRepository.DeleteDataAsync(book.N);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                return RedirectToAction(nameof(Delete), new { id, saveChangesError = true });
            }
        }
    }
}
