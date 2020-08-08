using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackOffice.Business.Interfaces;
using BackOffice.Models;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebServices.Interfaces;
using WebServices.Services;
using BackOffice.Extensions;

namespace BackOffice.Controllers
{
    [Authorize]
    public class CampaignController : Controller
    {

        ICampaignBusiness _ICampaignBusiness;
        ICampaignRepository _ICampaignRepository;
        ICampaignAccountRepository _ICampaignAccountRepository;


        ITransactionRepository _ITransactionRepository;

        IAgencyBusiness _IAgencyBusiness;
        private readonly ISharedBusiness _ISharedBusiness;
        private readonly INotificationBusiness _INotificationBusiness;
        private readonly INotificationService _INotificationService;

        ICampaignService _ICampaignService;
        IWalletService _IWalletService;
        ISharedService _sharedService;

        private readonly ICampaignAccountCaptionService _campaignAccountCaptionService;
        private readonly ICampaignAccountContentService _campaignAccountContentService;
        private readonly ICampaignAccountStatisticService _campaignAccountStatisticService;

        ITransactionService _TransactionService;


        public CampaignController(ICampaignBusiness __ICampaignBusiness, ICampaignRepository __ICampaignRepository, IAgencyBusiness __IAgencyBusiness, 
            ISharedBusiness __ISharedBusiness, INotificationBusiness __INotificationBusiness, ICampaignService __ICampaignService, 
            ITransactionRepository __ITransactionRepository, ITransactionService __ITransactionService, 
            IWalletService __IWalletService, ISharedService __ISharedService, ICampaignAccountCaptionService __ICampaignAccountCaptionService,
            ICampaignAccountContentService __ICampaignAccountContentService, 
            ICampaignAccountStatisticService __ICampaignAccountStatisticService, ICampaignAccountRepository __ICampaignAccountRepository, INotificationService __INotificationService)
        {
            _ICampaignBusiness = __ICampaignBusiness;
            _ICampaignRepository = __ICampaignRepository;
            _IAgencyBusiness = __IAgencyBusiness;
            _ISharedBusiness = __ISharedBusiness;
            _INotificationBusiness = __INotificationBusiness;
            _ICampaignService = __ICampaignService;
            _ITransactionRepository = __ITransactionRepository;
            _TransactionService = __ITransactionService;
            _IWalletService = __IWalletService;
            _sharedService = __ISharedService;
            _campaignAccountCaptionService = __ICampaignAccountCaptionService;
            _campaignAccountContentService = __ICampaignAccountContentService;
            _campaignAccountStatisticService = __ICampaignAccountStatisticService;

            _ICampaignAccountRepository = __ICampaignAccountRepository;
            _INotificationService = __INotificationService;


        }

        public async Task<IActionResult> Index(CampaignStatus? status, int pageindex = 1)
        {
            var listing = await _ICampaignBusiness.GetCampaignByStatus(status, pageindex, 25);
            DataSelectionStatusAndType();
            return View(listing);
        }

        public async Task<IActionResult> CampaignFollowAgency(int agencyid = 0, int pageindex = 1)
        {
            var listing = await _ICampaignBusiness.GetListCampaignByAgency(agencyid, pageindex, 25);
            var agency = await _IAgencyBusiness.GetAgency(agencyid);
            ViewBag.Agency = agency;
            DataSelectionStatusAndType();
            return View(listing);
        }

