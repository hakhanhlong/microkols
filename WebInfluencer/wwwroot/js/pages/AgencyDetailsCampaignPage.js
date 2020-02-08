
var AgencyDetailsCampaignPage = (function () {

    function init() {


        handler();
    }
    function handler() {
        $('.btn-campaignaccount').click(function () {
            var url = $(this).data('url');
            AppBsModal.Init('static');
            AppBsModal.OpenRemoteModal(url, function () {
                handlerCampaignAccount();
            });
        });
    }


    function handlerCampaignAccount() {

        $('.frm-requestjoin').submit(function () {
            var $frm = $(this);

            var data = $frm.serialize();
            var url = $frm.data('action');
            var $frmbtn = $frm.find('.btn-requestjoin');
            var $i = $frmbtn.find('i');
            $frmbtn.prop('disabled', true);
            $i.removeClass('fa-plus').addClass('fa-spinner fa-spin');

            $.post(url, data, function (res) {
                if (res == 1) {

                    $frmbtn.removeClass('btn-outline-primary').addClass('btn-outline-success');
                    $i.removeClass('fa-spinner fa-spin').addClass('fa-check');
                } else {

                    $frmbtn.prop('disabled', false);
                    $frmbtn.removeClass('btn-outline-primary').addClass('btn-outline-danger');
                    $i.removeClass('fa-spinner fa-spin').addClass('fa-plus');
                }
                handlerReloadBtn(true);
            })



        })

        //$('.btn-requestjoin').click(function () {
        //    var $this = $(this);
        //    var $i = $(this).find('i');
        //    $this.prop('disabled', true);
        //    $i.removeClass('fa-plus').addClass('fa-spinner fa-spin');
        //    var url = $(this).data('url');
        //    $.get(url, function (res) {
        //        $this.removeClass('btn-outline-primary').addClass('btn-outline-success');
        //        $i.removeClass('fa-spinner fa-spin').addClass('fa-check');
        //        handlerReloadBtn(true);
        //    });
        //});
        handlerReloadBtn();

    }

    function handlerReloadBtn(force) {
        $('.btn-reload').unbind('click');
        $('.btn-reload').click(function (e) {
            if (force) {
                window.location = window.location;
            } else {
                AppBsModal.HideModal();

            }
        });
    }
    return {
        Init: init
    };

})();
