using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data
{
    public class QnAImageRepository : EfRepository<QnAImage>, IQnAImageRepository
    {
        public QnAImageRepository(AppDbContext dbContext) : base(dbContext)
        {

        }
    }
}
