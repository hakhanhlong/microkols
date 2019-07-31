var AgencyCreateCampaignPage = (function () {

    function init() {
        handler();
    }

    function handler() {
        $('#dataImageFile').change(function () {
            var id = $(this).attr('id');
            var target = $(this).data('target');
            var preview = $(this).data('preview');
            var files = document.getElementById(id).files;

            AppCommon.uploadTempImage(files, function (datas) {
                if (datas.length > 0) {
                    $(target).val(datas[0].path);
                    $(target).trigger("change");
                    $(preview).attr('src', datas[0].url);
                }
            });

        });

        $('#btnCreateCampaign').click(function () {

            $('#submittype').val(1);
            createCampaign(function (response) {
                setTimeout(function () {
                    window.location = response.url;
                }, 1000);


            });
        });


        $('#btnCreateCampaignRecharge').click(function () {

            $('#submittype').val(0);
            createCampaign(function (response) {
                AppBsModal.OpenRemoteModal(AppConstants.UrlRecharge(response.campaignid), function () {
                    AppWallet.HandlerRecharge();
                });
            });
        });

        handlerType();
        $('#Type').change(function () {
            handlerType();
        });

        handlerAccountType();
        $('input[name=AccountType]').change(function () {
            handlerAccountType();
        });


        /*
        $('#frmCreateCampaign input,#frmCreateCampaign select').change(function () {
            console.log('input change');
          
            pricingCalculator();
        });
        */

        $('#campaignOptions .form-option').change(function () {
            pricingCalculator();
        });


        $('.btn-suggestaccount').click(function () {

            suggestAccount();
        });

    }
    function createCampaign(callback) {


        var $frm = $('#frmCreateCampaign');

        console.log('createCampaign');
        if ($frm.valid()) {
            AppBsModal.Init('static');
            AppBsModal.ShowLoading();
            var action = $frm.data('action');
            var formdata = $frm.serialize();
            $.post(action, formdata, function (response) {
                if (response.status == -1) {

                    AppBsModal.HideModal();
                    $.notify({
                        message: response.message,
                        type: 'danger'
                    })
                } else {
                    callback(response);

                }

            });

        }
    }

    function handlerAccountType() {
        var accouttype = $('input[name=AccountType]:checked').val();
        console.log('accouttype', accouttype);
        if (accouttype === 'Regular') {
            $('.d-withoutRegular').addClass('d-none');
            $('#actionWrap').removeClass('d-none');
            $('.d-withRegular').removeClass('d-none');
        } else {

            $('.d-withoutRegular').removeClass('d-none');
            $('.d-withRegular').addClass('d-none');

            if ($('#suggestAccount tr').length > 1) {
                $('#actionWrap').removeClass('d-none');
            } else {

                $('#actionWrap').addClass('d-none');
            }

        }

            
    }

    function handlerType() {
        var type = $('#Type').val();
        console.log('type', type);
        $('#enabledExtraTypeWrap,#requirementWrap,#changeAvatarWrap').addClass('d-none');
        var selectedOption = $('#Type').children('option:selected');
        var accountpricetext = selectedOption.data('accountpricetext');
        var datatext = selectedOption.data('datatext');
        $('#dataText').text(datatext);
        if (type === 'ChangeAvatar') {

            $('#dataInput').addClass('d-none');
            $('#dataImage').removeClass('d-none');
        } else {

            $('#dataInput').removeClass('d-none');
            $('#dataImage').addClass('d-none');
        }

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

    function suggestAccount() {

        var url = $('#suggestAccount').data('url');
        var urlparams = '';

        var accountType = $('input[name=AccountType]:checked').val();
        urlparams += '&accountTypes=' + accountType;
        var enabledGender = $('input[name=EnabledGender]:checked').val();
        if (enabledGender === "true") {
            urlparams += '&gender=' + $('input[name=Gender]:checked').val();
        }

        var enabledAgeRange = $('input[name=EnabledAgeRange]:checked').val();
        if (enabledAgeRange === "true") {
            urlparams += '&agestart=' + $('#AgeStart').val() + '&ageend=' + $('#AgeEnd').val();
        }

        var enabledCity = $('input[name=EnabledCity]:checked').val();
        if (enabledCity === "true") {
            urlparams += '&cityid=' + $('#CityId').val();
        }

        var enabledCategory = $('input[name=EnabledCategory]:checked').val();
        if (enabledCategory === "true") {
            var categoryids = $('#CategoryId').val();
            for (var i = 0; i < categoryids.length; i++) {
                urlparams += '&categoryid=' + categoryids[i];
            }
        }

        urlparams += '&pagesize=' + $('#Quantity').val();
        urlparams += '&campaignType=' + $('#Type').val();


        urlparams += '&min=' + $('#amountMin').val();
        urlparams += '&max=' + $('#amountMax').val();


        console.log('urlparams', urlparams);

        $('#suggestAccount').html(AppConstants.HtmlSpinner);
        $.get(url + '?' + urlparams, function (html) {
            $('#suggestAccount').html(html);

            handlerSuggestAccount();
        });
    }

    function handlerSuggestAccount() {

        var renewUrl = $('#tblMatchedAccounts').data('renewurl');
        $('.btn-renewaccount').unbind('click');
        $('.btn-renewaccount').click(function () {
            var $tr = $(this).closest('tr');
            var ignoreids = '';
            $('.form-accountid').each(function () {
                ignoreids += '&ignoreids=' + $(this).val();
            }).promise().done(function () {

                $.get(renewUrl + ignoreids, function (html) {
                    if (html.length < 100) {
                        $.notify('Hệ thóng không có thành viên khác phù hợp các tiêu chí');
                    } else {
                        $tr.replaceWith(html);
                        handlerSuggestAccount();
                    }
                });
            });
        });

        handlerAccountType();
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


        $('#totalServicePrice').text(AppCommon.moneyFormat(totalServicePrice));
        $('#serviceCharge').text(AppCommon.moneyFormat(serviceCharge));
        $('#totalServiceCharge').text(AppCommon.moneyFormat(totalServiceCharge));


        //console.log('AppSettings.CurrentUser.Balance', AppSettings.CurrentUser.Balance, 'totalServicePrice', totalServicePrice, 'serviceCharge', serviceCharge, 'totalServiceCharge', totalServiceCharge, 'countOption', countOption);
        setTimeout(function () {
            if (AppSettings.CurrentUser.Balance <= totalServiceCharge) {

                $('#btnCreateCampaignRecharge').show();
                $('#btnCreateCampaign').hide();

            } else {
                $('#btnCreateCampaignRecharge').hide();
                $('#btnCreateCampaign').show();

            }
        }, 1000);


    }


    return {
        Init: init
    };

})();
