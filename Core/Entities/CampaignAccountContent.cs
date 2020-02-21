using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Entities
{
    public class CampaignAccountContent : BaseEntity
    { 
        public int CampaignAccountId { get; set; }
        public CampaignAccount CampaignAccount { get; set; }

        public string Content { get; set; }
        public string Image { get; set; }
        public string Note { get; set; }
        public CampaignAccountContentStatus Status { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string UserCreated { get; set; }
        public string UserModified { get; set; }
    }

    public enum CampaignAccountContentStatus
    {
        [Display(Name = "Chờ duyệt")]
        ChoDuyet = 0,
        [Display(Name = "Chưa duyệt")]
        ChuaDuyet = 1,
        [Display(Name = "Đã duyệt")]
        DaDuyet = 2,
    }
}
