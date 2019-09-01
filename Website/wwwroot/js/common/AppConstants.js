var AppConstants = {
    UrlUploadFile: "/home/uploadimage",
    UrlGetAmount: "/wallet/GetAmount",
    UrlRecharge: function (campaignid) {
        if (!campaignid) {
            campaignid = 0;
        }
        return "/wallet/Recharge?campaignid=" + campaignid;
    },
    UrlWithdraw: "/wallet/Withdraw",
    UrlGetNotificationCount: "/Notification/Count",
    UrlUpdateNotificationChecked: "/Notification/UpdateChecked",
    UrlGetNotification: "/Notification/IndexPartial",
    UrlUploadTempImage: "/Home/UploadImage",
    UrlGetAccounts: "/AgencyCampaign/GetAccounts",
    UrlGetDistricts: function (cityid) { return "/home/GetDistricts?cityid=" + cityid; },
    UrlAgencyPayment: function (campaignid) { return "/AgencyPayment/CampaignPayment?campaignid=" + campaignid; },
    ModalSpinner: '<div class="modal-dialog modal-dialog-centered"><div class="modal-content"><div class="py-5 text-center text-success loading"><i class="fas fa-spinner fa-spin"></i></div> </div></div>',
    HtmlSpinner: '<div class="py-5 text-center text-success loading"><i class="fas fa-spinner fa-spin"></i></div>'
};

