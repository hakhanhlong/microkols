
var AppWallet = (function () {


    function init() {
        $('.wallet-recharge').click(function (e) {
            var campaignid = $(this).data('campaignid');
            e.preventDefault();

            AppBsModal.Init();
            AppBsModal.OpenRemoteModal(AppConstants.UrlRecharge(campaignid), function () {
                handlerRecharge();
            });
        });

        $('.wallet-withdraw').click(function (e) {
            e.preventDefault();
            AppBsModal.Init();
            AppBsModal.OpenRemoteModal(AppConstants.UrlWithdraw, function () {
                handlerWithdraw();
            });

        });
    }

    function handlerWithdraw() {
        $.validator.unobtrusive.parse($('#frmWithDraw'));
        $('#frmWithDraw').submit(function (e) {
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


    function handlerRecharge() {
        $.validator.unobtrusive.parse($('#frmRecharge'));
        $('#frmRecharge').submit(function (e) {
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
        handlerMethod();
        $('#frmRecharge input[name="Method"]').change(function () {
            handlerMethod();
        });
        handlerBank();
        $('#Bank').change(function () {
            handlerBank();
        });
    }

    function handlerBank() {

        $(".bankinfo").addClass('d-none');
        var val = $('#Bank').val();
        $('#bankinfo' + val).removeClass('d-none');
    }
    function handlerMethod() {
        var val = $('#frmRecharge input[name="Method"]:checked').val();
        console.log('handlerMethod', val);
        if (val === 'Chuyển khoản') {
            $('#bankWrap').removeClass('d-none');
        } else {
            $('#bankWrap').addClass('d-none');
        }
    }

    return {
        Init: init,
        HandlerRecharge: handlerRecharge
    };
})();
