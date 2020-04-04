var App = (function () {

    function init() {
        setTimeout(function () {

            //console.log('AppPayment Init', AppSettings);
            if (AppSettings.IsAuthenticated) {
                AppCommon.bindingWalletBalance();
                AppNotification.Init();
                //AppWallet.Init();


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
        if (currentPage === 'campaign_create' || currentPage == 'campaign_editinfo') {
            CampaignCreatePage.Init();
        }
        else if (currentPage === 'campaign_createinfo') {
            CampaignCreateTargetPage.Init();
        }
        else if (currentPage === 'campaign_details') {
            CampaignDetailsPage.Init();
        } 
        else if (currentPage === 'campaign_index') {
            CampaignIndexPage.Init();
        } 
        else if (currentPage === 'home_index') {
            HomeIndexPage.Init();
        }
        else if (currentPage === 'campaign_caption' || currentPage === 'campaign_content') {
            CampaignCaptionPage.Init();
        }
        
        
    }
    function handler() {


        $.notifyDefaults({
            
            placement: {
                from: 'bottom',
                align: 'right'
            },
        });

        $('[data-toggle="tooltip"]').tooltip();
        $('[data-toggle="popover"]').popover();

        $('.campaign-image-carousel').owlCarousel({
            margin: 10,
            items: 1, autoHeight: true

        });

        $('.fbpost-carousel').owlCarousel({
            margin: 10,
            items: 4

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

        $('.form-select2').select2({ theme: "bootstrap" });
        $('.form-select2-tags').select2({
            theme: "bootstrap",
            tags: true,
            tokenSeparators: [',']
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
                format: 'hh:mm A DD/MM/YYYY'
            }
        }, function (start, end, label) {
                var years = moment().diff(start, 'years');

        });

        $('.form-daterangepicker').daterangepicker({

            autoUpdateInput: false,
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

        //AppCommon.toggleAffix();

        if ($('#cookieConsentModal').length > 0) {
            $('#cookieConsentModal').modal('show');
        }




        $.extend($.validator.messages, {
            required: "Hãy nhập dữ liệu",
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


        /*

        $('.nav-fbfeed').click(function (e) {
            e.preventDefault();

            FB.login(function (response) {
                if (response.status === 'connected') {
                    var token = response.authResponse.accessToken;

                    $('#frmUpdatefbpostToken').val(token);
                    $('#frmUpdatefbpost').submit();
                } else {
                    alert('Bạn cần cập nhật quyền trên hệ thống của Facebook');
                }

            }, { scope: 'user_posts' });

        });



        $('.nav-friends').click(function (e) {
            e.preventDefault();

            FB.login(function (response) {
                if (response.status === 'connected') {
                    var token = response.authResponse.accessToken;

                    $('#frmUpdatefbfriendsToken').val(token);
                    $('#frmUpdatefbfriends').submit();
                } else {
                    alert('Bạn cần cập nhật quyền trên hệ thống của Facebook');
                }

            }, { scope: 'user_friends' });

        });
        */
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
    uploadTempImage: function (files,  callback,sizetype) {
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

        if (sizetype) {
            formData.append('sizetype', sizetype);
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
    UrlGetAccounts: "/Campaign/GetAccounts",
    UrlGetDistricts: function (cityid) { return "/home/GetDistricts?cityid=" + cityid; },
    UrlAgencyPayment: function (campaignid) { return "/Payment/CampaignPayment?campaignid=" + campaignid; },
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

            setTimeout(function () {
                getNotificationCount();
            }, 5000);
        });

    }

    function updateNotificationChecked() {
        $.post(AppConstants.UrlUpdateNotificationChecked, function () {


            $('.nav-notif .badge').hide();
            $('.nav-notif .badge').text('0');
        });
    }
    function getNotificationDropdown() {

        console.log('getNotificationDropdown');
        $.get(AppConstants.UrlGetNotification, function (html) {
            $notifDropdown.html(html);

            $notifDropdown.find('.item-Created').each(function () {
                var id = $(this).data('id');
                var cookiename = 'notif' + id;
                var val = Cookies.get(cookiename); // => undefined
                if (!val) {
                    Cookies.set(cookiename, '1');
                    var html = $(this).html();
                    $.notify(html);
                }


            });

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
        /*
        $('.wallet-recharge').click(function (e) {
            var campaignid = $(this).data('campaignid');
            e.preventDefault();

            AppBsModal.Init();
            AppBsModal.OpenRemoteModal(AppConstants.UrlRecharge(campaignid), function () {
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
        */
    }

    function handlerWithdraw() {
        $.validator.unobtrusive.parse($('#frmWithDraw'));
        $('#frmWithDraw').submit(function (e) {


            //e.preventDefault();
            var isvalid = $(this).valid();
            if (isvalid) {

                $(this).find('.btn-submit').hide();
                /*
                var url = $(this).data('action');
                var data = $(this).serialize();
                AppBsModal.ShowLoading();
                $.post(url, data, function (html) {
                    AppBsModal.OpenModal(html);
                });
                */
            }
        });
    }


    function handlerRecharge() {
        $.validator.unobtrusive.parse($('#frmRecharge'));

        $('#frmRecharge').submit(function (e) {
           
            //e.preventDefault();
            var isvalid = $(this).valid();
            if (isvalid) {

                $(this).find('.btn-submit').hide();
                /*
                var url = $(this).data('action');
                var data = $(this).serialize();
                AppBsModal.ShowLoading();
                $.post(url, data, function (html) {
                    AppBsModal.OpenModal(html);
                });
                */
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
        HandlerRecharge: handlerRecharge,
        HandlerWithdraw: handlerWithdraw
    };
})();


var AccountIndexPage = (function () {

    function init() {
        loadFriends();
    }
    
    function loadFriends() {
      
        FB.getLoginStatus(function (response) {

            console.log('getLoginStatus', response);
            if (response.status === 'connected') {
                // The user is logged in and has authenticated your
                // app, and response.authResponse supplies
                // the user's ID, a valid access token, a signed
                // request, and the time the access token
                // and signed request each expire.
                var uid = response.authResponse.userID;
                var accessToken = response.authResponse.accessToken;

                FB.api('/me?fields=friends.limit(10){id,link,name}', function (data) {

                    console.log('loadfriend', data);

                });
            } else if (response.status === 'not_authorized') {
                // The user hasn't authorized your application.  They
                // must click the Login button, or you must call FB.login
                // in response to a user gesture, to launch a login dialog.
            } else {
                // The user isn't logged in to Facebook. You can launch a
                // login dialog with a user gesture, but the user may have
                // to log in to Facebook before authorizing your application.
            }


        });





    }

    return {
        Init: init
    };

})();

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

var CampaignCreatePage = (function () {

    function init() {
        handler();
    }

    function handler() {
        $('#dataImageFile').change(function () {
            var id = $(this).attr('id');
            var target = $(this).data('target');
            var preview = $(this).data('preview');
            var sizetype = $(this).data('sizetype');
            var files = document.getElementById(id).files;

            AppCommon.uploadTempImage(files, function (datas) {
                if (datas.length > 0) {
                    $(target).val(datas[0].path);
                    $(target).trigger("change");
                    $(preview).attr('src', datas[0].url);
                }
            }, sizetype);

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


        $('#sampleContent').change(function () {
            var id = $(this).attr('id');
            var target = $(this).data('target');
            var files = document.getElementById(id).files;

            AppCommon.uploadTempImage(files, function (datas) {
                datas.forEach(function (item) {
                    var html = '<div class="addonimage"><span class="remove"><i class="fal fa-times"></i></span> <img src="' + item.url + '" class="img-thumbnail mt-2" style="" /><input type="hidden" name="SampleContent" value="' + item.path + '" /></div>';
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

        handlerMethod();
        $('input[name=Method]').change(function () {
            handlerMethod();
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



        $('#HashTag').select2({
            maximumSelectionLength: 3,
            theme: "bootstrap", tags: true
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

        handlerAccountType();

        $('#SendProduct').change(function () {
            if ($(this).is(':checked')) {

                $('#modal-timereview').modal('show');
            }

        });


        if ($('.reviewaddress-container').length > 0) {


            handlerReviewType();

            $('input[name=ReviewType]').change(function () {
                handlerReviewType();
            });

          
            $('#ReviewDate').daterangepicker({
                timePicker: true,
                minDate: moment(),
                startDate: moment(),
                endDate: moment().startOf('hour').add(10, 'hour'),
                locale: {
                    format: 'hh:mm A DD/MM/YYYY'
                },
                parentEl: ""
            });

            handlerReviewPayback();
            $('input[name=ReviewPayback]').change(function () {
                handlerReviewPayback();
            });
        }

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


    function handlerReviewType() {
        $('.reviewaddress-container').hide();
        var val = $('input[name=ReviewType]:checked').val();
        console.log('handlerReviewType', val);
        if (val) {
            $('.reviewaddress-container-' + val).show();

        } 
    }

    function handlerReviewPayback() {
        $('.form-reviewAddress').hide();
        var val = $('input[name=ReviewPayback]:checked').val();
        console.log('handlerReviewPayback', val);
        if (val) {
             

            $('.form-reviewAddress').show();

        } else {
            $('.form-reviewAddress').hide();
        }
    }


    function handlerAccountType() {
        var accouttype = $('input[name=AccountType]:checked').val();
        console.log('accouttype', accouttype);
        if (accouttype === 'Regular') {
            $('.d-withoutRegular').hide();
            $('.d-withRegular').show();
        } else if (accouttype === 'HotMom') {
            $('.d-withoutHotMom').addClass('d-none');
            $('.d-withHotMom').removeClass('d-none');
        } else {
            $('.d-withRegular').hide();

        }


    }

    function handlerMethod() {
        var method = $('input[name=Method]:checked').val();
        if (method == 'OpenJoined') {
            $('.d-withoutOpenJoined').addClass('d-none');
            $('#actionWrap').removeClass('d-none');
        } else {
            $('.d-withoutOpenJoined').removeClass('d-none');

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


var CampaignDetailsPage = (function () {

    function init() {

        CampaignIndexPage.Init();
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

var CampaignIndexPage = (function () {

    function init() {
        handler();
    }

    function handler() {
        $('.modal-accountcampaign').on('hidden.bs.modal', function () {
            // do something…
            $(this).find('.list-campaignaccount').html('');
        });

        $('.modal-accountcampaign').on('shown.bs.modal', function () {
            // do something…
            var $this = $(this);
            var $target = $this.find('.list-campaignaccount');
            getList($target);


            $this.find('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
                var type = $(e.target).data('type');
                if (type == 1) {

                    var $target = $this.find('.list-campaignaccount');
                    getList($target);
                } else {

                    var $target2 = $this.find('.list-matchedaccount');
                    getList($target2);
                }
            })

        });



       

    }

    function handlerVisibleCheckItem($target) {
        console.log('$target2', $target)
        var cb = '.cb-checkitemAccountRequest:checked';// '.cb-checkitemMatchAccount:checked';


        var length = $target.find(cb).length;
        if (length > 0) {
            $target.find('.frmFeedbackAll').show();

            var html = '';
            $target.find(cb).each(function () {
                var val = $(this).val();
                html += '<input type="hidden" name="accountid" value="' + val + '" />';
            }).promise().done(function () {
                console.log('html', html);
                $target.find('.frmFeedbackAll-ids').html(html);
            });
            

        } else {
            $target.find('.frmFeedbackAll-ids').html('');
            $target.find('.frmFeedbackAll').hide();
        }
     
    }


    function getList($target, url) {


        var url = url ? url : $target.data('url');
        console.log('url', url);
        $target.html(AppConstants.HtmlSpinner);
        $.get(url, function (html) {

            $target.html(html);
            handlerAccountCampaign($target);
        })
    }


    function handlerAccountCampaign($target) {

        handlerFrm($target);

        $target.find('.page-link').click(function (e) {
            e.preventDefault();

            var url = $(this).attr('href');
            getList($target, url);
        });

        $target.find('.cb-checkall').click(function () {
            $('.cb-checkitem').prop('checked', this.checked);
            handlerVisibleCheckItem($target);
        });

        $target.find('.cb-checkitem').change(function () {
            handlerVisibleCheckItem($target);
        });

        handlerVisibleCheckItem($target);

    }

    function handlerFrm($target) {
        var form = $target.find(".frmFeedback");

        $(":submit", form).click(function () {
            if ($(this).attr('name')) {
                $(form).append(
                    $("<input type='hidden'>").attr({
                        name: $(this).attr('name'),
                        value: $(this).attr('value')
                    })
                );
            }
        });

        $(form).submit(function (e) {

            console.log('submit');
            e.preventDefault();

            $target.html(AppConstants.HtmlSpinner);
            var action = $(this).data('action');

            console.log('handlerAccountCampaign', action);
            var data = $(this).serialize();
            $.post(action, data, function (html) {
                $target.html(html);
                handlerAccountCampaign($target);
            });
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

    }


    function handler() {
       
    }
    return {
        Init: init
    };

})();
$().ready(function () {

    App.Init();
});