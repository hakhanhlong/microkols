using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebServices.ViewModels;

namespace WebServices.Interfaces
{
    public interface ITransactionService
    {
        Task<int> GetCount(int agencyid, TransactionType type);

        Task<TransactionViewModel> GetTransaction(TransactionType type, int RefId);

        Task<long> GetTotalAmount(int agencyid, TransactionType type);
        Task<int> CreateTransaction(EntityType entityType, int entityId, RechargeViewModel model, string username);
        Task<int> CreateTransaction(EntityType entityType, int entityId, WithDrawViewModel model, string username);
        Task<ListTransactionHistoryViewModel> GetTransactionHistory(EntityType entityType, int entityid, TransactionType type, string daterange, int page, int pagesize);
        Task<ListTransactionHistoryViewModel> GetTransactionHistory(EntityType entityType, int entityid, string daterange, int page, int pagesize);


        #region statistic

        Task<List<TransactionStatisticViewModel>> Statistic_CampaignServicePaid(string startDate, string endDate, TransactionStatus status);
        Task<List<TransactionStatisticViewModel>> Statistic_CampaignAccountPaybackPaid(string startDate, string endDate, TransactionStatus status);
        Task<List<TransactionStatisticViewModel>> Statistic_CampaignServiceCashback(string startDate, string endDate, TransactionStatus status);
        Task<CampaignDetailRevenuePieChartViewModel> Statistic_CampaignDetailRevenuePieChart(int campaignid);
        Task<CampaignDetailRevenuePieChartViewModel> Statistic_CampaignRevenuePieChart(string startDate, string endDate);

        #endregion


    }
}
