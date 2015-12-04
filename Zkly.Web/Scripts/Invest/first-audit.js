(function ($) {
    if (!$('#first-audit').hasClass('on')) return;


    //不可编辑
    if ($('#first-audit-submit').length === 0) {
        $('#ApplyLoan input, #ApplyLoan textarea, #ApplyLoan select').prop('disabled', true);
        return;
    }


        /*一审数据验证*/
        DataValidator();
   

        $('#first-audit-protocol').click(function () {

            if ($("#first-audit-submit").prop("disabled")!=="") {

                $("#first-audit-submit").removeAttr("disabled");//去除disabled元素
            }

        $('#first-audit-submit').toggleClass('disabled', !$(this).prop('checked'));
    });

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
        format: 'yyyy-mm-dd',
        autoclose: true,
        todayHighlight: true,
        toggleActive: true
    });


 
}(jQuery));




/*法定代表人,联系人手机或电话选填一个*/
var phonePattern = /^((\d{7,8})|(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1}))$/;
var cellPhonePattern = /^1\d{10}$/;
var financialData = /^(([1-9][0-9]*\.[0-9][0-9]*)|([0]\.[0-9][0-9]*)|([1-9][0-9]*)|([0]{1}))$/;
var integer = /^[1-9]\d*$/; /*正整型*/

