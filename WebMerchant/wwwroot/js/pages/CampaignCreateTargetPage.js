
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
                /*
                processResults: function (data) {

                    var list = [];

                    data.forEach(function (item) {
                        list.push(new {
                            id: item.id,
                            text: item.name
                            
                        })
                    });
                    console.log('123', list);
                    return {
                        results: list
                    };
                }
                */

            }
        });
        handlerAccountType();
        $('input[name=AccountType]').change(function () {

            handlerAccountType();
        });
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
