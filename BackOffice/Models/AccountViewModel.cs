using Core.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackOffice.Models
{
    public class ListAccountViewModel
    {
        public List<AccountViewModel> Accounts { get; set; }
        public PagerViewModel Pager { get; set; }
    }

    public class AccountViewModel
    {

        public AccountViewModel() { }

        public AccountViewModel(Account _Account)
        {
            Id = _Account.Id;
            DateCreated = _Account.DateCreated;
            UserCreated = _Account.UserCreated;
            UserModified = _Account.UserModified;
            Type = _Account.Type;
            TypeData = _Account.TypeData;
            Email = _Account.Email;
            Name = _Account.Name;
            Birthday = _Account.Birthday;
            Gender = _Account.Gender;
            Avatar = _Account.Avatar;
            Actived = _Account.Actived;
            Deleted = _Account.Deleted;
            Address = _Account.Address;
            CityId = _Account.CityId;
            City = _Account.City;
            DistrictId = _Account.DistrictId;
            District = _Account.District;
            Phone = _Account.Phone;
            IDCardName = _Account.IDCardName;
            IDCardNumber = _Account.IDCardNumber;
            IDCardTime = _Account.IDCardTime;
            IDCardCity = _Account.IDCardCity;
            IDCardImageFront = _Account.IDCardImageFront;
            IDCardImageBack = _Account.IDCardImageBack;
            BankAccountName = _Account.BankAccountName;
            BankAccountNumber = _Account.BankAccountNumber;
            BankAccountBank = _Account.BankAccountBank;
            BankAccountBranch = _Account.BankAccountBranch;
        }

        public int Id { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string UserCreated { get; set; }
        public string UserModified { get; set; }

        public AccountType Type { get; set; }

        public string TypeData { get; set; }

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


    }
}
