using System.Threading.Tasks;

namespace Website.Jobs
{
    public interface ICampaignJob
    {
        Task UpdateCompletedCampagin(int campaignid = 0);
    }
}