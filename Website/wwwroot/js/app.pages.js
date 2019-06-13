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

var AgencyCreateCampaignPage = (function () {

    function init() {
        handlerType();
        $('#Type').change(function () {
            handlerType();
        });

        $('#campaignOptions .form-option').change(function () {

            pricingCalculator();

        });
    }


    function handlerType() {
        var type = $('#Type').val();
        console.log('type', type);
        $('#enabledExtraTypeWrap,#requirementWrap,#changeAvatarWrap').addClass('d-none');
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
        else if (type === 'ChangeAvatar') {
            $('#changeAvatarWrap').removeClass('d-none');
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


        $('.btn-requestjoin').click(function () {
            var $this = $(this);
            var $i = $(this).find('i');
            $this.prop('disabled', true);
            $i.removeClass('fa-plus').addClass('fa-spinner fa-spin');
            var url = $(this).data('url');
            $.get(url, function (res) {
                $this.removeClass('btn-outline-primary').addClass('btn-outline-success');
                $i.removeClass('fa-spinner fa-spin').addClass('fa-check');

                handlerReloadBtn(true);
            });


        });
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


var AccountDetailsCampaignPage = (function () {

    function init() {


        handler();
    }
    function handler() {
        $('.btn-updateref').click(function () {
            var url = $(this).data('url');
            AppBsModal.Init('static');
            AppBsModal.OpenRemoteModal(url, function () {
                handlerUpdateRef();
            });
        });

    }
    function handlerUpdateRef() {

    }


    
    return {
        Init: init
    };

})();