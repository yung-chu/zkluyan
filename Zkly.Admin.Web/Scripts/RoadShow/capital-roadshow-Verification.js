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

            CompanyName: {
                validators: {
                    notEmpty: {
                        message: '公司名称不能为空!'
                    },
                    stringLength: {
                        message: '不能超过100字符',
                        max: 100
                    }
                }
            },
            Subject: {
                validators: {
                    notEmpty: {
                        message: '活动主题不能为空！'
                    },
                    stringLength: {
                        message: '不能超过50字符',
                        max:50
                    }
                }
            },
            StartDate: {
                validators: {
                    notEmpty: {
                        message: '活动开始时间不能为空'
                    },
                    date: {
                        format: 'YYYY/MM/DD HH:mm:ss',
                        message: '时间格式不正确'
                    }
                }
            },
            EndDate: {
                validators: {
                    notEmpty: {
                        message: '活动结束时间不能为空'
                    },
                    date: {
                        format: 'YYYY/MM/DD HH:mm:ss',
                        message: '时间格式不正确'
                    }
                }
            },
            Hoster: {
                validators: {
                    stringLength: {
                        message: '主持人姓名不能超过50字符',
                        max: 50
                    }
                }
            },
            Description: {
                validators: {
                    stringLength: {
                        message: '活动描述不能超过4000字符 6-20位英文字母、数字或组合',
                        max: 4000
                    }
                }
            },
            PublicPassword: {
                validators: {
                    regexp: {
                        regexp: /^[A-Za-z0-9]{6,20}$/,
                        message: '公共密码为6-20位英文字母、数字或组合 '
                    }
                }
            },
            coverFile: {
                validators: {
                    file: {
                        extension: 'png,jpg,jpeg,pjpeg,gif,x-png,bmp',
                        maxSize: 2 * 1024 * 1024,
                        message: '请选择正确的2m以内的图片'
                    }
                }
            }
        }
        
    });
}