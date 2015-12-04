(function ($) {

    if (!$('#second-audit').hasClass('on')) return;

    //不可编辑
    if ($('#second-audit-submit').length === 0) {
        $('#invest-second-audit-form input, #invest-second-audit-form textarea, #invest-second-audit-form select').prop('disabled', true);
        $("div[name='fileUploadIframe']").css("display", "none");
        $("#zkly-float-left").css("display", "none");
        $("#Front_IdCard").css("display", "none");
        return;
    }

    //表单验证
    DataValidator();



    //是否有知识产权
    var isHasIPR = $("input[name='IsHasIPR']");
    $(isHasIPR).click(function () {

        $(this).each(function () {
            if ($(this).val() == "False") {

                $("#showIsHasIPR").css("display", "none");
                $("#PatentInventorSpanError").css("display", "none");
                $("#PatentOwnerSpanError").css("display", "none");

            } else {

                $("#showIsHasIPR").css("display", "block");
            }
        });
    });


    //申请专利号是显示文本框
    var patentStatus = $("input[name='PatentStatus']");
    $(patentStatus).click(function () {

        $(this).each(function () {

            if ($(this).val() != "0") {
                $("#PatentNumber").css("display", "block");
            } else {
                $("#PatentNumber").css("display", "none");
            }
        });
    });

    //项目来源 其他显示文本框
    var projectSource = $("input[name='ProjectSource'][type='radio']");
    $(projectSource).click(function () {

        $(this).each(function () {
            if ($(this).val() != "6") {
                $("#ProjectSourceInfo").css("display", "none");
            } else {
                $("#ProjectSourceInfo").css("display", "block");
            }
        });

    });

    //知识产权形式 其他显示文本框
    var iPRform = $("input[name='IPRform'][type='radio']");
    $(iPRform).click(function () {
        $(this).each(function () {

            if ($(this).val() != "4") {
                $("#IPRformOther").css("display", "none");
            } else {
                $("#IPRformOther").css("display", "block");
                if ($("#IPRformOther").val() == "0") {
                    $("#IPRformOther").val("");
                }
            }
        });

    });


    //公司债务
    $("#currentDebtAmount").click(function () {

        if ($(this).children("#Debt1").prop("checked")) {

            $("#showDebtAmount").css("display", "inline");
        } else {
            $("#showDebtAmount").css("display", "none");
        }
    });

    //公司债务数据校验
    $("#DebtAmount").on("keyup", function () {
        
        var reg = /^[1-9]\d*$/;

        if ($.trim($(this).val())!="") {

            if ($.trim($(this).val())=="0") {

                SetError($(this), "请输入公司债务");
                $("#debtAmountError").show();
                $("#debtAmountError").html("请输入公司债务");
            } else {
                
                if (!$.trim($(this).val()).match(reg))
                {
                    SetError($(this), "格式错误");
                    $("#debtAmountError").show();
                    $("#debtAmountError").html("格式错误");

                } else {
                    
                    SetOk($(this));
                    $("#debtAmountError").hide();
                }
            }
        } else {

            SetError($(this), "请输入公司债务");
            $("#debtAmountError").show();
            $("#debtAmountError").html("请输入公司债务");
            
        }


     
    });






    //同意协议
    $('#second-audit-protocol').click(function () {

        if ($("#second-audit-submit").prop("disabled") !== "") {
            $("#second-audit-submit").removeAttr("disabled");//去除disabled元素
        }

        $('#second-audit-submit').toggleClass('disabled', !$(this).prop('checked'));
    });



    
    //二审提交 enter事件
    var inputElement = $("#invest-second-audit-form input, #invest-second-audit-form  textarea");
    inputElement.keypress(function (event) {
        var key = event.which;
        if (key === 13) {

            $("[id='second-audit-submit']").click();

            $("[id='second-audit-submit']").focus();
            //以上两句实现既支持IE也支持 firefox
        }
    });


}(jQuery));





