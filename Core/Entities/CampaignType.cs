using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Entities
{
    public enum CampaignType
    {
        [Display(Name ="Chia sẻ thông điệp, không cần viết caption", Description = "" , ShortName = "/img/1.jpg")]
        ShareContent = 1,
        [Display(Name ="Chia sẻ thông điệp, viết thêm caption", Description = "", ShortName = "/img/2.jpg")]
        ShareContentWithCaption = 2,
        [Display(Name ="Thay hình Avatar", Description = "", ShortName = "/img/3.jpg")]
        ChangeAvatar = 3,
        [Display(Name ="Viết comment seeding cho chiến dịch", Description ="", ShortName = "/img/4.jpg")]
        PostComment = 4,
        [Display(Name ="Đăng ký tham dự sự kiện", Description = "", ShortName = "/img/5.jpg")]
        JoinEvent = 5,
        [Display(Name ="Share link livestream chương trình", Description = "", ShortName = "/img/6.jpg")]
        ShareStreamUrl = 6,
        [Display(Name ="Yêu cầu khác", Description = "", ShortName = "/img/7.png")]
        CustomService = 7
    }
    public static class CampaignTypeExentions
    {
        public static bool IsShareCampaign(this CampaignType type)
        {
            return type == CampaignType.ShareContent || type == CampaignType.ShareContentWithCaption || type == CampaignType.ShareStreamUrl;

        }


        public static string ToText(this CampaignType type)
        {
            if (type == CampaignType.ShareContent)
            {
                return "Chia sẻ thông điệp, không cần viết caption";
            }
            if (type == CampaignType.ShareContentWithCaption)
            {
                return "Chia sẻ thông điệp, viết thêm caption";
            }
            if (type == CampaignType.ChangeAvatar)
            {
                return "Thay hình Avatar";
            }

            if (type == CampaignType.PostComment)
            {
                return "Viết comment seeding cho chiến dịch";
            }

            if (type == CampaignType.JoinEvent)
            {
                return "Đăng ký tham gia sự kiện";
            }

            if (type == CampaignType.ShareStreamUrl)
            {
                return "Share link livestream chương trình";
            }

            if (type == CampaignType.CustomService)
            {
                return "Yêu cầu khác";
            }

            return type.ToString();
        }

        public static string ToActionText(this CampaignType type)
        {
            if (type == CampaignType.ShareContent || type == CampaignType.ShareContentWithCaption)
            {
                return "Chia sẻ";
            }
            if (type == CampaignType.PostComment)
            {
                return "Viết bình luận";
            }
            if (type == CampaignType.ShareStreamUrl)
            {
                return "Chia sẻ link Stream";
            }

            if (type == CampaignType.ChangeAvatar)
            {
                return "Thay hình Avatar";
            }

            if (type == CampaignType.JoinEvent)
            {
                return "Đăng ký tham gia sự kiện";
            }

            if (type == CampaignType.CustomService)
            {
                return "Thực hiện ngay";
            }

            return type.ToString();
        }
        public static string ToPriceLabel(this CampaignType type)
        {
            if (type == CampaignType.ShareContent || type == CampaignType.ShareContentWithCaption || type == CampaignType.PostComment || type == CampaignType.ShareStreamUrl)
            {
                return "/người/lần";
            }

            if (type == CampaignType.ChangeAvatar)
            {
                return "/người/tuần";
            }
            if (type == CampaignType.ShareContent || type == CampaignType.ShareContentWithCaption)
            {
                return "/người/lần";
            }
            if (type == CampaignType.CustomService || type == CampaignType.JoinEvent)
            {
                return "";
            }
            return type.ToString();
        }

        public static string ToDataText(this CampaignType type)
        {
           
            if (type == CampaignType.ChangeAvatar)
            {
                return "Hình ảnh Avatar";
            }
            if (type == CampaignType.ShareContent || type == CampaignType.ShareContentWithCaption)
            {
                return "Link nội dung";
            }
          

            if (type == CampaignType.PostComment )
            {
                return "Link để bình luận";
            }
            if ( type == CampaignType.ShareStreamUrl)
            {
                return "Link stream";
            }

            if ( type == CampaignType.JoinEvent)
            {
                return "Link sự kiện";
            }

            if (type == CampaignType.CustomService)
            {
                return "";
            }
            return type.ToString();
        }
    }

}
