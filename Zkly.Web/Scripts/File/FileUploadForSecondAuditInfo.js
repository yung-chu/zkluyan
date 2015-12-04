(function($) {

    if ($("#flag").val() !== "") {

        window.parent.FileUploadSuccess($("#fileType").val(), $("#fileHidId").val(), $("#fileId").val(), $("#path").val(), $("#errorInfo").val()); //调用父级页面的Js函数

    }

    if ($("#errorInfo").val() !== "") {

        //返回错误信息到父页面
        window.parent.showUploadFileError($("#fileHidId").val(), $("#errorInfo").val());
    } else {
        window.parent.hideUploadFileError($("#fileHidId").val());
    }

}(jQuery))


function FormSubmit() {

    document.getElementById("fromFile").submit();
}


