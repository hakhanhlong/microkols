var ChangeAccountTypePage = (function () {

    function init() {
        var datas = $.parseJSON($('#modelHotMomData').html());
        new Vue({
            el: '#dataHotMom',
            data: {
                childrens: []
            },
            created() {
                this.childrens = datas;
            },
            methods: {
                createItem: function () {
                    this.childrens.push({
                        Gender: 1,
                        Age: 0,
                        AgeType: 2
                    });
                },
                removeItem: function (idx) {
                    this.data.splice(idx, 1);
                }

            }
        });
        handlerType();
        $("input[name=Type]").change(function () {

            handlerType();
        });
    }


    function handlerType() {
        var type = $("input[name=Type]:checked").val();
        if (type === 'HotMom') {
            $('#dataHotMom').removeClass('d-none');
        } else {
            $('#dataHotMom').addClass('d-none');
        }
    }
    return {
        Init: init
    };

})();


var CreateCampaignPage = (function () {

    function init() {
        handlerType();
        $('#Type').change(function () {
            handlerType();
        });

        $('#campaignOptions .form-option').change(function () {

            pricingCalculator();

        })


    }


    function handlerType() {
        var type = $('#Type').val();
        console.log('type', type);
        $('#enabledExtraTypeWrap,#requirementWrap').addClass('d-none');
        var selectedOption = $('#Type').children('option:selected');
        var accountpricetext = selectedOption.data('accountpricetext');

        $('#editAccountPriceWrap span').text(accountpricetext);
        $('#editAccountPriceWrap').removeClass('openedit');

        if (type === 'ShareContent' || type === 'ShareContentWithCaption') {
            $('#enabledExtraTypeWrap').removeClass('d-none');
        }
        else if (type === 'CustomService') {
            $('#requirementWrap').removeClass('d-none');
            $('#editAccountPriceWrap').addClass('openedit');
        }
        else if (type === 'JoinEvent') {
            $('#editAccountPriceWrap').addClass('openedit');
        }


        pricingCalculator();
    }

    function pricingCalculator() {
        var type = $('#Type').val();
        var selectedOption = $('#Type').children('option:selected');
        var serviceprice = selectedOption.data('serviceprice');
        var accountprice = selectedOption.data('accountprice');
        var accountextrapricepercent = selectedOption.data('accountextrapricepercent');
        

        if (type === 'CustomService' || type === 'JoinEvent') {
            accountprice = $('#AccountPrice').val();
        }
        //else if (type === 'ShareContent' || type === 'ShareContentWithCaption') {
        //}

        var numberOfAccount = $('#NumberOfAccount').val();
        var countOption = $('#campaignOptions .form-option:checked').length;

        var settingExtraOptionCharge = $('#settingExtraOptionCharge').val();
        var settingServiceCharge = $('#settingServiceCharge').val();

        var totalServicePrice = serviceprice + (countOption * settingExtraOptionCharge * serviceprice / 100);
        var serviceCharge = totalServicePrice * settingServiceCharge / 100;
        var totalServiceCharge = totalServicePrice + serviceCharge;

        console.log('totalServicePrice', totalServicePrice, 'serviceCharge', serviceCharge, 'totalServiceCharge', totalServiceCharge, 'countOption', countOption);

        $('#totalServicePrice').text(AppCommon.moneyFormat(totalServicePrice));
        $('#serviceCharge').text(AppCommon.moneyFormat(serviceCharge));
        $('#totalServiceCharge').text(AppCommon.moneyFormat(totalServiceCharge));

    }
    return {
        Init: init
    };

})();