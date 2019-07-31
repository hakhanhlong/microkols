var App = (function () {

    function init() {
        setTimeout(function () {

            //console.log('AppPayment Init', AppSettings);
            if (AppSettings.IsAuthenticated) {
                AppCommon.bindingWalletBalance();
                AppNotification.Init();
                AppWallet.Init();


                if (AppSettings.CurrentUser.Type === 2) {
                    AppPayment.Init();
                }

            }
        }, 500);


        handler();
        handlerPages();
    }
    function handlerPages() {
        var currentPage = $('#CurrentPage').val();
        if (currentPage === 'account_changeaccounttype') {
            ChangeAccountTypePage.Init();
        }
        else if (currentPage === 'agencycampaign_create') {
            AgencyCreateCampaignPage.Init();
        } else if (currentPage === 'agencycampaign_details') {
            AgencyDetailsCampaignPage.Init();
        }
        else if (currentPage === 'accountcampaign_details') {
            AccountDetailsCampaignPage.Init();
        } else if (currentPage === 'home_index') {
            HomeIndexPage.Init();
        }
    }
    function handler() {


        $('[data-toggle="tooltip"]').tooltip()


        $('.btn-facebook').click(function () {
            var $frm = $($(this).data('target'));
            FB.login(function (response) {
                console.log('login-facebook', response);
                // handle the response
                if (response.status === 'connected') {
                    $frm.find('input[name=token]').val(response.authResponse.accessToken);
                    $frm.submit();
                }
            }, { scope: 'public_profile,email,user_likes,user_friends,user_link,user_posts' });
        });

        $('.btn-linkfacebook').click(function () {
            var $frm = $($(this).data('target'));
            FB.login(function (response) {
                console.log('login-facebook', response);
                // handle the response
                if (response.status === 'connected') {
                    $frm.find('input[name=token]').val(response.authResponse.accessToken);
                    $frm.submit();
                }
            }, { scope: 'public_profile,email,user_likes,user_friends,user_link,user_posts,user_link' });
        });

        $('.btn-remotemodal').click(function () {
            var url = $(this).data('url');

            AppBsModal.Init();
            AppBsModal.OpenRemoteModal(url, function () { });


        });

        $('.image-upload').change(function () {
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

        $('.form-select2').select2({ theme: "bootstrap4" });
        $('.form-select2-tags').select2({
            theme: "bootstrap4",
            tags: true,
            tokenSeparators: [',', ' ']
        });


        
        $('.form-datepicker').daterangepicker({
            singleDatePicker: true,
            showDropdowns: true,
            autoApply: true,
            //autoUpdateInput: false,
            //startDate: "01/01/2000",
            locale: {
                format: 'DD/MM/YYYY'
            }
        });

        $('.form-datetimepicker').daterangepicker({
            singleDatePicker: true,
            //autoUpdateInput: false,
            timePicker: true,
            autoApply: 'true',
            showDropdowns: true,
            //startDate: "01/01/2000",
            locale: {
                format: 'hh:mm DD/MM/YYYY'
            }
        }, function (start, end, label) {
                var years = moment().diff(start, 'years');

        });

        $('.form-daterangepicker').daterangepicker({
           
            locale: {
                format: 'DD/MM/YYYY'
            }
        }, function (start, end, label) {
            var years = moment().diff(start, 'years');

        });

        
        //$('.form-datepicker').daterangepicker({
        //    "singleDatePicker": true,
        //    "showWeekNumbers": true,
        //    "linkedCalendars": false,
        //    "showCustomRangeLabel": false,
        //    "startDate": "04/24/2019",
        //    "endDate": "04/30/2019"
        //}, function (start, end, label) {
        //    console.log('New date range selected: ' + start.format('YYYY-MM-DD') + ' to ' + end.format('YYYY-MM-DD') + ' (predefined range: ' + label + ')');
        //});

        if ($('#CityId').length > 0) {

            filldistrict();
            setTimeout(function () {
                $('#CityId').change(function () {
                    console.log('CityId change-');
                    filldistrict();
                });
            }, 500);
        }

        AppCommon.toggleAffix();

        if ($('#cookieConsentModal').length > 0) {
            $('#cookieConsentModal').modal('show');
        }




        $.extend($.validator.messages, {
            required: "Hãy nhập.",
            remote: "Hãy sửa cho đúng.",
            email: "Hãy nhập email.",
            url: "Hãy nhập URL.",
            date: "Hãy nhập ngày.",
            dateISO: "Hãy nhập ngày (ISO).",
            number: "Hãy nhập số.",
            digits: "Hãy nhập chữ số.",
            creditcard: "Hãy nhập số thẻ tín dụng.",
            equalTo: "Hãy nhập thêm lần nữa.",
            extension: "Phần mở rộng không đúng.",
            maxlength: $.validator.format("Hãy nhập từ {0} kí tự trở xuống."),
            minlength: $.validator.format("Hãy nhập từ {0} kí tự trở lên."),
            rangelength: $.validator.format("Hãy nhập từ {0} đến {1} kí tự."),
            range: $.validator.format("Hãy nhập từ {0} đến {1}."),
            max: $.validator.format("Hãy nhập từ {0} trở xuống."),
            min: $.validator.format("Hãy nhập từ {0} trở lên.")
        });
    }


    function filldistrict() {
        var cityid = $('#CityId').val();
        var districtid = $('#CityId').data('district');
        $.get(AppConstants.UrlGetDistricts(cityid), function (districts) {
            if (districts.length === 0) {
                $('.pdistrict').hide();
                $('#DistrictId').html('');
            } else {
                $('.pdistrict').show();
                var html = '';
                for (var i = 0; i < districts.length; i++) {

                    var selected = '';
                    if (districtid === districts[i].id) {
                        selected += 'selected=selected';
                    }
                    html += '<option value="' + districts[i].id + '" ' + selected + '>' + districts[i].name + '</option>';
                }
                $('#DistrictId').html(html);
            }
        });
    }


    return {
        Init: init,
    };
})();


var AppCommon = {
   
    loadScrollTop: function () {
        $(window).scroll(function () {
            if ($(this).scrollTop() > 500) {
                $('#stickyFooterActions').fadeIn();
            } else {
                $('#stickyFooterActions').fadeOut();
            }
        });
        $('#scrollToTop').click(function () {
            $('html, body').animate({ scrollTop: 0 }, 800);
        });

    },
    
    handlerBtnReload: function() {
        $('.btn-reload').click(function (e) {
            window.location = window.location;
        });
    },
    uploadTempImage: function (files,  callback) {
        var xhr, formData;
        xhr = new XMLHttpRequest();
        xhr.withCredentials = false;
        xhr.open('POST', AppConstants.UrlUploadTempImage);
        xhr.onload = function () {
            if (xhr.status !== 200) {
                failure('HTTP Error: ' + xhr.status);
                return;
            }
            var json = JSON.parse(xhr.responseText);
            callback(json);
        };
        formData = new FormData();
        for (var i = 0; i < files.length; i++) {
            formData.append('files', files[i]);
        }
        xhr.send(formData);
    },
  
    bindingWalletBalance: function () {
        $.get(AppConstants.UrlGetAmount, function (val) {
            $('.wallet-balance').html(AppCommon.moneyFormat(val));
            AppSettings.CurrentUser.Balance = val;
        });
    },
    
    moneyFormat: function (input, n, x) {
        var re = '\\d(?=(\\d{' + (x || 3) + '})+' + (n > 0 ? '\\.' : '$') + ')';

        return input.toFixed(Math.max(0, ~~n)).replace(new RegExp(re, 'g'), '$&,') + ' đ';
    },
  
    toggleAffix: function () {

        var toggleAffix = function (affixElement, scrollElement, wrapper) {

            var height = affixElement.outerHeight(),
                top = wrapper.offset().top;

            if (scrollElement.scrollTop() >= top) {
                wrapper.height(height);
                affixElement.addClass("affix");
            }
            else {
                affixElement.removeClass("affix");
                wrapper.height('auto');
            }

        };


        $('[data-toggle="affix"]').each(function () {
            var ele = $(this),
                wrapper = $('<div></div>');

            ele.before(wrapper);
            $(window).on('scroll resize', function () {
                toggleAffix(ele, $(this), wrapper);
            });

            // init
            toggleAffix(ele, $(window), wrapper);
        });
    }
 
};

var AppConstants = {
    UrlUploadFile: "/home/uploadimage",
    UrlGetAmount: "/wallet/GetAmount",
    UrlRecharge: function (campaignid) {
        if (!campaignid) {
            campaignid = 0;
        }
        return "/wallet/Recharge?campaignid=" + campaignid;
    },
    UrlWithdraw: "/wallet/Withdraw",
    UrlGetNotificationCount: "/Notification/Count",
    UrlUpdateNotificationChecked: "/Notification/UpdateChecked",
    UrlGetNotification: "/Notification/IndexPartial",
    UrlUploadTempImage: "/Home/UploadImage",

    UrlGetDistricts: function (cityid) { return "/home/GetDistricts?cityid=" + cityid; },
    UrlAgencyPayment: function (campaignid) { return "/AgencyPayment/CampaignPayment?campaignid=" + campaignid; },
    ModalSpinner: '<div class="modal-dialog modal-dialog-centered"><div class="modal-content"><div class="py-5 text-center text-success loading"><i class="fas fa-spinner fa-spin"></i></div> </div></div>',
    HtmlSpinner: '<div class="py-5 text-center text-success loading"><i class="fas fa-spinner fa-spin"></i></div>'
};


var AppSettings = {
    IsAuthenticated: false,
    CurrentUser: {
        Username: '',
        Name: '',
        Type: -1,
        Balance: 0
    },
};

var AppBsModal = (function () {
    var selectorId = '#appbsmodal';

    function showLoading() {
        $(selectorId).html(AppConstants.ModalSpinner);
        $(selectorId).modal('show');
    }

    function hideModal() {
        $(selectorId).modal('hide');
        $(selectorId).remove();
    }
    function openModal(html, callback) {

        $(selectorId).html(html);
        if (callback && typeof callback === "function") {
            callback();
        }
        $(selectorId).modal('show');
    }
    function openRemoteModal(url, callback) {
        showLoading();
        $(selectorId).modal('show');
        $.get(url, function (html) {
            $(selectorId).html(html);
            if (callback && typeof callback === "function") {
                callback();
            }
        });
    }
    function init(backdrop) {
        removeModal();

        if (backdrop === undefined) {
            backdrop = true;
        }
        var html = '<div id="appbsmodal" class="modal" data-backdrop="' + backdrop + '"></div>';
        $('body').append(html);
        $(selectorId).on('hidden.bs.modal', function (e) {
            removeModal();
        });
        //$('#appbsmodal').on('show.bs.modal', function (e) {
        //    showLoading();
        //});
    }

    function removeModal() {
        $(selectorId).remove();
    }
    return {
        Init: init,
        OpenModal: openModal,
        OpenRemoteModal: openRemoteModal,
        ShowLoading: showLoading,
        HideModal: hideModal
    };
})();


var AppNotification = (function () {

    var $notifCount = $('.nav-notif .badge');
    var $notifDropdown = $('.nav-notif .dropdown-menu');
    function init() {
        handler();
        getNotificationCount();

    }

    function handler() {

        getNotificationDropdown();

        $('.nav-notif').on('show.bs.dropdown', function () {

            updateNotificationChecked();
        });
    }

    function getNotificationCount() {


        var currentcount = parseInt($notifCount.text());

        $.get(AppConstants.UrlGetNotificationCount, function (count) {

            $notifCount.text(count);
            if (count === 0) {

                $('.nav-notif .badge').hide();
            } else {

                $('.nav-notif .badge').show();
            }

            if (count !== currentcount) {
                getNotificationDropdown();
            }
        });

    }

    function updateNotificationChecked() {
        $.post(AppConstants.UrlUpdateNotificationChecked, function () {


            $('.nav-notif .badge').hide();
            $('.nav-notif .badge').text('0');
        });
    }
    function getNotificationDropdown() {

        $.get(AppConstants.UrlGetNotification, function (html) {
            $notifDropdown.html(html);
            //handleCheckAll();
        });


    }


    return {
        Init: init
    };
})();


var AppPayment = (function () {

    function init() {
        $('.btn-payment').click(function (e) {
            e.preventDefault();
            var campaignid = $(this).data('id');
            AppBsModal.Init('static');
            AppBsModal.OpenRemoteModal(AppConstants.UrlAgencyPayment(campaignid));
        });
    }

    function handlerPayment() {
        $.validator.unobtrusive.parse($('#frmPayment'));
        $('#frmPayment').submit(function (e) {
            e.preventDefault();
            var isvalid = $(this).valid();
            if (isvalid) {
                var url = $(this).data('action');
                var data = $(this).serialize();
                AppBsModal.ShowLoading();
                $.post(url, data, function (html) {
                    AppBsModal.OpenModal(html);
                });
            }
        });
    }
    function handlerMessage() {
        AppCommon.handlerBtnReload();
    }
    return {
        Init: init,
        HandlerPayment: handlerPayment,
        HandlerMessage: handlerMessage
    };

})();

var AppWallet = (function () {


    function init() {
        $('.wallet-recharge').click(function (e) {
            e.preventDefault();

            AppBsModal.Init();
            AppBsModal.OpenRemoteModal(AppConstants.UrlRecharge(), function () {
                handlerRecharge();
            });
        });

        $('.wallet-withdraw').click(function (e) {
            e.preventDefault();
            AppBsModal.Init();
            AppBsModal.OpenRemoteModal(AppConstants.UrlWithdraw, function () {
                handlerWithdraw();
            });

        });
    }

    function handlerWithdraw() {
        $.validator.unobtrusive.parse($('#frmWithDraw'));
        $('#frmWithDraw').submit(function (e) {
            e.preventDefault();
            var isvalid = $(this).valid();
            if (isvalid) {
                var url = $(this).data('action');
                var data = $(this).serialize();
                AppBsModal.ShowLoading();
                $.post(url, data, function (html) {
                    AppBsModal.OpenModal(html);
                });
            }
        });
    }


    function handlerRecharge() {
        $.validator.unobtrusive.parse($('#frmRecharge'));
        $('#frmRecharge').submit(function (e) {
            e.preventDefault();
            var isvalid = $(this).valid();
            if (isvalid) {
                var url = $(this).data('action');
                var data = $(this).serialize();
                AppBsModal.ShowLoading();
                $.post(url, data, function (html) {
                    AppBsModal.OpenModal(html);
                });
            }
        });
        handlerMethod();
        $('#frmRecharge input[name="Method"]').change(function () {
            handlerMethod();
        });
        handlerBank();
        $('#Bank').change(function () {
            handlerBank();
        });
    }

    function handlerBank() {

        $(".bankinfo").addClass('d-none');
        var val = $('#Bank').val();
        $('#bankinfo' + val).removeClass('d-none');
    }
    function handlerMethod() {
        var val = $('#frmRecharge input[name="Method"]:checked').val();
        console.log('handlerMethod', val);
        if (val === 'Chuyển khoản') {
            $('#bankWrap').removeClass('d-none');
        } else {
            $('#bankWrap').addClass('d-none');
        }
    }

    return {
        Init: init,
        HandlerRecharge: handlerRecharge
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

var ChangeAccountTypePage = (function () {

    function init() {
        handler();
    }

    function handler() {
        /*
        var datas = $.parseJSON($('#modelHotMomData').html());
        
        var vm = new Vue({
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
        */
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
        });
    }
    return {
        Init: init
    };

})();
$().ready(function () {
    var appId = $('#FbAppId').val();
    $.ajax({
        type: "GET",
        url: 'https://connect.facebook.net/en_US/sdk.js',
        success: function () {
            FB.init({
                appId: appId,
                version: 'v3.2'
            });
            App.Init();
        },
        dataType: "script",
        cache: true
    });
});
//App.Init();