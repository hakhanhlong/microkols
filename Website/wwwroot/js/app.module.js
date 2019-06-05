
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
    function init(backdrop) {
        removeModal();

        if (backdrop === undefined) {
            backdrop = true;
        }
        var html = '<div id="appbsmodal" class="modal" data-backdrop="' + backdrop + '"></div>';
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

    var $notifCount = $('.nav-notif .badge');
    var $notifDropdown = $('.nav-notif .dropdown-menu');
    function init() {
        handler();
        getNotificationCount();

    }

    function handler() {

        getNotificationDropdown();
    }

    function getNotificationCount() {


        var currentcount = parseInt($notifCount.text());

        $.get(AppConstants.UrlGetNotificationCount, function (count) {

            $notifCount.text(count);

            if (count !== currentcount) {
                getNotificationDropdown();
            }
        });

        //if ($('.notif-uncheck').length === 0) {
        //    $('.notif-checkedall').hide();
        //}

        //$('.notif-checkedall').click(function () {
        //    var url = $(this).data('url');
        //    $.post(url, function () {
        //        $('.notif-item').addClass('notif-checked');
        //        $('.notif-checkedall').hide();
        //    });
        //});

        //handleCheckAll();

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

        $.get(AppConstants.UrlGetNotification, function (html) {
            $notifDropdown.html(html);
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