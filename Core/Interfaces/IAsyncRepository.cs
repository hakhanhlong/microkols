using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IAsyncRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(int id, bool disableTracking = true);
        Task<T> GetSingleBySpecAsync(ISpecification<T> spec, bool disableTracking = true);
        Task<List<T>> ListAllAsync(bool disableTracking = true);
        Task<List<T>> ListAsync(ISpecification<T> spec, bool disableTracking = true);

        Task<List<T>> ListPagedAsync(ISpecification<T> spec, string sortOrder, int page = 1, int pagesize = 20, bool disableTracking = true);
        Task<int> CountAsync(ISpecification<T> spec, bool disableTracking = true);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);

        IQueryable<T> GetQueryBySpecification(ISpecification<T> spec, bool disableTracking = true);
    }
}
