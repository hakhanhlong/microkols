using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebServices.ViewModels
{
    public class CampaignAccountContentViewModel
    {
        public CampaignAccountContentViewModel()
        {

        }
        public CampaignAccountContentViewModel(CampaignAccountContent CampaignAccountContent)
        {
            Content = CampaignAccountContent.Content;
            Status = CampaignAccountContent.Status;
            DateCreated = CampaignAccountContent.DateCreated;
            DateModified = CampaignAccountContent.DateModified;
            UserCreated = CampaignAccountContent.UserCreated;
            UserModified = CampaignAccountContent.UserModified;
            Note = CampaignAccountContent.Note;
        }
        public static List<CampaignAccountContentViewModel> GetList(IEnumerable<CampaignAccountContent> CampaignAccountContents)
        {
            return CampaignAccountContents.Select(m => new CampaignAccountContentViewModel(m)).ToList();
        }
        public string Content { get; set; }
        public string Note { get; set; }
        public CampaignAccountContentStatus Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string UserCreated { get; set; }
        public string UserModified { get; set; }
    }

    public class CreateCampaignAccountContentViewModel
    {
        public int CampaignAccountId { get; set; }
        public string Content { get; set; }
    }

    public class EditCampaignAccountContentViewModel
    {
        public EditCampaignAccountContentViewModel()
        {

        }
        public EditCampaignAccountContentViewModel(CampaignAccountContent CampaignAccountContent)
        {
            Id = CampaignAccountContent.Id;
            Content = CampaignAccountContent.Content;
            Note = CampaignAccountContent.Note;

        }
        public int Id { get; set; }
        public string Content { get; set; }
        public string Note { get; set; }

    }
    public class UpdateNoteCampaignAccountContentViewModel
    {
        public int Id
        {
            get; set;
        }
        public string Note { get; set; }
    }


    public class ListCampaignAccountContentViewModel
    {
        public ListCampaignAccountContentViewModel()
        {

        }
        public ListCampaignAccountContentViewModel(IEnumerable<CampaignAccountContent> campaignAccountContents, int page, int pagesize, int total)
        {
            CampaignAccountContents = CampaignAccountContentViewModel.GetList(campaignAccountContents);
            Pager = new PagerViewModel(page, pagesize, total);

        }

        public List<CampaignAccountContentViewModel> CampaignAccountContents { get; set; }
        public PagerViewModel Pager { get; set; }
    }

}
