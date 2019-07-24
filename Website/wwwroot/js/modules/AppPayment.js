
var AppPayment = (function () {

    function init() {
        $('.btn-payment').click(function (e) {
            e.preventDefault();
            var campaignid = $(this).data('id');
            AppBsModal.Init('static');
            AppBsModal.OpenRemoteModal(AppConstants.UrlAgencyPayment(campaignid));
        });
    }

    function handlerPayment() {
        $.validator.unobtrusive.parse($('#frmPayment'));
        $('#frmPayment').submit(function (e) {
            e.preventDefault();
            var isvalid = $(this).valid();
            if (isvalid) {
                var url = $(this).data('action');
                var data = $(this).serialize();
                AppBsModal.ShowLoading();
                $.post(url, data, function (html) {
                    AppBsModal.OpenModal(html);
                });
            }
        });
    }
    function handlerMessage() {
        AppCommon.handlerBtnReload();
    }
    return {
        Init: init,
        HandlerPayment: handlerPayment,
        HandlerMessage: handlerMessage
    };

})();