        private void DataSelectionStatusAndType()
        {
            ViewBag.CampaignStatus = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>
            {
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Tất cả", Value = ""},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = CampaignStatus.Created.ToShowName(), Value = "0"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = CampaignStatus.Confirmed.ToShowName(), Value = "1"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = CampaignStatus.Started.ToShowName(), Value = "2"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = CampaignStatus.Ended.ToShowName(), Value = "3"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = CampaignStatus.Completed.ToShowName(), Value = "4"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = CampaignStatus.Canceled.ToShowName(), Value = "5"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = CampaignStatus.Error.ToShowName(), Value = "6"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = CampaignStatus.Locked.ToShowName(), Value = "7"},

            };

            ViewBag.CampaignTypes = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>
            {
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = "Tất cả", Value = ""},
                //new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = CampaignType.ShareContent.ToShowName(), Value = "1"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = CampaignType.ShareContentWithCaption.ToShowName(), Value = "2"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = CampaignType.ChangeAvatar.ToShowName(), Value = "3"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = CampaignType.ReviewProduct.ToShowName(), Value = "4"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = CampaignType.JoinEvent.ToShowName(), Value = "5"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = CampaignType.ShareStreamUrl.ToShowName(), Value = "6"},
                new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem {Text = CampaignType.CustomService.ToShowName(), Value = "7"},
            };
        }

        public IActionResult Search(string kw, CampaignType? type, CampaignStatus? status, DateTime? StartDate, DateTime? EndDate, int pageindex = 1)
        {

            DataSelectionStatusAndType();
            var list = _ICampaignBusiness.Search(kw, type, status, StartDate, EndDate, pageindex, 25);

            return View(list);
        }

        public async Task<IActionResult> Detail(int agencyid = 0, int campaignid = 0, string vt = "1", int tab = 0)
        {

            var campaign = await _ICampaignBusiness.GetCampaign(campaignid);
            if (campaign == null) return NotFound();


            var model = await _ICampaignService.GetCampaignDetailsByAgency(campaign.AgencyId, campaignid);
            if (model == null) return NotFound();
           
            ViewBag.Tab = tab;
            if (tab == 1)
            {
                ViewBag.Statistics = await _campaignAccountStatisticService.GetCampaignAccountStatisticsByCampaignId(model.Id, string.Empty, 1, 1000);

                return View("DetailsStatistic", model);
            }

            await ViewbagData();
            ViewBag.activedTab = vt;
            DataSelectionStatusAndType();
            return View(model);
        }

        private async Task ViewbagData()
        {
            ViewBag.Categories = await _sharedService.GetCategories();
            ViewBag.CampaignTypeCharges = await _ICampaignService.GetCampaignTypeCharges();
            ViewBag.Cities = await _sharedService.GetCities();
        }




        public async Task<IActionResult> Configuration(int campaignid = 0)
        {

            WebServices.ViewModels.CampaignViewModel campaign;
            campaign = await _ICampaignService.GetCampaign(campaignid);            
            return View(campaign);
        }

        [HttpPost]
        public async Task<IActionResult> Configuration(WebServices.ViewModels.CampaignViewModel model)
        {

            await _ICampaignService.UpdateCampaignServiceChargePercent(model.ServiceChargePercent, model.Id);
            TempData["MessageSuccess"] = string.Format("Cấu hình chiến dịch {0} thành công!", model.Title);
            return RedirectToAction("Configuration", "Campaign", new { campaignid = model.Id });
        }


        public async Task<IActionResult> TakeNoteChangeStatus(int id, CampaignStatus status)
        {            
            var campaign = await _ICampaignRepository.GetByIdAsync(id);
            //get info payment from agency fee service on campaign
            //id = campaignid
            var payment = await _TransactionService.GetTransaction(TransactionType.CampaignServiceCharge, id);

            if (payment != null) {
                ViewBag.Payment = payment;
            }           
            DataSelectionStatusAndType();
            return View(new CampaignViewModel(campaign));
        }

