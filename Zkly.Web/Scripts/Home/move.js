//滚动速度 数值越大，滚动速度越慢。
var speed = 50;
document.getElementById("gonggao2").innerHTML = document.getElementById("gonggao1").innerHTML;
document.getElementById("XinwenNews2").innerHTML = document.getElementById("XinwenNews1").innerHTML;

//实现滚动的方法
function Marquee() {

    //公告滚动
    if (document.getElementById('gonggao2').offsetWidth - document.getElementById('gonggao').scrollLeft <= 0)
        document.getElementById('gonggao').scrollLeft -= document.getElementById('gonggao1').offsetWidth;
    else {
        document.getElementById('gonggao').scrollLeft++;
    }

    //新闻滚动
    if (document.getElementById('XinwenNews2').offsetWidth - document.getElementById('XinwenNews').scrollLeft <= 0)
        document.getElementById('XinwenNews').scrollLeft -= document.getElementById('XinwenNews1').offsetWidth;
    else {
        document.getElementById('XinwenNews').scrollLeft++;
    }

}
var MyMar = setInterval(Marquee, speed);
document.getElementById("gonggao").onmouseover = function () { clearInterval(MyMar) };
document.getElementById("gonggao").onmouseout = function () { MyMar = setInterval(Marquee, speed) };

document.getElementById("XinwenNews").onmouseover = function () { clearInterval(MyMar) };
document.getElementById("XinwenNews").onmouseout = function () { MyMar = setInterval(Marquee, speed) };






////滚动速度 数值越大，滚动速度越慢。
////var XinwenSpeed = 50;
//document.getElementById("XinwenNews2").innerHTML = document.getElementById("XinwenNews1").innerHTML;
////实现滚动的方法
//function Marquee() {
//    if (document.getElementById('XinwenNews2').offsetWidth - document.getElementById('XinwenNews').scrollLeft <= 0)
//        document.getElementById('XinwenNews').scrollLeft -= document.getElementById('XinwenNews1').offsetWidth;
//    else {
//        document.getElementById('XinwenNews').scrollLeft++;
//    }
//}
//var MyMar = setInterval(Marquee, speed);
//document.getElementById("XinwenNews").onmouseover = function () { clearInterval(MyMar) };
//document.getElementById("XinwenNews").onmouseout = function () { MyMar = setInterval(Marquee, speed) };

