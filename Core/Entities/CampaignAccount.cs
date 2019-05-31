using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entities
{
    public class CampaignAccount
    {
        public int CampaignId { get; set; }
        public Campaign Campaign { get; set; }
        public int AccountId { get; set; }
        public Account Account { get; set; }
        public CampaignAccountStatus Status { get; set; }
        public CampaignType Type { get; set; }
        public int AccountCharge { get; set; } // chi phi cho tung nguoi tham gia 
        public int ExtraAccountCharge { get; set; } // tinh theo Extra ở CampaignTypeCharge
        public int AccountChargeTimes { get; set; } // lượt thực hiện - chi dành cho Thay hình Avatar

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
                    if(Type == CampaignType.ChangeAvatar)
                    {
                        return JsonConvert.DeserializeObject<List<CampaignAccountReFDataChangeAvatar>>(RefData);
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

    public class CampaignAccountReFDataChangeAvatar
    {
        public string Avatar { get; set; }
        public DateTime TimeUpdate { get; set; }
    }


    public class CampaignAccountReFDataShareContent
    {
        public string FacebookUrl { get; set; }
        public string Content { get; set; }
        public string CountLike { get; set; }
        public string CountShare { get; set; }
        public string CountComment { get; set; }
        public string CountReaction { get; set; }
        public DateTime TimeUpdate { get; set; }
    }

    public class CampaignAccountReFDataPostComment : CampaignAccountReFDataShareContent
    {

    }
    public class CampaignAccountReFDataShareStreamUrl : CampaignAccountReFDataShareContent
    {
    
    }
    public class CampaignAccountReFDataJoinEvent
    {
        public string FacebookUrl { get; set; }
        public DateTime TimeUpdate { get; set; }

    }

    public enum CampaignAccountStatus
    {
        AccountRequestJoin = 0,
        AgencyRequestJoinb = 1,

        ConfirmJoined = 2
    }
}
