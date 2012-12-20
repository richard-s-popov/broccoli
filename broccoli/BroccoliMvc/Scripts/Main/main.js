$(document).ready(function () {
    $('input').attr('autocomplete', 'off');

    $('#loginBtn').click(function () {
        setTimeout(function () {
            if ($('#loginBtn').is(':hover') === true) {
                $('.login-window').fadeIn(100);
            }
        }, 200);
    });
    
    $('#loginBtn').mouseleave(function () {
        setTimeout(function () {
            if ($('#loginBtn').is(':hover') !== true) {
                $('.login-window').fadeOut(200);
            }
        }, 100);
    });


    $('#logOn').click(function() {
        $.ajax({
            url: document.LogOnUrl,
            data:
            {
                login: "rick_box@mail.ru",
                password: "hbxfhl91",
                rememberMe: false
            },
            success: function(data) {
                console.log(data);
            }
        });
    });
});