$(document).ready(function() {
    $('#personalCabinetPage').hide();

    var error = false;
    
    var validate = function() {
        $('body').append('<div id="orderInfo" class="info"></div>');
        var src = $('#srcBtnOk').attr('src');
        var orderInfo = $('#orderInfo');
        var ele = $('#AccountId');
        var pos = ele.offset();
        orderInfo.css({
            top: pos.top + 1,
            left: pos.left + ele.width() + 20
        });

        var patt = /^[0-9]*$/i;

        if (!patt.test(ele.val())) {
            error = true;
            orderInfo.addClass('error').html('Выберите торговый счет').show();
            ele.removeClass('normal').addClass('wrong').css({ 'font-weight': 'normal' });
        } else {
            orderInfo.removeClass('error').html('').show();
            ele.removeClass('wrong').addClass('normal');
        }
    };

    $('#order').click(function() {
        validate();
        
        if (!error) {
            $('#activateForm').submit();
        }
    });
    $('#AccountId').change(validate);
});