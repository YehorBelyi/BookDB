using DBFirst.Models;
using DBFirst.Pagination;
using Microsoft.EntityFrameworkCore;

namespace DBFirst.Services
{
    public class BooksRepository : EFRepository<Book>
    {
        private readonly BooksContext _context;
        public BooksRepository(BooksContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<PaginatedList<Book>> GetListDataFilterAsync(string filter, string sort, int pageIndex, int pageSize)
        {
            IQueryable<Book> books;

            if (!string.IsNullOrEmpty(filter))
            {
                books = _context.Books.Where(b => b.Name.Contains(filter)
                                           || b.Izd.Contains(filter));
            }
            else
            {
                books = _context.Books;
            }

            switch (sort)
            {
                case "bookName_desc":
                    books = books.OrderByDescending(b => b.Name);
                    break;
                case "izd_desc":
                    books = books.OrderByDescending(b => b.Izd);
                    break;
                case "bookName_asc":
                    books = books.OrderBy(b => b.Name);
                    break;
                case "izd_asc":
                    books = books.OrderBy(b => b.Izd);
                    break;
                case "category_asc":
                    books = books.OrderBy(b => b.Category);
                    break;
                case "category_desc":
                    books = books.OrderByDescending(b => b.Category);
                    break;
                default:
                    books = books.OrderBy(b => b.Name);
                    break;
            }

            var count = await books.CountAsync();
            var items = await books.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PaginatedList<Book>(items, count, pageIndex, pageSize);
        }

        public override async Task<IEnumerable<Book>> GetListDataAsync()
        {
            return await _context.Books.ToListAsync();
        }

        public override async Task<Book?> GetDataAsync(int id)
        {
            return await _context.Books.FirstOrDefaultAsync(m => m.N == id);
        }
    }
}
