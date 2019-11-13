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
