using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data
{
    public class QnAVideoRepository : EfRepository<QnAVideo>, IQnAVideoRepository
    {
        public QnAVideoRepository(AppDbContext dbContext) : base(dbContext)
        {

        }
    }
}
