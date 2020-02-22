
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
                            for (var i = 0; i < model.AccountIds.Count; i++)
                            {
                                var amount = model.AccountType.Contains(AccountType.Regular) ? model.AccountChargeAmount ?? 0 : model.AccountChargeAmounts[i];
                                BackgroundJob.Enqueue<ICampaignService>(m => m.CreateCampaignAccount(CurrentUser.Id, id, model.AccountIds[i], amount, CurrentUser.Username));

                            }
                        return RedirectToAction("Details", new { id = id });

                    }
                    else
                    {
                        this.AddAlertDanger("Lỗi khi tạo chiến dịch vui lòng thử lại");
                    }
                }

            }
            await ViewbagData();
            return View("CreateTarget", model);
            //await ViewbagData();
            //return View(model);
        }


        /*
        [HttpPost]
        public async Task<IActionResult> Create(CreateCampaignViewModel model, int submittype = 0)
        {
            var error = "";
            if (ModelState.IsValid)
            {

                var valid = true;

                if(model.Method == CampaignMethod.ChooseAccount)
                {
                    if (model.AccountType.Contains(AccountType.Regular))
                    {

                        //generate accountids

                        var matchedAccountIds = new List<int>();
                        var matchedAccountChargeAmounts = new List<int>();
                        var matchedAccounts = await _accountService.GetListAccount(model.AccountType, model.CategoryId, model.Gender, model.CityId, model.AgeStart, model.AgeEnd, string.Empty, 1, model.Quantity, null, 0, 0);

                        foreach (var matchedAccount in matchedAccounts.Accounts)
                        {
                            matchedAccountIds.Add(matchedAccount.Id);
                            matchedAccountChargeAmounts.Add(0);
                        }
                        model.AccountIds = matchedAccountIds;
                        model.AccountChargeAmounts = matchedAccountChargeAmounts;
                    }



                    if (model.AccountType == null || model.AccountType.Count == 0)
                    {
                        valid = false;
                           error = "Hãy chọn đối tượng ";
                    }
                    else if (model.AccountIds == null || model.AccountIds.Count == 0 || model.AccountIds.Count != model.AccountChargeAmounts.Count)
                    {
                        valid = false;
                        error = "Không có Kol phù hợp. Vui lòng chộn các tiêu chí khác";
                    }
                }
                else
                {
                    model.AccountIds = new List<int>();
                    model.AccountChargeAmounts = new List<int>();
                }

                if (valid)
                {
                    if (!string.IsNullOrEmpty(model.Image))
                    {
                        model.Image = _fileHelper.MoveTempFile(model.Image, "campaign");
                    }
                    var id = await _campaignService.CreateCampaign(CurrentUser.Id, model, CurrentUser.Username);
                    if (id > 0)
                    {
                        for (var i = 0; i < model.AccountIds.Count; i++)
                        {
                            var amount = model.AccountType.Contains(AccountType.Regular) ? model.AccountChargeAmount ?? 0 : model.AccountChargeAmounts[i];
                            BackgroundJob.Enqueue<ICampaignService>(m => m.CreateCampaignAccount(CurrentUser.Id, id, model.AccountIds[i], amount, CurrentUser.Username));

                        }
                        return Json(new
                        {
                            status = 1,
                            message = "Thêm chiến dịch mới thành công",
                            campaignid = id,
                            url = Url.Action("Details", new { id = id })
                        });


                    }
                    else
                    {
                        error = "Lỗi khi khởi tạo chiến dịch. Vui lòng thử lại";
                    }
                }

            }
            return Json(new
            {
                status = -1,
                message = error
            });
            //await ViewbagData();
            //return View(model);
        }
        */
        private async Task ViewbagData()
        {
            ViewBag.Categories = await _sharedService.GetCategories();
            ViewBag.CampaignTypeCharges = await _campaignService.GetCampaignTypeCharges();
            ViewBag.Cities = await _sharedService.GetCities();
        }


        #endregion

        #region Details

        public async Task<IActionResult> Details(int id, string vt = "1", int tab = 0)
        {
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

        public async Task<IActionResult> GetAccounts(AccountType type, string kw, int page = 1, int pagesize = 10)
        {
            var model = await _accountService.GetAccounts(type, kw, string.Empty, page, pagesize);

            return Ok(model);
        }
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
                this.AddAlert(r);
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
                this.AddAlert(r);
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


        [HttpPost]
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
        public async Task<IActionResult> Caption(int campaignid, string order, int pageindex = 1, int pagesize = 1)
        {
            var campaign = await _campaignService.GetCampaign(campaignid);
            if (campaign == null) return NotFound();
            ViewBag.Campaign = campaign;
            var model = await _campaignAccountCaptionService.GetGroupCampaignAccountCaptionsByCampaignId(campaign.Id, order, pageindex, pagesize);

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> FeedbackCaption(int campaignid, List<int> ids, int type, string returnurl = "")
        {
            foreach (var item in ids)
            {
                await _campaignAccountCaptionService.UpdateStatus(item, type == 1 ? CampaignAccountCaptionStatus.DaDuyet : CampaignAccountCaptionStatus.KhongDuyet, CurrentUser.Name);
            }

            this.AddAlert(true);
            if (!string.IsNullOrEmpty(returnurl))
            {
                return Redirect(returnurl);
            }
            return RedirectToAction("Caption", new { id = campaignid });
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
            return RedirectToAction("Caption", new { id = campaignid });
        }



        #endregion
    }

}