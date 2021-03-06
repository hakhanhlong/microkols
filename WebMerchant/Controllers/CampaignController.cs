﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebServices.Code.Helpers;
using WebServices.Interfaces;
using WebServices.Jobs;
using WebServices.ViewModels;

namespace WebMerchant.Controllers
{

    public class CampaignController : BaseController
    {
        private readonly ICampaignService _campaignService;
        private readonly ISharedService _sharedService;
        private readonly INotificationService _notificationService;
        private readonly IAccountService _accountService;
        private readonly IPaymentService _paymentService;
        private readonly IFileHelper _fileHelper;
        private readonly IWalletService _walletService;
        private readonly IAgencyService _agencyService;

        private readonly ICampaignAccountCaptionService _campaignAccountCaptionService;
        private readonly ICampaignAccountContentService _campaignAccountContentService;
        private readonly ICampaignAccountStatisticService _campaignAccountStatisticService;

        public CampaignController(ISharedService sharedService, IWalletService walletService,
            ICampaignAccountCaptionService campaignAccountCaptionService,
            ICampaignAccountContentService campaignAccountContentService,
            ICampaignAccountStatisticService campaignAccountStatisticService,
             IAccountService accountService, IFileHelper fileHelper, IPaymentService paymentService, IAgencyService agencyService,
            ICampaignService campaignService, INotificationService notificationService)
        {
            _campaignService = campaignService;
            _sharedService = sharedService;
            _notificationService = notificationService;
            _accountService = accountService;
            _fileHelper = fileHelper;
            _paymentService = paymentService;
            _walletService = walletService;
            _agencyService = agencyService;
            _campaignAccountCaptionService = campaignAccountCaptionService;
            _campaignAccountContentService = campaignAccountContentService;
            _campaignAccountStatisticService = campaignAccountStatisticService;
        }



        public async Task<IActionResult> Index(CampaignType? type, CampaignStatus? status, string kw, int pageindex = 1, int pagesize = 20)
        {

            var model = await _campaignService.GetListCampaignByAgency(CurrentUser.Id, type, status, kw, pageindex, pagesize);
            ViewBag.Kw = kw;
            ViewBag.type = type;
            ViewBag.status = status;

            try {
                var agency = await _agencyService.GetAgencyById(CurrentUser.Id);
                if (agency != null)
                {
                    if (string.IsNullOrEmpty(agency.Phone) && string.IsNullOrEmpty(agency.Email) && string.IsNullOrEmpty(agency.TaxIdNumber))
                    {
                        string msg = "Bạn cần hoàn tất việc đăng ký thông tin doanh nghiệp trong phần <a href=\"/Agency/ChangeInfo\">“thông tin tài khoản”</a> để thực hiện chiến dịch, cảm ơn bạn.";
                        TempData["MessageInfo"] = msg;
                    }
                }
            }
            catch { }
            



            return View(model);
        }

        #region Create


        public async Task<IActionResult> Create(CampaignType? campaignType)
        {
            if (!campaignType.HasValue)
            {
                return View("CreateChooseType");
            }

            var agencyinfo = await _agencyService.GetAgency(CurrentUser.Id);
            if (!agencyinfo.Type.HasValue)
            {
                return View("CreateError");
            }
            ViewBag.CampaignType = campaignType.Value;
            await ViewbagData();
            var model = await _campaignService.GetCreateCampaign(CurrentUser.Id, campaignType.Value);
            return View(model);
        }

