$(document).ready(function () {
    $('#signInBtn').click(function () {
        $('.login-window').delay(200).fadeIn(400);
        setTimeout(function() {
            $('#login').focus();
        }, 300);
        $('.opacity').fadeTo(300, 0.4);
    });
    
    $('.opacity').click(function () {
        $('.login-window').fadeOut(300);
        $('.opacity').fadeOut(300);
    });
    
    $("body").keydown(function (e) {
        if (e.keyCode == 27) {
            $('.login-window').fadeOut(300);
            $('.opacity').fadeOut(300);
        }
    });
});