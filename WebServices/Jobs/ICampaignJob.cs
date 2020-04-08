using System.Threading.Tasks;

namespace WebServices.Jobs
{
    public interface ICampaignJob
    {
        Task UpdateCompletedCampagin(int campaignid = 0);

        //################## addition by longgk ########

        Task CheckLockedCampagin();

        //##############################################


        Task UpdateCampaignAccountExpired();

        Task UpdateCampaignProcess();

        Task UpdateCampaignStart();
        Task UpdateCampaignEnd();
    }
}