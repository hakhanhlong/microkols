﻿using Common;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ISettingRepository : IRepository<Setting>, IAsyncRepository<Setting>
    {
        Task<Core.Models.SettingModel> GetSetting();
    }
}
