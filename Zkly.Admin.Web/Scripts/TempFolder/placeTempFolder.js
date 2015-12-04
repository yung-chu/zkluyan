(function ($) {
    $("#btnCheckAll").click(function() {

        if ($('#btnCheckAll').prop('checked')) {

            $("[name='chkItem']:checkbox").prop("checked", true);
        } else {
            $("[name='chkItem']:checkbox").prop("checked", false);
        }
    });


    $("#placeTempFolder").click(function() {

        var array = $("input[name='chkItem']:checkbox");
        var list = [];

        $(array).each(function() {

            if ($(this).prop('checked')) {
                list.push($(this).val());
            }
        });

        if (list.length !== 0) {
            var url = $(this).data('post-url');
            var requestUrl = $(this).data('request-url');//材料详细进入时，返回列表

            $.ajax({
                type: "POST",
                url: url,
                data: { investId: list.join(",") },
                success: function(data) {
                    if (data.status) {
                        alert(data.message);

                        if (requestUrl != undefined && requestUrl !== "") {

                            window.location.href = requestUrl;
                        }
                        else {
                            window.location.href = window.location.href.replace(/#/g, '');
                        }

                    } else {
                        alert(data.message);
                    }
                }
            });
        }
    });

}(jQuery))



