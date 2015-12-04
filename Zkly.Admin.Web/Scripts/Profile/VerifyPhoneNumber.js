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
            Code: {
                validators: {
                    notEmpty: {
                        message: '验证码不能为空'
                    },
                    regexp: {
                        regexp: /^[1-9]\d{5}(?!\d)$/,
                        message: '验证码输入格式有误'
                    }
                }
            },
        }

    });
}