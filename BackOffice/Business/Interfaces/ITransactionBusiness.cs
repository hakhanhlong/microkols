using BackOffice.Models;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackOffice.Business.Interfaces
{
    public interface ITransactionBusiness
    {
        Task<ListTransactionViewModel> GetTransactionByType(TransactionType type, int pageindex, int pagesize);
        Task<ListTransactionViewModel> GetTransactionByStatus(TransactionStatus status, int pageindex, int pagesize);

        ListTransactionViewModel GetTransactions(int pageindex, int pagesize);

        Task<ListGroupTransactionViewModel> GetTotalPayoutTransactions(TransactionType type, TransactionStatus status, List<AccountType> accounttype, int pageindex, int pagesize);

        Task<TransactionViewModel> Get(int id);


        Task<ListTransactionViewModel> GetTransactions(TransactionType type, TransactionStatus status, int pageindex, int pagesize);

        Task<ListTransactionViewModel> GetTransactions(int sender_wallet_id, int reciever_wallet_id, int pageindex, int pagesize);

        Task<List<GroupTransactionViewModel>> GetPayoutTransactions(TransactionType type, TransactionStatus status, List<AccountType> accounttype);

        Task<int> UpdateStatus(TransactionStatus status, int id, string username, string adminnote);

        int UpdateCashOut(int transactionid);

        Task<int> CalculateBalance(int transactionid, long transactionAmount, int senderid, int receiverid, string header_log, string username);

        bool CheckExist(int senderid, int receiverid, TransactionType type, int RefId);

        Task<ListTransactionViewModel> TransactionAgencyWalletRechargeSearch(string keyword, TransactionStatus status, DateTime? StartDate, DateTime? EndDate, int pageindex, int pagesize);
        Task<ListTransactionViewModel> TransactionAgencyCampaignServiceCashBackSearch(string keyword, TransactionStatus status, DateTime? StartDate, DateTime? EndDate, int pageindex, int pagesize);

        Task<ListTransactionViewModel> TransactionAgencyCampaignServiceSearch(string keyword, TransactionStatus status, DateTime? StartDate, DateTime? EndDate, int pageindex, int pagesize);

        Task<ListTransactionViewModel> GetTransactions(string searchtype, int? sender_wallet_id, int? reciever_wallet_id, DateTime? StartDate, DateTime? EndDate, int pageindex, int pagesize);

        Task<ListTransactionViewModel> GetTransactionsByType(TransactionType? searchtype, int? sender_wallet_id, int? reciever_wallet_id, DateTime? StartDate, DateTime? EndDate, int pageindex, int pagesize);



    }
}
