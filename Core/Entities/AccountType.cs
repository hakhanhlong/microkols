using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [DisplayName("Tài khoản thường")]
        Regular = 0,
        [DisplayName("Hot Teen")]
        HotTeen = 1,
        [DisplayName("Hot Mom")]
        HotMom = 2,
        [DisplayName("Hot Facebooker")]
        HotFacebooker = 3,
        [DisplayName("Kols")]
        Kols = 4,
    }
    public class AccountTypeHotMomData
    {
        public Gender Gender { get; set; }
        public int Age { get; set; }
        public int AgeType { get; set; }
    }

}
