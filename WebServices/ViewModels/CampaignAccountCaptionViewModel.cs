using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebServices.ViewModels
{
    public class CampaignAccountCaptionViewModel
    {
        public CampaignAccountCaptionViewModel()
        {

        }
        public CampaignAccountCaptionViewModel(CampaignAccountCaption campaignAccountCaption)
        {
            Content = campaignAccountCaption.Content;
            Status = campaignAccountCaption.Status;
            DateCreated = campaignAccountCaption.DateCreated;
            DateModified = campaignAccountCaption.DateModified;
            UserCreated = campaignAccountCaption.UserCreated;
            UserModified = campaignAccountCaption.UserModified;
            Note = campaignAccountCaption.Note;
        }
        public static List<CampaignAccountCaptionViewModel> GetList(IEnumerable<CampaignAccountCaption> campaignAccountCaptions)
        {
            return campaignAccountCaptions.Select(m => new CampaignAccountCaptionViewModel(m)).ToList();
        }
        public string Content { get; set; }
        public string Note { get; set; }
        public CampaignAccountCaptionStatus Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string UserCreated { get; set; }
        public string UserModified { get; set; }
    }

    public class CreateCampaignAccountCaptionViewModel
    {
        public int CampaignAccountId { get; set; }
        public int CampaignId { get; set; }
        public string Content { get; set; }
    }

    public class EditCampaignAccountCaptionViewModel
    {
        public EditCampaignAccountCaptionViewModel()
        {

        }
        public EditCampaignAccountCaptionViewModel(CampaignAccountCaption campaignAccountCaption)
        {
            Id = campaignAccountCaption.Id;
            Content = campaignAccountCaption.Content;
            Note = campaignAccountCaption.Note;

        }
        public int Id { get; set; }
        public string Content { get; set; }
        public string Note { get; set; }

    }
    public class UpdateNoteCampaignAccountCaptionViewModel
    {
        public int Id
        {
            get; set;
        }
        public string Note { get; set; }
    }


    public class ListCampaignAccountCaptionViewModel
    {
        public ListCampaignAccountCaptionViewModel()
        {

        }
        public ListCampaignAccountCaptionViewModel(IEnumerable<CampaignAccountCaption> campaignAccountCaptions, int page, int pagesize, int total)
        {
            CampaignAccountCaptions = CampaignAccountCaptionViewModel.GetList(campaignAccountCaptions);
            Pager = new PagerViewModel(page, pagesize, total);

        }

        public List<CampaignAccountCaptionViewModel> CampaignAccountCaptions { get; set; }
        public PagerViewModel Pager { get; set; }
    }

}
