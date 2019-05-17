﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Account : BaseEntity
    {
        public AccountType? AccountType { get; set; }
        public string AccountTypeData { get; set; }
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


        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string UserCreated { get; set; }
        public string UserModified { get; set; }


        private List<AccountCategory> _AccountCategory = new List<AccountCategory>();
        public IEnumerable<AccountCategory> AccountCategory => _AccountCategory.AsReadOnly();


        private List<AccountProvider> _AccountProvider = new List<AccountProvider>();
        public IEnumerable<AccountProvider> AccountProvider => _AccountProvider.AsReadOnly();
    }

    public enum AccountType
    {
        [DisplayName("Hot Teen")]
        HotTeen,
        [DisplayName("Hot Mom")]
        HotMom,
        [DisplayName("Hot Facebooker")]
        HotFacebooker,
        [DisplayName("Kols")]
        Kols,
    }
    public class AccountTypeHotMomData
    {
        public Gender Gender { get; set; }
        public int Age { get; set; }
    }

    public enum Gender
    {
        [DisplayName("Không xác định")]
        Undefined,
        [DisplayName("Con trai")]
        Male,
        [DisplayName("Con gái")]
        Female,
        [DisplayName("Khác")]
        Other
    }
    public enum MaritalStatus
    {
        [DisplayName("Không xác định")]
        Undefined,
        [DisplayName("Độc thân")]
        Single,
        [DisplayName("Kết hôn")]
        Marriage,
        [DisplayName("Đã có con")]
        HaveChildren
    }
}
