$(document).ready(function () {
    jQuery(window).bind("load", function () {
        jQuery("div#slider1").codaSlider();
        jQuery("div#slider1").show();
    });
    
    jQuery(window).bind("load", function () {
        jQuery("div#mainBanner").codaSlider();
    });
    
    $('#indexPage').hide();
    
    $(".various").fancybox({
        maxWidth: 800,
        maxHeight: 600,
        fitToView: false,
        width: '70%',
        height: '70%',
        autoSize: false,
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
    
    $('#tab1').click();
});