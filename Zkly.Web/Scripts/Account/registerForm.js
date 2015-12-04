(function($) {
    
    $('#registForm').bootstrapValidator({
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
                verbose: false,//Client first, server last
                validators: {
                    notEmpty: {
                        message: '用户名不能为空'
                    },
                    regexp: {
                        regexp: /^(?=.*[0-9]+)(?=.*[a-z]+).{6,16}$/,
                        message: '用户名长度是6到16位,必须有数字和小写字母'
                    },
                    remote: {

                        url: '/Account/CheckIsExistUserName',
                        type: 'POST',
                        delay: 2000     // Send Ajax request every 2 seconds

                    }
                }
            },


            Email: {
                verbose: false,//Client first, server last
                validators: {
                    notEmpty: {
                        message: '电子邮箱不能为空'
                    },
                    emailAddress: {
                        message: '电子邮箱格式输入有误'
                    },
                    remote: {

                        url: '/Account/CheckIsExistEmail',
                        type: 'POST',
                        delay: 2000     // Send Ajax request every 2 seconds

                    }
                }
            },

            PhoneNumber: {
                verbose: false,//Client first, server last
                validators:
                    {
                        notEmpty: {
                            message: '手机号码不能为空'
                        },
                        regexp: {
                            regexp: /^1\d{10}$/,
                            message: '手机号码输入格式有误'
                        },
                        remote: {

                            url: '/Account/CheckIsExistPhone',
                            type: 'POST',
                            delay: 2000     // Send Ajax request every 2 seconds

                        }
                    }
            },

            Role: {
                validators:
                    {
                        notEmpty: {
                            message: '角色不能为空'
                        }
                    }
            },


            Password: {
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





}(jQuery))