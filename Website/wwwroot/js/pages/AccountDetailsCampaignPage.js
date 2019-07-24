
var AccountDetailsCampaignPage = (function () {

    function init() {


        handler();
    }
    function handler() {
        $('.btn-updateref').click(function () {
            var url = $(this).data('url');
            AppBsModal.Init('static');
            AppBsModal.OpenRemoteModal(url, function () {
                handlerUpdateRef();
            });
        });


        $('.btn-shareui').click(function () {
            AppBsModal.Init('static');
            AppBsModal.OpenModal('');
            AppBsModal.ShowLoading();
            var href = $(this).data('href');
            var urlsubmit = $(this).data('urlsubmit');
            //var caption = $(this).data('caption');

            FB.ui(
                {
                    method: 'share',
                    href: href
                    //quote: caption,
                },
                function (response) {
                    if (response && !response.error_message) {
                        $.post(urlsubmit, function (html) {
                            AppBsModal.OpenModal(html, function () { AppCommon.handlerBtnReload(); });
                        });
                    } else {
                        AppBsModal.HideModal();
                    }
                });
        });

    }
    function handlerUpdateRef() {
        $.validator.unobtrusive.parse($('#frmUpdateCampaignAccountRef'));
        $('#frmUpdateCampaignAccountRef').submit(function (e) {
            e.preventDefault();
            var isvalid = $(this).valid();
            if (isvalid) {
                var url = $(this).data('action');
                var data = $(this).serialize();
                AppBsModal.ShowLoading();
                $.post(url, data, function (html) {
                    AppBsModal.OpenModal(html, function () { AppCommon.handlerBtnReload(); });

                });
            }
        });
    }

    return {
        Init: init
    };

})();
