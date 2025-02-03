using DBFirst.Pagination;

namespace DBFirst.Services
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetListDataAsync();

        Task<PaginatedList<T>> GetListDataFilterAsync(string filter, string sort, int pageIndex, int pageSize);

        Task<T?> GetDataAsync(int id);

        Task<bool> DeleteDataAsync(int id);

        Task<bool> AddDataAsync(T data);

        Task<bool> UpdateDataAsync(T data);

        Task<int> SaveDataAsync();
    }
}
