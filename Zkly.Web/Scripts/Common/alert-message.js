(function ($) {
    $.AlertType = {
        SUCCESS: 'alert-success',
        WARNING: 'alert-warning',
        DANGER: 'alert-danger',
        INFO: 'alert-info'
    };

    $.AlertMessage = {
        showSuccess: function (message) {
            $.AlertMessage.show({
                alertType: $.AlertType.SUCCESS,
                message: message
            });
        },

        showWarning: function (message) {
            $.AlertMessage.show({
                alertType: $.AlertType.WARNING,
                message: message
            });
        },

        showDanger: function (message) {
            $.AlertMessage.show({
                alertType: $.AlertType.DANGER,
                message: message
            });
        },

        showInfo: function (message) {
            $.AlertMessage.show({
                alertType: $.AlertType.INFO,
                message: message
            });
        },

        show: function(options) {
            var $msg = $('#message');
            $msg.find('span.msg-text').html('').html(options.message);
            $msg.addClass(options.alertType).removeClass('hidden');

            $('button.close').on('click', function () {
                $msg.addClass('hidden');
            });
        },

        hide: function () {
            $('#message').addClass('hidden');
        }
    };
}(jQuery));