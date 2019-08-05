
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
using Website.Jobs;
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
        private readonly IPaymentService _paymentService;
        private readonly IFileHelper _fileHelper;
        public AgencyCampaignController(ISharedService sharedService,
             IAccountService accountService, IFileHelper fileHelper, IPaymentService paymentService,
            ICampaignService campaignService, INotificationService notificationService)
        {
            _campaignService = campaignService;
            _sharedService = sharedService;
            _notificationService = notificationService;
            _accountService = accountService;
            _fileHelper = fileHelper;
            _paymentService = paymentService;
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
            var model = await _campaignService.GetCreateCampaign(CurrentUser.Id);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateCampaignViewModel model, int submittype = 0)
        {
            var error = "";
            if (ModelState.IsValid)
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
                    error = "Hãy chọn đối tượng ";
                }
                else if (model.AccountIds == null || model.AccountIds.Count == 0 || model.AccountIds.Count != model.AccountChargeAmounts.Count)
                {
                    error = "Không có Kol phù hợp. Vui lòng chộn các tiêu chí khác";
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


                        for (var i = 0; i < model.AccountIds.Count; i++)
                        {
                            var amount = model.AccountType.Contains(AccountType.Regular) ? model.AccountChargeAmount ?? 0 : model.AccountChargeAmounts[0];
                            BackgroundJob.Enqueue<ICampaignService>(m => m.CreateCampaignAccount(CurrentUser.Id, id, model.AccountIds[i], amount, CurrentUser.Username));

                        }
                        return Json(new
                        {
                            status = 1,
                            message = "Thêm chiến dịch mới thành công",
                            campaignid = id,
                            url = Url.Action("Details", new { id = id})
                        });


                        /*
                        //this.AddAlertSuccess("Thêm chiến dịch mới thành công");
                        //return RedirectToAction("Details", new { id = id });

                        if (submittype == 1)
                        {
                            var paymentResult = await _paymentService.CreateAgencyPayment(CurrentUser.Id, new CreateCampaignPaymentViewModel()
                            {
                                CampaignId = id,
                                Note = string.Empty
                            }, CurrentUser.Username);


                            if (paymentResult.Status == TransactionStatus.Completed)
                            {

                                // tam thoi chua khac phuc dc loi tracking id
                                BackgroundJob.Enqueue<ICampaignService>(m => m.UpdateCampaignStatusByAgency(CurrentUser.Id, id, CampaignStatus.Created, CurrentUser.Username));
                                //await _campaignService.UpdateCampaignStatusByAgency(CurrentUser.Id, id, CampaignStatus.Created , CurrentUser.Name);
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
                                error = $"{paymentResult.ErrorMessage} - Mã lỗi: {paymentResult.ErrorCode}";
                            }

                        }
                        else
                        {
                         
                        }
                        //payment luon
                        */

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
        private async Task ViewbagData()
        {
            ViewBag.Categories = await _sharedService.GetCategories();
            ViewBag.CampaignTypeCharges = await _campaignService.GetCampaignTypeCharges();
            ViewBag.Cities = await _sharedService.GetCities();
        }


        #endregion

        #region Details

        public async Task<IActionResult> Details(int id, string vt = "1")
        {
            var model = await _campaignService.GetCampaignDetailsByAgency(CurrentUser.Id, id);
            if (model == null) return NotFound();
            await ViewbagData();
            ViewBag.activedTab = vt;
            return View(model);
        }

        #endregion


        #region MatchedAccount


        public async Task<IActionResult> MatchedAccount(CampaignType campaignType,
            IEnumerable<AccountType> accountTypes, IEnumerable<int> categoryid, Gender? gender,
            IEnumerable<int> cityid, int? agestart, int? ageend, int campaignId = 0, int pageindex = 1, int pagesize = 20, int min = 0, int max = 0)
        {

            ViewBag.Pagesize = pagesize;

            if (pagesize > 0)
            {
                var model = await _accountService.GetListAccount(accountTypes, categoryid, gender, cityid, agestart, ageend,
                    string.Empty, pageindex, pagesize, null, min, max);

                ViewBag.CampaignId = campaignId;
                ViewBag.CampaignType = campaignType;
                ViewBag.AccountTypes = accountTypes;
                ViewBag.Min = min;
                ViewBag.Max = max;
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



        public async Task<IActionResult> RequestAccountJoinCampaign(int campaignid, int accountid, int? amount)
        {
            var result = await _campaignService.RequestJoinCampaignByAgency(CurrentUser.Id, campaignid, accountid,  CurrentUser.Name);
            return Json(result ? 1 : 0);
        }

        public async Task<IActionResult> FeedbackAccountJoinCampaign(int campaignid, int accountid, int type)
        {
            var result = await _campaignService.FeedbackJoinCampaignByAgency(CurrentUser.Id, campaignid, accountid, type == 1, CurrentUser.Name);

            this.AddAlert(result);
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
    }
}