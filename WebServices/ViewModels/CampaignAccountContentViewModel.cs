using Common.Extensions;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebServices.ViewModels
{


    public class GroupCampaignAccountContentViewModel
    {

        public GroupCampaignAccountContentViewModel()
        {

        }
        public GroupCampaignAccountContentViewModel(IEnumerable<CampaignAccountContent> campaignAccountContents)
        {
            var query = campaignAccountContents.OrderByDescending(m => m.DateModified);
            var lastcamption = query.FirstOrDefault();
            if (lastcamption != null)
            {

                LastContent = new CampaignAccountContentViewModel(lastcamption);
            }

            Contents = CampaignAccountContentViewModel.GetList(query);
        }


        public static List<GroupCampaignAccountContentViewModel> GetList(IEnumerable<CampaignAccountContent> campaignAccountContents)
        {
            var result = new List<GroupCampaignAccountContentViewModel>();
            var ids = campaignAccountContents.OrderByDescending(m => m.DateModified).Select(m => m.CampaignAccountId).Distinct();

            foreach (var itemid in ids)
            {

                var itemCampaignAccountContents = campaignAccountContents.Where(m => m.CampaignAccountId == itemid);

                if (itemCampaignAccountContents.Any())
                {
                    result.Add(new GroupCampaignAccountContentViewModel(itemCampaignAccountContents));
                }

            }

            return result;
        }
        public CampaignAccountContentViewModel LastContent { get; set; }
        public List<CampaignAccountContentViewModel> Contents { get; set; }
    }

    public class ListGroupCampaignAccountContentViewModel
    {

        public ListGroupCampaignAccountContentViewModel(IEnumerable<CampaignAccountContent> campaignAccountContents, int page, int pagesize, int total)
        {
            CampaignGroupAccountContents = GroupCampaignAccountContentViewModel.GetList(campaignAccountContents);
            Pager = new PagerViewModel(page, pagesize, total);

        }

        public List<GroupCampaignAccountContentViewModel> CampaignGroupAccountContents { get; set; }
        public PagerViewModel Pager { get; set; }
    }
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
            AccountId = CampaignAccountContent.CampaignAccount.AccountId;
            Note = CampaignAccountContent.Note;
            Image = CampaignAccountContent.Image.ToListString();
            Id = CampaignAccountContent.Id;
        }
        public static List<CampaignAccountContentViewModel> GetList(IEnumerable<CampaignAccountContent> CampaignAccountContents)
        {
            return CampaignAccountContents.Select(m => new CampaignAccountContentViewModel(m)).ToList();
        }
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string Content { get; set; }
        public List<string> Image { get; set; }
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
        public int CampaignId { get; set; }
        public List<string> Image { get; set; }
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
