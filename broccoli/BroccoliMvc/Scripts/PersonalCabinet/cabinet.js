$(document).ready(function () {
    $('#personalCabinetPage').hide();
    var deleteContainer;

    // Валидация при вводе номера счета
    $('#activate').click(function () {
        $('body').append('<div id="accountInfo" class="info"></div>');
        var src = $('#srcBtnOk').attr('src');
        var accountInfo = $('#accountInfo');
        var ele = $('#account');
        var pos = ele.offset();
        accountInfo.css({
            top: pos.top - 1,
            left: pos.left + ele.width() + 15
        });
        var error = false;
        var patt = /^[0-9]{5,9}$/i;
        
        if ($.trim(ele.val()).length == 0) {
            error = true;
            accountInfo.addClass('error').html('Введите номер счета').show();
            ele.removeClass('normal').addClass('wrong').css({ 'font-weight': 'normal' });
        } else if (!patt.test(ele.val())) {
            error = true;
            accountInfo.addClass('error').html('Номер счета может содержать только цифры').show();
            ele.removeClass('normal').addClass('wrong').css({ 'font-weight': 'normal' });
        } else if ($.trim(ele.val()).length > 9) {
            error = true;
            accountInfo.addClass('error').html('Превышена максимальная длина').show();
            ele.removeClass('normal').addClass('wrong').css({ 'font-weight': 'normal' });
        }
        else {
            accountInfo.removeClass('error').html('').show();
            ele.removeClass('wrong').addClass('normal');
        }
        
        if (!error) {
            $('#activateForm').submit();
        }
    });
    
    $('.delete-link').click(function() {
        deleteContainer = $(this).parent().parent().parent();

        $.ajax({
            url: document.deleteUrl,
            data: { account: $(this).attr('val') },
            success: function(data) {
                if (data.result) {
                    $('.error-message').hide();
                    deleteContainer.hide();
                    deleteContainer.remove();
                } else {
                    $('.error-message').html(data.reason + '<br /><br />');
                    $('.error-message').show();
                }
            }
        });
    });
});