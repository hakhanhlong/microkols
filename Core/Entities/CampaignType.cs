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
        ShareContent,
        [DisplayName("Chia sẻ thông điệp, viết thêm caption")]
        ShareContentWithCaption,
        [DisplayName("Thay hình Avatar")]
        ChangeAvatar,
        [DisplayName("Viết comment seeding cho chiến dịch")]
        PostComment,
        [DisplayName("Đăng ký tham dự sự kiện")]
        JoinEvent,
        [DisplayName("Share link livestream chương trình")]
        ShareStreamUrl,
        [DisplayName("Yêu cầu khác")]
        CustomService
    }
    public static class CampaignTypeExentions {
        public static string ToPriceLabel(this CampaignType type)
        {
            if(type== CampaignType.ShareContent || type == CampaignType.ShareContentWithCaption || type == CampaignType.PostComment || type == CampaignType.ShareStreamUrl)
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
            return type.ToString();
        }
    }
}
