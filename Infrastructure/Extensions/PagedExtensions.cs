using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Extensions
{

    public static class PagedResultEFCoreExtensions
    {
        public static List<T> GetPaged<T>(this IQueryable<T> query, int page, int pageSize)
        {
            var skip = (page - 1) * pageSize;
            return query.Skip(skip).Take(pageSize).ToList();
        }

        public static async Task<List<T>> GetPagedAsync<T>(this IQueryable<T> query, int page, int pageSize)
        {
            var skip = (page - 1) * pageSize;
            return await query.Skip(skip).Take(pageSize).ToListAsync();
        }
    }
}
