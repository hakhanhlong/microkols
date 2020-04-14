using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackOffice.Business.Interfaces
{
    public interface INotificationBusiness
    {
        Task CreateNotificationCampaignByStatus(int campaignid, int entityid, NotificationType notificationType, string msg, string text);

        Task CreateNotificationTransactionDepositeByStatus(int transaction, int agency_entityid, NotificationType notificationType, string msg, string text);

        Task CreateNotificationExcecutedPaymentToAccountBanking(int campaignid, int entityid, NotificationType notificationType, string msg, string text);

        Task CreateNotificationAccountVerify(int accountid, int entityid, NotificationType notificationType, string msg, string text);

        Task CreateNotificationTransactionByStatus(int transactionid, int agency_entityid, NotificationType notificationType, string msg, string text);



    }
}
