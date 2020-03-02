
var CampaignCreateTargetPage = (function () {

    function init() {
        handler();
    }

    function handler() {

        $('#CategoryId').select2({
            maximumSelectionLength: 3,
            theme: "bootstrap"
        });

        $('#RegisterTime').daterangepicker({
            timePicker: true,
            minDate: moment(),
            startDate: moment(),
            endDate: moment().startOf('hour').add(10, 'hour'),
            locale: {
                format: 'hh:mm A DD/MM/YYYY'
            }
        });

        $('#FeedbackBefore').daterangepicker({
            timePicker: true,
            minDate: moment(),
            startDate: moment(),
            endDate: moment().startOf('hour').add(10, 'hour'),
            locale: {
                format: 'hh:mm A DD/MM/YYYY'
            }
        });

        $('#ExecutionTime').daterangepicker({
            timePicker: true,
            minDate: moment(),
            startDate: moment(),
            endDate: moment().startOf('hour').add(10, 'hour'),
            locale: {
                format: 'hh:mm A DD/MM/YYYY'
            }
        });


        /*
        $('#AccountIds').select2({
            theme: "bootstrap",
            ajax: {
                url: AppConstants.UrlGetAccounts,
                data: function (params) {
                    var accouttype = $('input[name=AccountType]:checked').val();

                    var query = {
                        kw: params.term,
                        type: accouttype
                    }
                    // Query parameters will be ?search=[term]&type=public
                    return query;
                },
                dataType: 'json',
                processResults: function (data) {
                    return {
                        results: $.map(data, function (item) {
                            return {
                                text: item.name,
                                id: item.id
                            }
                        })
                    };
                }
              

            }
        });
        */

        handlerAccountType();
        $('input[name=AccountType]').change(function () {

            handlerAccountType();
        });


        $('#modal-influencer-selection').on('shown.bs.modal', function (e) {
            // do something...
            $('#modal-influencer-selection .frm-search').submit();
        });


        $('#modal-influencer-selection .frm-search').submit(function () {
            var url = $(this).data('action');

            var parram = $(this).serialize(0);
            $('#list-influencer').html(AppConstants.HtmlSpinner);
            $.post(url, parram, function (html) {

                $('#list-influencer').html(html);
                handlerSearchInfluencer();
            });

        });
    }

    function handlerSearchInfluencer() {

        var $target = $('#list-influencer');

        $target.find('.cb-checkall').click(function () {
            $('.cb-checkitem').prop('checked', this.checked);
            handlerVisibleCheckItem($target);
        });

        $target.find('.cb-checkitem').change(function () {      
            handlerVisibleCheckItem($target);
        });

        var $btnsubmit = $('#btn-submitInfluencer');

        $btnsubmit.hide();

        $btnsubmit.unbind('click');
        $btnsubmit.click(function () {

           

            var html = '';
            var $cb = $('#list-influencer .cb-checkitem:checked');
            $cb.each(function () {
                var val = $(this).val(); 
                var name = '';// $(this).data('name'); 
                html += '<span><input type="hidden" name="AccountIds" value="' + val + '" />' + name + '</span>';
            }).promise().done(function () {
                console.log('html', html);
                $('#AccountIdsArea').append(html);
                $('.badge-influencer-count').html($('#AccountIdsArea span').length);
                $('#modal-influencer-selection').modal('hide');
            });


        });

    }

    function handlerVisibleCheckItem($target) {
        var cb = '.cb-checkitem:checked';// '.cb-checkitemMatchAccount:checked';


        var length = $target.find(cb).length;
        if (length > 0) {
            $('#btn-submitInfluencer').show();
        } else {
            $('#btn-submitInfluencer').hide();
        }

    }



    function handlerAccountType() {
        var accouttype = $('input[name=AccountType]:checked').val();
        console.log('accouttype', accouttype);
        $('.d-accounttype').hide();
        $('.d-accounttype-' + accouttype).show();
        $('.h-accounttype').show();
        $('.h-accounttype-' + accouttype).hide();

    }




    return {
        Init: init
    };

})();
