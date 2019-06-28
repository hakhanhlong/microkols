﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Core.Entities
{
    public enum CampaignType
    {
        [DisplayName("Chia sẻ thông điệp, không cần viết caption")]
        ShareContent = 1,
        [DisplayName("Chia sẻ thông điệp, viết thêm caption")]
        ShareContentWithCaption = 2,
        [DisplayName("Thay hình Avatar")]
        ChangeAvatar = 3,
        [DisplayName("Viết comment seeding cho chiến dịch")]
        PostComment = 4,
        [DisplayName("Đăng ký tham dự sự kiện")]
        JoinEvent = 5,
        [DisplayName("Share link livestream chương trình")]
        ShareStreamUrl = 6,
        [DisplayName("Yêu cầu khác")]
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
