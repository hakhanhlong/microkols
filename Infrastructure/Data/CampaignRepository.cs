using Common;
using Common.Helpers;
using Core.Entities;
using Core.Interfaces;
using Core.Models;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Extensions;
namespace Infrastructure.Data
{
    public class CampaignRepository : EfRepository<Campaign>, ICampaignRepository
    {

        public CampaignRepository(AppDbContext dbContext) : base(dbContext)
        {

        }


        public async Task<string> GetValidCode(int agencyid)
        {
            var code = string.Format("{0}{1:ddMMyy}", agencyid, DateTime.Now);

            var count = await _dbContext.Campaign.CountAsync(m => m.Code.Contains(code));

            return string.Format("{0}{1:D2}", code, count + 1);
        }

        public async Task<List<int>> GetCampaignIds(CampaignStatus status)
        {
            return await _dbContext.Campaign.Where(m => m.Status == status).Select(m => m.Id).ToListAsync();
        }


        public async Task<List<int>> GetCampaignIdNeedToStart()
        {
            var now = DateTime.Now;
            var campaignids = await _dbContext.Campaign.Where(m => m.Status == CampaignStatus.Confirmed && m.DateStart <= now).Select(m => m.Id).ToListAsync();
            return campaignids;
        }

       

        public async Task<List<int>> GetCampaignIdNeedToEnd()
        {
            var now = DateTime.Now;
            var campaignids = await _dbContext.Campaign.Where(m => m.Status == CampaignStatus.Started && m.DateEnd <= now).Select(m => m.Id).ToListAsync();
            return campaignids;
        }

        public async Task<CampaignPaymentModel> GetCampaignPaymentByAgency(int agencyid, int id)
        {

            var campaign = await _dbContext.Campaign.Include(m => m.CampaignOption).Include(m => m.CampaignAccount).ThenInclude(m => m.Account)
                .FirstOrDefaultAsync(m => m.AgencyId == agencyid && m.Published && m.Id == id);
            if (campaign != null)
            {
                var transactions = await _dbContext.Transaction.Where(m => m.RefId == campaign.Id).ToListAsync();
                return new CampaignPaymentModel(campaign, campaign.CampaignOption,
                    campaign.CampaignAccount, transactions);
            }
            return null;

        }

        public async Task<List<Campaign>> GetCampaignsMatchedAccount(int accountid)
        {

            var account = await _dbContext.Account.FirstOrDefaultAsync(m => m.Id == accountid && m.Actived);
            if (account == null) return new List<Campaign>();

            var campaigns = await _dbContext.Campaign.Where(m => m.Status == CampaignStatus.Created
                && !m.CampaignAccount.Any(n => n.AccountId == accountid)
                && m.CampaignAccountType.Any(n => n.AccountType == account.Type))
                .Include(m => m.CampaignOption)
                .Include(m => m.CampaignAccount)
                .Include(m => m.CampaignAccountType)
                .ToListAsync();



            var age = 0;
            var gender = "";
            var cityid = 0;
            var accountCategoryIds = new List<int>();

            if (account.Birthday.HasValue)
            {
                var today = DateTime.Today;

                age = today.Year - account.Birthday.Value.Year;
                if (account.Birthday.Value.Date > today.AddYears(-age)) age--;
            }

            gender = account.Gender.ToString();
            cityid = account.CityId ?? 0;
            accountCategoryIds = await _dbContext.AccountCategory.Where(m => m.AccountId == account.Id).Select(m => m.CategoryId).Distinct().ToListAsync();

            var result = new List<Campaign>();

            foreach (var campaign in campaigns)
            {
                var isValid = true;

                var campaignOptions = campaign.CampaignOption.ToList();

                if (campaignOptions.Count > 0)
                {
                    isValid = false;
                    foreach (var campainOption in campaignOptions)
                    {
                        if (!isValid)
                        {

                            if (campainOption.Name == CampaignOptionName.AgeRange)
                            {
                                if (age > 0)
                                {
                                    var ageArr = campainOption.Value.Split('-');
                                    if (ageArr.Length == 2)
                                    {
                                        var minAge = 0;
                                        var maxAge = 0;
                                        if (int.TryParse(ageArr[0], out minAge) && int.TryParse(ageArr[1], out maxAge))
                                        {
                                            if (age >= minAge && age <= maxAge)
                                            {
                                                isValid = true;
                                            }
                                        }
                                    }
                                }

                            }
                            else if (campainOption.Name == CampaignOptionName.Gender)
                            {
                                if (!string.IsNullOrEmpty(gender))
                                {
                                    if (gender == campainOption.Value)
                                    {
                                        isValid = true;
                                    }
                                }
                            }
                            else if (campainOption.Name == CampaignOptionName.City)
                            {
                                if (cityid > 0)
                                {
                                    if (campainOption.Value == cityid.ToString())
                                    {
                                        isValid = true;
                                    }
                                }
                            }
                            else if (campainOption.Name == CampaignOptionName.Category)
                            {
                                if (cityid > 0)
                                {
                                    if (campainOption.Value == cityid.ToString())
                                    {
                                        isValid = true;
                                    }
                                }
                            }
                            else if (campainOption.Name == CampaignOptionName.Category)
                            {
                                if (accountCategoryIds.Count > 0)
                                {

                                    if (accountCategoryIds.Any())
                                    {
                                        var categoryid = 0;
                                        if (int.TryParse(campainOption.Value, out categoryid))
                                        {
                                            isValid = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }


                if (isValid)
                {
                    result.Add(campaign);
                }
            }

            return result;


        }


        public async Task<IQueryable<Campaign>> QueryCampaignByAccount(int accountid, int type, string keyword)
        {

            /*
               public CampaignByAccountSpecification(int accountid, string kw)
          : base(m => m.CampaignAccount.Any(n => n.AccountId == accountid) && (string.IsNullOrEmpty(kw) || m.Description.Contains(kw)))
        {
            AddInclude(m => m.CampaignOption);
            AddInclude(m => m.CampaignAccountType);
        }
             */

            var query = _dbContext.CampaignAccount.Where(m => m.AccountId == accountid);




            if (type == 1)
            {
                query = query.Where(m => m.Status == CampaignAccountStatus.AccountRequest || m.Status == CampaignAccountStatus.AccountRequest);
            }
            else if (type == 2)
            {
                query = query.Where(m => m.Status == CampaignAccountStatus.Confirmed || m.Status == CampaignAccountStatus.SubmittedContent
                || m.Status == CampaignAccountStatus.DeclinedContent || m.Status == CampaignAccountStatus.ApprovedContent);

            }
            else if (type == 3)
            {

                query = query.Where(m => m.Status == CampaignAccountStatus.UpdatedContent || m.Status == CampaignAccountStatus.Finished);

            }
            else if (type == 4)
            {
                query = query.Where(m => m.Status == CampaignAccountStatus.Canceled);
            }
            var campaignids = query.Select(m => m.CampaignId).Distinct();


            var queryCampaign = _dbContext.Campaign.Where(m => campaignids.Contains(m.Id));

            if (!string.IsNullOrEmpty(keyword))
            {
                queryCampaign = queryCampaign.Where(m => m.Code.Contains(keyword) || m.Description.Contains(keyword));
            }
            return queryCampaign.Include(m => m.CampaignOption).Include(m => m.CampaignAccountType);
        }

        public int CountAll()
        {
            return _dbContext.Campaign.Count();
        }

    }
}
