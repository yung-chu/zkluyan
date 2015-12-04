(function ($) {

    $('#logonForm').bootstrapValidator({
        //        live: 'disabled',
        message: '输入有误,请重新输入',
        feedbackIcons: {
            valid: 'glyphicon glyphicon-ok',
            invalid: 'glyphicon glyphicon-remove',
            validating: 'glyphicon glyphicon-refresh'
        },
        button: {
            selector: '[type="submit"]:not([formnovalidate])',
            disabled: ''
        },

        fields: {
            UserName: {
                validators: {
                    notEmpty: {
                        message: '用户名输入不能为空'
                    }
                }
            },
            Password: {
                validators: {
                    notEmpty: {
                        message: '密码输入不能为空'
                    }
                }
            }
        }
    });


    $("#logon").click(function() {

        //阻止表单提交
        if ($.trim($("#UserName").val()) == ""||$.trim($("#Password").val())=="") {

            //手动触发验证
            $('#logonForm').bootstrapValidator('validate');

            return false;

        }
    });

}(jQuery))