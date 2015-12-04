(function($) {
    //行换色
    $("table tr td").hover(function() {

        $(this).addClass("bgColor");
    }, function() {

        $(this).removeClass("bgColor");
    });




    //全选
    $("#selectAll").click(function () {

        if ($("#selectAll").prop("checked")) {

            $("[name='checkItem']:checkbox").prop("checked", true);

        } else {

            $("[name='checkItem']:checkbox").prop("checked", false);
        }

    });

    //删除消息
    $("#deleteMess").click(function () {


        if (confirm("确定要删除吗？")) {

            var array = $("[name='checkItem']:checkbox");
            var list = [];
            $(array).each(function () {

                if ($(this).prop("checked")) {

                    list.push($(this).val());
                }

            });

            if (list.length != 0) {

                $.ajax({
                    type: "POST",
                    url: "/Enterprise/DeleteMessage",
                    data: { id: list.join(',') },
                    success: function (data) {

                        if (data.status) {

                            window.location.href = window.location.href.replace(/#/g, '');
                        }
                        else {
                            alert(data.message);
                        }
                    }
                });
            } else {
                alert("请选择删除的信息！");
            }
        }


    });


}(jQuery));


function UpdateMessState(obj) {
    var getObj = $(obj).parent().parent().siblings("div");
    var setImg = $(obj).parent().parent().find("a[name='changeImg']");

    //展开的修改信息为已读
    if ($(getObj).css('display') == "none") {

        var messId = $(obj).parent().parent().find("[name='MessId']").val();
        var messState = $(obj).parent().parent().find("[name='MessState']").val();


        if (messState == "False") {
            $.ajax({
                type: "POST",
                url: "/Enterprise/UpdateMessageState",
                data: { id: messId, state: messState },
                success: function (result) {
                    if (result.status) {

                        $(setImg).html("<img src='/images/PublicImg/Read.png' alt='已读'/>");

                    }
                }
            });
        }

        $(getObj).show(500);
    }
    else {
        $(getObj).hide(500);
    }

}
