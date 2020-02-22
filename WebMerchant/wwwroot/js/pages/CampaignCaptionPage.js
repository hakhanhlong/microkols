var CampaignCaptionPage = (function () {

    function init() {
        handler();
    }

    function handler() {
        

        $('.cb-checkall').click(function () {
            $('.cb-checkitem').prop('checked', this.checked);
            handlerVisibleCheckItem();
        });

        $('.cb-checkitem').change(function () {
            handlerVisibleCheckItem();
        });


        handlerVisibleCheckItem()


    }

    function handlerVisibleCheckItem() {
        var cb = '.cb-checkitemChuaDuyet:checked';// '.cb-checkitemMatchAccount:checked';


        var length = $(cb).length;
        if (length > 0) {
            $('.frmFeedbackAll').show();

            var html = '';
            $(cb).each(function () {
                var val = $(this).val();
                html += '<input type="hidden" name="ids" value="' + val + '" />';
            }).promise().done(function () {
                console.log('html', html);
                $('.frmFeedbackAll-ids').html(html);
            });
            

        } else {
            $('.frmFeedbackAll-ids').html('');
            $('.frmFeedbackAll').hide();
        }
     
    }


    return {
        Init: init
    };

})();
