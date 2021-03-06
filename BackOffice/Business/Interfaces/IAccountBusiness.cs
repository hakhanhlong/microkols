﻿using BackOffice.Models;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackOffice.Business.Interfaces
{
    public interface IAccountBusiness
    {
        Task<AccountViewModel> GetAccount(int id);
        ListAccountViewModel GetListAccount(int pageindex, int pagesize);

        ListAccountViewModel Search(string keyword, AccountType type ,int pageindex, int pagesize);

    }
}
