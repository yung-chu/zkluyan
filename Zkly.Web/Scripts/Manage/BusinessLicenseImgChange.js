(function($) {
    
    $("[name=BusinessLicense]").change(function () {
        $(".thumbnail").css("display", "none");
        $("[name=i]").css("display", "block");
        $("#BusinessLicenseError").hide();
        $("#businessLicenseValidation").hide();
    });

}(jQuery))

function checkform_success() {

    var result = true;

    if ($("#BusinessLicense").find("img").length === 0) {

        result = false;
        $("#BusinessLicenseError").show();

    } else {
       
        $("#BusinessLicenseError").hide();
    }

    return result;
}