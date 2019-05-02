using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class Category : BaseEntityWithMeta
    {
        public string Name { get; set; }



        private List<AccountCategory> _AccountCategory = new List<AccountCategory>();
        public IEnumerable<AccountCategory> AccountCategory => _AccountCategory.AsReadOnly();


        private List<CampaignCategory> _CampaignCategory = new List<CampaignCategory>();
        public IEnumerable<CampaignCategory> CampaignCategory => _CampaignCategory.AsReadOnly();
    }
}