/*前端验证*/
function DataValidator() {

    $('#upload-form').bootstrapValidator({
        //        live: 'disabled',
        message: '输入有误,请重新输入',
        feedbackIcons: {
            valid: 'glyphicon glyphicon-ok',
            invalid: 'glyphicon glyphicon-remove',
            validating: 'glyphicon glyphicon-refresh'
        },
        button: {
            selector: '[id="second-audit-submit"]:not([formnovalidate])',
            disabled: ''
        },

        fields: {

            Address: {
                validators: {
                    notEmpty: {
                        message: '公司地址不能为空'
                    },
                    stringLength: {
                        max: 500,
                        message: '不超过500个字符'
                    }
                }
            },

            RegisteredCapital: {

                validators: {
                    notEmpty: {
                        message: '公司注册资本不能为空'
                    },
                    regexp: {
                        regexp: /^(([1-9][0-9]*\.[0-9][0-9]*)|([0]\.[0-9][0-9]*)|([1-9][0-9]*)|([0]{1}))$/,
                        message: '格式输入有误'
                    },
                    lessThan: {
                        value: 1000000,
                        message: '不能超过1000000万'
                    },
                    integer: {
                        message: '只能为整数'
                    }
                }
            },


            Introduction: {
                validators: {
                    notEmpty: {
                        message: '简介不能为空'
                    },
                    stringLength: {
                        max: 500,
                        message: '不超过500个字符'
                    }
                }
            },

            ProjectIntroduction: {
                validators: {
                    notEmpty: {
                        message: '项目介绍不能为空'
                    },
                    stringLength: {
                        max: 500,
                        message: '不超过500个字符'
                    }
                }
            },
            Inferiority: {
                validators: {
                    notEmpty: {
                        message: '公司劣势不能为空'
                    },
                    stringLength: {
                        max: 500,
                        message: '不超过500个字符'
                    }
                }
            },
            Plan: {
                validators: {
                    notEmpty: {
                        message: '融资后的战略规划不能为空'
                    },
                    stringLength: {
                        max: 500,
                        message: '不超过500个字符'
                    }
                }
            },
            RiskPrevention: {
                validators: {
                    notEmpty: {
                        message: '目前存在的问题风险及对策不能为空'
                    },
                    stringLength: {
                        max: 500,
                        message: '不超过500个字符'
                    }
                }
            }
        }
    });


    /*发明人*/
    $("#PatentInventor").on("keyup", function() {

        var patentInventor = $.trim($("#PatentInventor").val());

        CheckData(patentInventor, $("#PatentInventor"), "发明人", $("#PatentInventorSpanError"));

    });


    /*所有权人*/
    $("#PatentOwner").on("keyup", function () {

        var patentOwner = $.trim($("#PatentOwner").val());

        CheckData(patentOwner, $("#PatentOwner"), "所有权人", $("#PatentOwnerSpanError"));

    });

}

/*提交前校验*/
function beforeSubmitCheckData() {

    var result = true;
    var showMess = [];

    //手动触发验证
    $('#upload-form').bootstrapValidator('validate');


    //上传文件非空
    if ($("img[id='img1']").attr("src") === "") {

        showMess.push("资信证明需要上传!");
        result= false;
    }
    if ($("img[id='img3']").attr("src") === "") {

        showMess.push("身份证正面需要上传!");
        result = false;
    }
    if ($("img[id='img4']").attr("src") === "") {

        showMess.push("身份证反面需要上传!");
        result = false;
    }
    if ($("img[id='img13']").attr("src") === "") {

        showMess.push("项目图片需要上传!");
        result = false;
    }


    

    //文件，图片--格式,大小验证
    if ($("[name='errorMessInfo']").text() !== "")
    {
        result = false;
    }

    if (showMess.length!==0) {

        $.AlertMessage.showWarning(showMess.join("<br/>"));
    }


    //字段非空，长度验证
    var patentInventor = $.trim($("#PatentInventor").val());
    var patentOwner = $.trim($("#PatentOwner").val());

    if ($("#IsHasIPRRadio1").prop("checked")) {

        if (patentInventor === "") {

            SetError($("#PatentInventor"), "发明人不能为空");
            $("#PatentInventorSpanError").text("发明人不能为空");
            $("#PatentInventorSpanError").css("display", "block");
            result = false;
        } else {

            if (patentInventor.length > 50) {
                $("#PatentInventorSpanError").text("发明人超过50个字符");
                $("#PatentInventorSpanError").css("display", "block");
                result = false;
            }
        }


        if (patentOwner === "") {

            SetError($("#PatentOwner"), "所有权人不能为空");
            $("#PatentOwnerSpanError").text("所有权人不能为空");
            $("#PatentOwnerSpanError").css("display", "block");
            result = false;
        } else {

            if (patentOwner.length > 50) {
                $("#PatentOwnerSpanError").text("所有权人超过50个字符");
                $("#PatentOwnerSpanError").css("display", "block");
                result = false;
            }
        }

    }


    if ($("#Debt1").prop("checked")) {

        var reg = /^[1-9]\d*$/;

        if ($.trim($("#DebtAmount").val()) != "") {

            if ($.trim($("#DebtAmount").val()) == "0") {

                $("#debtAmountError").show();
                $("#debtAmountError").html("请输入公司债务");
                result = false;

            } else {

                if (!$.trim($("#DebtAmount").val()).match(reg)) {

                    $("#debtAmountError").show();
                    $("#debtAmountError").html("格式错误");
                    result = false;


                } else {

                    SetOk($("#DebtAmount"));
                    $("#debtAmountError").hide();
                }
            }
        } else {

            $("#DebtAmount").text("");
            SetError($("#DebtAmount"), "请输入公司债务");
            $("#debtAmountError").show();
            $("#debtAmountError").html("请输入公司债务");
            result = false;

        }


    }


    return result;

}


