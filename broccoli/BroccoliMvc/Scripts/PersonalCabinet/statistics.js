$(document).ready(function () {
    $('#personalCabinetPage').hide();
    
    $("#radio").buttonset();
    $('#radioPieChart').buttonset();
    $('#radioLineChart').buttonset();
    
    $("#accountsTable").stupidtable();
    $('#dateFrom').datepicker();
    $('#dateTo').datepicker();
    $('.period-date button').button();

    $("input[name='radio-period']").change(function () {
        var selectedRadio = $("input[name='radio-period']:checked").val();

        switch (selectedRadio) {
            case 'today':
                window.location = document.statisticsToday;
                break;
            case 'yesterday':
                window.location = document.statisticsYesterday;
                break;
            case 'week':
                window.location = document.statisticsWeek;
                break;
            case 'month':
                window.location = document.statisticsMonth;
                break;
            default:
                window.location = document.statisticsAllTime;
        }
    });

    $('.period-date button').click(function() {
        var startDate = $('#dateFrom').datepicker('getDate');
        var endDate = $('#dateTo').datepicker('getDate');
        
        if (startDate == null) {
            alert('Дата начала периода не выбрана');
            return;
        }
        
        if (endDate == null) {
            alert('Дата окончания периода не выбрана');
            return;
        }
        
        if (startDate > endDate) {
            alert('Дата начала не может быть больше даты окончания периода');
            return;
        }
        
        window.location = document.statisticsByDatePeriod + '?start=' + dateToString(startDate) + '&end=' + dateToString(endDate);
    });

    function dateToString(date) {
        var day = date.getDate();
        var month = date.getMonth() + 1;
        var year = date.getFullYear();
        return month + "." + day + "." + year;
    }

    // открытие саб-грида (группы)
    $('.group').click(function () {
        var group = $(this).attr('group');
        $(this).parent().after($('.' + group));
        
        $('.' + group).show();
        $('.' + group).find('.grid').stupidtable();
        
        // событие для скрытия саб-грида
        $(this).one('click', hideSubGrid);

        $(this).children('.plus-minus').html('-&nbsp;');
    });

    // скрытие саб-грида
    var hideSubGrid = function() {
        $(this).parent().next().hide();
        $(this).children('.plus-minus').html('+&nbsp;');
    };
    
    // скрытие всех саб-гридов
    var hideAllSubGrid = function () {
        $('.subgrid').hide();
        $('#accountsTable').find('.plus-minus').each(function () {
            $(this).html('+&nbsp;');
        });
    };
    
    // событие сортировки главного грида с группами
    $('#sort').find('th').click(hideAllSubGrid);
    
    drawPieChart();
    
    $("input[name='radio-pie-chart']").change(function () {
        drawPieChart();
    });
});

function drawPieChart() {
    var chart;
    
    $('#chartdiv').empty();

    // PIE CHART
    chart = new AmCharts.AmPieChart();

    // title of the chart
    chart.addTitle("Статистика по сайтам", 16);

    chart.dataProvider = chartData;
    chart.titleField = "HostName";
    chart.valueField = $("input[name='radio-pie-chart']:checked").val();;
    chart.sequencedAnimation = true;
    chart.startEffect = "elastic";
    chart.innerRadius = "30%";
    chart.startDuration = 2;
    chart.labelRadius = 25;
    chart.radius = 120;

    // the following two lines makes the chart 3D
    chart.depth3D = 20;
    chart.angle = 25;

    // WRITE                                 
    chart.write("chartdiv");
    $('tspan:contains("chart by amcharts.com")').hide();
}

