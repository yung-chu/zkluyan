$(function () {
    $("#UI li").each(function () {
        var i = $(this).find(".aa-2").find("img").height();
        $(this).find(".tiao").height(parseInt(i+82)+"px");
    });
    var imgWid = 0;
    var imgHei = 0; //变量初始化
    var big = 1.1; //放大倍数
    $(".aa-2 a").hover(function () {
        $(this).find("img").stop(true, true);
        var imgWid2 = 0;
        var imgHei2 = 0; //局部变量
        imgWid = $(this).find("img").width();
        imgHei = $(this).find("img").height();
        imgWid2 = imgWid * big;
        imgHei2 = imgHei * big;
         $(this).parent().parent().siblings('.tiao').css({
            "width": "2px",
            "height": "" +parseInt( imgHei2 + 100)+"px",
            "background": "#3b9ff3",
            "display": "block"
        });
        $(this).find("img").css({ "z-index": 2 });
        $(this).find("img").animate({ "width": imgWid2, "height": imgHei2 });
    }, function () {
        $(this).find("img").stop().animate({ "width": imgWid, "height": imgHei, "z-index": 1 });
        $(this).parent().parent().siblings('.tiao').css({
            "width": "2px",
            "height": ""+parseInt(imgHei+82)+"px",
            "background": "#3b9ff3",
            "display": "block"
        });
    });
});