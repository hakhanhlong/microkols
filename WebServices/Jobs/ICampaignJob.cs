using System.Threading.Tasks;

namespace WebServices.Jobs
{
    public interface ICampaignJob
    {
        Task UpdateCompletedCampagin(int campaignid = 0);
        Task UpdateCampaignAccountExpired();

        Task UpdateCampaignProcess();

        Task UpdateCampaignStart();
        Task UpdateCampaignEnd();
    }
}