$(document).ready(function () {
    $('#personalCabinetPage').hide();
    
    $("#radio").buttonset();
    $('#radioPieChart').buttonset();
    
    $("#accountsTable").stupidtable();
    $('#dateFrom').datepicker();
    $('#dateTo').datepicker();
    $('#showByDate').button('destroy');
    $('#showByDate').button();

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

    $('#showByDate').unbind().click(function() {
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

    var chart2;

    var chartData2 = [{
        year: 1930,
        italy: 1,
        germany: 5,
        uk: 3
    }, {
        year: 1934,
        italy: 1,
        germany: 2,
        uk: 6
    }, {
        year: 1938,
        italy: 2,
        germany: 3,
        uk: 1
    }, {
        year: 1950,
        italy: 3,
        germany: 4,
        uk: 1
    }, {
        year: 1954,
        italy: 5,
        germany: 1,
        uk: 2
    }, {
        year: 1958,
        italy: 3,
        germany: 2,
        uk: 1
    }, {
        year: 1962,
        italy: 1,
        germany: 2,
        uk: 3
    }, {
        year: 1966,
        italy: 2,
        germany: 1,
        uk: 5
    }, {
        year: 1970,
        italy: 3,
        germany: 5,
        uk: 2
    }, {
        year: 1974,
        italy: 4,
        germany: 3,
        uk: 6
    }, {
        year: 1978,
        italy: 1,
        germany: 2,
        uk: 4
    }];


    AmCharts.ready(function () {
        // SERIAL CHART
        chart2 = new AmCharts.AmSerialChart();
        chart2.dataProvider = chartData2;
        chart2.categoryField = "year";
        chart2.startDuration = 0.5;
        chart2.balloon.color = "#000000";

        // AXES
        // category
        var categoryAxis = chart2.categoryAxis;
        categoryAxis.fillAlpha = 1;
        categoryAxis.fillColor = "#FAFAFA";
        categoryAxis.gridAlpha = 0;
        categoryAxis.axisAlpha = 0;
        categoryAxis.gridPosition = "start";
        categoryAxis.position = "top";

        // value
        var valueAxis = new AmCharts.ValueAxis();
        valueAxis.title = "Place taken";
        valueAxis.dashLength = 5;
        valueAxis.axisAlpha = 0;
        valueAxis.minimum = 1;
        valueAxis.maximum = 6;
        valueAxis.integersOnly = true;
        valueAxis.gridCount = 10;
        valueAxis.reversed = true; // this line makes the value axis reversed
        chart2.addValueAxis(valueAxis);

        // GRAPHS
        // Italy graph						            		
        var graph = new AmCharts.AmGraph();
        graph.title = "Italy";
        graph.valueField = "italy";
        graph.hidden = true; // this line makes the graph initially hidden           
        graph.balloonText = "place taken by Italy in [[category]]: [[value]]";
        graph.lineAlpha = 1;
        graph.bullet = "round";
        chart2.addGraph(graph);

        // Germany graph
        var graph = new AmCharts.AmGraph();
        graph.title = "Germany";
        graph.valueField = "germany";
        graph.balloonText = "place taken by Germany in [[category]]: [[value]]";
        graph.bullet = "round";
        chart2.addGraph(graph);

        // United Kingdom graph
        var graph = new AmCharts.AmGraph();
        graph.title = "United Kingdom";
        graph.valueField = "uk";
        graph.balloonText = "place taken by UK in [[category]]: [[value]]";
        graph.bullet = "round";
        chart2.addGraph(graph);

        // LEGEND
        var legend = new AmCharts.AmLegend();
        legend.markerType = "circle";
        chart2.addLegend(legend);
        
        // CURSOR
        var chartCursor = new AmCharts.ChartCursor();
        chartCursor.cursorPosition = "mouse";
        chart2.addChartCursor(chartCursor);

        // WRITE
        chart2.write("chartdiv2");
        $('tspan:contains("chart by amcharts.com")').parent().parent().hide();
    });
});

function drawPieChart() {
    var chart;
    
    $('#chartdiv').empty();

    // PIE CHART
    chart = new AmCharts.AmPieChart();

    // title of the chart
    chart.addTitle("Посетители с источников", 16);

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

