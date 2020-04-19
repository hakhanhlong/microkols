using Common.Extensions;
using Core.Entities;
using Core.Extensions;
using Core.Interfaces;
using Core.Specifications;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebServices.Interfaces;
using WebServices.ViewModels;

namespace WebServices.Services
{
    public class PaymentService : BaseService, IPaymentService
    {
        private readonly ILogger<TransactionService> _logger;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICampaignRepository _campaignRepository;
        private readonly IWalletRepository _walletRepository;
        private readonly ICampaignAccountRepository _campaignAccountRepository;


        public PaymentService(ILogger<TransactionService> logger,
             ICampaignAccountRepository campaignAccountRepository,
            IWalletRepository walletRepository, ICampaignRepository campaignRepository,
           ITransactionRepository transactionRepository)
        {
            _logger = logger;
            _walletRepository = walletRepository;
            _campaignRepository = campaignRepository;
            _transactionRepository = transactionRepository;
            _campaignAccountRepository = campaignAccountRepository;
        }

        public async Task<bool> IsExistPaymentServiceCashBack(int agencyId,int campaignid)
        {
            return true;
        }
        public async Task<PaymentResultViewModel> CreateAgencyPayment(int agencyId, CreateCampaignPaymentViewModel model, string username)
        {
            var payment = await _campaignRepository.GetCampaignPaymentByAgency(agencyId, model.CampaignId);
            if (payment == null)
            {
                return new PaymentResultViewModel(PaymentResultErrorCode.ThongTinThanhToanKhongChinhXac);
            }
            long amount = 0;
            var senderId = 0;
            var receiverId = 0;
            var transactionType = TransactionType.Undefined;
            var refId = payment.CampaignId;
            var refData = string.Empty;

            if (payment.IsValid)
            {
                //service charge --> tru tien cho agency ; + tien cho he thong
                receiverId = await _walletRepository.GetSystemId();
                senderId = await _walletRepository.GetWalletId(Core.Entities.EntityType.Agency, agencyId);
                amount = payment.TotalChargeValue;

                transactionType = amount > 0 ? TransactionType.CampaignServiceCharge : TransactionType.CampaignServiceCashBack;
                if (transactionType == TransactionType.CampaignServiceCharge)
                {
                    return await Pay(senderId, receiverId, amount, transactionType, model.Note, username, refId, refData);
                }

                // nếu là yêu cầu rút tiền -- Đổi sender - recivert -> số tiền dương
                var transactionid = await _transactionRepository.CreateTransaction(receiverId, senderId, 0 - amount, transactionType, model.Note, string.Empty, username, refId, refData);
                return new PaymentResultViewModel(PaymentResultErrorCode.ChoHeThongDuyetRutTien, transactionid, 0 - amount); // anh Longhk bổ xung
            }

            return new PaymentResultViewModel(PaymentResultErrorCode.ThongTinThanhToanKhongChinhXac);
        }

        public async Task<PaymentResultViewModel> Pay(int senderId, int receiverId, long amount, TransactionType transactionType,
            string note, string username, int refId, string refData)
        {
            var transactionid = await _transactionRepository.CreateTransaction(senderId, receiverId, amount, transactionType, note,
                string.Empty, username, refId, refData);

            string logText = $"CreatePayment -> {transactionid}|{senderId}|{receiverId}|{amount}";
            _logger.LogInformation($"{logText} -> Start");

            var transactionStatus = TransactionStatus.Processing;
            var errorCode = PaymentResultErrorCode.KhongLoi;
            var transactionNote = "";
            try
            {
                //tru tien
                var senderBalance = await _walletRepository.Exchange(senderId, 0 - amount, username);
                _logger.LogInformation($"{logText} -> SenderExchange {senderBalance}");
                if (senderBalance > 0)
                {
                    await _transactionRepository.UpdateTransactionHistory(transactionid, senderId, 0 - amount, senderBalance, $"");
                    // cong tien
                    var receiverBalance = await _walletRepository.Exchange(receiverId, amount, username);
                    _logger.LogInformation($"{logText} -> ReceiverExchange {senderBalance}");

                    if (receiverBalance > 0)
                    {
                        await _transactionRepository.UpdateTransactionHistory(transactionid, receiverId, amount, receiverBalance, $"");
                        await _transactionRepository.UpdateTransactionStatus(transactionid, TransactionStatus.Completed, string.Empty, username);

                        return new PaymentResultViewModel()
                        {
                            Amount = amount,
                            ReceiverBalance = receiverBalance,
                            SenderBalance = senderBalance,
                            Status = TransactionStatus.Completed,
                            ErrorCode = PaymentResultErrorCode.KhongLoi,
                            TransactionId = transactionid,
                            Type = transactionType
                        };
                    }
                    else
                    {
                        errorCode = PaymentResultErrorCode.CongTienLoi;
                    }
                }
                else if (senderBalance == -2)
                {
                    errorCode = PaymentResultErrorCode.KhongDuTien;
                }
                else
                {
                    errorCode = PaymentResultErrorCode.TruTienLoi;
                }
            }
            catch (Exception ex)
            {
                transactionStatus = TransactionStatus.Error;
                if (errorCode == PaymentResultErrorCode.KhongLoi)
                {
                    errorCode = PaymentResultErrorCode.KhongXacDinh;
                }
                transactionNote = ex.Message;
            }
            _logger.LogInformation($"{logText} -> End -> {transactionStatus}|{transactionNote}");
            await _transactionRepository.UpdateTransactionStatus(transactionid, transactionStatus, transactionNote, username);

            return new PaymentResultViewModel(errorCode);
        }


        public async Task<bool> VerifyPaybackCampaignAccount(int campaignid)
        {
            var totalServiceCharge = await _transactionRepository.GetTotalAmount(TransactionType.CampaignAccountCharge, campaignid);
            var totalPayBack = await _transactionRepository.GetTotalAmount(TransactionType.CampaignAccountPayback, campaignid);

            return (totalPayBack <= totalServiceCharge);

        }
        public async Task<PaymentResultViewModel> CreatePaybackCampaignAccount(int campaignid, int accountid, string username)
        {
            var campaign = await _campaignRepository.GetByIdAsync(campaignid);
            if (campaign == null)
            {
                return new PaymentResultViewModel(PaymentResultErrorCode.ThongTinThanhToanKhongChinhXac);
            }
            var campaignAccount = await _campaignAccountRepository.GetSingleBySpecAsync(new CampaignAccountByAccountSpecification(accountid, campaignid));


            if (campaignAccount == null || campaignAccount.Status != CampaignAccountStatus.Finished)
            {
                return new PaymentResultViewModel(PaymentResultErrorCode.ThongTinThanhToanKhongChinhXac);
            }

            long amount = campaign.GetAccountChagreAmount(campaignAccount);
            var senderId = await _walletRepository.GetSystemId();
            var receiverId = await _walletRepository.GetWalletId(Core.Entities.EntityType.Account, campaignAccount.AccountId);
            var transactionType = TransactionType.CampaignAccountPayback;
            var refId = campaignAccount.CampaignId;
            var refData = campaignAccount.Id.ToString();


            if (amount > 0 && senderId > 0 && receiverId > 0 && transactionType != TransactionType.Undefined)
            {
                return await Pay(senderId, receiverId, amount, transactionType, string.Empty, username, refId, refData);
            }


            return new PaymentResultViewModel(senderId == 0 || receiverId == 0 ? PaymentResultErrorCode.ThongTinThanhToanKhongChinhXac :

                PaymentResultErrorCode.ThongTinThanhToanKhongChinhXac);
        }

    }
}
