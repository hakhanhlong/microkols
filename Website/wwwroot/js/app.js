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
