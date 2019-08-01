using Common.Extensions;
using Core.Entities;
using Core.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Website.ViewModels
{
    public class ListAccountViewModel
    {
        public List<AccountViewModel> Accounts { get; set; }
        public PagerViewModel Pager { get; set; }
    }
    public class AccountViewModel
    {
        public AccountViewModel()
        {

        }
        public AccountViewModel(Account customer, AccountCountingModel accountCounting)
        {
            Id = customer.Id;
            Email = customer.Email;
            Name = customer.Name;
            Avatar = !string.IsNullOrEmpty(customer.Avatar) ? customer.Avatar : "account/avatar.png";
            Type = customer.Type;
            AccountCounting = new AccountCountingViewModel(accountCounting);
        }

        public static List<AccountViewModel> GetList(IEnumerable<Account> accounts, IEnumerable<AccountCountingModel> accountCountings)
        {
            var result = new List<AccountViewModel>();
            foreach(var account in accounts)
            {
                var accountCouting = accountCountings.FirstOrDefault(m => m.AccountId == account.Id);

                result.Add(new AccountViewModel(account, accountCouting));
            }
            return result;
        }
        public AccountType Type { get; set; }
        public int Id { get; set; }
        public string Email { get; set; }

        public string Name { get; set; }

        public string Avatar { get; set; }

        public AccountCountingViewModel AccountCounting { get; set; }
    }

    public class AccountProviderViewModel
    {
        public AccountProviderViewModel()
        {

        }
        public AccountProviderViewModel(AccountProvider accountProvider)
        {
            Id = accountProvider.Id;
            AccountId = accountProvider.AccountId;
            AccessToken = accountProvider.AccessToken;
            Expired = accountProvider.Expired;
            ProviderId = accountProvider.ProviderId;
            Name = accountProvider.Name;
            Email = accountProvider.Email;
        }
        public static List<AccountProviderViewModel> GetList(IEnumerable<AccountProvider> accounts)
        {
            return accounts.Select(m => new AccountProviderViewModel(m)).ToList();
        }
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string AccessToken { get; set; }
        public DateTime Expired { get; set; }

        public AccountProviderNames Provider { get; set; }
        public string ProviderId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Hãy nhập {0}")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu cũ")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Hãy nhập {0}")]
        [StringLength(100, ErrorMessage = "Độ dài {0} phải lớn hơn {2} ký tự.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu mới")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Nhập lại mật khẩu mới")]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu và xác nhận mật khẩu không trùng nhau.")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangeAvatarViewModel : ChangeCoverViewModel
    {
    }
    public class ChangeCoverViewModel
    {
        public string Image { get; set; }
        public string ReturnUrl { get; set; }
    }

    public class ChangeInformationViewModel
    {
        public ChangeInformationViewModel()
        {

        }
        public ChangeInformationViewModel(Account entity)
        {

            Birthday = entity.Birthday.HasValue ? entity.Birthday.Value.ToViDate() : string.Empty;
            Name = entity.Name;
            Gender = entity.Gender;
            MaritalStatus = entity.MaritalStatus;


        }

        [Required(ErrorMessage = "Hãy nhập {0}")]
        [Display(Name = "Họ và tên")]
        public string Name { get; set; }

        [Display(Name = "Ngày sinh")]
        public string Birthday { get; set; }

        [Display(Name = "Giới tính")]
        public Gender Gender { get; set; } = Gender.Male;


        [Display(Name = "Tình trạng hôn nhân")]
        public MaritalStatus MaritalStatus { get; set; } = MaritalStatus.Single;


    }

    public class ChangeContactViewModel
    {
        public ChangeContactViewModel()
        {

        }
        public ChangeContactViewModel(Account entity)
        {
            Phone = entity.Phone;
            Address = entity.Address;
            CityId = entity.CityId ?? 0;
            DistrictId = entity.DistrictId ?? 0;

        }

        [Required(ErrorMessage = "Hãy nhập {0}")]
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Hãy nhập {0}")]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Số điện thoại không đúng")]
        [Display(Name = "Số điện thoại")]
        public string Phone { get; set; }

        [Display(Name = "Tỉnh/Thành phố")]
        public int CityId { get; set; }

        [Display(Name = "Quận huyện")]
        public int DistrictId { get; set; }
    }


    public class ChangeBankAccountViewModel
    {
        public ChangeBankAccountViewModel()
        {

        }
        public ChangeBankAccountViewModel(Account entity)
        {
            Name = entity.BankAccountName;
            Number = entity.BankAccountNumber;
            Bank = entity.BankAccountBank;
            Branch = entity.BankAccountBranch;

        }


        [Required(ErrorMessage = "Hãy nhập {0}")]
        [Display(Name = "Tài khoản")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Hãy nhập {0}")]
        [Display(Name = "Số tài khoản")]
        public string Number { get; set; }

        [Required(ErrorMessage = "Hãy nhập {0}")]
        [Display(Name = "Ngân hàng")]
        public string Bank { get; set; }

        [Required(ErrorMessage = "Hãy nhập {0}")]
        [Display(Name = "Chi nhánh")]
        public string Branch { get; set; }
    }

    public class ChangeIDCardViewModel
    {
        public ChangeIDCardViewModel()
        {

        }
        public ChangeIDCardViewModel(Account entity)
        {
            Name = entity.IDCardName;
            Number = entity.IDCardNumber;
            ImageFront = entity.IDCardImageFront;
            ImageBack = entity.IDCardImageBack;

            Time = entity.IDCardTime;
            City = entity.IDCardCity;

        }

        [Required(ErrorMessage = "Hãy nhập {0}")]
        [Display(Name = "Họ và tên")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Hãy nhập {0}")]
        [Display(Name = "Số CMND")]
        public string Number { get; set; }

        [Required(ErrorMessage = "Hãy nhập {0}")]
        [Display(Name = "Ngày cấp")]
        public string Time { get; set; }


        [Required(ErrorMessage = "Hãy nhập {0}")]
        [Display(Name = "Nơi cấp")]
        public string City { get; set; }

        [Display(Name = "Mặt trước")]
        public string ImageFront { get; set; }
        [Display(Name = "Mặt sau")]
        public string ImageBack { get; set; }
    }

    public class ChangeAccountTypeViewModel
    {
        public ChangeAccountTypeViewModel()
        {

        }
        public ChangeAccountTypeViewModel(Account entity)
        {
            Type = entity.Type;
            if (entity.Type == AccountType.HotMom)
            {
                HotMomData = (List<AccountTypeHotMomData>)entity.TypeDataObj;
            }

        }
        [Display(Name ="Loại tài khoản")]
        public AccountType Type { get; set; }

        public List<AccountTypeHotMomData> HotMomData { get; set; } = new List<AccountTypeHotMomData>();
    }

    public class AccountCampaignChargeViewModel
    {
        public AccountCampaignChargeViewModel()
        {

        }
        public AccountCampaignChargeViewModel(AccountCampaignCharge entity)
        {
            Id = entity.Id;
            Type = entity.Type;
            AccountChargeAmount = entity.AccountChargeAmount;
        }

        public static List<AccountCampaignChargeViewModel> GetList(IEnumerable<AccountCampaignCharge> entities)
        {
            var ignoreTypes = new List<CampaignType>()
            {
                CampaignType.CustomService,
                CampaignType.JoinEvent
            };
            var result = new List<AccountCampaignChargeViewModel>();
            var types = Common.Helpers.StringHelper.GetEnumArray<CampaignType>();
            foreach (var type in types.Where(m => !ignoreTypes.Contains(m)))
            {
                var entity = entities.FirstOrDefault(m => m.Type == type);
                if (entity != null)
                {
                    result.Add(new AccountCampaignChargeViewModel()
                    {
                        Type = entity.Type,
                        AccountChargeAmount = entity.AccountChargeAmount,
                        Id = entity.Id
                    });
                }
                else
                {
                    result.Add(new AccountCampaignChargeViewModel()
                    {
                        Type = type,
                        AccountChargeAmount = 0,
                        Id = 0
                    });

                }
            }
            return result;

        }
        public int Id { get; set; }
        public CampaignType Type { get; set; }
        public int AccountChargeAmount { get; set; }
    }

    public class UpdateAccountCampaignChargeViewModel
    {
        public List<CampaignType> Type { get; set; }
        public List<int> AccountChargeAmount { get; set; }
        public List<int> Id { get; set; }

    }

        public class UpdateAccountProviderViewModel : LoginProviderViewModel
    {

    }
}
