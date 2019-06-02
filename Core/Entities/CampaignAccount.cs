using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entities
{
    public class CampaignAccount : BaseEntity
    {
        public int CampaignId { get; set; }
        public Campaign Campaign { get; set; }
        public int AccountId { get; set; }
        public Account Account { get; set; }
        public CampaignAccountStatus Status { get; set; }
        public CampaignType Type { get; set; }
        public int AccountChargeAmount { get; set; } // chi phi cho tung nguoi tham gia

        public string RefUrl { get; set; }
        public string RefId { get; set; }

        public string RefData { get; set; }
        [NotMapped]
        public object RefDataObj
        {
            get
            {
                if (!string.IsNullOrEmpty(RefData))
                {
                    if (Type == CampaignType.ChangeAvatar)
                    {
                        return JsonConvert.DeserializeObject<List<CampaignAccountRefDataChangeAvatar>>(RefData);
                    }

                    // .....
                }
                return null;
            }

        }




        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string UserCreated { get; set; }
        public string UserModified { get; set; }
    }


    public enum CampaignAccountStatus
    {
        [DisplayName("Thành viên yêu cầu tham gia chiến dịch")]
        AccountRequest = 0,
        [DisplayName("Doanh nghiệp yêu cầu tham gia chiến dịch")]
        AgencyRequest = 1,

        [DisplayName("Đã xác nhận tham gia chiến dịch")]
        Confirmed = 2,
        [DisplayName("Đang thực hiện")]
        Processing = 3,
        [DisplayName("Đã hoàn thành")]
        Done = 3,

    }
}
