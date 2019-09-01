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


        $('#addonImages').change(function () {
            var id = $(this).attr('id');
            var target = $(this).data('target');
            var files = document.getElementById(id).files;

            AppCommon.uploadTempImage(files, function (datas) {
                datas.forEach(function (item) {
                    var html = '<div class="addonimage"><span class="remove"><i class="fal fa-times"></i></span> <img src="' + item.url + '" id="imagePreview" class="img-thumbnail mt-2" style="" /><input type="hidden" name="AddonImages" value="' + item.path + '" /></div>';
                    $(target).append(html);
                });

                $('.addonimage .remove').unbind('click');
                $('.addonimage .remove').click(function () {
                    $(this).closest('.addonimage').remove();
                });
               
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

        //$('#ExecutionTime').daterangepicker({
        //    timePicker: true,
        //    minDate: moment().startOf('hour').add(5, 'hour'),
        //    startDate: moment().startOf('hour').add(5, 'hour'),
        //    endDate: moment().startOf('hour').add(10, 'hour'),
        //    locale: {
        //        format: 'hh:mm A DD/MM/YYYY'
        //    }
        //});

        $('#ExecutionTime').daterangepicker({
            timePicker: true,
            minDate: moment(),
            startDate: moment(),
            endDate: moment().startOf('hour').add(10, 'hour'),
            locale: {
                format: 'hh:mm A DD/MM/YYYY'
            }
        });

        $('#CategoryId').select2({
            maximumSelectionLength: 3,
            theme: "bootstrap4" 
        });


        $('#CustomKolNames').select2({
            theme: "bootstrap4",
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
        } else  {

            $('.d-withoutRegular').removeClass('d-none');
            $('.d-withRegular').addClass('d-none');

            if ($('#suggestAccount tr').length > 1) {
                $('#actionWrap').removeClass('d-none');
            } else {

                $('#actionWrap').addClass('d-none');
            }

        }

        if (accouttype === 'HotMom') {
            $('.d-withoutHotMom').addClass('d-none');
            $('.d-withHotMom').removeClass('d-none');
        } else {

            $('.d-withoutHotMom').removeClass('d-none');
            $('.d-withHotMom').addClass('d-none');

        }

            
    }

    function handlerType() {
        var type = $('#Type').val();
        console.log('type', type);
        $('#enabledExtraTypeWrap,#requirementWrap,#changeAvatarWrap').addClass('d-none');

        $('.d-type').hide();
        $('.d-type-' + type).show();

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

            var cityIds = $('#CityId').val();
            console.log('cityids', cityIds);
            for (var i = 0; i < cityIds.length; i++) {
                urlparams += '&cityid=' + cityIds[i];
            }
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

        $('.btn-removeaccount').unbind('click');
        $('.btn-removeaccount').click(function () {
            var $tr = $(this).closest('tr');
            $tr.remove();
        });

        handlerAccountType();
    }

    function pricingCalculator() {

        /*
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

        */
        $('#btnCreateCampaign').show();
    }


    return {
        Init: init
    };

})();