        [HttpPost]
        public async Task<IActionResult> TakeNoteChangeStatus(int id, CampaignStatus status, string txt_note = "")
        {
            var campaign = await _ICampaignRepository.GetByIdAsync(id);
            DataSelectionStatusAndType();

            try {
                if (campaign != null)
                {

                    campaign.Status = status;
                    campaign.UserModified = HttpContext.User.Identity.Name;
                    campaign.DateModified = DateTime.Now;
                    campaign.SystemNote = txt_note;
                    _ICampaignRepository.Update(campaign);


                    NotificationType notificationType = NotificationType.CampaignCanceled;
                    string msg = string.Empty;
                    if (status == CampaignStatus.Canceled)
                    {
                        notificationType = NotificationType.CampaignCanceled;
                        msg = string.Format("Chiến dịch \"{0}\" bạn tạo đã bị hủy, bởi hệ thống. Lý do: {1}", campaign.Title, txt_note);
                    }
                    else if (status == CampaignStatus.Error)
                    {
                        notificationType = NotificationType.CampaignError;
                        msg = string.Format("Chiến dịch \"{0}\" bạn tạo đã có lỗi, hệ thống đã phát hiện lỗi và gửi thông báo đến bạn. Lý do: {1}", campaign.Title, txt_note);
                    }
                    else if (status == CampaignStatus.Ended)
                    {
                        notificationType = NotificationType.CampaignEnded;
                        msg = string.Format("Chiến dịch \"{0}\" bạn tạo đã kết thúc", campaign.Title);
                    }
                    else if(status == CampaignStatus.Confirmed)
                    {
                        notificationType = NotificationType.CampaignConfirmed;
                        msg = string.Format("Chiến dịch \"{0}\" bạn tạo đã được duyệt bởi hệ thống", campaign.Title);

                        // gửi notification đến các user được chỉ định
                        var campaignAccount = await _ICampaignAccountRepository.ListAsync(new CampaignAccountByAgencySpecification(campaign.Id));
                        foreach(var item in campaignAccount)
                        {
                            if(item.Status == CampaignAccountStatus.AgencyRequest) // Doanh nghiệp mời tham gia chiến dịch
                            {
                                //item.Status = CampaignAccountStatus.AgencyRequest;
                                //await _ICampaignAccountRepository.UpdateAsync(item);

                                try {
                                    var agency = await _IAgencyBusiness.GetAgency(campaign.AgencyId);

                                    NotificationType _notiType = NotificationType.AgencyRequestJoinCampaign;
                                    string notify_message = string.Format("Bạn đã được doanh nghiệp \"{0}\" mời tham gia chiến dịch \"{1}\"", agency.Name, campaign.Title);
                                    await _INotificationService.CreateNotification(campaign.Id, EntityType.Account, item.AccountId, _notiType, notify_message, "");
                                }
                                catch { }
                                
                            }
                            
                        }
                        //###########################################################################################################################################

                    }
                    else if (status == CampaignStatus.Completed)
                    {
                        notificationType = NotificationType.CampaignCompleted;
                        msg = string.Format("Chiến dịch \"{0}\" bạn tạo đã hoàn thành", campaign.Title);
                    }

                    if (!string.IsNullOrEmpty(msg))
                    {
                        await _INotificationBusiness.CreateNotificationCampaignByStatus(campaign.Id, campaign.AgencyId, notificationType, msg, txt_note);
                        
                    }
                    TempData["MessageSuccess"] = string.Format("Thay đổi trạng thái \"{0}\" thành công", status.ToString());




                    //if(status == CampaignStatus.Canceled || status == CampaignStatus.Error || status == CampaignStatus.Ended)
                    //{

                    //}
                    //else
                    //{
                    //    TempData["MessageError"] = "Status campaign do not fit";
                    //}


                }
                else
                {
                    TempData["MessageError"] = "Chiến dịch không tồn tại!";
                }
            }
            catch(Exception ex) {
                TempData["MessageError"] = ex.Message;
            }




            return RedirectToAction("TakeNoteChangeStatus", "Campaign", new { id = id, status = (int)status });
        }



