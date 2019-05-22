﻿using BackOffice.Business;
using BackOffice.Business.Interfaces;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackOffice
{
    public static class AppContext
    {
        public static void AddAppServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped(typeof(IAsyncRepository<>), typeof(EfRepository<>));

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IAgencyRepository, AgencyRepository>();


            //business
            services.AddScoped<IAgencyBusiness, AgencyBusiness>();



        }
    }
}
