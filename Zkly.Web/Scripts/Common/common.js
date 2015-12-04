var NS = (function (global) {
    function namespace() { }

    return function (space) {
        var bits = space.split(".");
        var gl = global;
        var lastRef = null;
        for (var i = 0; i < bits.length; i++) {
            lastRef = bits[i];
            if (typeof global[lastRef] !== "undefined") {
                if (global[lastRef] instanceof namespace === false) {
                    throw new Error("Non-namespace object exists.");
                }
                gl = gl[lastRef];
                continue;
            }
            gl[lastRef] = new namespace();
            gl = gl[lastRef];
        }
        return gl;
    }
}(this));

///////////////////////////////////////////////////////////////////////////////////////
///jQuery.post原生方法post不了数据，要加上contentType: "application/json"才能post数据
///////////////////////////////////////////////////////////////////////////////////////
(function ($) {
    $.ajaxWraper = {
        post: function (url, jsonData, success) {
            $.ajax({
                url: url,
                type: "POST",
                dataType: "json",
                contentType: "application/json",
                data: jsonData,
                success: success
            });
        },

        get: function (url, success) {
            $.ajax({
                url: url,
                type: "GET",
                contentType: "application/json",
                success: success
            });
        }
    };

}(jQuery));

//实现文件预览代码
function PreViewImg(fileId,imgId) {
    $("#" + fileId).on("change", function () {
        var file = this.files[0];
        if (this.files && file) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $("#" + imgId).attr("src", e.target.result);
            };
            reader.readAsDataURL(file);
        }
    });
}
