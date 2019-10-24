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