        public IActionResult CreateInfo()
        {
            return RedirectToAction("Create");
        }
        [HttpPost]
        public async Task<IActionResult> CreateInfo(CreateCampaignInfoViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(model.Image))
                {
                    model.Image = _fileHelper.MoveTempFile(model.Image, "campaign");
                    ViewBag.Image = model.Image;
                }
                var paymentModel = new CreateCampaignTargetViewModel()
                {
                    InfoModel = JsonConvert.SerializeObject(model),
                    Type = model.Type,
                    KPIMin = model.Type.GetKpiMin(),
                    InteractiveMin = model.Type.GetInteractiveMin(),
                    AccountType = new List<AccountType>() { AccountType.Kols }
                };
                await ViewbagData();
                return View("CreateTarget", paymentModel);

            }
            return View("Create", model);
            //await ViewbagData();
            //return View(model);
        }
        public IActionResult CreateTarget()
        {
            return RedirectToAction("Create");
        }
        [HttpPost]
        public async Task<IActionResult> CreateTarget(CreateCampaignTargetViewModel model)
        {
            if (ModelState.IsValid)
            {
                var valid = true;
                if (valid)
                {

                    var info = JsonConvert.DeserializeObject<CreateCampaignInfoViewModel>(model.InfoModel);

                    var id = await _campaignService.CreateCampaign(CurrentUser.Id, info, model, CurrentUser.Username);
                    if (id > 0)
                    {
                        if (model.AccountIds != null)
                        {

                            var accountids = model.AccountIds.Distinct().ToList();

                            for (var i = 0; i < accountids.Count; i++)
                            {
                                var amount = model.AmountMax; //model.AccountType.Contains(AccountType.Regular) ? model.AccountChargeAmount ?? 0 : model.AccountChargeAmounts[i];

                                //Anh Long sửa lại, không cần thiết phải BackgroundJob
                                await _campaignService.CreateCampaignAccount(CurrentUser.Id, id, accountids[i], amount, CurrentUser.Username);
                                //BackgroundJob.Enqueue<ICampaignService>(m => m.CreateCampaignAccount(CurrentUser.Id, id, accountids[i], amount, CurrentUser.Username));



                            }
                        }
                        //########### Longhk add create notification ##########################################################

                        string _msg = string.Format("Chiến dịch \"{0}\" đã được tạo bởi \"{1}\".", info.Title, CurrentUser.Name);
                        string _data = "Campaign";
                        await _notificationService.CreateNotification(id, EntityType.System, 0, NotificationType.CampaignCreated, _msg, _data);

                        //#####################################################################################################

                        TempData["MessageSuccess"] = "Tạo chiến dịch thành công, bạn vui lòng chờ hệ thống duyệt chiến dịch của bạn!";

                        return RedirectToAction("Details", new { id = id });

                    }
                    else
                    {
                        TempData["MessageError"] = "Lỗi khi tạo chiến dịch vui lòng thử lại";
                    }
                }

            }
            await ViewbagData();
            return View("CreateTarget", model);
            //await ViewbagData();
            //return View(model);
        }


        private async Task ViewbagData()
        {
            ViewBag.Categories = await _sharedService.GetCategories();
            ViewBag.CampaignTypeCharges = await _campaignService.GetCampaignTypeCharges();
            ViewBag.Cities = await _sharedService.GetCities();
        }


        #endregion

        #region Edit
        public async Task<IActionResult> EditInfo(int id)
        {
            var model = await _campaignService.GetEditCampaignInfo(CurrentUser.Id, id);
            if (model == null) return NotFound();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditInfo(EditCampaignInfoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var r = await _campaignService.EditCampaignInfo(model, CurrentUser.Username);

                this.AddAlert(r);


                return RedirectToAction("Details", new { id = model.Id });

            }
            return View(model);
        }

        public async Task<IActionResult> EditTarget(int id)
        {
            var model = await _campaignService.GetEditCampaignTarget(CurrentUser.Id, id);
            if (model == null) return NotFound();
            await ViewbagData();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditTarget(EditCampaignTargetViewModel model)
        {
            if (ModelState.IsValid)
            {
                var r = await _campaignService.EditCampaignTarget(model, CurrentUser.Username);

                this.AddAlert(r);

                if (r)
                {
                    await _campaignService.RemoveCampaignAccount(model.Id);
                    if (model.EnabledAccount && model.AccountIds != null)
                    {

                        var accountids = model.AccountIds.Distinct().ToList();

                        for (var i = 0; i < accountids.Count; i++)
                        {
                            var amount = model.AmountMax; //model.AccountType.Contains(AccountType.Regular) ? model.AccountChargeAmount ?? 0 : model.AccountChargeAmounts[i];
                            //Anh Long sửa lại, không cần thiết phải BackgroundJob
                            await _campaignService.CreateCampaignAccount(CurrentUser.Id, model.Id, accountids[i], amount, CurrentUser.Username);
                            //BackgroundJob.Enqueue<ICampaignService>(m => m.CreateCampaignAccount(CurrentUser.Id, model.Id, accountids[i], amount, CurrentUser.Username));

                        }
                    }
                }

                return RedirectToAction("Details", new { id = model.Id });

            }
            await ViewbagData();
            return View(model);
        }
        #endregion

        #region Details

        public async Task<IActionResult> Details(int id, string vt = "1", int tab = 0)
        {

            //await _campaignService.AutoUpdateStartedStatus(666);

            var model = await _campaignService.GetCampaignDetailsByAgency(CurrentUser.Id, id);
            
            if (model == null) return NotFound();


            ViewBag.Tab = tab;
            if (tab == 1)
            {
                ViewBag.Statistics = await _campaignAccountStatisticService.GetCampaignAccountStatisticsByCampaignId(model.Id, string.Empty, 1, 1000);

                return View("DetailsStatistic", model);
            }

            await ViewbagData();
            ViewBag.activedTab = vt;
            ViewBag.Balance = await _walletService.GetAmount(CurrentUser.Type, CurrentUser.Id);

            if (model.Payment.IsValid)
            {
                if (model.Payment.TotalChargeValue < 0)
                {
                    ViewBag.IsRutTienExist = await _paymentService.IsExistPaymentServiceCashBack(CurrentUser.Id, model.Id);
                }
            }
            return View(model);
        }

        #endregion


        #region MatchedAccount


        public async Task<IActionResult> MatchedAccountPartial(int campaignid, int pageindex = 1, int pagesize = 20)
        {

            ViewBag.CampaignId = campaignid;
            var model = await _accountService.GetMatchedAccountByCampaignId(campaignid, "", pageindex, pagesize);
            return PartialView(model);

        }
        public async Task<IActionResult> MatchedAccount(CampaignType campaignType,
            IEnumerable<AccountType> accountTypes, IEnumerable<int> categoryid, Gender? gender,
            IEnumerable<int> cityid, int? agestart, int? ageend, int campaignId = 0, int pageindex = 1, int pagesize = 20, int min = 0, int max = 0)
        {

            ViewBag.Pagesize = pagesize;

            if (pagesize > 0)
            {  //lam tam phan nay phai dieu chinh lai sau 

                var pagesize2 = pagesize + 10;
                var model = await _accountService.GetListAccount(accountTypes, categoryid, gender, cityid, agestart, ageend,
                    string.Empty, pageindex, pagesize2, null, min, max);

                ViewBag.CampaignId = campaignId;
                ViewBag.CampaignType = campaignType;
                ViewBag.AccountTypes = accountTypes;
                ViewBag.Min = min;
                ViewBag.Max = max;

                ViewBag.Pagesize = pagesize;
                ViewBag.RenewUrl = Url.Action("RenewAccount", new { accountTypes, categoryid, gender, cityid, agestart, ageend, campaignType, min, max });
                return PartialView(model);
            }
            return PartialView();
        }

        public async Task<IActionResult> RenewAccount(IEnumerable<AccountType> accountTypes, IEnumerable<int> categoryid, Gender? gender,
           IEnumerable<int> cityid, int? agestart, int? ageend,
            IEnumerable<int> ignoreIds, CampaignType campaignType, int min = 0, int max = 0)
        {


            ViewBag.CampaignType = campaignType;
            ViewBag.AccountTypes = accountTypes;
            ViewBag.Min = min;
            ViewBag.Max = max;
            var model = await _accountService.GetListAccount(accountTypes, categoryid, gender, cityid,
                agestart, ageend, string.Empty, 1, 1, ignoreIds, min, max);


            return PartialView(model);

        }

        public async Task<IActionResult> RenewAccountModal

            (IEnumerable<AccountType> accountTypes, IEnumerable<int> categoryid, Gender? gender,
          IEnumerable<int> cityid, int? agestart, int? ageend,
           IEnumerable<int> ignoreIds, CampaignType campaignType, int min = 0, int max = 0)
        {


            ViewBag.CampaignType = campaignType;
            ViewBag.AccountTypes = accountTypes;
            ViewBag.Min = min;
            ViewBag.Max = max;
            var model = await _accountService.GetListAccount(accountTypes, categoryid, gender, cityid,
                agestart, ageend, string.Empty, 1, 1, ignoreIds, min, max);


            return PartialView(model);

        }

        #endregion

        #region Search Account


        public async Task<IActionResult> CampaignAccountPartial(int campaignid, int pageindex = 1, int pagesize = 10)
        {
            ViewBag.CampaignId = campaignid;
            var model = await _campaignService.GetCampaignAccount(campaignid, pageindex, pagesize);
            return PartialView(model);
        }

        public async Task<IActionResult> GetAccounts(AccountType? type, string kw, int pageindex = 1, int pagesize = 8)
        {
            var model = await _accountService.GetAccounts(type ?? AccountType.All, kw, string.Empty, pageindex, pagesize);

            return PartialView(model);
        }
        /*
        public async Task<IActionResult> GetAccounts(AccountType type, string kw, int page = 1, int pagesize = 10)
        {
            var model = await _accountService.GetAccounts(type, kw, string.Empty, page, pagesize);

            return Ok(model);
        }

        */
        #endregion

        #region Action


        public async Task<IActionResult> ReportCampaignAccount(int id, int campaignid)
        {

            var model = new ReportCampaignAccountViewModel()
            {
                Id = id,
                CampaignId = campaignid
            };
            return PartialView(model);
        }
        [HttpPost]
        public async Task<IActionResult> ReportCampaignAccount(ReportCampaignAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                var r = await _campaignService.ReportCampaignAccount(CurrentUser.Id, model, CurrentUser.Username);

                if (r == true)
                {
                    this.AddAlert(r, "Báo cáo người ảnh hưởng thành công!");
                }
                else { this.AddAlert(r); }
            }
            return RedirectToAction("Details", new { id = model.CampaignId, vt = "2" });
        }


        public async Task<IActionResult> UpdateCampaignAccountRating(int id, int campaignid, CampaignAccountRating? rating)
        {

            var model = new UpdateCampaignAccountRatingViewModel()
            {
                Id = id,
                CampaignId = campaignid,
                Rating = rating
            };
            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCampaignAccountRating(UpdateCampaignAccountRatingViewModel model)
        {
            if (ModelState.IsValid)
            {
                var r = await _campaignService.UpdateCampaignAccountRating(CurrentUser.Id, model, CurrentUser.Username);
                if(r == true)
                {
                    this.AddAlert(r, "Đánh giá người ảnh hưởng thành công!");
                }
                else { this.AddAlert(r); }
                
            }
            return RedirectToAction("Details", new { id = model.CampaignId, vt = "2" });
        }



        [HttpPost]
        public async Task<IActionResult> RequestAccountJoinCampaign(int campaignid, int accountid, string returnurl = "")
        {
            await _campaignService.RequestJoinCampaignByAgency(CurrentUser.Id, campaignid, accountid, CurrentUser.Name);

            if (!string.IsNullOrEmpty(returnurl))
            {
                return Redirect(returnurl);
            }
            return RedirectToAction("Details", new { id = campaignid });
        }
        [HttpPost]
        public async Task<IActionResult> RequestAllAccountJoinCampaign(int campaignid, List<int> accountid, int? amount, string returnurl = "")
        {
            foreach (var item in accountid)
            {
                await _campaignService.RequestJoinCampaignByAgency(CurrentUser.Id, campaignid, item, CurrentUser.Name);
            }


            if (!string.IsNullOrEmpty(returnurl))
            {
                return Redirect(returnurl);
            }
            return RedirectToAction("Details", new { id = campaignid });
        }


        [HttpPost] //duyệt influencer tham gia chiến dịch (influencer request)
        public async Task<IActionResult> FeedbackAccountJoinCampaign(int campaignid, int accountid, int type, string returnurl = "")
        {
            var result = await _campaignService.FeedbackJoinCampaignByAgency(CurrentUser.Id, campaignid, accountid, type == 1, CurrentUser.Name);

            this.AddAlert(result);
            if (!string.IsNullOrEmpty(returnurl))
            {
                return Redirect(returnurl);
            }
            return RedirectToAction("Details", new { id = campaignid });
        }

        [HttpPost]
        public async Task<IActionResult> FeedbackAllAccountJoinCampaign(int campaignid, List<int> accountid, int type, string returnurl = "")
        {
            foreach (var item in accountid)
            {
                await _campaignService.FeedbackJoinCampaignByAgency(CurrentUser.Id, campaignid, item, type == 1, CurrentUser.Name);
            }

            this.AddAlert(true);
            if (!string.IsNullOrEmpty(returnurl))
            {
                return Redirect(returnurl);
            }
            return RedirectToAction("Details", new { id = campaignid });
        }


        [HttpPost]
        public async Task<IActionResult> UpdateCampaignStatus(int campaignid, CampaignStatus status)
        {
            var result = await _campaignService.UpdateCampaignStatusByAgency(CurrentUser.Id, campaignid, status, CurrentUser.Username);
            if (result > 0)
            {

                if (status == CampaignStatus.Started)
                {
                    BackgroundJob.Enqueue<INotificationService>(m => m.CreateNotificationCampaignStarted(campaignid));
                }
                else if (status == CampaignStatus.Ended)
                {
                    BackgroundJob.Enqueue<INotificationService>(m => m.CreateNotificationCampaignEnded(campaignid));
                    BackgroundJob.Enqueue<ICampaignJob>(m => m.UpdateCompletedCampagin(campaignid));

                }


                this.AddAlert(true);
            }
            else if (result == -1)
            {
                this.AddAlert(false, "Không được phép thay đổi trạng thái Chiến dịch");
            }
            else if (result == -2)
            {
                this.AddAlert(false, "Không được phép kết thúc Chiến dịch nếu vẫn còn thành viên không thực hiện");
            }
            else if (result == -3)
            {
                this.AddAlert(false, "Không được phép bắt đầu chiến dịch khi không có thành viên tham gia hoặc thành viên chưa được duyệt hoặc hủy");
            }
            else
            {
                this.AddAlert(false);
            }

            return RedirectToAction("Details", new { id = campaignid });
        }


        [HttpPost]
        public async Task<IActionResult> FeedbackCampaignAccountRefContent(int campaignid, int accountid, int type, string refContent)
        {
            if (ModelState.IsValid)
            {
                var r = await _campaignService.FeedbackCampaignAccountRefContent(CurrentUser.Id, campaignid, accountid, CurrentUser.Username, type, refContent);
                if (r > 0)
                {
                    this.AddAlertSuccess((type == 1) ? $"Bạn đã xác nhận thành công nội dung Caption." : type == 2 ? "Bạn đã cập nhật nội dung caption thành công"
                        : "Bạn đã yêu cầu sửa lại nội dung caption thành công");


                }
                else
                {
                    this.AddAlertDanger("Thông tin chiến dịch không đúng");
                }

            }
            else
            {
                this.AddAlertDanger("Thông tin chiến dịch không đúng");
            }

            return RedirectToAction("Details", new { id = campaignid });
        }


        #endregion

        #region Caption
        public async Task<IActionResult> Caption(int campaignid, string order, int pageindex = 1, int pagesize = 20)
        {
            var campaign = await _campaignService.GetCampaign(campaignid);
            if (campaign == null) return NotFound();
            ViewBag.Campaign = campaign;
            var model = await _campaignAccountCaptionService.GetGroupCampaignAccountCaptionsByCampaignId(campaign.Id, order, pageindex, pagesize);
            ViewBag.CampaignId = campaign.Id;
            return View(model);
        }


        public async Task<IActionResult> CaptionHistory(int campaignaccountid, int accountId, int campaignid, int pageindex = 1)
        {            
            var model = await _campaignAccountCaptionService.GetCampaignAccountCaptions(campaignaccountid, "", pageindex, 25);
            var campaign = await _campaignService.GetCampaign(campaignid);
            if (campaign == null) return NotFound();
            ViewBag.Campaign = campaign;
            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> FeedbackCaption(int campaignid, List<int> ids, int type, string returnurl = "")
        {


            try
            {
                var campaign = await _campaignService.GetCampaign(campaignid);
                if (campaign != null)
                {
                    DateTime now = DateTime.Now;
                    if (campaign.ExecutionStart.Value <= now)
                    {
                        this.AddAlert(true, "Chiến dịch đã đến giờ thực hiện, nên bạn không thể tiếp tục duyệt caption");
                        return RedirectToAction("Details", new { id = campaignid });
                    }

                }
            }
            catch { }
            

            foreach (var item in ids)
            {
                await _campaignAccountCaptionService.UpdateStatus(item, type == 1 ? CampaignAccountCaptionStatus.DaDuyet : CampaignAccountCaptionStatus.KhongDuyet, CurrentUser.Name);
            }

            this.AddAlert(true);
            if (type == 1)
            {
                return RedirectToAction("Details", new { id = campaignid });
            }
            if (!string.IsNullOrEmpty(returnurl))
            {

                return Redirect(returnurl);
            }
            return RedirectToAction("Caption", new { campaignid = campaignid });
        }
        [HttpPost]
        public async Task<IActionResult> UpdateCaptionNote(int campaignid, int id, string note, string returnurl = "")
        {

            await _campaignAccountCaptionService.UpdateNote(id, note, CurrentUser.Name);

            this.AddAlert(true);
            if (!string.IsNullOrEmpty(returnurl))
            {
                return Redirect(returnurl);
            }
            return RedirectToAction("Caption", new { campaignid = campaignid });
        }



        #endregion


        #region Content
        public async Task<IActionResult> Content(int campaignid, string order, int pageindex = 1, int pagesize = 20)
        {
            var campaign = await _campaignService.GetCampaign(campaignid);
            if (campaign == null) return NotFound();
            ViewBag.Campaign = campaign;
            var model = await _campaignAccountContentService.GetGroupCampaignAccountContentsByCampaignId(campaign.Id, order, pageindex, pagesize);
            ViewBag.CampaignId = campaign.Id;
            return View(model);
        }

        public async Task<IActionResult> ContentHistory(int campaignaccountid, int accountId, int campaignid, int pageindex = 1)
        {
            var campaign = await _campaignService.GetCampaign(campaignid);
            if (campaign == null) return NotFound();            
            var model = await _campaignAccountContentService.GetCampaignAccountContents(campaignaccountid, "", pageindex, 25);
            ViewBag.CampaignId = campaign.Id;
            ViewBag.Campaign = campaign;
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> FeedbackContent(int campaignid, List<int> ids, int type, string returnurl = "")
        {
            try
            {
                var campaign = await _campaignService.GetCampaign(campaignid);
                if (campaign != null)
                {
                    DateTime now = DateTime.Now;
                    if (campaign.ExecutionStart.Value <= now)
                    {
                        this.AddAlert(true, "Chiến dịch đã đến giờ thực hiện, nên bạn không thể tiếp tục duyệt nội dung");
                        return RedirectToAction("Details", new { id = campaignid });
                    }

                }
            }
            catch { }


            foreach (var item in ids)
            {
                await _campaignAccountContentService.UpdateStatus(item, type == 1 ? CampaignAccountContentStatus.DaDuyet : CampaignAccountContentStatus.KhongDuyet, CurrentUser.Name);
            }

            this.AddAlert(true);

            if (type == 1)
            {
                return RedirectToAction("Details", new { id = campaignid });
            }
            if (!string.IsNullOrEmpty(returnurl))
            {
                return Redirect(returnurl);
            }
            return RedirectToAction("Content", new { campaignid });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateContentNote(int campaignid, int id, string note, string returnurl = "")
        {

            await _campaignAccountContentService.UpdateNote(id, note, CurrentUser.Name);

            this.AddAlert(true);
            if (!string.IsNullOrEmpty(returnurl))
            {
                return Redirect(returnurl);
            }
            return RedirectToAction("Content", new { campaignid = campaignid });
        }



        #endregion


        #region Thong ke chien dich hoan thanh
        public async Task<IActionResult> StatisticReportCampain(int campaignid)
        {

            var model = await _campaignService.GetCampaignDetailsByAgency(CurrentUser.Id, campaignid);

            return PartialView(model);
        }

        #endregion


        #region Update  Execution

        public async Task<IActionResult> UpdateExecutionTime(int campaignid)
        {

            ViewBag.Campaign = await _campaignService.GetCampaign(campaignid);

            return PartialView();
        }
        [HttpPost]
        public async Task<IActionResult> UpdateExecutionTime(int campaignid, string date)
        {
            var r = await _campaignService.UpdateExecutionTime(CurrentUser.Id, campaignid, date, CurrentUser.Username);
            if (r)
            {
                this.AddAlertSuccess("Cập nhật thời gian thực hiện thành công");
            }
            else
            {
                this.AddAlert(r);
            }
            if (!string.IsNullOrEmpty(UrlReferrer))
            {
                return Redirect(UrlReferrer);
            }


            return RedirectToAction("Details", new { id = campaignid });
        }
        #endregion
    }

}