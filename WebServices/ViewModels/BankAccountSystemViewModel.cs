using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebServices.ViewModels
{


    
    public class BankAccountSystemViewModel
    {


        public BankAccountSystemViewModel() { }

        public BankAccountSystemViewModel(BankAccountSystem q) {

            Id = q.Id;
            BankAccountName = q.BankAccountName;
            BankAccountNumber = q.BankAccountNumber;
            BankBranch = q.BankBranch;

            BankName = q.BankName;
            BankCode = q.BankCode;
            BankImageUrl = q.BankImageUrl;
            IsActive = q.IsActive;

        }


        public int Id { get; set; }

        public string BankAccountName { get; set; }

        public string BankAccountNumber { get; set; }

        public string BankBranch { get; set; }

        public string BankName { get; set; }

        public string BankCode { get; set; }

        public string BankImageUrl { get; set; }

        public bool IsActive { get; set; }


    }

    public class ListBankAccountSystemViewModel
    {
        public List<BankAccountSystemViewModel> BankAccountSystems { get; set; }
        public PagerViewModel Pager { get; set; }
    }



    public class EditBankAccountSystemViewModel
    {


        public EditBankAccountSystemViewModel() { }
     

        public int Id { get; set; }


        [Required(ErrorMessage = "Hãy nhập {0}")]
        [Display(Name = "Tên tài khoản")]
        public string BankAccountName { get; set; }

        [Required(ErrorMessage = "Hãy nhập {0}")]
        [Display(Name = "Số tài khoản")]
        public string BankAccountNumber { get; set; }

        [Required(ErrorMessage = "Hãy nhập {0}")]
        [Display(Name = "Tên ngân hàng")]
        public string BankName { get; set; }

        [Required(ErrorMessage = "Hãy nhập {0}")]
        [Display(Name = "Chi nhánh")]
        public string BankBranch { get; set; }
        
        [Display(Name = "Hoạt động/Không hoạt động ")]
        public bool IsActive { get; set; }






        public string BankCode { get; set; }
        public string BankImageUrl { get; set; }


    }




}
