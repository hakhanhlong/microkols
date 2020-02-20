var CampaignIndexPage = (function () {

    function init() {
        handler();
    }

    function handler() {
        $('.modal-accountcampaign').on('hidden.bs.modal', function () {
            // do something…
            $(this).find('.list-campaignaccount').html('');
        });

        $('.modal-accountcampaign').on('shown.bs.modal', function () {
            // do something…
            var $this = $(this);
            var $target = $this.find('.list-campaignaccount');
            getList($target);


            $this.find('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
                var type = $(e.target).data('type');
                if (type == 1) {

                    var $target = $this.find('.list-campaignaccount');
                    getList($target);
                } else {

                    var $target2 = $this.find('.list-matchedaccount');
                    getList($target2);
                }
            })

        });



       

    }

    function handlerVisibleCheckItem($target) {
        console.log('$target2', $target)
        var cb = '.cb-checkitemAccountRequest:checked';// '.cb-checkitemMatchAccount:checked';


        var length = $target.find(cb).length;
        if (length > 0) {
            $target.find('.frmFeedbackAll').show();

            var html = '';
            $target.find(cb).each(function () {
                var val = $(this).val();
                html += '<input type="hidden" name="accountid" value="' + val + '" />';
            }).promise().done(function () {
                console.log('html', html);
                $target.find('.frmFeedbackAll-ids').html(html);
            });
            

        } else {
            $target.find('.frmFeedbackAll-ids').html('');
            $target.find('.frmFeedbackAll').hide();
        }
     
    }


    function getList($target, url) {


        var url = url ? url : $target.data('url');
        console.log('url', url);
        $target.html(AppConstants.HtmlSpinner);
        $.get(url, function (html) {

            $target.html(html);
            handlerAccountCampaign($target);
        })
    }


    function handlerAccountCampaign($target) {

        handlerFrm($target);

        $target.find('.page-link').click(function (e) {
            e.preventDefault();

            var url = $(this).attr('href');
            getList($target, url);
        });

        $target.find('.cb-checkall').click(function () {
            $('.cb-checkitem').prop('checked', this.checked);
            handlerVisibleCheckItem($target);
        });

        $target.find('.cb-checkitem').change(function () {
            handlerVisibleCheckItem($target);
        });

        handlerVisibleCheckItem($target);

    }

    function handlerFrm($target) {
        var form = $target.find(".frmFeedback");

        $(":submit", form).click(function () {
            if ($(this).attr('name')) {
                $(form).append(
                    $("<input type='hidden'>").attr({
                        name: $(this).attr('name'),
                        value: $(this).attr('value')
                    })
                );
            }
        });

        $(form).submit(function (e) {

            console.log('submit');
            e.preventDefault();

            $target.html(AppConstants.HtmlSpinner);
            var action = $(this).data('action');

            console.log('handlerAccountCampaign', action);
            var data = $(this).serialize();
            $.post(action, data, function (html) {
                $target.html(html);
                handlerAccountCampaign($target);
            });
        });
    }

    return {
        Init: init
    };

})();
