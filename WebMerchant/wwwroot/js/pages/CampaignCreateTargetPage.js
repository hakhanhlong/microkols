
var CampaignCreateTargetPage = (function () {

    function init() {
        handler();

        console.log('CampaignCreateTargetPage');
    }

    function handler() {

        $('#CategoryId').select2({
            maximumSelectionLength: 3,
            theme: "bootstrap"
        });

        var defaultHour = 10;

        $('#FeedbackBefore').daterangepicker({
            timePicker: true,
            minDate: moment(),
            startDate: moment(),
            endDate: moment().startOf('hour').add(defaultHour, 'hour'),
            locale: {
                format: 'hh:mm A DD/MM/YYYY'
            }
        });

        $('#RegisterTime').daterangepicker({
            timePicker: true,
            minDate: moment(),
            startDate: moment(),
            endDate: moment().startOf('hour').add(defaultHour, 'hour'),
            locale: {
                format: 'hh:mm A DD/MM/YYYY'
            }
        });
        $('#RegisterTime').on('apply.daterangepicker', function (ev, picker) {
            
            var startDate = moment(picker.endDate).add(1, 'minute');
            var endDate = moment(picker.endDate).add(defaultHour, 'hour');
            console.log('start11',startDate);
            console.log('end11',endDate);
            $('#ExecutionTime').data('daterangepicker').setStartDate(startDate);
            $('#ExecutionTime').data('daterangepicker').setEndDate(endDate);

        });
        $('#ExecutionTime').daterangepicker({
            timePicker: true,
            minDate: moment(),
            startDate: moment().startOf('hour').add(defaultHour, 'hour').add(1, 'minute'),
            endDate: moment().startOf('hour').add(2 * defaultHour, 'hour'),
            locale: {
                format: 'hh:mm A DD/MM/YYYY'
            }
        });
        $('#ExecutionTime').on('apply.daterangepicker', function (ev, picker) {

            var regDrp = $('#RegisterTime').data('daterangepicker');
            var d1 = moment(regDrp.endDate);

            var d2 = moment(picker.startDate);

            if (d1.isBefore(d2)) // false
            {

            } else {

                $.notify({
                    // options
                    message: 'Thời gian thực hiện phải lớn hơn thời gian nhận đăng ký'
                }, {
                    // settings
                    type: 'danger'
                });
            }


            var startDate = d1.add(1, 'minute'); 
            $('#ExecutionTime').data('daterangepicker').setStartDate(startDate); 

        });

        $('#FeedbackBefore2').daterangepicker({
            timePicker: true,
            locale: {
                format: 'hh:mm A DD/MM/YYYY'
            }
        });
        $('#RegisterTime2').daterangepicker({
            timePicker: true,
            locale: {
                format: 'hh:mm A DD/MM/YYYY'
            }
        });
        $('#ExecutionTime2').daterangepicker({
            timePicker: true,
            locale: {
                format: 'hh:mm A DD/MM/YYYY'
            }
        });

        $('.form-create-campaign').submit(function (e) {


            var regDrp = $('#RegisterTime').data('daterangepicker');
            var excDrp = $('#ExecutionTime').data('daterangepicker');
            console.log('regDrp', regDrp.startDate, regDrp.endDate);
            console.log('excDrp', excDrp.startDate, excDrp.endDate);

            var d1 = moment(regDrp.endDate);
            var d2 = moment(excDrp.startDate);
            if (d1.isBefore(d2)) // false
            {
                
            } else {
                e.preventDefault();
            }

          
        });


        $('#AmountMin').change(function () {

            var val = $('#AmountMin').val();

            console.log('amount min', val);

            var valmax = $('#AmountMax').val();
            if (valmax < val) {
                $('#AmountMax').val(val);
            }
            $('#AmountMax').attr('min', val);



        })


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