/*二审提交*/
function checkform_success()
{

        /*校验数据*/
        var result=  beforeSubmitCheckData();
 
        if (!result) {

            alert("验证不通过,请重新填写！");

            $("#second-audit-protocol").prop("checked", false);
            $('#second-audit-submit').addClass('disabled');
            //去除提交按钮的 disabled
            $("#second-audit-submit").removeAttr("disabled");

            return false;
        }
        else {
            var test = document.getElementById("second-audit-protocol").checked;
                if (test == false) {
                    alert(" 请先阅读协议并勾选前面复选框！");
                    return false;
                }
                
        }


        /*提交数据*/

        var bar = $('.bar');
        var percent = $('.percent');
        var status = $('#status');

        $("#upload-form").ajaxSubmit({
            type: "POST",
            url: $("#upload-form").attr('action'),
            dataType: 'json',
            beforeSend: function () {

                //progressBar.begin()
                $('div.zkly-progress-bar').removeClass('hidden');
                status.empty();
                var percentVal = '0%';
                bar.width(percentVal);
                percent.html(percentVal);

            },
            uploadProgress: function (event, position, total, percentComplete) {

                var percentVal = percentComplete + '%';
                bar.width(percentVal);
                percent.html(percentVal);
                //console.log(percentVal, position, total);
            },
            success: function (result) {

                if (result.status) {

                    //progressBar.update();
                    var percentVal = '100%';
                    bar.width(percentVal);
                    percent.html(percentVal);


                    window.location = '/Enterprise/upload-success';

                } else { //失败

                    alert("验证不通过,请重新填写！");

                    // progressBar.hide();
                    $('div.zkly-progress-bar').addClass('hidden');

                    var errorArray = result.message.split(",");
                    $.AlertMessage.showWarning(errorArray.join("<br/>"));

                    $("#second-audit-protocol").prop("checked", false);
                    $('#second-audit-submit').addClass('disabled');
                   
                }
                $('#second-audit-submit').removeClass('disabled');

                return false;
            },
            error: function (data) {

                $.AlertMessage.showDanger(data.message);
                $("#second-audit-protocol").prop("checked", false);
                $('#second-audit-submit').addClass('disabled');
            },
            complete: function (xhr) {
                // status.html(xhr.responseText);
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

function CheckData(value,obj,word,showSpan) {

    if (value === "") {
        SetError(obj, word + "不能为空");
        showSpan.css("display", "none");
    }
    else {

        if (value.length > 100) {
            SetError(obj, word + "不能超过100个字符");
            showSpan.text(word + "不能超过100个字符");
            showSpan.css("display","block");
 
        } else {

            showSpan.css("display", "none");
            SetOk(obj);
        }
    }
}



//文件上传临时文件成功
function FileUploadSuccess(fileType, fileHidId, fileId, pathInfo, errorInfo) {

    //没有错误时
    if (errorInfo == "") {
        var paths = pathInfo.split(":");
        //图片类型
        if (fileType == 1) {

            $("#" + fileId).show();
            $("#" + fileId).attr("src", "/file/FileDisplayByPath?path=" + paths[0] + "&fileName=" + paths[1]);

        } else {

            $("#file" + fileHidId).show();
            $("#fileName" + fileHidId).show();
            $("#fileName" + fileHidId).text("文件名:" + paths[1]);
            $("#file" + fileHidId).attr("href", "/file/TempFileDownLoad?path=" + paths[0] + "&fileName=" + paths[1]);
        }

        $("#hid" + fileHidId).val(pathInfo); //guid+图片名
        $("#fileId" + fileHidId).val(fileHidId);//文件Id
    }
}

//显示文件上传错误信息
function showUploadFileError(fileId, errorInfo) {
    $("#error" + fileId).show();
    $("#error" + fileId).text(errorInfo);
}
//隐藏文件上传错误信息
function hideUploadFileError(fileId) {
    $("#error" + fileId).hide();
    $("#error" + fileId).text("");
}
