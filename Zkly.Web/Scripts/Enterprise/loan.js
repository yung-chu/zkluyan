
var email = /^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
var datePattern = /((^((1[8-9]\d{2})|([2-9]\d{3}))(-)(10|12|0?[13578])(-)(3[01]|[12][0-9]|0?[1-9])$)|(^((1[8-9]\d{2})|([2-9]\d{3}))(-)(11|0?[469])(-)(30|[12][0-9]|0?[1-9])$)|(^((1[8-9]\d{2})|([2-9]\d{3}))(-)(0?2)(-)(2[0-8]|1[0-9]|0?[1-9])$)|(^([2468][048]00)(-)(0?2)(-)(29)$)|(^([3579][26]00)(-)(0?2)(-)(29)$)|(^([1][89][0][48])(-)(0?2)(-)(29)$)|(^([2-9][0-9][0][48])(-)(0?2)(-)(29)$)|(^([1][89][2468][048])(-)(0?2)(-)(29)$)|(^([2-9][0-9][2468][048])(-)(0?2)(-)(29)$)|(^([1][89][13579][26])(-)(0?2)(-)(29)$)|(^([2-9][0-9][13579][26])(-)(0?2)(-)(29)$))/;
var phonePattern = /^((\d{7,8})|(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1}))$/;
var cellPhonePattern = /^1\d{10}$/;
var reg = /^(([1-9][0-9]*\.[0-9][0-9]*)|([0]\.[0-9][0-9]*)|([1-9][0-9]*)|([0]{1}))$/;


(function ($) {


    //不可编辑
    if ($("#invert-apply-loan").length === 0)
    {
        $('#invert-apply-loan-form input, #invert-apply-loan-form textarea, #invert-apply-loan-form select').prop('disabled', true);
        return;

    }


    $('div.apply').find("input[type='checkbox']").each(function () {
        var _this = $(this);
        _this.on('click', function () {
            var textbox = _this.next().next();
            var cls = 'hidden';
            var val = textbox.hasClass(cls);
            textbox.toggleClass(cls, !val);
        });
    });

    $('#foundingDate').datepicker({
        format: 'yyyy-mm-dd'
    });



    /*贷款数据验证*/
    DataValidator();




}(jQuery))


