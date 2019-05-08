
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

            if (count != currentcount) {
                getNotificationDropdown();
            }
        });

        if ($('.notif-uncheck').length == 0) {
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
        Init: init,
    };
})();
