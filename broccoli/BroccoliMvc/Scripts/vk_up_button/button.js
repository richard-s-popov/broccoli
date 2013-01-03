jQuery.extend(jQuery.fn, {
	toplinkwidth: function(){
	    var totalContentWidth = jQuery('.center-content').outerWidth(); // ширина блока с контентом, включая padding
	    var totalTopLinkWidth = jQuery(this).children('a').outerWidth(true) + 5; // ширина самой кнопки наверх, включая padding и margin
		var h = jQuery(window).width()/2-totalContentWidth/2-totalTopLinkWidth;
		if(h<0){
			// если кнопка не умещается, скрываем её
			jQuery(this).hide();
		} else {
			if($(window).scrollTop() >= 1){
				jQuery(this).show();
			}
			jQuery(this).css({'padding-right': h+'px'});
		}
	}
});

jQuery(function($){
    var topLink = $('#top-link');
    var topLinkFull = $('#top-link-full');
    topLink.css({ 'padding-bottom': $(window).height() });
    topLinkFull.css({ 'padding-bottom': $(window).height() });
	// если вам не нужно, чтобы кнопка подстраивалась под ширину экрана - удалите следующие четыре строчки в коде
	topLinkFull.toplinkwidth();
	$(window).resize(function(){
		topLinkFull.toplinkwidth();
	});
	$(window).scroll(function() {
		if($(window).scrollTop() >= 1) {
		    topLink.fadeIn(300);
		    topLinkFull.fadeIn(300);
		} else {
		    topLink.fadeOut(300);
		    topLinkFull.fadeOut(300);
		}
	});
	topLink.click(function (e) {
	    $("body,html").animate({ scrollTop: 0 }, 500);
	    return false;
	});
	topLinkFull.click(function (e) {
	    $("body,html").animate({ scrollTop: 0 }, 500);
	    return false;
	});

    $('#top-link').mouseenter(function() {
        $(this).children('a').css('color', '#609949');
    });
    
    $('#top-link').mouseleave(function () {
        $(this).children('a').css('color', 'black');
    });
    
    $('#top-link-full').mouseenter(function () {
        $(this).find('a').css('color', '#609949');
        $('#top-link').css('background-color', 'rgba(51, 51, 51, 0.2)');
    });
    
    $('#top-link-full').mouseleave(function () {
        $(this).find('a').css('color', 'black');
        $('#top-link').css('background-color', 'rgba(51, 51, 51, 0.1)');
    });
});