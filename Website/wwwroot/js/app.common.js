
var AppSettings = {
    IsAuthenticated: false,
    CurrentUser: {
        Username: '',
        Name: ''
    },
};


var AppConstants = {
    UrlUploadFile: "/home/uploadimage",
    UrlGetAmount: "/wallet/GetAmount",
    UrlRecharge: "/wallet/Recharge",
    UrlWithdraw: "/wallet/Withdraw",
    UrlGetDistricts: function (cityid) { return "/home/GetDistricts?cityid=" + cityid; },
    UrlAgencyPayment: function (campaignid) { return "/AgencyPayment/CampaignPayment?campaignid=" + campaignid; },
    ModalSpinner: '<div class="modal-dialog modal-dialog-centered"><div class="modal-content"><div class="py-5 text-center text-success loading"><i class="fas fa-spinner fa-spin"></i></div> </div></div>'

};




var AppCommon = {
    loadScrollTop: function () {
        $(window).scroll(function () {
            if ($(this).scrollTop() > 500) {
                $('#stickyFooterActions').fadeIn();
            } else {
                $('#stickyFooterActions').fadeOut();
            }
        });

        $('#scrollToTop').click(function () {
            $('html, body').animate({ scrollTop: 0 }, 800);

        });

    },
    uploadTempImage: function (files, path, callback) {
        var xhr, formData;

        xhr = new XMLHttpRequest();
        xhr.withCredentials = false;
        xhr.open('POST', AppUrls.ImageMultipleUpload);

        xhr.onload = function () {
            if (xhr.status !== 200) {
                failure('HTTP Error: ' + xhr.status);
                return;
            }
            var json = JSON.parse(xhr.responseText);
            callback(json);
        };
        formData = new FormData();
        for (var i = 0; i < files.length; i++) {
            formData.append('files', files[i]);
        }


        xhr.send(formData);
    },
    bindingWalletBalance: function () {
        $.get(AppConstants.UrlGetAmount, function (val) {
            $('.wallet-balance').html(val);
        });
    }
};
