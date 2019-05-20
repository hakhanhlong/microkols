using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Core.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System;
using Infrastructure.Extensions;

namespace Infrastructure.Data
{

    public class EfRepository<T> : IRepository<T>, IAsyncRepository<T> where T : BaseEntity
    {
        protected readonly AppDbContext _dbContext;

        public EfRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual T GetById(int id, bool disableTracking = true)
        {

            if (disableTracking)
            {
                return _dbContext.Set<T>().AsNoTracking().FirstOrDefault(m => m.Id == id);
            }
            return _dbContext.Set<T>().FirstOrDefault(m => m.Id == id);

        }
        public virtual async Task<T> GetByIdAsync(int id, bool disableTracking = true)
        {
            if (disableTracking)
            {
                return await _dbContext.Set<T>().AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
            }
            return await _dbContext.Set<T>().FirstOrDefaultAsync(m => m.Id == id);
        }

        private IQueryable<T> GetQueryBySpecification(ISpecification<T> spec, bool disableTracking = true)
        {
            var queryableResultWithIncludes = spec.Includes
           .Aggregate(_dbContext.Set<T>().AsQueryable(),
               (current, include) => current.Include(include));

            // modify the IQueryable to include any string-based include statements
            var secondaryResult = spec.IncludeStrings
                .Aggregate(queryableResultWithIncludes,
                    (current, include) => current.Include(include));

            // return the result of the query using the specification's criteria expression


            var query = secondaryResult
                            .Where(spec.Criteria);

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }
            return query;
        }

        public T GetSingleBySpec(ISpecification<T> spec, bool disableTracking = true)
        {
            return GetQueryBySpecification(spec, disableTracking).FirstOrDefault();
        }
        public virtual async Task<T> GetSingleBySpecAsync(ISpecification<T> spec, bool disableTracking = true)
        {
            return await GetQueryBySpecification(spec, disableTracking)
                            .FirstOrDefaultAsync();
        }


        public List<T> ListAll(bool disableTracking = true)
        {
            var query = _dbContext.Set<T>().AsQueryable();
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }
            return query.ToList();
        }

        public async Task<List<T>> ListAllAsync(bool disableTracking = true)
        {
            var query = _dbContext.Set<T>().AsQueryable();
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.ToListAsync();
        }

        public List<T> List(ISpecification<T> spec, bool disableTracking = true)
        {
            return GetQueryBySpecification(spec, disableTracking).ToList();
        }
        public async Task<List<T>> ListAsync(ISpecification<T> spec, bool disableTracking = true)
        {
            return await GetQueryBySpecification(spec, disableTracking).ToListAsync();
        }

        public int Count(ISpecification<T> spec, bool disableTracking = true)
        {
            return GetQueryBySpecification(spec, disableTracking).Count();
        }
        public async Task<int> CountAsync(ISpecification<T> spec, bool disableTracking = true)
        {
            return await GetQueryBySpecification(spec, disableTracking).CountAsync();
        }

        public List<T> ListPaged(ISpecification<T> spec, string sortOrder, int page = 1, int pagesize = 20, bool disableTracking = true)
        {

            var query = GetQueryBySpecification(spec, disableTracking);
            bool descending = false;

            if (string.IsNullOrEmpty(sortOrder))
            {
                sortOrder = "Id_desc";
            }

            if (sortOrder.EndsWith("_desc"))
            {
                sortOrder = sortOrder.Substring(0, sortOrder.Length - 5);
                descending = true;
            }

            if (descending)
            {
                query = query.OrderByDescending(e => EF.Property<object>(e, sortOrder));
            }
            else
            {
                query = query.OrderBy(e => EF.Property<object>(e, sortOrder));
            }


            // return the result of the query using the specification's criteria expression
            return query.GetPaged(page, pagesize);
        }


        public List<T> ListPaging(string sortOrder, int page = 1, int pagesize = 20)
        {

            var query = _dbContext.Set<T>().AsQueryable();
            bool descending = false;

            if (string.IsNullOrEmpty(sortOrder))
            {
                sortOrder = "Id_desc";
            }

            if (sortOrder.EndsWith("_desc"))
            {
                sortOrder = sortOrder.Substring(0, sortOrder.Length - 5);
                descending = true;
            }

            if (descending)
            {
                query = query.OrderByDescending(e => EF.Property<object>(e, sortOrder));
            }
            else
            {
                query = query.OrderBy(e => EF.Property<object>(e, sortOrder));
            }


            // return the result of the query using the specification's criteria expression
            return query.GetPaged(page, pagesize);
        }

        public async Task<List<T>> ListPagedAsync(ISpecification<T> spec, string sortOrder, int page = 1, int pagesize = 20, bool disableTracking = true)
        {

            var query = GetQueryBySpecification(spec, disableTracking);

            if (!string.IsNullOrEmpty(sortOrder))
            {
                bool descending = false;
                // Not set default -> DB don't include Id property
                //if (string.IsNullOrEmpty(sortOrder))
                //{
                //    sortOrder = "Id";
                //}
                //else


                if (sortOrder.EndsWith("_desc"))
                {
                    sortOrder = sortOrder.Substring(0, sortOrder.Length - 5);
                    descending = true;
                }

                if (descending)
                {
                    query = query.OrderByDescending(e => EF.Property<object>(e, sortOrder));
                }
                else
                {
                    query = query.OrderBy(e => EF.Property<object>(e, sortOrder));
                }
            }



            // return the result of the query using the specification's criteria expression
            return await query.GetPagedAsync(page, pagesize);
        }




        public T Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            _dbContext.SaveChanges();

            return entity;
        }

        public async Task<T> AddAsync(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public void Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }
        public async Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            _dbContext.SaveChanges();
        }
        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
