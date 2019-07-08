using BackOffice.Business.Interfaces;
using BackOffice.Models;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackOffice.Business
{
    public class TransactionBusiness: ITransactionBusiness
    {

        ITransactionRepository _ITransactionRepository;
        IWalletRepository _IWalletRepository;
        private readonly ILogger<TransactionBusiness> _logger;

        public TransactionBusiness(ITransactionRepository __ITransactionRepository, 
            ILoggerFactory _loggerFactory, IWalletRepository __IWalletRepository) {
            _ITransactionRepository = __ITransactionRepository;
            _logger = _loggerFactory.CreateLogger<TransactionBusiness>();
            _IWalletRepository = __IWalletRepository;
        }

        public async Task<ListTransactionViewModel> GetTransactionByType(TransactionType type, int pageindex, int pagesize)
        {
            var filter = new TransactionSpecification(type);

            var transactions = await _ITransactionRepository.ListPagedAsync(filter, "DateModified_desc", pageindex, pagesize);

            var total = await _ITransactionRepository.CountAsync(filter);

            return new ListTransactionViewModel()
            {
                Transactions = transactions.Select(t => new TransactionViewModel(t)).ToList(),
                Pager = new PagerViewModel(pageindex, pagesize, total)
            };

        }


        public async Task<ListTransactionViewModel> GetTransactionByStatus(TransactionStatus status, int pageindex, int pagesize)
        {
            var filter = new TransactionSpecification(status);
            var transactions = await _ITransactionRepository.ListPagedAsync(filter, "DateModified_desc", pageindex, pagesize);
            var total = await _ITransactionRepository.CountAsync(filter);

            return new ListTransactionViewModel()
            {
                Transactions = transactions.Select(t => new TransactionViewModel(t)).ToList(),
                Pager = new PagerViewModel(pageindex, pagesize, total)
            };

        }

        public async Task<ListTransactionViewModel> GetTransactions(TransactionType type, TransactionStatus status, int pageindex, int pagesize)
        {
            var filter = new TransactionSpecification(type, status);
            var transactions = await _ITransactionRepository.ListPagedAsync(filter, "DateModified_desc", pageindex, pagesize);
            var total = await _ITransactionRepository.CountAsync(filter);

            return new ListTransactionViewModel()
            {
                Transactions = transactions.Select(t => new TransactionViewModel(t)).ToList(),
                Pager = new PagerViewModel(pageindex, pagesize, total)
            };
        }

        public ListTransactionViewModel GetTransactions(int pageindex, int pagesize)
        {
            var transactions = _ITransactionRepository.ListPaging("DateModified_desc", pageindex, pagesize);

            var total = _ITransactionRepository.CountAll();

            return new ListTransactionViewModel()
            {
                Transactions = transactions.Select(t => new TransactionViewModel(t)).ToList(),
                Pager = new PagerViewModel(pageindex, pagesize, total)
            };
        }

        public async Task<int> UpdateStatus(TransactionStatus status, int id, string username)
        {
            int retValue = -1;
            var _transaction = _ITransactionRepository.GetById(id);
            if(_transaction != null)
            {
                _transaction.Status = status;

                try {
                    

                    //naptien
                    if (_transaction.Type == TransactionType.WalletRecharge)
                    {
                        retValue = await CalculateBalance(_transaction.Id, _transaction.Amount, _transaction.SenderId, _transaction.ReceiverId, "[Nạp Tiền][WalletRecharge]", username);
                    }
                    else if (_transaction.Type == TransactionType.WalletWithdraw) //ruttien
                    {
                        retValue = await CalculateBalance(_transaction.Id, _transaction.Amount, _transaction.SenderId, _transaction.ReceiverId, "[Rút tiền][WalletWithdraw]", username);
                    }
                    else if(_transaction.Type == TransactionType.CampaignAccountCharge)
                    {
                        retValue = await CalculateBalance(_transaction.Id, _transaction.Amount, _transaction.SenderId, _transaction.ReceiverId, "[Phí thành viên][CampaignAccountCharge]", username);
                    }
                    else if (_transaction.Type == TransactionType.CampaignServiceCharge)
                    {
                        retValue = await CalculateBalance(_transaction.Id, _transaction.Amount, _transaction.SenderId, _transaction.ReceiverId, "[Phí dịch vụ][CampaignServiceCharge]", username);
                    }
                    else if (_transaction.Type == TransactionType.CampaignAccountPayback)
                    {
                        retValue = await CalculateBalance(_transaction.Id, _transaction.Amount, _transaction.SenderId, _transaction.ReceiverId, "[Chiến dịch trả thành viên][CampaignAccountPayback]", username);
                    }

                    _ITransactionRepository.Update(_transaction);

                }
                catch { }
                                
            }            
            return retValue;
        }

        public async Task<int> CalculateBalance(int transactionid, long transactionAmount, int senderid, int receiverid, string header_log, string username)
        {

            /*
             * 09: success
             * 10: wallet do not exist
             * 11: wallet balance sender or receiver less then zero or amount could be abstract
             * 
             */

            int retValue = -1;
            var walletSender = _IWalletRepository.GetById(senderid);// get wallet sender
            var walletRecevier = _IWalletRepository.GetById(receiverid); // get wallet receiver
            long newamount_sender = 0;
            long newamount_receiver = 0;

            if (walletSender != null && walletRecevier != null)
            {

                //trừ ví người gửi
                long _old_sender_wallet_balance = walletSender.Balance;
                //walletSender.Balance = walletSender.Balance - transactionAmount;
                //walletSender.DateModified = DateTime.Now;
                //_IWalletRepository.Update(walletSender);
                newamount_sender = await _IWalletRepository.Exchange(senderid, 0 - transactionAmount, username);




                //cộng ví người nhận
                long _old_receiver_wallet_balance = walletRecevier.Balance;
                //walletRecevier.Balance = walletRecevier.Balance + transactionAmount;
                //walletRecevier.DateModified = DateTime.Now;
                //_IWalletRepository.Update(walletRecevier);
                newamount_receiver = await _IWalletRepository.Exchange(receiverid, transactionAmount, username);

                if (newamount_sender > 0 && newamount_receiver > 0)
                {
                    //tạo transactionhistory trừ ví người gửi
                    string _note = string.Format("{4} - Trừ ví người gửi: walletid = {0}, amount = {1}, old_sender_wallet_balance={2}, transactionId={3}", walletSender.Id, transactionAmount, _old_sender_wallet_balance, transactionid, header_log);
                    await _ITransactionRepository.UpdateTransactionHistory(transactionid, walletSender.Id, 0 - transactionAmount, _old_sender_wallet_balance, _note);
                    //tạo transactionhistory cộng ví người nhận
                    _note = string.Format("{4} - Cộng ví người nhận: walletid = {0}, amount = {1}, _old_receiver_wallet_balance={2}, transactionId={3}", walletRecevier.Id, transactionAmount, _old_receiver_wallet_balance, transactionid, header_log);
                    await _ITransactionRepository.UpdateTransactionHistory(transactionid, walletRecevier.Id, 0 - transactionAmount, _old_receiver_wallet_balance, _note);

                    retValue = 9;
                }
                else
                {
                    retValue = 11;
                }




            }
            else
            {
                retValue = 10;
            }

            return retValue;
        }

    }
}
