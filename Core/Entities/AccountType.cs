using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entities
{
    public class AccountType2 : BaseEntity
    {
        public int AccountId { get; set; }
        public int Account { get; set; }
        public AccountType Type { get; set; }

        public string Data { get; set; }

        [NotMapped]
        public object DataObj
        {
            get
            {
                if (!string.IsNullOrEmpty(Data))
                {
                    if (Type == AccountType.HotMom)
                    {
                        return JsonConvert.DeserializeObject<List<AccountTypeHotMomData>>(Data);
                    }

                }

                return null;
            }
        }

    }

    public enum AccountType
    {
        [Display(Name ="Tất cả", ShortName ="")]
        [Description("")]
        All = -1,
        [Display(Name ="Tài khoản thường", ShortName = "Facebooker (người dùng thường)", Description = "Mô tả tài khoản thường")]
        Regular = 0,
        [Display(Name = "Tài khoản Hot Teen", ShortName = "Hot Teen", Description = "Mô tả Hotteen")]
        HotTeen = 1,
        [Display(Name = "Tài khoản Hot Mom", ShortName = "Hot Mom", Description = "Mô tả Hotmom")]
        HotMom = 2,
        [Display(Name = "Tài khoản Hot Facebooker", ShortName = "Hot Facebooker", Description = "Mô tả Hot Facebooker")]
        HotFacebooker = 3,
        [Display(Name = "Tài khoản Kols", ShortName = "Kols", Description = "Mô tả Kols")] 
        Kols = 4,
    }
    public class AccountTypeHotMomData
    {
        public Gender Gender { get; set; }
        public int Age { get; set; }
        public int AgeType { get; set; }
    }

}
