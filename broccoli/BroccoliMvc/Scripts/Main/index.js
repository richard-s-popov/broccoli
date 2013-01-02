$(document).ready(function() {
    $('#indexPage').hide();
    
    $(document).ready(function () {
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
    });
    
    jQuery(window).bind("load", function () {
        jQuery("div#slider1").codaSlider();
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