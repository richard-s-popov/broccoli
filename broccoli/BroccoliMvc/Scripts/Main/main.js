$(document).ready(function () {
    $('#loginBtn').click(function () {
        setTimeout(function () {
            if ($('#loginBtn').is(':hover') === true) {
                $('.login-window').fadeIn(400);
                $('#login').focus();
                $('.opacity').fadeIn(300);
            }
        }, 200);
    });
    
    $('.opacity').click(function () {
        setTimeout(function () {
            if ($('#loginBtn').is(':hover') !== true) {
                $('.login-window').fadeOut(300);
                $('.opacity').fadeOut(300);
            }
        }, 100);
    });
    
    $("body").keydown(function (e) {
        if (e.keyCode == 27) {
            setTimeout(function () {
                if ($('#loginBtn').is(':hover') !== true) {
                    $('.login-window').fadeOut(300);
                    $('.opacity').fadeOut(300);
                }
            }, 100);
        }
    });
});