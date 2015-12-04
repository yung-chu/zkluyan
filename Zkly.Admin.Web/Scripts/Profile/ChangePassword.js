(function ($) {

    //表单验证
    DataValidator();
}(jQuery));

/*前段验证*/
function DataValidator() {
    $('#info-form').bootstrapValidator({
        message: '输入有误,请重新输入',
        feedbackIcons: {
            valid: 'glyphicon glyphicon-ok',
            invalid: 'glyphicon glyphicon-remove',
            validating: 'glyphicon glyphicon-refresh'
        },
        fields: {
            OldPassword: {
                validators: {
                    notEmpty: {
                        message: '密码不能为空'
                    },
                    regexp: {
                        regexp: /^(?=.*[0-9]+)(?=.*[a-z]+).{6,16}$/,
                        message: '密码长度6到16位,必须有数字和小写字母'
                    }
                }
            },
            NewPassword: {
                validators: {
                    notEmpty: {
                        message: '密码不能为空'
                    },
                    regexp: {
                        regexp: /^(?=.*[0-9]+)(?=.*[a-z]+).{6,16}$/,
                        message: '密码长度6到16位,必须有数字和小写字母'
                    }
                }
            },
            ConfirmPassword: {
                validators: {
                    notEmpty: {
                        message: '确认密码不能为空'
                    },
                    identical: {
                        field: 'Password',
                        message: '密码输入不一致'
                    }
                }
            }
        }

    });
}