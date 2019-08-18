using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackOffice.Business.Interfaces;
using BackOffice.Models;
using Common.Helpers;
using Core.Entities;
using Core.Interfaces;
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

        public MicroKolController(IAccountBusiness __IAccountBusiness, IAccountRepository __IAccountRepository, IAccountCampaignChargeRepository __IAccountCampaignChargeRepository,
            IAccountCampaignChargeBusiness __IAccountCampaignChargeBusiness, ICampaignBusiness __ICampaignBusiness)
        {
            _IAccountBusiness = __IAccountBusiness;
            _IAccountRepository = __IAccountRepository;
            _IAccountCampaignChargeRepository = __IAccountCampaignChargeRepository;
            _IAccountCampaignChargeBusiness = __IAccountCampaignChargeBusiness;
            _ICampaignBusiness = __ICampaignBusiness;
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
                    obj.AccountChargeAmount = item.AccountChargeAmount;
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


        public IActionResult CampaignMicrokol(CampaignAccountStatus status = CampaignAccountStatus.WaitToPay, int pageindex = 1)
        {
            ViewBag.Status = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>
            {
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


            var list = _ICampaignBusiness.GetCampaignAccountByStatus(status, pageindex, 25);

            return View(list);
        }

    }
}