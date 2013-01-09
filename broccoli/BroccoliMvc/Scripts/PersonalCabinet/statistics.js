$(document).ready(function () {
    $('#personalCabinetPage').hide();
    
    $("#accountsTable").tablesorter();
    
    var chart;

    var chartData = [{
        country: "United States",
        visits: 9252
    }, {
        country: "China",
        visits: 1882
    }, {
        country: "Japan",
        visits: 1809
    }, {
        country: "Germany",
        visits: 1322
    }, {
        country: "United Kingdom",
        visits: 1122
    }, {
        country: "France",
        visits: 1114
    }, {
        country: "India",
        visits: 984
    }, {
        country: "Spain",
        visits: 711
    }];


    AmCharts.ready(function () {
        // PIE CHART
        chart = new AmCharts.AmPieChart();

        // title of the chart
        chart.addTitle("Visitors countries", 16);

        chart.dataProvider = chartData;
        chart.titleField = "country";
        chart.valueField = "visits";
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
    });
});