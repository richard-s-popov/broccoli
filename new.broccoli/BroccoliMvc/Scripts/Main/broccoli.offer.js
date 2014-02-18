$(document).ready(function() {
    $('.money').spinner({
        step: 100,
        min: 0,
        start: 1000
    });
    
    $('.percent').spinner({
        step: 0.5,
        min: 5,
        numberFormat: "n"
    });

    $('.ui-icon.ui-icon-triangle-1-n').click(CalcProfit);
    $('.ui-icon.ui-icon-triangle-1-s').click(CalcProfit);
    $('.ui-spinner-input').change(CalcProfit);
    $('.ui-spinner-input').keyup(CalcProfit);

    CalcProfit();

    $(".various").fancybox({
        maxWidth: 1000,
        maxHeight: 900,
        fitToView: false,
        width: '100%',
        height: '80%',
        autoSize: true,
        closeClick: false,
        openEffect: 'elastic',
        closeEffect: 'none'
    });
});

function CalcProfit(parameters) {
    var monthSum = $('#startSum').spinner('value');
    var percent = $('#percent').spinner('value');
    var transfer = $('#transfer').spinner('value');

    for (var i = 1; i <= 24; i++) {
        if (i % 3 == 0) {
            $('#calculator tr#month-' + i).addClass('dark');
        }
        
        if (i % 6 == 0) {
            $('#calculator tr#month-' + i).addClass('bold');
        }

        monthSum = monthSum + monthSum * percent / 100 + transfer;
        
        $('#calculator tr#month-' + i).children('.month-name').html(i + ' месяц');
        $('#calculator tr#month-' + i).children('.month-sum').html(monthSum.toFixed(2) + ' $');
        $('#calculator tr#month-' + i).children('.month-percent').html(percent + ' %');
        $('#calculator tr#month-' + i).children('.month-transfer').html(transfer + ' $');
    }
}