function DataValidator() {


    $('#ApplyLoan').bootstrapValidator({
        //        live: 'disabled',
        message: '输入有误,请重新输入',
        feedbackIcons: {
            valid: 'glyphicon glyphicon-ok',
            invalid: 'glyphicon glyphicon-remove',
            validating: 'glyphicon glyphicon-refresh'
        },
        button: {
            selector: '[id="invert-apply-loan"]:not([formnovalidate])',
            disabled: ''
        },

        fields:
        {
            CompanyName: {
                validators: {
                    notEmpty: {
                        message: '公司名称不能为空'
                    },
                    stringLength: {
                        message: '不能超过100字符',
                        max: function (value, validator, $field) {
                            return 100 - (value.match(/\r/g) || []).length;
                        }
                    }
                }
            },
            CompanyDescription: {
                validators: {
                    notEmpty: {
                        message: '公司简介不能为空'
                    },
                    stringLength: {
                        message: '不能超过4000字符',
                        max: function (value, validator, $field) {
                            return 4000 - (value.match(/\r/g) || []).length;
                        }
                    }
                }
            },
            FoundingDate: {
                validators: {
                    notEmpty: {
                        message: '时间不能为空'
                    },
                    date: {
                        format: 'YYYY-MM-DD',
                        message: '时间格式不正确'
                    }
                }
            },
            ProjectName: {
                validators: {
                    notEmpty: {
                        message: '项目名不能为空'
                    },
                    stringLength: {
                        message: '不能超过100字符',
                        max: function (value, validator, $field) {
                            return 100 - (value.match(/\r/g) || []).length;
                        }
                    }
                }
            },
            ApplyQuota: {
                validators: {
                    notEmpty: {
                        message: '申请金额不能为空'
                    },

                    lessThan: {
                        value: 1000000,
                        message: '不超过1000000万'
                    },
                    regexp: {
                        regexp: /^(([1-9][0-9]*\.[0-9][0-9]*)|([0]\.[0-9][0-9]*)|([1-9][0-9]*)|([0]{1}))$/,
                        message: '输入格式有误'
                    }

                }
            },


            GuaranteeAssessment: {

                    validators: {
                        notEmpty: {
                            message: '抵押物估值不能为空'
                        },

                        lessThan: {
                            value: 1000000,
                            message: '不超过1000000万'
                        },
                        regexp: {
                            regexp: /^(([1-9][0-9]*\.[0-9][0-9]*)|([0]\.[0-9][0-9]*)|([1-9][0-9]*)|([0]{1}))$/,
                            message: '输入格式有误'
                        }

                    }

            },

            Contract: {
                validators: {
                    notEmpty: {
                        message: '联系人不能为空'
                    },
                    stringLength: {
                        message: '不能超过50字符',
                        max: function (value, validator, $field) {
                            return 50 - (value.match(/\r/g) || []).length;
                        }
                    }
                }
            },

            Phone: {
                
                validators: {
                    notEmpty: {
                        message: '手机不能为空'
                    },
                    regexp: {
                        regexp:  /^1\d{10}$/,
                        message: '输入格式有误'
                    }
                }
            },

            Email: {
                validators: {
                    notEmpty: {
                        message: '邮箱不能为空'
                    },
                    emailAddress: {
                        message: '邮箱格式输入有误'
                    }
                }

            }

        }
    });



    /*所属行业*/
    $("#industry").change(function () {

        if ($.trim($("#industry option:selected").val()) === "请选择") {

            $("#industry").css("border", "1px #A94442 solid");
            $("#IndustrySpanError").css("display", "block");
            $("#IndustrySpanError").text("所属行业不能为空");

            $("#IndustryOk").css("display", "none");
            $("#IndustryError").css("display", "block");

        } else {

            $("#industry").css("border", "1px #2B542C solid");

            $("#IndustrySpanError").css("display", "none");
            $("#IndustryError").css("display", "none");
            $("#IndustryOk").css("display", "block");

        }

    });




    /*所在区域*/
    $("#s_county").on("change", function() {

        $("#AreaError").css("display", "none");
        $("#AreaOk").css("display", "block");
        $("#area").css("border", "1px #2B542C solid");
    });



    $("#area").on("keyup", function () {

        if ($(this).val() != "") {

            $("#AreaOk").css("display", "block");
            $("#AreaError").css("display", "none");
            $(this).css("border", "1px #2B542C solid");
            $(this).prop("placeholder", "");


        } else {

            $("#AreaOk").css("display", "none");
            $("#AreaError").css("display", "block");
            $(this).css("border", "1px #A94442 solid");
            $(this).prop("placeholder", "所在地区不能为空");
            $(this).focus();
        }

    });












    /*公司优势*/
    function advSuccess() {

        $("#showAdvError").css("display", "none");

        $("#teamAdv").css("border", "1px #2B542C solid");
        $("#prodAdv").css("border", "1px #2B542C solid");
        $("#techAdv").css("border", "1px #2B542C solid");
        $("#scaleAdv").css("border", "1px #2B542C solid");
        $("#saleAdv").css("border", "1px #2B542C solid");
        $("#industryAdv").css("border", "1px #2B542C solid");
        $("#resourceAdv").css("border", "1px #2B542C solid");
        $("#otherAdv").css("border", "1px #2B542C solid");
    }

    $('#compnayAdvance').find("input[type='checkbox']").each(function () {

        var _this = $(this);
        var textbox = _this.next().next();

        _this.on('change', function () {
            if ($(this).is(':checked') == false && $(this).next().next().val() != "") {
                $(this).prop("checked", "true");
            }
            if ($("#teamAdv").val() === "" && $("#prodAdv").val() === "" && $("#techAdv").val() === "" && $("#scaleAdv").val() === "" && $("#saleAdv").val() === "" && $("#industryAdv").val() === "" && $("#resourceAdv").val() === "" && $("#otherAdv").val() === "") {

                $("#showAdvError").text("公司核心优势至少填一项").css("display", "block");
                $(textbox).css("border", "1px #A94442 solid");
            } else if ($("#teamAdv").val().length > 500 || $("#prodAdv").val().length > 500 || $("#techAdv").val().length > 500 || $("#scaleAdv").val().length > 500 || $("#saleAdv").val().length > 500 || $("#industryAdv").val().length > 500 || $("#resourceAdv").val().length > 500 || $("#otherAdv").val().length > 500) {
                $("#showAdvError").text("不能超过500字符").css("display", "block");
                $(textbox).css("border", "1px #A94442 solid");
            } else {

                advSuccess();
            }


        });
    });

    $('#compnayAdvance').find("input[type='checkbox']").next().next().each(function () {

        $(this).on("keyup", function () {

            if ($(this).prev().prev("input[type='checkbox']").is(':checked') == false && $(this).val() != "") {
                $(this).prev().prev("input[type='checkbox']").prop("checked", "true");
            }
            if ($(this).prev().prev("input[type='checkbox']").is(':checked') == true && $(this).val() == "") {
                $(this).prev().prev("input[type='checkbox']").removeAttr("checked");
            }
            if ($("#teamAdv").val() === "" && $("#prodAdv").val() === "" && $("#techAdv").val() === "" && $("#scaleAdv").val() === "" && $("#saleAdv").val() === "" && $("#industryAdv").val() === "" && $("#resourceAdv").val() === "" && $("#otherAdv").val() === "") {
                $("#showAdvError").text("公司核心优势至少填一项").css("display", "block");
                $(this).css("border", "1px #A94442 solid");

            } else if ($("#teamAdv").val().length > 500 || $("#prodAdv").val().length > 500 || $("#techAdv").val().length > 500 || $("#scaleAdv").val().length > 500 || $("#saleAdv").val().length > 500 || $("#industryAdv").val().length > 500 || $("#resourceAdv").val().length > 500 || $("#otherAdv").val().length > 500) {

                $("#showAdvError").text("不能超过500字符").css("display", "block");
                $(this).css("border", "1px #A94442 solid");
            } else {
                advSuccess();
            }

        });


    });



    ///*财务数据*/
    $("#myData").find("input[type='text']").each(function () {

        var _this = $(this);

        _this.on('keyup', function () {

            var getData = _this.val();

            if (getData !== "") {

                var reg = /^[0-9]+$/;
                if (!getData.match(reg)) {
                    _this.css("border", "1px #A94442 solid");
                    _this.prop("placeholder", "格式错误");

                } else if (getData > 1000000) {
                    _this.css("border", "1px #A94442 solid");
                    _this.prop("placeholder", "金额不能大于一百亿");
                } else {
                    _this.css("border", "2px #2B542C solid");
                    _this.prop("placeholder", "");
                }
            } else {
                _this.css("border", "2px #2B542C solid");
                _this.prop("placeholder", "");
            }
        });

    });





}







