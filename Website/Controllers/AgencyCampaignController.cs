
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Website.Code.Helpers;
using Website.Interfaces;
using Website.ViewModels;

namespace Website.Controllers
{
    [Authorize(Roles = "Agency")]
    public class AgencyCampaignController : BaseController
    {
        private readonly ICampaignService _campaignService;
        private readonly ISharedService _sharedService;
        private readonly INotificationService _notificationService;
        private readonly IAccountService _accountService;
        private readonly IFileHelper _fileHelper;
        public AgencyCampaignController(ISharedService sharedService,
             IAccountService accountService, IFileHelper fileHelper,
            ICampaignService campaignService, INotificationService notificationService)
        {
            _campaignService = campaignService;
            _sharedService = sharedService;
            _notificationService = notificationService;
            _accountService = accountService;
            _fileHelper = fileHelper;
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

        public async Task<IActionResult> Create()
        {
            await ViewbagData();
            return View(new CreateCampaignViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateCampaignViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.AccountType == null || model.AccountType.Count == 0)
                {
                    ModelState.AddModelError("AccountType", "Hãy chọn đối tượng");
                }
                else
                {
                    if (!string.IsNullOrEmpty(model.Image))
                    {
                        model.Image = _fileHelper.MoveTempFile(model.Image, "campaign");
                    }
                    var id = await _campaignService.CreateCampaign(CurrentUser.Id, model, CurrentUser.Username);
                    if (id > 0)
                    {
                        this.AddAlertSuccess("Thêm chiến dịch mới thành công");
                        return RedirectToAction("Details", new { id = id }) ;
                    }
                }

            }
            await ViewbagData();
            return View(model);
        }
        private async Task ViewbagData()
        {
            ViewBag.Categories = await _sharedService.GetCategories();
            ViewBag.CampaignTypeCharges = await _campaignService.GetCampaignTypeCharges();
            ViewBag.Cities = await _sharedService.GetCities();
        }


        #endregion
        #region Details

        public async Task<IActionResult> Details(int id)
        {
            var model = await _campaignService.GetCampaignDetailsByAgency(CurrentUser.Id, id);
            if (model == null) return NotFound();
            await ViewbagData();
            return View(model);
        }

        #endregion


        #region MatchedAccount


        public async Task<IActionResult> MatchedAccount(IEnumerable<AccountType> accountTypes, IEnumerable<int> categoryid, Gender? gender, int? cityid, int? agestart, int? ageend,
               IEnumerable<int> ignoreIds, int campaignId, int pageindex = 1)
        {
            const int pagesize = 20;
            var model = await _accountService.GetListAccount(accountTypes, categoryid, gender, cityid, agestart, ageend, string.Empty, pageindex, pagesize, ignoreIds);

            ViewBag.CampaignId = campaignId;
            return PartialView(model);
        }
        #endregion


        #region Action

        public async Task<IActionResult> RequestAccountJoinCampaign(int campaignid, int accountid)
        {
            var result = await _campaignService.RequestJoinCampaignByAgency(CurrentUser.Id, campaignid, accountid, CurrentUser.Name);
            return Json(result ? 1 : 0);
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
        public async Task<IActionResult> FeedbackCampaignAccountRefContent(int campaignid, int accountid, int type)
        {
            if (ModelState.IsValid)
            {
                var r = await _campaignService.FeedbackCampaignAccountRefContent(CurrentUser.Id, campaignid, accountid, CurrentUser.Username, type);
                if (r > 0)
                {
                    this.AddAlertSuccess((type == 1) ? $"Bạn đã xác nhận thành công nội dung Caption." : "Bạn đã hủy nội dung caption thành công");


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
    }
}