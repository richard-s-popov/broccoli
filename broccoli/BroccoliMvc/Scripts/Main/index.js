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
    
    $('#tab1').click(function () {
        $('#stripNav0').find('.tab1').find('a').trigger('click');
        $('.step').removeClass('active');
        $(this).addClass('active');
    });

    $('#tab2').click(function () {
        $('#stripNav0').find('.tab2').find('a').trigger('click');
        $('.step').removeClass('active');
        $(this).addClass('active');
    });

    $('#tab3').click(function () {
        $('#stripNav0').find('.tab3').find('a').trigger('click');
        $('.step').removeClass('active');
        $(this).addClass('active');
    });

    setInterval(function () {
        if (!$('.banner').is(':hover')) {
            $('.banner .stripNavR a').click();
        }
    }, 10000);
    
    $('#tab1').click();
});