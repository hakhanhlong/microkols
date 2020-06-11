using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackOffice.Security.Enum
{
    public enum Role
    {
        Administratror,
        FinanceManager,
        CampaignManager
    }


    public static class RoleExtensions
    {
        public static string GetRoleName(this Role role)
        {
            return role.ToString();
        }
    }



}
