
var AppNotification = (function () {

    var $notifCount = $('.nav-notif .badge');
    var $notifDropdown = $('.nav-notif .dropdown-menu');
    function init() {
        handler();
        getNotificationCount();

    }

    function handler() {

        getNotificationDropdown();

        $('.nav-notif').on('show.bs.dropdown', function () {

            updateNotificationChecked();
        });
    }

    function getNotificationCount() {


        var currentcount = parseInt($notifCount.text());

        $.get(AppConstants.UrlGetNotificationCount, function (count) {

            $notifCount.text(count);
            if (count === 0) {

                $('.nav-notif .badge').hide();
            } else {

                $('.nav-notif .badge').show();
            }

            if (count !== currentcount) {
                getNotificationDropdown();
            }

            setTimeout(function () {
                getNotificationCount();
            }, 5000);
        });

    }

    function updateNotificationChecked() {
        $.post(AppConstants.UrlUpdateNotificationChecked, function () {


            $('.nav-notif .badge').hide();
            $('.nav-notif .badge').text('0');
        });
    }
    function getNotificationDropdown() {

        $.get(AppConstants.UrlGetNotification, function (html) {
            $notifDropdown.html(html);
            //handleCheckAll();
        });


    }


    return {
        Init: init
    };
})();
