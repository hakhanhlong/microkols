﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Common.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Core.Entities
{
    
    public class Account : BaseEntityWithDate
    {
        public string IgnoreCampaignTypes { get; set; }

        [NotMapped]
        public List<CampaignType> IgnoreCampaignTypesObj
        {
            get
            {
                var result = new List<CampaignType>();
                if (!string.IsNullOrEmpty(IgnoreCampaignTypes))
                {
                    var campaignTypeArr = IgnoreCampaignTypes.ToListInt();
                    foreach(var campaignType in campaignTypeArr)
                    {
                        result.Add((CampaignType)campaignType);
                    }
                }

                return result;
            }
        }


        public AccountStatus? Status { get; set; }

        public AccountType Type { get; set; }

        public string TypeData { get; set; }
        [NotMapped]
        public object TypeDataObj
        {
            get
            {
                if (!string.IsNullOrEmpty(TypeData))
                {
                    if (Type == AccountType.HotMom)
                    {
                        return JsonConvert.DeserializeObject<List<AccountTypeHotMomData>>(TypeData);
                    }

                }

                return null;
            }
        }

        public string Email { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public DateTime? Birthday { get; set; }
        public Gender Gender { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public MaritalStatus MaritalStatus { get; set; }
        public bool Actived { get; set; }
        public bool Deleted { get; set; }

        public string Address { get; set; }
        public int? CityId { get; set; }
        public City City { get; set; }
        public int? DistrictId { get; set; }
        public District District { get; set; }
        public string Phone { get; set; }


        public string IDCardName { get; set; }
        public string IDCardNumber { get; set; }
        public string IDCardTime { get; set; }
        public string IDCardCity { get; set; }
        public string IDCardImageFront { get; set; }
        public string IDCardImageBack { get; set; }

        public string BankAccountName { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankAccountBank { get; set; }
        public string BankAccountBranch { get; set; }

        public bool? IsReviewer { get; set; }





        public IEnumerable<AccountCategory> AccountCategory { get; set; }


        
        public IEnumerable<AccountProvider> AccountProvider { get; set; }
    }

    public enum AccountStatus
    {
        Normal = 0,
        NeedVerified = 1,
        SystemVerified = 2,
    }
  
    public enum Gender
    {
     
        [Display(Name ="Nam")]
        Male = 1,
        [Display(Name = "Nữ")]
        Female = 2
    }
    public enum MaritalStatus
    {
       
        [Display(Name = "Độc thân")]
        Single,
        [Display(Name = "Kết hôn")]
        Marriage,
        [Display(Name = "Đã có con")]
        HaveChildren
    }
}
