
var AppBsModal = (function () {
    var selectorId = '#appbsmodal';

    function showLoading() {
        $(selectorId).html(AppConstants.ModalSpinner);
    }

    function hideModal() {
        $(selectorId).modal('hide');
    }
    function openModal(html, callback) {

        $(selectorId).html(html);
        if (callback && typeof callback === "function") {
            callback();
        }
        $(selectorId).modal('show');
    }
    function openRemoteModal(url, callback) {
        showLoading();
        $(selectorId).modal('show');
        $.get(url, function (html) {
            $(selectorId).html(html);
            if (callback && typeof callback === "function") {
                callback();
            }
        });
    }
    function init() {
        removeModal();
        var html = '<div id="appbsmodal" class="modal"></div>';
        $('body').append(html);
        $(selectorId).on('hidden.bs.modal', function (e) {
            removeModal();
        });
        //$('#appbsmodal').on('show.bs.modal', function (e) {
        //    showLoading();
        //});
    }

    function removeModal() {
        $(selectorId).remove();
    }
    return {
        Init: init,
        OpenModal: openModal,
        OpenRemoteModal: openRemoteModal,
        ShowLoading: showLoading,
        HideModal: hideModal
    };
})();

var AppNotification = (function () {

    function init() {
        if (AppSettings.IsAuthenticated) {

            handler();
            getNotificationCount();
        }

    }

    function handler() {

    }

    function getNotificationCount() {


        var currentcount = parseInt($('#header-notif-count').text());

        $.get(AppUrls.NotificationCount, function (count) {

            $('.notif-count').html(count);

            if (count !== currentcount) {
                getNotificationDropdown();
            }
        });

        if ($('.notif-uncheck').length === 0) {
            $('.notif-checkedall').hide();
        }

        $('.notif-checkedall').click(function () {
            var url = $(this).data('url');
            $.post(url, function () {
                $('.notif-item').addClass('notif-checked');
                $('.notif-checkedall').hide();
            });
        });

        handleCheckAll();

    }

    function handleCheckAll() {
        $('.notif-checkedall').unbind('click');
        $('.notif-checkedall').click(function () {
            var url = $(this).data('url');
            $.post(url, function () {
                $('.notif-item').addClass('notif-checked');
                $('.notif-checkedall').hide();
                $('.notif-count').text('0');
            });
        });
    }
    function getNotificationDropdown() {

        $.get(AppUrls.NotificationPartial, function (html) {
            $('.dropdown-menu-notif').html(html);
            handleCheckAll();
        });


    }


    return {
        Init: init
    };
})();

var AppWallet = (function () {


    function init() {
        $('.wallet-recharge').click(function (e) {
            e.preventDefault();

            AppBsModal.Init();
            AppBsModal.OpenRemoteModal(AppConstants.UrlRecharge);
        });

        $('.wallet-widthdraw').click(function (e) {
            e.preventDefault();

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


var AppPayment = (function () {

    function init() {
        $('.btn-apayment').click(function (e) {
            e.preventDefault();
            var campaignid = $(this).data('campaignid');
            AppBsModal.Init();
            AppBsModal.OpenRemoteModal(AppConstants.UrlAgencyPayment(campaignid));
        });
    }
    return {
        Init: init
    };

})();