﻿using Common.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Entities
{
    public enum CampaignType
    {
        //[Display(Name = "Chia sẻ thông điệp, không cần viết caption", Description = "", ShortName = "/img/share_no_caption.png")]
        //ShareContent = 1,
        [Display(Name = "Chia sẻ thông điệp, viết thêm caption", Description = "", ShortName = "/img/share_with_caption.png")]
        ShareContentWithCaption = 2,
        [Display(Name = "Thay hình Avatar", Description = "", ShortName = "/img/share_change_avatar.png")]
        ChangeAvatar = 3,
        [Display(Name = "Viết Review một sản phẩm hoặc tham gia trải nghiệm", Description = "", ShortName = "/img/share_review_product.png")]
        ReviewProduct = 4,
        [Display(Name = "Tham gia sự kiện và check in", Description = "", ShortName = "/img/share_joinevent_checkin.png")]
        JoinEvent = 5,
        [Display(Name = "Share link livestream chương trình", Description = "", ShortName = "/img/share_link_livestream.png")]
        ShareStreamUrl = 6,
        [Display(Name = "Yêu cầu khác", Description = "", ShortName = "/img/share_other.png")]
        CustomService = 7
    }


    public static class CampaignTypeExentions
    {
        public static bool IsShareCampaign(this CampaignType type)
        {
            //return type == CampaignType.ShareContent || type == CampaignType.ShareContentWithCaption || type == CampaignType.ShareStreamUrl;
            return  type == CampaignType.ShareContentWithCaption || type == CampaignType.ShareStreamUrl;

        }
        public static bool IsHasAccountProcess(this CampaignStatus status)
        {
            return status != CampaignStatus.Canceled && status != CampaignStatus.Ended && status != CampaignStatus.Completed;
        }
        public static bool IsHasCaption(this CampaignType type)
        {
            return type == CampaignType.ShareContentWithCaption;
        }

        public static bool IsHasContent(this CampaignType type)
        {
            return type == CampaignType.ReviewProduct;
        }



        public static int GetKpiMin(this CampaignType type)
        {
            //if (type == CampaignType.ShareContent)
            //{
            //    return 30;
            //}

            if (type == CampaignType.ShareContentWithCaption)
            {
                return 100;
            }
            if (type == CampaignType.ReviewProduct)
            {
                return 200;
            }

            return 0;
        }

        public static int GetInteractiveMin(this CampaignType type)
        {
            //if (type == CampaignType.ShareContent)
            //{
            //    return 0;
            //}
            if (type == CampaignType.ShareContentWithCaption)
            {
                return 100;
            }
            if (type == CampaignType.ReviewProduct)
            {
                return 200;
            }

            return 0;
        }


        public static string ToText(this CampaignType type)
        {
            return type.ToDisplayName();
        }

        public static string ToActionText(this CampaignType type)
        {
            //if (type == CampaignType.ShareContent || type == CampaignType.ShareContentWithCaption)
            //{
            //    return "Chia sẻ";
            //}

            if (type == CampaignType.ShareContentWithCaption)
            {
                return "Chia sẻ";
            }

            if (type == CampaignType.ReviewProduct)
            {
                return "Review Sản phẩm";
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
            //if (type == CampaignType.ShareContent || type == CampaignType.ShareContentWithCaption || type == CampaignType.ReviewProduct || type == CampaignType.ShareStreamUrl)
            //{
            //    return "/người/lần";
            //}

            if (type == CampaignType.ShareContentWithCaption || type == CampaignType.ReviewProduct || type == CampaignType.ShareStreamUrl)
            {
                return "/người/lần";
            }

            if (type == CampaignType.ChangeAvatar)
            {
                return "/người/tuần";
            }
            //if (type == CampaignType.ShareContent || type == CampaignType.ShareContentWithCaption)
            //{
            //    return "/người/lần";
            //}

            if (type == CampaignType.ShareContentWithCaption)
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
                return "Hình ảnh dùng làm Avatar";
            }
            //if (type == CampaignType.ShareContent || type == CampaignType.ShareContentWithCaption)
            //{
            //    return "Link nội dung";
            //}

            if (type == CampaignType.ShareContentWithCaption)
            {
                return "Link nội dung";
            }




            if (type == CampaignType.ReviewProduct)
            {
                return "Link sản phẩm/dịch vụ";
            }
            if (type == CampaignType.ShareStreamUrl)
            {
                return "Link livestream";
            }

            if (type == CampaignType.JoinEvent)
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
