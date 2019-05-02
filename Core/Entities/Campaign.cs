using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class Campaign : BaseEntityWithMeta
    {
        public int AgencyId { get; set; }
        public Agency Agency { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Data { get; set; }
        public string Image { get; set; }
        public CampaignType Type { get; set; }
        public CampaignContentType ContentType { get; set; }
        public CampaignStatus Status { get; set; }
        public int Cost { get; set; }

        public Gender Gender { get; set; }



        private List<CampaignAccount> _CampaignAccount = new List<CampaignAccount>();
        public IEnumerable<CampaignAccount> CampaignAccount => _CampaignAccount.AsReadOnly();

        private List<CampaignCategory> _CampaignCategory = new List<CampaignCategory>();
        public IEnumerable<CampaignCategory> CampaignCategory => _CampaignCategory.AsReadOnly();


    }
    public enum CampaignType
    {
        Share,
        Comment
    }
    public enum CampaignContentType
    {
        Text,
        Image,
        Links,

    }

    public enum CampaignStatus
    {

    }
}
