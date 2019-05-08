
var App = (function () {

    function init() {
        if (AppSettings.IsAuthenticated) {
            AppCommon.bindingWalletBalance();
            handlerWallet();
        }


        handler();
    }

    function handler() {
       



        $('.btn-facebook').click(function () {
            var $frm = $($(this).data('target'));
            FB.login(function (response) {
                console.log('login-facebook', response)
                // handle the response
                if (response.status === 'connected') {
                    $frm.find('input[name=token]').val(response.authResponse.accessToken);
                    $frm.submit();
                } else {

                }
            }, { scope: 'public_profile,email,user_likes,user_friends,user_link,user_posts' });
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
            })

        });


        $('.form-datepicker').daterangepicker({
            singleDatePicker: true,
            showDropdowns: true,
            //startDate: "01/01/2000",
            locale: {
                format: 'DD/MM/YYYY'
            }
        }, function (start, end, label) {
            var years = moment().diff(start, 'years');
            //alert("You are " + years + " years old!");
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



    }



    function handlerWallet() {
        $('.wallet-recharge').click(function (e) {
            e.preventDefault();

            AppBsModal.Init();
            AppBsModal.OpenRemoteModal(AppConstants.UrlRequestRecharge)
        });

        $('.wallet-widthdraw').click(function (e) {
            e.preventDefault();
            
        });


    }

    function filldistrict() {
        var cityid = $('#CityId').val();
        var districtid = $('#CityId').data('district');
        $.get(AppConstants.UrlGetDistricts(cityid), function (districts) {
            if (districts.length == 0) {
                $('.pdistrict').hide();
                $('#DistrictId').html('');
            } else {
                $('.pdistrict').show();
                var html = '';
                for (var i = 0; i < districts.length; i++) {

                    var selected = '';
                    if (districtid == districts[i].id) {
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
