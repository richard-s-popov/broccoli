$(document).ready(function () {
    var src = $('#srcBtnOk').attr('src');
    $('input').attr('autocomplete', 'off');
    $('#registerPage').hide();
    
    var jVal = {
        'name': function () {
            $('body').append('<div id="nameInfo" class="info"></div>');
            var nameInfo = $('#nameInfo');
            var ele = $('#name');
            var pos = ele.offset();
            
            nameInfo.css({
                top: pos.top - 3,
                left: pos.left + ele.width() + 15
            });
            if ($.trim(ele.val()).length < 6) {
                jVal.errors = true;
                nameInfo.addClass('error').html('Введите Имя и Фамилию').show();
                ele.removeClass('normal').addClass('wrong');
            } else if (ele.val().length > 255) {
                jVal.errors = true;
                nameInfo.addClass('error').html('Превышен максимальный размер 255 символов').show();
                ele.removeClass('normal').addClass('wrong').css({ 'font-weight': 'normal' });
            } else {
                nameInfo.removeClass('error').html('<img src="' + src + '" />').show();
                ele.removeClass('wrong').addClass('normal');
            }
        },
        'birthday': function () {
            $('body').append('<div id="birthdayInfo" class="info"></div>');
            var birthday = $('#birthdayInfo');
            var ele = $('#birthday');
            var pos = ele.parent().offset();
            ele.val($('#Day :selected').val() + '.' + $('#Month :selected').val() + '.' + $('#Year :selected').val());

            birthday.css({
                top: pos.top + 7,
                left: pos.left + ele.width() + 25
            });
            if ($('#Day :selected').val() == '0' || $('#Month :selected').val() == '0' || $('#Year :selected').val() == '0') {
                jVal.errors = true;
                birthday.addClass('error').html('Выберите дату рождения').show();
                ele.removeClass('normal').addClass('wrong');
            } else if (!DateIsValid(ele.val())) {
                jVal.errors = true;
                birthday.addClass('error').html('Такая дата не существует').show();
                ele.removeClass('normal').addClass('wrong').css({ 'font-weight': 'normal' });
            } else {
                birthday.removeClass('error').html('<img src="' + src + '" />').show();
                ele.removeClass('wrong').addClass('normal');
            }
        },
        'email': function () {
            $('body').append('<div id="emailInfo" class="info"></div>');
            var emailInfo = $('#emailInfo');
            var ele = $('#email');
            var pos = ele.offset();
            emailInfo.css({
                top: pos.top - 3,
                left: pos.left + ele.width() + 15
            });

            var patt = /^.+@.+[.].{2,}$/i;
            
            if ($.trim(ele.val()).length > 0) {
               if (!patt.test(ele.val())) {
                    jVal.errors = true;
                    emailInfo.addClass('error').html('Введите корректный Email').show();
                    ele.removeClass('normal').addClass('wrong');
               } else if (ele.val().length > 50) {
                    jVal.errors = true;
                    emailInfo.addClass('error').html('Превышен максимальный размер 50 символов').show();
                    ele.removeClass('normal').addClass('wrong').css({ 'font-weight': 'normal' });
               } else {
                    $.getJSON(document.EmailCheckUrl + '?email=' + $.trim(ele.val()), function(data) {
                        var emailInfo1 = $('#emailInfo');
                        var el = $('#email');
                        var pos1 = ele.offset();
                        emailInfo1.css({
                            top: pos1.top - 3,
                            left: pos1.left + ele.width() + 15
                        });
                            
                        if (data.result == true) {
                            jVal.errors = true;
                            emailInfo1.addClass('error').html('Пользователь с таким Email-ом уже существует').show();
                            el.removeClass('normal').addClass('wrong');
                        } else {
                            emailInfo1.removeClass('error').html('<img src="' + src + '" />').show();
                            el.removeClass('wrong').addClass('normal');
                        }
                    });
                }
            } else {
                jVal.errors = true;
                emailInfo.addClass('error').html('Введите Email').show();
                ele.removeClass('normal').addClass('wrong');
            }
        },
        'nickname': function () {
            $('body').append('<div id="nicknameInfo" class="info"></div>');
            var el = $('#nickname');
            
            if ($.trim(el.val()).length > 0) {
                $.getJSON(document.NicknameCheckUrl + '?nickname=' + $.trim(el.val()), function (data) {
                    var nicknameInfo = $('#nicknameInfo');
                    var ele = $('#nickname');
                    var pos = ele.offset();
                    nicknameInfo.css({
                        top: pos.top - 3,
                        left: pos.left + ele.width() + 15
                    });
                    
                    if (data.result == true) {
                        jVal.errors = true;
                        nicknameInfo.addClass('error').html('Пользователь с таким Ником уже существует').show();
                        ele.removeClass('normal').addClass('wrong');
                    } else if (ele.val().length > 50) {
                        jVal.errors = true;
                        nicknameInfo.addClass('error').html('Превышен максимальный размер 50 символов').show();
                        ele.removeClass('normal').addClass('wrong').css({ 'font-weight': 'normal' });
                    }  else {
                        nicknameInfo.removeClass('error').html('<img src="' + src + '" />').show();
                        ele.removeClass('wrong').addClass('normal');
                    }
                });
            } else {
                $('#nicknameInfo').removeClass('error').html('').show();
                el.removeClass('wrong').addClass('normal');
            }
        },
        'country': function () {
            $('body').append('<div id="countryInfo" class="info"></div>');
            var countryInfo = $('#countryInfo');
            var ele = $('#country');
            var pos = ele.offset();
            countryInfo.css({
                top: pos.top - 3,
                left: pos.left + ele.width() + 15
            });

            var patt = /^[a-zA-Zа-яА-Я\s]*$/i;
            
            if (!patt.test(ele.val())) {
                jVal.errors = true;
                countryInfo.addClass('error').html('Поле может содержать только латинские и кириллические символы').show();
                ele.removeClass('normal').addClass('wrong').css({ 'font-weight': 'normal' });
            } else if (ele.val().length > 50) {
                jVal.errors = true;
                countryInfo.addClass('error').html('Превышен максимальный размер 50 символов').show();
                ele.removeClass('normal').addClass('wrong').css({ 'font-weight': 'normal' });
            } else if (ele.val().length == 0) {
                countryInfo.removeClass('error').html('').show();
                ele.removeClass('wrong').addClass('normal');
            } else {
                countryInfo.removeClass('error').html('<img src="' + src + '" />').show();
                ele.removeClass('wrong').addClass('normal');
            }
        },
        'city': function () {
            $('body').append('<div id="cityInfo" class="info"></div>');
            var cityInfo = $('#cityInfo');
            var ele = $('#city');
            var pos = ele.offset();
            cityInfo.css({
                top: pos.top - 3,
                left: pos.left + ele.width() + 15
            });
            
            var patt = /^[a-zA-Zа-яА-Я\s]*$/i;

            if (!patt.test(ele.val())) {
                jVal.errors = true;
                cityInfo.addClass('error').html('Поле может содержать только латинские и кириллические символы').show();
                ele.removeClass('normal').addClass('wrong').css({ 'font-weight': 'normal' });
            } else if (ele.val().length > 50) {
                jVal.errors = true;
                cityInfo.addClass('error').html('Превышен максимальный размер 50 символов').show();
                ele.removeClass('normal').addClass('wrong').css({ 'font-weight': 'normal' });
            } else if (ele.val().length == 0) {
                cityInfo.removeClass('error').html('').show();
                ele.removeClass('wrong').addClass('normal');
            } else {
                cityInfo.removeClass('error').html('<img src="' + src + '" />').show();
                ele.removeClass('wrong').addClass('normal');
            }
        },
        'phone': function () {
            $('body').append('<div id="phoneInfo" class="info"></div>');
            var phoneInfo = $('#phoneInfo');
            var ele = $('#phone');
            var pos = ele.offset();
            phoneInfo.css({
                top: pos.top - 3,
                left: pos.left + ele.width() + 15
            });
            
            var patt = /^([\+]{1})?[0-9\-]*$/i;

            if (ele.val().length != 0 && (ele.val().length > 15 || ele.val().length < 5 || !patt.test(ele.val()))) {
                jVal.errors = true;
                phoneInfo.addClass('error').html('Введите корректный номер телефона').show();
                ele.removeClass('normal').addClass('wrong').css({ 'font-weight': 'normal' });
            } else if (ele.val().length == 0) {
                phoneInfo.removeClass('error').html('').show();
                ele.removeClass('wrong').addClass('normal');
            } else {
                phoneInfo.removeClass('error').html('<img src="' + src + '" />').show();
                ele.removeClass('wrong').addClass('normal');
            }
        },
        'password': function () {
            $('body').append('<div id="passwordInfo" class="info"></div>');
            var passwordInfo = $('#passwordInfo');
            var ele = $('#password');
            var pos = ele.offset();
            passwordInfo.css({
                top: pos.top - 3,
                left: pos.left + ele.width() + 15
            });
            
            if (ele.val().length == 0) {
                jVal.errors = true;
                passwordInfo.addClass('error').html('Введите пароль').show();
                ele.removeClass('normal').addClass('wrong').css({ 'font-weight': 'normal' });
            } else if (ele.val().length > 32 || ele.val().length < 6) {
                jVal.errors = true;
                passwordInfo.addClass('error').html('Длина пароля должна быть от 6 до 32 символов').show();
                ele.removeClass('normal').addClass('wrong').css({ 'font-weight': 'normal' });
            } else if ($('#confirmPassword').val().length > 0 && $('#confirmPassword').val() != ele.val()) {
                jVal.errors = true;
                passwordInfo.addClass('error').html('Пароли не совпадают').show();
                ele.removeClass('normal').addClass('wrong').css({ 'font-weight': 'normal' });
            } else {
                passwordInfo.removeClass('error').html('<img src="' + src + '" />').show();
                ele.removeClass('wrong').addClass('normal');
            }
        },
        'confirmPassword': function () {
            $('body').append('<div id="confirmPasswordInfo" class="info"></div>');
            var confirmPasswordInfo = $('#confirmPasswordInfo');
            var ele = $('#confirmPassword');
            var pos = ele.offset();
            confirmPasswordInfo.css({
                top: pos.top - 3,
                left: pos.left + ele.width() + 15
            });
            
            if (ele.val().length == 0) {
                jVal.errors = true;
                confirmPasswordInfo.addClass('error').html('Введите пароль').show();
                ele.removeClass('normal').addClass('wrong').css({ 'font-weight': 'normal' });
            } else if (ele.val().length > 32 || ele.val().length < 6) {
                jVal.errors = true;
                confirmPasswordInfo.addClass('error').html('Длина пароля должна быть от 6 до 32 символов').show();
                ele.removeClass('normal').addClass('wrong').css({ 'font-weight': 'normal' });
            } else if ($('#password').val().length > 0 && $('#password').val() != ele.val()) {
                jVal.errors = true;
                confirmPasswordInfo.addClass('error').html('Пароли не совпадают').show();
                ele.removeClass('normal').addClass('wrong').css({ 'font-weight': 'normal' });
            } else {
                if ($('#password').val().length > 0) {
                    jVal.password();
                }
                confirmPasswordInfo.removeClass('error').html('<img src="' + src + '" />').show();
                ele.removeClass('wrong').addClass('normal');
            }
        },
        'sendIt': function () {
            if (!jVal.errors) {
                $('#form').submit();
            }
        }
    };
    // ====================================================== //
    $('#send').click(function () {
        var obj = $.browser.webkit ? $('body') : $('html');
        obj.animate({ scrollTop: $('#form').offset().top }, 750, function () {
            jVal.errors = false;
            jVal.name();
            jVal.birthday();
            jVal.email();
            jVal.nickname();
            jVal.country();
            jVal.city();
            jVal.password();
            jVal.confirmPassword();
            jVal.phone();
            jVal.sendIt();
        });
        return false;
    });
    $('#name').change(jVal.name);
    $('#nickname').change(jVal.nickname);
    $('#city').change(jVal.city);
    $('#country').change(jVal.country);
    $('#password').change(jVal.password);
    $('#confirmPassword').change(jVal.confirmPassword);
    $('#birthday').change(jVal.birthday);
    $('#email').change(jVal.email);
    $('#phone').change(jVal.phone);

    $('#nickname').keyup(function (e) {
        if (e.keyCode == 8 && $.trim($('#nickname').val()).length == 0) {
            jVal.nickname();
            $('#nickname').blur();
            $('#nickname').focus();
        }
    });
});

function DateIsValid(text) {
    var comp = text.split('.');
    var m = parseInt(comp[0], 10);
    var d = parseInt(comp[1], 10);
    var y = parseInt(comp[2], 10);
    var date = new Date(y,m-1,d);
    if (date.getFullYear() == y && date.getMonth() + 1 == m && date.getDate() == d) {
        return true;
    } else {
        return false;
    }
}