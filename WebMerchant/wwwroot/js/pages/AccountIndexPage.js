
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
