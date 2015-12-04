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
           
            Number: {
                validators: {
                    notEmpty: {
                        message: '手机号码不能为空'
                    },
                    regexp: {
                        regexp: /^1\d{10}$/,
                        message: '手机号码输入格式有误'
                    }
                }
            }
        }

    });
}