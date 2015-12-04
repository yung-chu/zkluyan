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
            CashPercent: {
                validators: {
                    notEmpty:
                        {
                        message: '现金比例不能为空'
                    },
                    lessThan: {
                        value: 1,
                        message: '现金比例不能超过100%；注：1即代表100%'
                    },
                    regexp:
                        {
                        regexp: /^(([1-9][0-9]*\.[0-9][0-9]*)|([0]\.[0-9][0-9]*)|([1-9][0-9]*)|([0]{1}))$/,
                        message: '现金比例格式输入有误'
                    }
                }
            },

            StockPercent: {
                validators: {
                    notEmpty:
                        {
                            message: '股权比例不能为空'
                        },
                    lessThan: {
                        value: 100,
                        message: '股权比例不能超过100%'
                        },
                    regexp:
                        {
                            regexp: /^(([1-9][0-9]*\.[0-9][0-9]*)|([0]\.[0-9][0-9]*)|([1-9][0-9]*)|([0]{1}))$/,
                            message: '股权比例格式输入有误'
                        }
                }
            },
            Description: {
                validators: {
                   
                    stringLength: {
                        max: 100,
                        message: '描述不得超过100字'
                    }
                   
                }
            },
        }
       
    });
}