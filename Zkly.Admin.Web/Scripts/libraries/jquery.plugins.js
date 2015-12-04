(function ($) {

	$.fn.hoverClass = function (hoverClass, normalClass) {
		return this.hover(function() {
			$(this).addClass(hoverClass);
			normalClass && $(this).removeClass(normalClass);
		}, function() {
			normalClass && $(this).addClass(normalClass);
			$(this).removeClass(hoverClass);
		});
	};

    //bootstrap button group highlight effect
	$.fn.highlight = function (activeClass) {
	    return this.hover(function () {
	        $(this).addClass(activeClass);
	    }, function () {
	        !$(this).hasClass('active') && $(this).removeClass(activeClass);
	    }).click(function () {
	        $(this).addClass(activeClass).siblings().removeClass(activeClass);
	    });
	};

})(jQuery);
