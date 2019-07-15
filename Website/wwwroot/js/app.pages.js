var ChangeAccountTypePage = (function () {

    function init() {
        handler();
    }

    function handler() {
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


        $('.checkbox-ignorecampaigntype').change(function () {
            var url = $(this).data('url');
            $.post(url, function () { });

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
        handler();
        handlerType();
        $('#Type').change(function () {
            handlerType();
        });

        $('#campaignOptions .form-option').change(function () {

            pricingCalculator();

        });
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


        console.log('AppSettings.CurrentUser.Balance', AppSettings.CurrentUser.Balance, 'totalServicePrice', totalServicePrice, 'serviceCharge', serviceCharge, 'totalServiceCharge', totalServiceCharge, 'countOption', countOption);
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

        $('.frm-requestjoin').submit(function () {
            var $frm = $(this);

            var data = $frm.serialize();
            var url = $frm.data('action');
            var $frmbtn = $frm.find('.btn-requestjoin');
            var $i = $frmbtn.find('i');
            $frmbtn.prop('disabled', true);
            $i.removeClass('fa-plus').addClass('fa-spinner fa-spin');

            $.post(url, data, function (res) {
                if (res == 1) {

                    $frmbtn.removeClass('btn-outline-primary').addClass('btn-outline-success');
                    $i.removeClass('fa-spinner fa-spin').addClass('fa-check');
                } else {

                    $frmbtn.prop('disabled', false);
                    $frmbtn.removeClass('btn-outline-primary').addClass('btn-outline-danger');
                    $i.removeClass('fa-spinner fa-spin').addClass('fa-plus');
                }
                handlerReloadBtn(true);
            })



        })

        //$('.btn-requestjoin').click(function () {
        //    var $this = $(this);
        //    var $i = $(this).find('i');
        //    $this.prop('disabled', true);
        //    $i.removeClass('fa-plus').addClass('fa-spinner fa-spin');
        //    var url = $(this).data('url');
        //    $.get(url, function (res) {
        //        $this.removeClass('btn-outline-primary').addClass('btn-outline-success');
        //        $i.removeClass('fa-spinner fa-spin').addClass('fa-check');
        //        handlerReloadBtn(true);
        //    });
        //});
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


        $('.btn-shareui').click(function () {
            AppBsModal.Init('static');
            AppBsModal.OpenModal('');
            AppBsModal.ShowLoading();
            var href = $(this).data('href');
            var urlsubmit = $(this).data('urlsubmit');
            //var caption = $(this).data('caption');

            FB.ui(
                {
                    method: 'share',
                    href: href
                    //quote: caption,
                },
                function (response) {
                    if (response && !response.error_message) {
                        $.post(urlsubmit, function (html) {
                            AppBsModal.OpenModal(html, function () { AppCommon.handlerBtnReload(); });
                        });
                    } else {
                        AppBsModal.HideModal();
                    }
                });
        });

    }
    function handlerUpdateRef() {
        $.validator.unobtrusive.parse($('#frmUpdateCampaignAccountRef'));
        $('#frmUpdateCampaignAccountRef').submit(function (e) {
            e.preventDefault();
            var isvalid = $(this).valid();
            if (isvalid) {
                var url = $(this).data('action');
                var data = $(this).serialize();
                AppBsModal.ShowLoading();
                $.post(url, data, function (html) {
                    AppBsModal.OpenModal(html, function () { AppCommon.handlerBtnReload(); });

                });
            }
        });




    }

    return {
        Init: init
    };

})();

var HomeIndexPage = (function () {

    function init() {
        handler();

        $('#main').css('min-height', '0');
    }


    function handler() {
        $('.owl-carousel').owlCarousel({
            margin: 10,
            loop: true,
            autoWidth: true,
            items: 4
        })
    }
    return {
        Init: init
    };

})();