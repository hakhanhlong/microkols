using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Core.Entities
{
    public class CampaignTypePrice : BaseEntity
    {
        public CampaignType Type { get; set; }
        public int ServicePrice { get; set; }
        public int AccountPrice { get; set; }
        public int AccountExtraPricePercent { get; set; }

    }

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
            if(type== CampaignType.CustomService)
            {
                return "";
            }
            return type.ToString();
        }
    }
}
