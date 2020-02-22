using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebServices.ViewModels
{

    public class GroupCampaignAccountCaptionViewModel
    {

        public GroupCampaignAccountCaptionViewModel()
        {

        }
        public GroupCampaignAccountCaptionViewModel(IEnumerable<CampaignAccountCaption> campaignAccountCaptions)
        {
            var query = campaignAccountCaptions.OrderByDescending(m => m.DateModified);
            var lastcamption = query.FirstOrDefault();
            if (lastcamption != null)
            {

                LastCaption = new CampaignAccountCaptionViewModel(lastcamption);
            }

            Captions = CampaignAccountCaptionViewModel.GetList(query);
        }


        public static List<GroupCampaignAccountCaptionViewModel> GetList(IEnumerable<CampaignAccountCaption> campaignAccountCaptions)
        {
            var result = new List<GroupCampaignAccountCaptionViewModel>();
            var ids = campaignAccountCaptions.OrderByDescending(m => m.DateModified).Select(m => m.CampaignAccountId).Distinct();

            foreach(var itemid in ids)
            {

                var itemCampaignAccountCaptions = campaignAccountCaptions.Where(m => m.CampaignAccountId == itemid);

                if (itemCampaignAccountCaptions.Any())
                {
                    result.Add(new GroupCampaignAccountCaptionViewModel(itemCampaignAccountCaptions));
                }
                
            }

            return result;
        }
        public CampaignAccountCaptionViewModel LastCaption { get; set; }
        public List<CampaignAccountCaptionViewModel> Captions { get; set; }
    }

    public class ListGroupCampaignAccountCaptionViewModel
    { 

        public ListGroupCampaignAccountCaptionViewModel(IEnumerable<CampaignAccountCaption> campaignAccountCaptions, int page, int pagesize, int total)
        {
            CampaignGroupAccountCaptions = GroupCampaignAccountCaptionViewModel.GetList(campaignAccountCaptions);
            Pager = new PagerViewModel(page, pagesize, total);

        }

        public List<GroupCampaignAccountCaptionViewModel> CampaignGroupAccountCaptions { get; set; }
        public PagerViewModel Pager { get; set; }
    }
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
            Id = campaignAccountCaption.Id;
            AccountId = campaignAccountCaption.CampaignAccount.AccountId;

        }
        public static List<CampaignAccountCaptionViewModel> GetList(IEnumerable<CampaignAccountCaption> campaignAccountCaptions)
        {
            return campaignAccountCaptions.Select(m => new CampaignAccountCaptionViewModel(m)).ToList();
        }
        public int AccountId { get; set; }
        public int Id { get; set; }
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
