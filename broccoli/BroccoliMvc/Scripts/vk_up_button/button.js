jQuery.extend(jQuery.fn, {
	toplinkwidth: function(){
	    var totalContentWidth = jQuery('.center-content').outerWidth(); // ширина блока с контентом, включая padding
	    console.log(totalContentWidth);
	    var totalTopLinkWidth = jQuery(this).children('a').outerWidth(true) + 50; // ширина самой кнопки наверх, включая padding и margin
	    console.log(totalTopLinkWidth);
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
	topLink.css({'padding-bottom': $(window).height()});
	// если вам не нужно, чтобы кнопка подстраивалась под ширину экрана - удалите следующие четыре строчки в коде
	//topLink.toplinkwidth();
	//$(window).resize(function(){
	//	topLink.toplinkwidth();
	//});
	$(window).scroll(function() {
		if($(window).scrollTop() >= 1) {
			topLink.fadeIn(300);
		} else {
			topLink.fadeOut(300);
		}
	});
	topLink.click(function (e) {
	    $("body,html").animate({ scrollTop: 0 }, 500);
	    return false;
	});

    $('#top-link').mouseenter(function() {
        $(this).children('a').css('color', '#609949');
    });
    
    $('#top-link').mouseleave(function () {
        $(this).children('a').css('color', 'black');
    });
});