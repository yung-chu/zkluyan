(function ($) {
    $("#sms-register").click(function(){
        DataValidator();
})
    //表单验证
    //DataValidator();
}(jQuery));

/*前段验证*/
function DataValidator() {
    alert("=");
    $('#info-form').bootstrapValidator({
        message: '输入有误,请重新输入',
        feedbackIcons: {
            valid: 'glyphicon glyphicon-ok',
            invalid: 'glyphicon glyphicon-remove',
            validating: 'glyphicon glyphicon-refresh'
        },
        fields: {
            userName: {
                validators: {
                    notEmpty: {
                        message: '用户名不能为空'
                    },
                    regexp: {
                        regexp: /^(?=.*[0-9]+)(?=.*[a-z]+).{6,16}$/,
                        message: '用户名长度是6到16位,必须有数字和小写字母'
                    }
                }
            },

            email: {
                validators: {
                    notEmpty: {
                        message: '电子邮箱不能为空'
                    },
                    emailAddress: {
                        message: '电子邮箱格式输入有误'
                    }
                }
            },
            mobile: {
                validators: {
                    notEmpty: {
                        message: '手机号码不能为空'
                    },
                    regexp: {
                        regexp: /^1\d{10}$/,
                        message: '手机号码输入格式有误'
                    }
                }
            },
            phone: {
                validators: {
                    notEmpty: {
                        message: '电话不能为空'
                    },
                    regexp: {
                        regexp: /^((\d{7,8})|(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1}))$/,
                        message: '电话输入格式有误'
                    }
                }
            },
            fax: {
                validators: {
                    notEmpty: {
                        message: '传真不能为空'
                    },
                    regexp: {
                        regexp: /^((\+?[0-9]{2,4}\-[0-9]{3,4}\-)|([0-9]{3,4}\-))?([0-9]{7,8})(\-[0-9]+)?$/,
                         message: '传真输入格式有误'
                     }
                }
            },
            postcode: {
                validators: {
                    notEmpty: {
                        message: '邮编不能为空'
                    },
                    regexp: {
                        regexp: /^[1-9]\d{5}(?!\d)$/,
                        message: '邮编输入格式有误'
                    }
                }
            },
            company: {
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
            address: {
                validators: {
                    notEmpty: {
                        message: '公司地址不能为空'
                    },
                    stringLength: {
                        max: 100,
                        message: '不超过100个字符'
                    }
                }
            },
        }

    });
}