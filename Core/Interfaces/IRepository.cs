using Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        T GetById(int id, bool disableTracking = true);
        T GetSingleBySpec(ISpecification<T> spec, bool disableTracking = true);

        List<T> ListAll( bool disableTracking = true);
        List<T> List(ISpecification<T> spec, bool disableTracking = true);
        List<T> ListPaged(ISpecification<T> spec, string sortOrder, int page = 1, int pagesize = 20, bool disableTracking = true);
        int Count(ISpecification<T> spec, bool disableTracking = true);
        T Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
