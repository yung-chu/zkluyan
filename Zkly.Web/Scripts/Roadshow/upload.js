(function ($) {

     //表单验证
    FormValidator();


    function BusinessRoadshowUploader() {
        var Uploader = function () {
            this.init = function () {
                var self = this;

                //修改切换
                $("#updateRoadshowVideo").on('click', function ()
                {
                    if ($("#updateRoadshowVideo").text()==="修改") {


                        $("#updateRoadshowVideo").text("播放");
                        $("#playRoadshowVideo").hide();
                        $("#showUploadVideo").show();
                    }
                    else {
                        $("#updateRoadshowVideo").text("修改");
                        $("#playRoadshowVideo").show();
                        $("#showUploadVideo").hide();
                    }

                });



                $('#uploadBtn').on('click', function () {
                    self.execute();
                });

                $('#coverFile').on('change', function () {
                    $(".thumbnail").css("display", "none");
                    $("[name=i]").css("display", "block");
                    $('#showCover').text('图片名称：' + self.getFileName($(this).val()));
                });

                $('#videoFile').on('change', function () {
                    $('#showVideo').text('视频名称：' + self.getFileName($(this).val()));
                });

                $('#protocol').on('click', function () {
                    var isChecked = $(this).attr('checked') === 'checked' || $(this).attr('checked') === undefined;
                    if (isChecked) {
                        $('#uploadBtn').removeClass('disabled');                        
                    } else {
                        $('#uploadBtn').addClass('disabled');
                    }
                });
            },
            this.execute = function () {

                var bar = $('.bar');
                var percent = $('.percent');
                var status = $('#status');

                var self = this,
                    isEdit = $('roadshow-id').val() !== 0;
                   // progressBar = new ProgressBar(),
                   // interval;

                var isValid = this.validate(isEdit);

                if (!isValid) {
                    return;
                }

                this.disable();

                $('#upload-form').ajaxSubmit({
                    //beforeSubmit: function() {
                       //// progressBar.show();
                       //// interval = setInterval(function () {
                       //     try {
                       //         progressBar.update();
                       //     } catch (e){
                       //         console.log(e);
                       //         clearInterval(interval);
                       //     }                            
                       // }, 1000);
                    //},

                    beforeSend: function () {

                        //progressBar.begin()
                        $('div.zkly-progress-bar').removeClass('hidden');
                        status.empty();
                        var percentVal = '0%';
                        bar.width(percentVal);
                        percent.html(percentVal);

                    },
                    uploadProgress: function (event, position, total, percentComplete) {
                        var percentVal = percentComplete + '%';
                        bar.width(percentVal);
                        percent.html(percentVal);

                    },

                    success: function (result) {
                        if (result.status) {

                            //progressBar.done()
                            var percentVal = '100%';
                            bar.width(percentVal);
                            percent.html(percentVal);
                    
                            window.location = '/roadshow/upload-success';
                        } else {

                            var errorArray = result.message.split(",");
                            $.AlertMessage.showWarning(errorArray.join("<br/>"));


                            // progressBar.hide();
                            $('div.zkly-progress-bar').addClass('hidden');
                        }
                        self.enable();
                        //clearInterval(interval);
                    },
                    error: function () {
                       // clearInterval(interval);
                        self.disable();
                    }
                });
            },
            this.disable = function () {
                $('#uploadBtn').addClass('disabled');
            },
            this.enable = function () {
                $('#uploadBtn').removeClass('disabled');
            },
            this.validate = function (isEdit) {
                $.AlertMessage.hide();

                if (!isEdit && $('#videoFile').val() === '') {
                    $.AlertMessage.showWarning('视频文件不能为空，请先选择要上传的视频文件！');
                    return false;
                }

                if (!isEdit && !this.isValidVideoSuffix()) {
                    $.AlertMessage.showWarning('视频格式不正确！');
                    return false;
                }

                if ($('#vedioName').val() === '') {
                    $.AlertMessage.showWarning('视频名称不能为空！');
                    return false;
                }

                if ($('#videoDescription').val() === '') {
                    $.AlertMessage.showWarning('视频简介不能为空！');
                    return false;
                }

                if (!isEdit && $('#coverFile').val() === '') {
                    $.AlertMessage.showWarning('封面文件不能为空，请先选择要上传的封面文件！');
                    return false;
                }

                if (!isEdit && !this.isValidCoverSuffix()) {
                    $.AlertMessage.showWarning('封面格式不正确！');
                    return false;
                }
                
                return true;
            },
            this.getFileName = function (path) {
                return path.substring(path.lastIndexOf('\\') + 1, path.length);
            },
            this.getFileSuffix = function (path) {
                var fileName = this.getFileName(path);
                return fileName.substring(fileName.indexOf('.') + 1, fileName.length);
            },
            this.isValidVideoSuffix = function () {
                var suffix = this.getFileSuffix($('#videoFile').val());
                if ('rm,rmvb,wmv,avi,mpg,mpeg,mp4'.indexOf(suffix) < 0) {
                    return false;
                }
                return true;
            },
            this.isValidCoverSuffix = function () {
                var suffix = this.getFileSuffix($('#coverFile').val());
                if ('jpg,png'.indexOf(suffix) < 0) {
                    return false;
                }
                return true;
            }
        };

        var ProgressBar = function () {
            this.container = $('div.zkly-progress-bar'),
            this.bar = this.container.find('div.progress-bar'),
            this.info = this.container.find('div.current-text'),
            this.loading = this.container.find('#loading'),
            this.guid = $('#Guid'),

            this.show = function () {
                this.container.removeClass('hidden');
            },

            this.hide = function () {
                this.container.addClass('hidden');
            },

            this.update = function () {
                var self = this;
                $.post('/Roadshow/UploadStatus/', $.toJSON({ Guid: this.guid.val() }), function (result) {
                    if (result == null) return;

                    var bar = self.bar;
                    bar.attr('aria-valuenow', result.valueNow);
                    bar.attr('aria-valuemax', result.valueMax);
                    var per = ((result.valueNow / result.valueMax) * 100).toFixed(0);
                    bar.attr('style', 'min-width: 2em; width:' + per + '%');
                    bar.text(per + '%');
                    self.info.text(result.text);
                }, 'json');
            },

            this.done = function () {
                var self = this;
                $.post('/Roadshow/RemoveCache/', $.toJSON({ Guid: this.guid.val() }), function () {
                    self.bar.text('100%');
                    self.info.text("文件上传成功！");
                    self.loading.hide();
                }, 'json');               
            }
        };

        return {
            init: function () {
                new Uploader().init();
            }
        }
    }

    new BusinessRoadshowUploader().init();
}(jQuery));





/*前段验证*/
function FormValidator() {

    $('#upload-form').bootstrapValidator({
        //        live: 'disabled',
        message: '输入有误,请重新输入',
        feedbackIcons: {
            valid: 'glyphicon glyphicon-ok',
            invalid: 'glyphicon glyphicon-remove',
            validating: 'glyphicon glyphicon-refresh'
        },
        button: {
            selector: '[id="uploadBtn"]:not([formnovalidate])',
            disabled: ''
        },

        fields: {

            VideoName: {
                validators: {
                    notEmpty: {
                        message: '公司地址不能为空'
                    }
                }
            },

            VideoDescrition: {

                validators: {
                    notEmpty: {
                        message: '公司注册资本不能为空'
                    }
                }
            }

        }
    });

}