function beforeSubmitCheckData()
{
   

    //手动触发验证
    $('#ApplyLoan').bootstrapValidator('validate');

    var result = true;

    if ($("#companyName").val() === "") {
        result = false;
        SetError($("#companyName"), "公司名称不能为空");
    }else {
        SetOk($("#companyName"));
    }

    if ($.trim($("#companyDescription").val()) == "") {
        result = false;
        SetError($("#companyDescription"), "公司简介不能为空");

    } else { SetOk($("#companyDescription")) };


    //成立
    var date = $("#foundingDate").val();
    var result1 = date.match(datePattern);

    if ($("#foundingDate").val() === "") {
        result = false;
        SetError($("#foundingDate"), "公司成立日期不能为空");

    } else if (result1 === null) {
        result = false;
        SetError($("#foundingDate"), "公司成立日期格式不对");
    } else {
        SetOk($("#foundingDate"));
    }

    /*所属行业*/
    if ($.trim($("#industry option:selected").val()) === "请选择") {

        result = false;

        $("#industry").css("border", "1px #A94442 solid");
        $("#IndustrySpanError").css("display", "block");
        $("#IndustrySpanError").text("所属行业不能为空");
    } else { SetOk($("#industry")) }



    /*所在区域*/
    if ($("#area").val() == "") {
        result = false;
        $("#AreaOk").css("display", "none");
        // $("#AreaError").css("display", "block");

        SetError($("#area"), "所在地区不能为空");

    } else { SetOk($("#area")) };

    if ($("#projectName").val() == "") {
        result = false;
        SetError($("#projectName"), "项目名不能为空");

    } else { SetOk($("#projectName")) };

    if ($("#applyQuota").val() == "") {
        result = false;
        SetError($("#applyQuota"), "申请金额不能为空");

    }else if ($("#applyQuota").val() < 0) {
        result = false;
        SetError($("#applyQuota"), "申请金额不能小于0");
    }else {
        SetOk($("#applyQuota"));
    };


    if ($("#guaranteeAssessment").val() == "") {
        result = false;
        SetError($("#guaranteeAssessment"), "抵押物估值不能为空");

    } else if ($("#guaranteeAssessment").val() < 0) {
        result = false;
        SetError($("#guaranteeAssessment"), "抵押物估值不能小于0");
    } else {
        SetOk($("#guaranteeAssessment"));
    };

    if ($("#contract").val() == "") {
        result = false;
        SetError($("#contract"), "联系人不能为空");

    }else {
        SetOk($("#contract"));
    };
   

    if ($("#phone").val() == "") {
        result = false;
        SetError($("#phone"), "手机不能为空");
    }
    else if ($("#phone").val().match(cellPhonePattern)===null && $("#phone").val().match(phonePattern)===null) {
           result = false;
         SetError($("#phone"), "手机格式不正确");
    }
    else {
        SetOk($("#phone"));
    };

  

    if ($("#email").val() == "") {
        result = false;
        SetError($("#email"), "邮箱不能为空");

    } else if ($("#email").val().match(email) === null) {
        result = false;
        SetError($("#email"), "邮箱格式不正确");
    } else {
        SetOk($("#email"));
    };




    /*公司优势*/
    $('#compnayAdvance').find("input[type='checkbox']").next().next().each(function () {
        if ($(this).prev().prev("input[type='checkbox']").is(':checked') == false && $(this).val() != "") {

            result = false;

            $("#showAdvError").text("请勾选公司优势").css("display", "block");
            $(this).css("border", "1px #A94442 solid");
        }
        if ($("#teamAdv").val() === "" && $("#prodAdv").val() === "" && $("#techAdv").val() === "" && $("#scaleAdv").val() === "" && $("#saleAdv").val() === "" && $("#industryAdv").val() === "" && $("#resourceAdv").val() === "" && $("#otherAdv").val() === "") {

            result = false;

            $("#showAdvError").text("公司核心优势至少填一项").css("display", "block");
            $(this).css("border", "1px #A94442 solid");

        } else if ($("#teamAdv").val().length > 4000 || $("#prodAdv").val().length > 4000 || $("#techAdv").val().length > 4000 || $("#scaleAdv").val().length > 4000 || $("#saleAdv").val().length > 4000 || $("#industryAdv").val().length > 4000 || $("#resourceAdv").val().length > 4000 || $("#otherAdv").val().length > 4000) {

            result = false;

            $("#showAdvError").text("不能超过4000字符").css("display", "block");
            $(this).css("border", "1px #A94442 solid");
        }

    });




    /*财务数据*/
    //计算错误
    var sum = 0;
    $("#myData").find("input[type='text']").each(function () {

        var _this = $(this);

        var getData = _this.val();

        if (getData !== "") {

        
            if (!getData.match(reg)) {
                sum += 1;
                result = false;
                _this.css("border", "1px #A94442 solid");
                $("#Prompt").text("格式错误").css("display", "block");
                $("#Prompt").css("color", "#A94442");
                _this.prop("placeholder", "格式错误");

            } else if (getData > 1000000) {
                sum += 1;
                result = false;
                _this.css("border", "1px #A94442 solid");
                $("#Prompt").text("金额不能大于一百亿").css("display", "block");
                _this.prop("placeholder", "金额不能大于一百亿");
            } else {
                _this.css("border", "2px #2B542C solid");
                _this.prop("placeholder", "");
            }
        } else {
            _this.css("border", "2px #2B542C solid");
            _this.prop("placeholder", "");
        }

    });

    if (sum == 0) {
        $("#Prompt").css("display", "none");
    }


    if (!result) {
        alert("验证不通过,请重新填写！");
        return false;
    } 


    $("#ApplyLoan").ajaxSubmit({
        type: "POST",
        url: $("#ApplyLoan").attr('action'),
        success: function (result) {
            if (result.status) {

                window.location = '/Enterprise/loan-success';

            } else { //失败

                alert("验证不通过,请重新填写！");

                var errorArray = result.message.split(",");
                $.AlertMessage.showWarning(errorArray.join("<br/>"));

            }
        }
    });

}



function SetError(obj, word) {
    obj.css("border", "1px #A94442 solid");
    obj.prop("placeholder", word);
    obj.focus();
}

function SetOk(obj) {
    obj.css("border", "1px #2B542C solid");
}