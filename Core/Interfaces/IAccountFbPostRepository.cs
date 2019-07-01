﻿using Core.Entities;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IAccountFbPostRepository : IRepository<AccountFbPost>, IAsyncRepository<AccountFbPost>
    {
        Task<AccountCountingModel> GetAccountCounting(int accountid);
    }
}