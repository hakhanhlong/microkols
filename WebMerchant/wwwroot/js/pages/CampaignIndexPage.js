var CampaignIndexPage = (function () {

    function init() {
        handler();
    }

    function handler() {
        $('.modal-accountcampaign').on('hidden.bs.modal', function () {
            // do something…
            $(this).find('.list-campaignaccount').html('loading...');
        });

        $('.modal-accountcampaign').on('shown.bs.modal', function () {
            // do something…
            var $target = $(this).find('.list-campaignaccount');
            getAccountCampaign($target);
        });
    }




    function getAccountCampaign($target, url) {


        var url = url ? url : $target.data('url');
        console.log('url', url);
        $target.html(AppConstants.HtmlSpinner);
        $.get(url, function (html) {

            $target.html(html);
            handlerAccountCampaign($target);
        })
    }
    function handlerAccountCampaign($target) {


        $target.find('.page-link').click(function (e) {
            e.preventDefault();

            var url = $(this).attr('href');
            getAccountCampaign($target, url);
        });

    }

    return {
        Init: init
    };

})();
