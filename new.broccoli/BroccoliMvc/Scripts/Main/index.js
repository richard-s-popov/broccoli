$(document).ready(function () {
    PammTable();

    jQuery(window).bind("load", function () {
        jQuery("div#slider1").codaSlider();
        jQuery("div#slider1").show();
    });
    
    jQuery(window).bind("load", function () {
        jQuery("div#mainBanner").codaSlider();
        jQuery("div#mainBanner").show();
    });
    
    $('#indexPage').hide();
    
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

    //setInterval(function () {
    //    if (!$('.banner').is(':hover')) {
    //        $('.banner .stripNavR a').click();
    //    }
    //}, 10000);
    
    $('#da-slider').cslider({
        autoplay: true,
        bgincrement: 50
    });
    
    $('#countdown_dashboard').countDown({
        targetOffset: {
            'day': 0,
            'month': 0,
            'year': 0,
            'hour': 0,
            'min': 15,
            'sec': 00
        },
        format: 'odHM'
    });
});

function PammTable()
{
    $('.blur').blurjs({
        source: 'body',
        radius: 10
    });

    $('#small').spinner({
        step: 10,
        min: 10,
        max: 1499,
        start: 1000
    }).on('change', function () {
        var val = this.value,
            $this = $(this),
            max = $this.spinner('option', 'max'),
            min = $this.spinner('option', 'min');
        if (!val.match(/^\d+$/)) val = 0; //we want only number, no alpha
        this.value = val > max ? max : val < min ? min : val;
    });

    $('#medium').spinner({
        step: 100,
        min: 1500,
        max: 2999,
        start: 2000
    }).on('change', function () {
        var val = this.value,
            $this = $(this),
            max = $this.spinner('option', 'max'),
            min = $this.spinner('option', 'min');
        if (!val.match(/^\d+$/)) val = 0; //we want only number, no alpha
        this.value = val > max ? max : val < min ? min : val;
    });

    $('#large').spinner({
        step: 100,
        min: 3000,
        start: 5000
    }).on('change', function () {
        var val = this.value,
            $this = $(this),
            max = $this.spinner('option', 'max'),
            min = $this.spinner('option', 'min');
        if (!val.match(/^\d+$/)) val = 0; //we want only number, no alpha
        this.value = val > max ? max : val < min ? min : val;
    });

    $('.pamm-offer-table tbody td:not(.broker-column)').mouseenter(function () {
        var index = $(this).index();

        $('.pamm-offer-table tbody tr').each(function () {
            $('td', this).eq(index).css('background', '#FFF5E5');
        });
    }).mouseleave(function () {
        var index = $(this).index();

        $('.pamm-offer-table tbody tr').each(function () {
            $('td', this).eq(index).css('background', '#F4F5F7');
        });
    });

    $('.small-container .ui-icon.ui-icon-triangle-1-n').click(CalcSmall);
    $('.small-container .ui-icon.ui-icon-triangle-1-s').click(CalcSmall);
    $('.small-container .ui-spinner-input').change(CalcSmall);
    $('.small-container .ui-spinner-input').keyup(CalcSmall);

    CalcSmall();
}