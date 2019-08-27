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
    }
}
