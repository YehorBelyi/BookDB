using DBFirst.Models;

namespace DBFirst.Services
{
    public static class TablesDependencies
    {
        public static void AddRepositoryService(this IServiceCollection services)
        {
            services.AddTransient<IRepository<Book>, BooksRepository>();
        }
    }
}
