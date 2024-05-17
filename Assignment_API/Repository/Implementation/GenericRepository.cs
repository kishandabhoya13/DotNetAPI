using Assignment_API.DataContext;
using Assignment_API.Repository.Interface;
using Microsoft.AspNetCore.JsonPatch.Internal;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Assignment_API.Repository.Implementation
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }


        public async Task<T?> GetFirstOfDefault(Expression<Func<T, bool>> filter)
        {
            return await _dbSet.AsNoTracking().FirstOrDefaultAsync(filter);
        }

        public async Task<List<T>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task Add(T model)
        {

            await _context.AddAsync(model);
            await _context.SaveChangesAsync();
        }
        public async Task Update(T model)
        {
            _context.Update(model);
            await _context.SaveChangesAsync();
        }

        public int GetTotalRecordCount(Expression<Func<T, bool>> where)
        {
            return _dbSet.Count(where);
        }

        public async Task Remove(T model)
        {
            _context.Remove(model);
            await _context.SaveChangesAsync();
        }

        public async Task<dynamic> GetAllDataWithPagination(Expression<Func<T, object>> select, Expression<Func<T, bool>> where, int PageIndex, int PageSize, Expression<Func<T, object>> orderBy, bool IsAcc)
        {
            if (IsAcc)
                return await _dbSet.Where(where).OrderBy(orderBy).Skip((PageIndex - 1) * PageSize).Take(PageSize).Select(select).ToListAsync();

            return await _dbSet.Where(where).OrderByDescending(orderBy).Skip((PageIndex - 1) * PageSize).Take(PageSize).Select(select).ToListAsync();
        }

        public async Task<dynamic> GetAllDataWithoutPagination(Expression<Func<T, object>> select, Expression<Func<T, bool>> where)
        {
            return await _dbSet.Where(where).Select(select).ToListAsync();
        }
    }
}
