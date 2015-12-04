(function ($) {
    $("#FirstAuditInfo").change(function () {
        var v = $("#FirstAuditInfo option:selected").text();
           
        if (v === "===请选择===") {
            $("#CompanyName").val("");
        } else {
            $("#CompanyName").val(v);
        }
    });
}(jQuery));