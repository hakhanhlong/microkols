var App = (function () {

    function init() {
        setTimeout(function () {

            //console.log('AppPayment Init', AppSettings);
            if (AppSettings.IsAuthenticated) {
                AppCommon.bindingWalletBalance();
                AppNotification.Init();
                //AppWallet.Init();


                //if (AppSettings.CurrentUser.Type === 2) {
                //    AppPayment.Init();
                //}

            }
        }, 500);


        handler();
        handlerPages();
        handlerAccountUpdateInfo();
    }
    function handlerPages() {
        var currentPage = $('#CurrentPage').val();
        if (currentPage === 'account_changeaccounttype') {
            ChangeAccountTypePage.Init();
        }
        else if (currentPage === 'account_fbaccount') {
            AccountFbAccountPage.Init();
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
        else if (currentPage === 'account_index') {
            AccountIndexPage.Init();
        }


    }
    function handler() {


        $('[data-toggle="tooltip"]').tooltip()


        $('.campaign-image-carousel').owlCarousel({
            margin: 10,
            items: 1, autoHeight: true

        });

        $('.fbpost-carousel').owlCarousel({
            margin: 10,
            items: 4

        });


        //$('.btn-facebook').click(function () {
        //    var $frm = $($(this).data('target'));
        //    FB.login(function (response) {
        //        console.log('login-facebook', response);
        //        // handle the response
        //        if (response.status === 'connected') {
        //            $frm.find('input[name=token]').val(response.authResponse.accessToken);
        //            $frm.submit();
        //        }
        //    }, { scope: 'public_profile,email,user_friends,user_link,user_posts' });
        //});

        //$('.btn-linkfacebook').click(function () {
        //    var $frm = $($(this).data('target'));
        //    FB.login(function (response) {
        //        console.log('login-facebook', response);
        //        // handle the response
        //        if (response.status === 'connected') {
        //            $frm.find('input[name=token]').val(response.authResponse.accessToken);
        //            $frm.submit();
        //        }
        //    }, { scope: 'public_profile,email,user_likes,user_friends,user_link,user_posts,user_link' });
        //});

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


    function handlerAccountUpdateInfo() {

        $.get(AppConstants.UrlGetAccountUpdateInfoStatus, function (res) {

            if (res == 1) {
                if ($('#frmChangeIdCard').length == 0) {

                    //tam thoi fix model
                    $('#modal-update-idcard').modal('show');
                }
            }
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
    UrlGetAccountUpdateInfoStatus:"/Account/GetAccountUpdateInfoStatus",
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
    UrlGetAccounts: "/AgencyCampaign/GetAccounts",
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


var AccountFbAccountPage = (function () {

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

                FB.api('/me?fields=friends.limit(40){id,link,name}', function (data) {

                    console.log('loadfriend', data);

                    if (data.friends &&  data.friends.data.length > 0) {
                        $('.accountfriendSection').html('');

                        data.friends.data.forEach(function (item) {

                            console.log('friends', item);
                            var $id = $('#item' + item.id);

                            if ($id.length > 0) {

                                $('.accountfriendSection').append('<div class="col-md-3">' + $id.html() + '</div>');
                            }

                        });


                        $('.kolfriend-count').html('' + data.friends.data.length);
                        $('.friends-count').html('/ ' + data.friends.summary.total_count + ' bạn bè trên Facebook');
                    }


                   

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

var AccountFbPost = (function () {

    function init() {
        updateFbPost();
    }

    function updateFbPost() {


        $('.btn-updateFbPost').click(function (e) {
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


    }

    return {
        Init: init
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


var  CampaignDetailsPage = (function () {

    function init() {

        handler();
    }
    function handler() {
        handlerAction();
        if ($('#chartModel').length > 0) {


            //handlerChart();
        }

    }

    function handlerAction() {
        $('.btn-updateref').click(function () {
            var url = $(this).data('url');
            AppBsModal.Init('static');
            AppBsModal.OpenRemoteModal(url, function () {
                handlerUpdateRef();
            });
        });
        $('.btn-updaterefimages').click(function () {
            var url = $(this).data('url');
            AppBsModal.Init('static');
            AppBsModal.OpenRemoteModal(url, function () {
                handlerUpdateRefImages();
            });
        });

        $('.btn-shareui').click(function () {
            AppBsModal.Init('static');
            AppBsModal.OpenModal('');
            AppBsModal.ShowLoading();
            var href = $(this).data('href');
            var urlsubmit = $(this).data('urlsubmit');
            var title = $(this).data('title');
            var picture = $(this).data('picture');
            var description = $(this).data('description');
            var caption = $(this).data('caption');
            
            //var caption = $(this).data('caption');

            FB.ui(
                {
                    method: 'share',
                    href: href,
                    //quote: caption,
                    title: title,  // The same than name in feed method
                    picture: picture,
                    caption: caption,
                    description: description,
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
    function handlerUpdateRefImages() {
        $.validator.unobtrusive.parse($('#frmUpdateCampaignAccountRefImages'));
       
        $('#addonImages').change(function () {
            var id = $(this).attr('id');
            var target = $(this).data('target');
            var files = document.getElementById(id).files;

            AppCommon.uploadTempImage(files, function (datas) {
                datas.forEach(function (item) {
                    var html = '<img src="' + item.url + '"  class="img-thumbnail mt-2" style="max-height:400px" /><input type="hidden" name="RefImage" value="' + item.path + '" />';
                    $(target).append(html);
                })

            });

        });
        $('#frmUpdateCampaignAccountRefImages').submit(function (e) {
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
        Init: init,
        HandlerAction: handlerAction
    };

})();


var  CampaignIndexPage = (function () {

    function init() {
        CampaignDetailsPage.HandlerAction();
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
                version: 'v4.0'
            });
            App.Init();
        },
        dataType: "script",
        cache: true
    });
});
//App.Init();