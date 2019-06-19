
var AppSettings = {
    IsAuthenticated: false,
    CurrentUser: {
        Username: '',
        Name: '',
        Type:-1
    },
};


var AppConstants = {
    UrlUploadFile: "/home/uploadimage",
    UrlGetAmount: "/wallet/GetAmount",
    UrlRecharge: "/wallet/Recharge",
    UrlWithdraw: "/wallet/Withdraw",
    UrlGetNotificationCount: "/Notification/Count",
    UrlUpdateNotificationChecked: "/Notification/UpdateChecked",
    UrlGetNotification: "/Notification/IndexPartial",
    UrlUploadTempImage: "/Home/UploadImage",

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
    handlerBtnReload() {
        $('.btn-reload').click(function (e) {
            window.location = window.location;
        });
    },
    uploadTempImage: function (files,  callback) {
        var xhr, formData;
        xhr = new XMLHttpRequest();
        xhr.withCredentials = false;
        xhr.open('POST', AppConstants.UrlUploadTempImage);
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
            $('.wallet-balance').html(AppCommon.moneyFormat(val));
        });
    },
    moneyFormat: function (input, n, x) {
        var re = '\\d(?=(\\d{' + (x || 3) + '})+' + (n > 0 ? '\\.' : '$') + ')';

        return input.toFixed(Math.max(0, ~~n)).replace(new RegExp(re, 'g'), '$&,') + ' đ';
    },
    toggleAffix: function () {

        var toggleAffix = function (affixElement, scrollElement, wrapper) {

            var height = affixElement.outerHeight(),
                top = wrapper.offset().top;

            if (scrollElement.scrollTop() >= top) {
                wrapper.height(height);
                affixElement.addClass("affix");
            }
            else {
                affixElement.removeClass("affix");
                wrapper.height('auto');
            }

        };


        $('[data-toggle="affix"]').each(function () {
            var ele = $(this),
                wrapper = $('<div></div>');

            ele.before(wrapper);
            $(window).on('scroll resize', function () {
                toggleAffix(ele, $(this), wrapper);
            });

            // init
            toggleAffix(ele, $(window), wrapper);
        });
    }
};
