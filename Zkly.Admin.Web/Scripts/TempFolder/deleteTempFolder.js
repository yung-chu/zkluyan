(function ($) {

    $("#btnCheckAll").click(function() {

        if ($('#btnCheckAll').prop('checked')) {

            $("[name='chkItem']:checkbox").prop("checked", true);
        } else {
            $("[name='chkItem']:checkbox").prop("checked", false);
        }
    });


    $("#deleteTempFolder").click(function() {

        var array = $("input[name='chkItem']:checkbox");
        var list = [];

        $(array).each(function() {

            if ($(this).prop('checked')) {
                list.push($(this).val());
            }
        });

        if (list.length != 0) {
            var url = $(this).data('post-url');
            $.ajax({
                type: "POST",
                url: url,
                data: { id: list.join(",") },
                success: function(data) {
                    if (data.status) {
                        alert(data.message);
                        window.location.href = window.location.href.replace(/#/g, '');
                    } else {
                        alert(data.message);
                    }
                }
            });
        }
    });

}(jQuery))



