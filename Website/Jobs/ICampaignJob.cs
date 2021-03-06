﻿using System.Threading.Tasks;

namespace Website.Jobs
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