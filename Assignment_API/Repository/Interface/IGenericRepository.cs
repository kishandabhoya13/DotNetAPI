using System.Linq.Expressions;

namespace Assignment_API.Repository.Interface
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetFirstOfDefault(Expression<Func<T, bool>> filter);

        Task<List<T>> GetAll();

        Task Add(T model);

        Task Update(T model);

        int GetTotalRecordCount(Expression<Func<T, bool>> where);

        Task Remove(T model);

        Task<dynamic> GetAllDataWithPagination(Expression<Func<T, object>> select, Expression<Func<T, bool>> where, int PageIndex, int PageSize, Expression<Func<T, object>> orderBy, bool IsAcc);

        Task<dynamic> GetAllDataWithoutPagination(Expression<Func<T, object>> select, Expression<Func<T, bool>> where);
    }
}