        [HttpPost]
        public async Task<JsonResult> ChangeStatus(int id, CampaignStatus status)
        {
            var campaign = _ICampaignRepository.GetById(id);
            if(campaign!= null)
            {
                campaign.Status = status;
                campaign.UserModified = HttpContext.User.Identity.Name;
                


                string str_icon = string.Empty;
                NotificationType notificationType = NotificationType.CampaignConfirmed;
                string msg = string.Empty;
                if (status == CampaignStatus.Created)
                {
                    str_icon = "<a href=\"#\" class=\"btn btn-primary m-btn m-btn--icon m-btn--icon-only m-btn--pill m-btn--air\">< i class=\"fa fa-sticky-note\"></i></a>";
                }
                else if (status == CampaignStatus.Started)
                {
                    str_icon = "<a href=\"#\" class=\"btn btn-success m-btn m-btn--icon m-btn--icon-only m-btn--pill m-btn--air\"><i class=\"fa fa-heartbeat\"></i></a>";
                }
                else if (status == CampaignStatus.Ended)
                {
                    str_icon = "<a href=\"#\" class=\"btn btn-primary m-btn m-btn--icon m-btn--icon-only m-btn--pill m-btn--air\">< i class=\"fa fa-stop\"></i></a>";
                }
                else if (status == CampaignStatus.Confirmed)
                {
                    str_icon = "<a href=\"#\" class=\"btn btn-primary m-btn m-btn--icon m-btn--icon-only m-btn--pill m-btn--air\"><i class=\"fa fa-check-circle-o\"></i></a>";
                    msg = string.Format("Chiến dịch \"{0}\" bạn tạo đã được duyệt bởi hệ thống", campaign.Title);
                    notificationType = NotificationType.CampaignConfirmed;
                    campaign.SystemNote = msg;
                    await _INotificationBusiness.CreateNotificationCampaignByStatus(campaign.Id, campaign.AgencyId, notificationType, msg, "");
                }
                else if (status == CampaignStatus.Error)
                {
                    str_icon = "<a href=\"#\" class=\"btn btn-danger m-btn m-btn--icon m-btn--icon-only m-btn--pill m-btn--air\"><i class=\"fa fa-times-circle\"></i></a>";
                }
                else if (status == CampaignStatus.Completed)
                {
                    str_icon = "<a href=\"#\" class=\"btn btn-primary m-btn m-btn--icon m-btn--icon-only m-btn--pill m-btn--air\"><i class=\"fa fa-check-circle-o\"></i></a>";
                    msg = string.Format("Chiến dịch \"{0}\" bạn tạo đã hoàn thành", campaign.Title);
                    notificationType = NotificationType.CampaignCompleted;
                    campaign.SystemNote = msg;
                    await _INotificationBusiness.CreateNotificationCampaignByStatus(campaign.Id, campaign.AgencyId, notificationType, msg, "");
                }
                else if (status == CampaignStatus.Canceled)
                {
                    str_icon = "<a href=\"#\" class=\"btn btn-warning m-btn m-btn--icon m-btn--icon-only m-btn--pill m-btn--air\"><i class=\"fa fa-ban\"></i></a>";
                }

                _ICampaignRepository.Update(campaign);

                return Json(new
                {
                    code = 1,
                    message = "Thay đổi trạng thái chiến dịch thành công",
                    str_icon = str_icon
                });
            }

            return Json(new {
                code = -1,
                message = "Lỗi! Không thể thay đổi trạng thái chiến dịch",
                str_icon = ""
            });
        }

        public IActionResult Microkol(string kw, CampaignType? type, CampaignStatus? status, int pageindex = 1)
        {

            DataSelectionStatusAndType();
            var list = _ICampaignBusiness.Search(kw, type, status, pageindex, 25);

            return View(list);
        }


        #region Statistic

        public IActionResult Statistic_CampaignRevenue()
        {
            return View();
        }

        public IActionResult Statistic_CampaignService()
        {
            return View();
        }

        public IActionResult Statistic_CampaignCashback()
        {
            return View();
        }

        public IActionResult Statistic_CampaignAccountCashback()
        {
            return View();
        }


        #endregion


    }
}