function DataValidator() {

 
    $('#info-form').bootstrapValidator({
        //        live: 'disabled',
        message: '输入有误,请重新输入',
        feedbackIcons: {
            valid: 'glyphicon glyphicon-ok',
            invalid: 'glyphicon glyphicon-remove',
            validating: 'glyphicon glyphicon-refresh'
        },
        button: {
            selector: '[id="first-audit-submit"]:not([formnovalidate])',
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
                        max: function(value, validator, $field) {
                            return 100 - (value.match(/\r/g) || []).length;
                        }
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
                        max: function(value, validator, $field) {
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
                        message: '申请金额不能小于0'
                    },
                    integer: {
                        message: '只能为整数'
                    }
                }
            },
            LegalPerson: {
                validators: {
                    notEmpty: {
                        message: '法定代表人不能为空'
                    },
                    stringLength: {
                        message: '不能超过50字符',
                        max: function(value, validator, $field) {
                            return 50 - (value.match(/\r/g) || []).length;
                        }
                    }
                }
            },
            Contact: {
                validators: {
                    notEmpty: {
                        message: '联系人不能为空'
                    },
                    stringLength: {
                        message: '不能超过50字符',
                        max: function(value, validator, $field) {
                            return 50 - (value.match(/\r/g) || []).length;
                        }
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

            },
            ProjectAwards: {
                validators: {
                    stringLength: {
                        message: '不能超过500字符',
                        max: function(value, validator, $field) {
                            return 500 - (value.match(/\r/g) || []).length;
                        }
                    }
                }
            },
            IndustryPosition: {
                validators: {
                    stringLength: {
                        message: '不能超过500字符',
                        max: function(value, validator, $field) {
                            return 500 - (value.match(/\r/g) || []).length;
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
            IndustryCompetition: {
                validators: {
                    stringLength: {
                        message: '不能超过500字符',
                        max: function (value, validator, $field) {
                            return 500 - (value.match(/\r/g) || []).length;
                        }
                    }
                }
            },
            MarketAndSales: {
                validators: {
                    stringLength: {
                        message: '不能超过500字符',
                        max: function (value, validator, $field) {
                            return 500 - (value.match(/\r/g) || []).length;
                        }
                    }
                }
            }
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

    $('div.apply').find("input[type='checkbox']").each(function () {

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

    $('div.apply').find("input[type='checkbox']").next().next().each(function () {

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

    /*所属行业*/
    $("#industry").change(function() {

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
    $("#s_county").on("change", function () {

        $("#AreaError").css("display", "none");
        $("#AreaOk").css("display", "block");

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


    ///*财务数据*/
    $("#myData").find("input[type='text']").each(function () {

        var _this = $(this);

        _this.on('keyup', function () {

            var getData = _this.val();

            if (getData !== "") {

                if (!getData.match(financialData)) {
                    _this.css("border", "1px #A94442 solid");
                    _this.prop("placeholder", "格式错误");

                } else if (getData > 1000000) {
                    _this.css("border", "1px #A94442 solid");
                    _this.prop("placeholder", "金额不能大于一百亿");
                }else {
                    _this.css("border", "2px #2B542C solid");
                    _this.prop("placeholder", "");
                }
            } else {
                _this.css("border", "2px #2B542C solid");
                _this.prop("placeholder", "");
            }
        });

    });





    $("#legalPhone").on('keyup', function () {

        if ($("#legalCellPhone").val() != "") {
            return;
        }

        if ($(this).val() !== "") {
            
            if (!$(this).val().match(phonePattern)) {
                $("#LegalSpanError").css("display", "block");
                $("#LegalSpanError").text("电话格式有误");
                $("#LegalError").css("display", "block");
                $(this).css("border", "1px #A94442 solid");

                $("#LegalOk").css("display", "none");

            }
            else
            {
                $("#LegalSpanError").css("display", "none");
                $("#LegalError").css("display", "none");

                $("#LegalOk").css("display", "block");
                $(this).css("border", "1px #2B542C solid");
                $("#legalCellPhone").css("border", "1px #2B542C solid");

            }

        } else {

            $("#LegalError").css("display", "block");
            $("#LegalOk").css("display", "none");
            $(this).css("border", "1px #A94442 solid");
        
            $("#LegalSpanError").css("display", "block");
            $("#LegalSpanError").text("电话或手机必填一个");
        }
    });
    $("#legalCellPhone").on('keyup', function () {

        if ($("#legalPhone").val() != "") {
            return;
        }

        if ($(this).val() !== "") {

            if (!$(this).val().match(cellPhonePattern)) {

                $("#LegalError").css("display", "block");
                $("#LegalOk").css("display", "none");
                $(this).css("border", "1px #A94442 solid");

                $("#LegalSpanError").css("display", "block");
                $("#LegalSpanError").text("手机格式错误");

            } else
            {
                $("#LegalError").css("display", "none");
                $("#LegalOk").css("display", "block");
                $(this).css("border", "1px #2B542C solid");

                $("#legalPhone").css("border", "1px #2B542C solid");
                $("#LegalSpanError").css("display", "none");
            }


        } else {

            $("#LegalError").css("display", "block");
            $("#LegalOk").css("display", "none");
            $(this).css("border", "1px #A94442 solid");
       
            $("#LegalSpanError").css("display", "block");
            $("#LegalSpanError").text("电话或手机必填一个");
        }

    });




    $("#contactPhone").on('keyup', function () {

        if ($("#contactCellPhone").val() !== "") {

            return;
        }


        if ($(this).val() !== "") {

            if (!$(this).val().match(phonePattern)) {

                $("#ContactError").css("display", "block");
                $("#ContactOk").css("display", "none");
                $(this).css("border", "1px #A94442 solid");
                $("#ContactSpanError").css("display", "block");
                $("#ContactSpanError").text("电话格式错误");


            } else {

                $("#ContactError").css("display", "none");
                $("#ContactOk").css("display", "block");
                $(this).css("border", "1px #2B542C solid");

                $("#contactCellPhone").css("border", "1px #2B542C solid");
                $("#ContactSpanError").css("display", "none");
                
            }

        } else {

            $("#ContactError").css("display", "block");
            $("#ContactOk").css("display", "none");
            $(this).css("border", "1px #A94442 solid");
            $("#ContactSpanError").css("display", "block");
            $("#ContactSpanError").text("电话格式错误");
        }
    });
    $("#contactCellPhone").on('keyup', function () {

        if ($("#contactPhone").val() != "") {

            return;
        }

        if ($(this).val() != "") {

            if (!$(this).val().match(cellPhonePattern)) {

                $("#ContactError").css("display", "block");
                $("#ContactOk").css("display", "none");
                $(this).css("border", "1px #A94442 solid");
           
                $("#ContactSpanError").css("display", "block");
                $("#ContactSpanError").text("手机格式错误");

            } else
            {
                $("#ContactError").css("display", "none");
                $("#ContactOk").css("display", "block");
                $(this).css("border", "1px #2B542C solid");

                $("#contactPhone").css("border", "1px #2B542C solid");
                $("#ContactSpanError").css("display", "none");
            }

        } else {

            $("#ContactError").css("display", "block");
            $("#ContactOk").css("display", "none");
            $(this).css("border", "1px #A94442 solid");
            $("#ContactSpanError").css("display", "block");
            $("#ContactSpanError").text("手机格式错误");
        }
    });



    //去除选中
    $("#first-audit-protocol").prop("checked", false);

}



/*提交时检验*/
function checkform_success() {
    var result = true;


    //companyName
    if ($("#companyName").val() === "") {
        result = false;
        SetError($("#companyName"), "公司名称不能为空");
    } else if ($("#companyName").val().length > 100) {
        SetError($("#companyName"), "公司名称长度不能大于100");
    } else {
        SetOk($("#companyName"));
    }

    //成立
    var date = $("#foundingDate").val();
    var result1 = date.match(/((^((1[8-9]\d{2})|([2-9]\d{3}))(-)(10|12|0?[13578])(-)(3[01]|[12][0-9]|0?[1-9])$)|(^((1[8-9]\d{2})|([2-9]\d{3}))(-)(11|0?[469])(-)(30|[12][0-9]|0?[1-9])$)|(^((1[8-9]\d{2})|([2-9]\d{3}))(-)(0?2)(-)(2[0-8]|1[0-9]|0?[1-9])$)|(^([2468][048]00)(-)(0?2)(-)(29)$)|(^([3579][26]00)(-)(0?2)(-)(29)$)|(^([1][89][0][48])(-)(0?2)(-)(29)$)|(^([2-9][0-9][0][48])(-)(0?2)(-)(29)$)|(^([1][89][2468][048])(-)(0?2)(-)(29)$)|(^([2-9][0-9][2468][048])(-)(0?2)(-)(29)$)|(^([1][89][13579][26])(-)(0?2)(-)(29)$)|(^([2-9][0-9][13579][26])(-)(0?2)(-)(29)$))/);
    if ($("#foundingDate").val() === "")
    {
        result = false;
        SetError($("#foundingDate"), "公司成立日期不能为空");

    } else if (result1===null) {
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

        $("#IndustryOk").css("display", "none");
        $("#IndustryError").css("display", "block");

    } else { SetOk($("#industry")) }




    /*所在区域*/
    if ($("#area").val() == "") {

        result = false;
        $("#AreaOk").css("display", "none");
        // $("#AreaError").css("display", "block");

        SetError($("#area"), "所在地区不能为空");

    } else { SetOk($("#area")) };


    //projectName
    if ($("#projectName").val() == "")
    {
        result = false;
        SetError($("#projectName"), "项目名不能为空");

    } else if ($("#projectName").val().length > 100) {
        result = false;
        SetError($("#projectName"), "项目名长度不能大于100");
    } else { SetOk($("#projectName")) };

    //applyQuota
    if ($("#applyQuota").val()=="")
    {
        result = false;
        SetError($("#applyQuota"), "申请金额不能为空");

    } else if ($("#applyQuota").val() > 1000000) {
        result = false;
        SetError($("#applyQuota"), "申请金额不得大于一百亿");
    } else if ($("#applyQuota").val() < 0) {
        result = false;
        SetError($("#applyQuota"), "申请金额不能小于0");
    }
    else if (!$("#applyQuota").val().match(integer))
    {
        result = false;
        SetError($("#applyQuota"), "只能为正整数");
    }
    else {
        SetOk($("#applyQuota"));
    };

    //legalPerson
    if ($("#legalPerson").val()=="")
    {
        result = false;
        SetError($("#legalPerson"), "法定代表人不能为空");

    } else if ($("#legalPerson").val().length > 50) {
        result = false;
        SetError($("#legalPerson"), "法定代表人长度不能大于50");
    } else {
        SetOk($("#legalPerson"));
    };

    /*手机,电话 任意一个非空*/
    if ($("#legalPhone").val() === "" && $("#legalCellPhone").val() === "") {

        result = false;
        $("#LegalOk").css("display", "none");

        //$("#LegalSpanError").css("display","block");
        //$("#LegalSpanError").text("电话或手机必填一个");
        // $("#LegalError").css("display", "block");

        SetError($("#legalPhone"), "电话或手机必填一个");

    } else { SetOk($("#legalPhone")) };

    //contact
    if ($("#contact").val()=="")
    {
        result = false;
        SetError($("#contact"), "联系人不能为空");

    } else if ($("#contact").val().length > 50) {
        result = false;
        SetError($("#contact"), "联系人长度不能大于50");
    } else {
        SetOk($("#contact"));
    };


    if ($("#contactPhone").val() === "" && $("#contactCellPhone").val() === "") {

        result = false;
        $("#ContactOk").css("display", "none");

        //$("#ContactSpanError").css("display", "block");
        //$("#ContactSpanError").text("电话或手机必填一个");
        //$("#ContactError").css("display", "block");


        SetError($("#contactPhone"), "电话或手机必填一个");

    } else { SetOk($("#contactPhone")) };

    /*手机,电话 格式验证*/
     if ($("#legalPhone").val() !== "") {


         if (!$("#legalPhone").val().match(phonePattern)) {

             result = false;
             $("#LegalOk").css("display", "none");

             SetError($("#legalPhone"), "电话格式错误");
         } else {
             SetOk($("#contactPhone"));
         }

     }


     if ($("#legalCellPhone").val() !== "") {

         if (!$("#legalCellPhone").val().match(cellPhonePattern))
         {
             result = false;
             $("#LegalOk").css("display", "none");

             SetError($("#legalPhone"), "手机格式错误");
         } else {

             SetOk($("#legalCellPhone"));

         }

     } 


     if ($("#contactPhone").val() !== "")
     {

         if (!$("#contactPhone").val().match(phonePattern)) {

             result = false;
             $("#ContactOk").css("display", "none");

             SetError($("#contactPhone"), "电话格式错误");
         }
         else {
             SetOk($("#contactPhone"));
         }
     } 


     if ($("#contactCellPhone").val() !== "")
     {

         if (!$("#contactCellPhone").val().match(cellPhonePattern)) {

             result = false;
             $("#ContactOk").css("display", "none");

             // $("#ContactError").css("display", "block");
             //$("#ContactSpanError").css("display", "block");
             //$("#ContactSpanError").text("手机格式错误");

             SetError($("#contactCellPhone"), "手机格式错误");
         } else
         {
             SetOk($("#contactCellPhone"));
         }
     

     } 

    //email
     if ($("#email").val()=="")
     {
         result = false;
         SetError($("#email"), "邮箱不能为空");

     } else { SetOk($("#email")) };

    //companyDescription
     if ($.trim($("#companyDescription").val())== "")
     {
         result = false;
         SetError($("#companyDescription"), "公司简介不能为空");

     } else if ($("#companyDescription").val().length > 4000) {
         result = false;
         SetError($("#companyDescription"), "公司简介长度不能大于4000");
     } else { SetOk($("#companyDescription")) };
    //行业竞争情况
     if ($("#industryCompetition").val().length > 500) {
         result = false;
         SetError($("#industryCompetition"), "行业竞争情况长度不能大于500");
     } else { SetOk($("#industryCompetition")) };
    //市场及营销
     if ($("#marketAndSales").val().length > 500) {
         result = false;
         SetError($("#marketAndSales"), "市场及营销长度不能大于500");
     } else { SetOk($("#marketAndSales")) };
    //所获奖项
     if ($("#projectAwards").val().length > 500) {
         result = false;
         SetError($("#projectAwards"), "所获奖项输入长度不能大于500");
     } else {
         SetOk($("#projectAwards"));
     };
    //行业地位
     if ($("#industryPosition").val().length > 500) {
         result = false;
         SetError($("#industryPosition"), "行业地位输入长度不能大于500");
     } else {
         SetOk($("#industryPosition"));
     };
    /*公司优势*/
     $('div.apply').find("input[type='checkbox']").next().next().each(function () {
         if ($(this).prev().prev("input[type='checkbox']").is(':checked') == false && $(this).val() != "") {

             result = false;

             $("#showAdvError").text("请勾选公司优势").css("display", "block");
             $(this).css("border", "1px #A94442 solid");
         }
         if ($("#teamAdv").val() === "" && $("#prodAdv").val() === "" && $("#techAdv").val() === "" && $("#scaleAdv").val() === "" && $("#saleAdv").val() === "" && $("#industryAdv").val() === "" && $("#resourceAdv").val() === "" && $("#otherAdv").val() === "")
         {
                result = false;

                $("#showAdvError").text("公司核心优势至少填一项").css("display", "block");
                $(this).css("border", "1px #A94442 solid");

         } else if ($("#teamAdv").val().length > 500 || $("#prodAdv").val().length > 500 || $("#techAdv").val().length > 500 || $("#scaleAdv").val().length > 500 || $("#saleAdv").val().length > 500 || $("#industryAdv").val().length > 500 || $("#resourceAdv").val().length > 500 || $("#otherAdv").val().length > 500) {

             result = false;

             $("#showAdvError").text("不能超过500字符").css("display", "block");
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

             if (!getData.match(financialData)) {
                 sum += 1;
                 result = false;
                 _this.css("border", "1px #A94442 solid");
                 $("#Prompt").text("格式错误").css("display", "block");
                 $("#Prompt").css("color", "#A94442");
                 _this.prop("placeholder", "格式错误");

             } else if (getData> 1000000) {
                 sum += 1;
                 result = false;
                 _this.css("border", "1px #A94442 solid");
                 $("#Prompt").text("金额不能大于一百亿").css("display", "block");
                 _this.prop("placeholder", "金额不能大于一百亿");
             }
             else if (!getData.match(integer))
             {
                 sum += 1;
                 result = false;
                 _this.css("border", "1px #A94442 solid");
                 $("#Prompt").text("数据错误").css("display", "block");
                 $("#Prompt").css("color", "#A94442");
                 _this.prop("placeholder", "只能为正整数");
             
             }
             else {
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

         $("#first-audit-protocol").prop("checked", false);

         //去除提交按钮的 disabled
        $("#first-audit-submit").removeAttr("disabled");
         return false;
     } else {
         var test = document.getElementById("first-audit-protocol").checked;
         if (test == false) {
             alert(" 请先阅读协议并勾选前面复选框！");
             return false;
         }
         return true;
     }
   
}



function SetError(obj,word)
{
    obj.css("border", "1px #A94442 solid");
    obj.prop("placeholder", word);
    obj.focus();
}

function SetOk(obj)
{
    obj.css("border", "1px #2B542C solid");
}