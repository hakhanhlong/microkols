using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data
{
    public class QnARepository: EfRepository<QnA>, IQnARepository
    {
        public QnARepository(AppDbContext dbContext) : base(dbContext)
        {

        }
    }
}
