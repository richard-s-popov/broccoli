$(document).ready(function () {
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