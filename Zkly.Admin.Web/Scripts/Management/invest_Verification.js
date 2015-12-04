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
            InvestDate: {
                validators: {
                    notEmpty: {
                        message: '投资时间不能为空'
                    },
                    date: {
                        format: 'YYYY-MM-DD',
                        message: '时间格式不正确'
                    }
                }
            },

            CompanyName: {
                validators: {
                    notEmpty: {
                        message: '公司名称不能为空'
                    },
                    
                }
            },
            AskAmount: {
                validators: {
                    notEmpty: {
                        message: '申请金额不能为空'
                    },
                    numeric: {
                        message: '申请金额输入格式有误'
                    }
                    , lessThan: {
                        value: 1000000,
                        message: '不超过1000000万'
                    },
                    regexp: {
                        regexp: /^[a-zA-Z0-9_\.]+$/,
                        message: '申请金额不能小于0'
                    }
                }
            },
            GainAmount: {
                validators: {
                    notEmpty: {
                        message: '申请金额不能为空'
                    },
                    numeric: {
                        message: '申请金额输入格式有误'
                    }
                    , lessThan: {
                        value: 1000000,
                        message: '不超过1000000万'
                    },
                    regexp: {
                        regexp: /^[a-zA-Z0-9_\.]+$/,
                        message: '申请金额不能小于0'
                    }
                }
            }
        }

    });
}