
var AccountDetailsCampaignPage = (function () {

    function init() {

        handler();
    }
    function handler() {
        handlerAction();

    }

    function handlerAction() {
        $('.btn-updateref').click(function () {
            var url = $(this).data('url');
            AppBsModal.Init('static');
            AppBsModal.OpenRemoteModal(url, function () {
                handlerUpdateRef();
            });
        });
        $('.btn-updaterefimages').click(function () {
            var url = $(this).data('url');
            AppBsModal.Init('static');
            AppBsModal.OpenRemoteModal(url, function () {
                handlerUpdateRefImages();
            });
        });

        $('.btn-shareui').click(function () {
            AppBsModal.Init('static');
            AppBsModal.OpenModal('');
            AppBsModal.ShowLoading();
            var href = $(this).data('href');
            var urlsubmit = $(this).data('urlsubmit');
            var title = $(this).data('title');
            var picture = $(this).data('picture');
            var description = $(this).data('description');
            var caption = $(this).data('caption');
            
            //var caption = $(this).data('caption');

            FB.ui(
                {
                    method: 'share',
                    href: href,
                    //quote: caption,
                    title: title,  // The same than name in feed method
                    picture: picture,
                    caption: caption,
                    description: description,
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
    function handlerUpdateRefImages() {
        $.validator.unobtrusive.parse($('#frmUpdateCampaignAccountRefImages'));
       
        $('#addonImages').change(function () {
            var id = $(this).attr('id');
            var target = $(this).data('target');
            var files = document.getElementById(id).files;

            AppCommon.uploadTempImage(files, function (datas) {
                datas.forEach(function (item) {
                    var html = '<img src="' + item.url + '"  class="img-thumbnail mt-2" style="max-height:400px" /><input type="hidden" name="RefImage" value="' + item.path + '" />';
                    $(target).append(html);
                })

            });

        });
        $('#frmUpdateCampaignAccountRefImages').submit(function (e) {
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
        Init: init,
        HandlerAction: handlerAction
    };

})();
