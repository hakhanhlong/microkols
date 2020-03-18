﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackOffice.Business.Interfaces;
using BackOffice.Models;
using Common.Helpers;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace BackOffice.Controllers
{
    public class MicroKolController : Controller
    {

        IAccountBusiness _IAccountBusiness;

        IAccountRepository _IAccountRepository;

        IAccountCampaignChargeRepository _IAccountCampaignChargeRepository;

        IAccountCampaignChargeBusiness _IAccountCampaignChargeBusiness;
        ICampaignBusiness _ICampaignBusiness;
        ICampaignAccountRepository _ICampaignAccountRepository;
        ITransactionRepository _ITransactionRepository;
        ITransactionBusiness _ITransactionBusiness;
        IWalletBusiness _IWalletBusiness;
        IWalletRepository _IWalletRepository;
        INotificationBusiness _INotificationBusiness;


        public MicroKolController(IAccountBusiness __IAccountBusiness, IAccountRepository __IAccountRepository, 
            IAccountCampaignChargeRepository __IAccountCampaignChargeRepository,
            IAccountCampaignChargeBusiness __IAccountCampaignChargeBusiness, ICampaignBusiness __ICampaignBusiness,
            ICampaignAccountRepository __ICampaignAccountRepository, ITransactionRepository __ITransactionRepository,
            ITransactionBusiness __ITransactionBusiness, IWalletBusiness __IWalletBusiness, IWalletRepository __IWalletRepository,
            INotificationBusiness __INotificationBusiness)
        {
            _IAccountBusiness = __IAccountBusiness;
            _IAccountRepository = __IAccountRepository;
            _IAccountCampaignChargeRepository = __IAccountCampaignChargeRepository;
            _IAccountCampaignChargeBusiness = __IAccountCampaignChargeBusiness;
            _ICampaignBusiness = __ICampaignBusiness;
            _ICampaignAccountRepository = __ICampaignAccountRepository;
            _ITransactionRepository = __ITransactionRepository;
            _ITransactionBusiness = __ITransactionBusiness;
            _IWalletBusiness = __IWalletBusiness;
            _IWalletRepository = __IWalletRepository;
            _INotificationBusiness = __INotificationBusiness;
        }

        public IActionResult Index(int pageindex = 1)
        {

            var list = _IAccountBusiness.GetListAccount(pageindex, 25);
            if(list == null)
            {
                TempData["MessageError"] = "No data microkols for binding";
            }
            return View(list);
        }


        public async Task<IActionResult> Verify(int id = 1)
        {

            var microkol = await _IAccountBusiness.GetAccount(id);
            if (microkol == null)
            {
                TempData["MessageError"] = "MicroKol do not exist!";
            }
            return View(microkol);
        }


        [HttpPost]
        public async Task<IActionResult> Verify(AccountViewModel model, int chkConfirmVerify = 0, string txtMessage = "")
        {

            var microkol = _IAccountRepository.GetById(model.Id);
            if (microkol == null)
            {
                TempData["MessageError"] = "MicroKol do not exist!";
            }
            else
            {
                if(chkConfirmVerify == 0)
                {
                    microkol.Status = AccountStatus.Verified;
                    TempData["MessageError"] = "MicroKol need Re-Verify!";
                    await _INotificationBusiness.CreateNotificationAccountVerify(model.Id, model.Id, NotificationType.AccountVerifyDenied, txtMessage, "");
                    
                }
                else
                {
                    microkol.Status = AccountStatus.SystemVerified;
                    TempData["MessageSuccess"] = "Verified Success!";
                    await _INotificationBusiness.CreateNotificationAccountVerify(model.Id, model.Id, NotificationType.AccountVerifySuccess, "Verified Success!", "");
                }

                await _IAccountRepository.UpdateAsync(microkol);
            }

            return RedirectToAction("Verify", "Microkol", new { id = model.Id });
        }








        public IActionResult Search(string keyword, AccountType type, int pageindex = 1)
        {

            var list = _IAccountBusiness.Search(keyword, type, pageindex, 25);
            if (list == null)
            {
                TempData["MessageError"] = "No data microkols for binding";
            }
            return View(list);
        }

        public async Task<IActionResult> Detail(int id = 0)
        {
            var microkol = await _IAccountBusiness.GetAccount(id);
            if(microkol == null)
            {
                TempData["MessageError"] = "MicroKol do not exist!";
            }

            return View(microkol);
        }

        [HttpPost]
        public async Task<IActionResult> Detail(AccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                if(model.Id > 0)
                {
                    var microkol = _IAccountRepository.GetById(model.Id);
                    if (microkol == null)
                    {
                        TempData["MessageError"] = "MicroKol do not exist!";
                    }
                    else
                    {
                        microkol.Name = model.Name;
                        microkol.Email = model.Email;
                        microkol.Address = model.Address;
                        microkol.Phone = model.Phone;

                        microkol.Actived = model.Actived;
                        microkol.Deleted = model.Deleted;

                        microkol.IDCardName = model.IDCardName;
                        microkol.IDCardNumber = model.IDCardNumber;
                        microkol.IDCardTime = model.IDCardTime;
                        microkol.IDCardCity = model.IDCardCity;

                        microkol.BankAccountName = model.BankAccountName;
                        microkol.BankAccountNumber = model.BankAccountNumber;
                        microkol.BankAccountBank = model.BankAccountBank;
                        microkol.BankAccountBranch = model.BankAccountBranch;


                        await _IAccountRepository.UpdateAsync(microkol);

                        TempData["MessageSuccess"] = "MicroKol update success!";


                    }
                }
            }
        

            return View(model);
        }


        public async Task<IActionResult> ChangePassword(int id = 0)
        {
            var microkol = await _IAccountBusiness.GetAccount(id);
            if (microkol == null)
            {
                TempData["MessageError"] = "MicroKol do not exist!";
            }

            return View(microkol);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(AccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = _IAccountRepository.GetById(model.Id);

                if (entity != null)
                {
                    var oldpw = Common.Helpers.SecurityHelper.HashPassword(entity.Salt, model.OldPassword);
                    if (oldpw.Contains(entity.Password))
                    {
                        var newpw = SecurityHelper.HashPassword(entity.Salt, model.NewPassword);

                        entity.Password = newpw;
                        entity.DateModified = DateTime.Now;

                        entity.UserModified = HttpContext.User.Identity.Name;

                        await _IAccountRepository.UpdateAsync(entity);

                        TempData["MessageError"] = "Change password success!";

                    }

                }
                else
                {
                    TempData["MessageError"] = "MicroKol do not exist!";
                }                
            }

            return RedirectToAction("changepassword", "microkol", new { id = model.Id });

          
        }



        public async Task<IActionResult> ChangeType(int id = 0)
        {
            var microkol = await _IAccountBusiness.GetAccount(id);

            ViewBag.MocrokolTypes = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>
            {
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Regular", Value = "0"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "HotTeen", Value = "1"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "HotMom", Value = "2"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "HotFacebooker", Value = "3"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Kols", Value = "4"}
            };


            if (microkol == null)
            {
                TempData["MessageError"] = "MicroKol do not exist!";
            }

            return View(microkol);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeType(AccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Id > 0)
                {
                    var microkol = _IAccountRepository.GetById(model.Id);
                    if (microkol == null)
                    {
                        TempData["MessageError"] = "MicroKol do not exist!";
                    }
                    else
                    {

                        microkol.Type = model.Type;
                        await _IAccountRepository.UpdateAsync(microkol);
                        TempData["MessageSuccess"] = "MicroKol update microkol type success!";

                    }
                }
            }


            return RedirectToAction("changetype", "microkol", new { id = model.Id });
        }


        public async Task<IActionResult> CampaignCharge(int id = 0)
        {
            var list = _IAccountCampaignChargeBusiness.GetByAccountID(id);
            var microkol = await _IAccountBusiness.GetAccount(id);

            if(microkol != null)
            {
                ViewBag.Microkol = microkol;
            }

            return View(list);
        }

        [HttpPost]
        public JsonResult CampaignCharge([FromBody] List<AccountCampaignChargeViewModel> model)
        {
            try {
                foreach (var item in model)
                {
                    var obj = _IAccountCampaignChargeRepository.GetById(item.Id);
                    //obj.AccountChargeAmount = item.AccountChargeAmount;
                    _IAccountCampaignChargeRepository.Update(obj);
                }
                TempData["MessageSuccess"] = "Update Success!";


            }
            catch(Exception ex)
            {
                TempData["MessageError"] = ex.Message;
                return Json(new
                {
                    state = 0,
                    msg = ex.Message
                });
                
            }

            return Json(new
            {
                state = 0,
                msg = string.Empty
            });
        }



        private void BindMicroKolStatusData()
        {
            ViewBag.Status = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>
            {
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Tất cả", Value = ""},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Chờ trả tiền", Value = "-1"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Thành viên xin tham gia chiến dịch", Value = "0"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Doanh nghiệp mời tham gia chiến dịch", Value = "1"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Đã xác nhận tham gia chiến dịch", Value = "2"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Đang gửi xét duyệt", Value = "3"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Yêu cầu sửa nội dung", Value = "31"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Đã duyệt nội dung", Value = "32"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Đã duyệt và cập nhật nội dung", Value = "33"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Đã hoàn thành", Value = "6"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Hủy tham gia", Value = "7"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Chưa hoàn thành", Value = "8"}
            };
        }

        public IActionResult CampaignMicrokol(CampaignAccountStatus? status, DateTime? StartDate, DateTime? EndDate, int pageindex = 1)
        {
            BindMicroKolStatusData();
            var list = _ICampaignBusiness.GetCampaignAccountByStatus(status, StartDate, EndDate , pageindex, 25);

            return View(list);
        }



        public IActionResult CampaignMicrokolDetail(int accountid, CampaignAccountStatus? status, DateTime? StartDate, DateTime? EndDate, int pageindex = 1)
        {
            BindMicroKolStatusData();

              var list = _ICampaignBusiness.GetCampaignAccountByAccount(status, accountid, StartDate, EndDate, pageindex, 20);

            return View(list);
        }


        public  IActionResult MicroKolSubstractMoney(int caid = 0)
        {

            var filter = new CampaignAccountByIdSpecification(caid);
            var campaignaccount = _ICampaignAccountRepository.GetSingleBySpec(filter);
            var campaignaccountmodel = new CampaignAccountViewModel(campaignaccount);


            return View(campaignaccountmodel);
        }


     

        [HttpPost]
        public async Task<IActionResult> MicroKolSubstractMoney(long money_number, string txt_note, int caid)
        {

            var filter = new CampaignAccountByIdSpecification(caid);
            var campaignaccount = _ICampaignAccountRepository.GetSingleBySpec(filter);

            //caid = campaignaccount id
            if (campaignaccount != null)
            {

                try
                {
                    int senderid = await _IWalletRepository.GetWalletId(EntityType.Account, campaignaccount.AccountId);
                    int recieverid = await _IWalletRepository.GetWalletId(EntityType.Agency, campaignaccount.Campaign.AgencyId);
                    int campaignid = campaignaccount.Campaign.Id;
                    //check xem đã trừ tiền chưa?
                    if (_ITransactionBusiness.CheckExist(senderid, recieverid, TransactionType.CampaignAccountRefundAgency, campaignid))
                    {
                        TempData["MessageError"] = "You was Refund money!";
                    }
                    else
                    {
                       
                        int transactionid = await _ITransactionRepository.CreateTransaction(senderid, recieverid, money_number, TransactionType.CampaignAccountRefundAgency, txt_note, string.Format("Campaign ID = {0}", campaignid), HttpContext.User.Identity.Name, campaignid);
                        if (transactionid > 0)
                        {
                            int retValue = await _ITransactionBusiness.CalculateBalance(transactionid, money_number, senderid, recieverid, "[Hoàn lại tiền Agency từ người dùng tham gia chiến dịch][CampaignAccountRefundAgency]", HttpContext.User.Identity.Name);
                            /*
                            * 09: success
                            * 10: wallet do not exist
                            * 11: wallet balance sender or receiver less then zero or amount could be abstract
                            * 
                            */

                            try
                            {
                                                            
                                switch (retValue)
                                {
                                    case 9:
                                        TempData["MessageSuccess"] = "Success Refund Money";
                                        campaignaccount.IsRefundToAgency = true;
                                        await _ICampaignAccountRepository.UpdateAsync(campaignaccount);
                                        await _ITransactionRepository.UpdateTransactionStatus(transactionid, TransactionStatus.Completed, "[Hoàn lại tiền Agency từ người dùng tham gia chiến dịch][CampaignAccountRefundAgency] Success", HttpContext.User.Identity.Name);// delete transaction if case error
                                        break;
                                    case 10:
                                        TempData["MessageError"] = "Wallet do not exist";
                                        await _ITransactionRepository.UpdateTransactionStatus(transactionid, TransactionStatus.Error, "[Hoàn lại tiền Agency từ người dùng tham gia chiến dịch][CampaignAccountRefundAgency] Wallet do not exist", HttpContext.User.Identity.Name);// delete transaction if case error
                                        break;
                                    case 11:
                                        TempData["MessageError"] = "Wallet balance sender or receiver less then zero or amount could be abstract";
                                        await _ITransactionRepository.UpdateTransactionStatus(transactionid, TransactionStatus.Error, "[Hoàn lại tiền Agency từ người dùng tham gia chiến dịch][CampaignAccountRefundAgency] Wallet balance sender or receiver less then zero or amount could be abstract", HttpContext.User.Identity.Name);// delete transaction if case error
                                        break;
                                    case 12:
                                        TempData["MessageError"] = "Wallet balance sender do not enought balance";
                                        await _ITransactionRepository.UpdateTransactionStatus(transactionid, TransactionStatus.Error, "[Hoàn lại tiền Agency từ người dùng tham gia chiến dịch][CampaignAccountRefundAgency] Wallet balance sender do not enought balance", HttpContext.User.Identity.Name);// delete transaction if case error
                                        break;
                                }
                            }
                            catch(Exception ex) {
                                TempData["MessageError"] = ex.Message;
                            }

                           
                        }
                        else
                        {
                            TempData["MessageError"] = "Can't created transaction!";
                        }
                    }


                }
                catch (Exception ex)
                {
                    TempData["MessageError"] = ex.Message;
                }



            }
            else
            {
                TempData["MessageError"] = "Campaign Account NULL!";
            }


            

           
            


            return RedirectToAction("MicroKolSubstractMoney", "MicroKol", new { caid = caid });
        }



    }
}