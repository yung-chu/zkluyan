//投资审批Tabs ajax 加载
//(function ($, panel) {
//    if (!panel.length) return;


//    var firstAffix = $('div[data-spy="affix"]:first', panel),
//        affixWidth = firstAffix.width();
//    firstAffix.width(affixWidth);

//    $('>ul>li>a', panel).each(function () {
//        var url = $(this).data('src');
//        if (!url) return;
//        $.get(url, function (html) {
//            $('>.tab-content', panel).append(html).find('div[data-spy="affix"]:last').affix().width(affixWidth);
//        });
//    });

//    $('>.tab-content>.tab-pane', panel).each(function () {
//        $(this).load($(this).data('src'));
//    });

//})(jQuery, jQuery('#invest-tabs'));

(function ($) {
    $('#invest-tabs >.tab-content>.tab-pane').each(function () {
        $(this).load($(this).data('src'));
    });


    $('ul[role="tablist"] >li>a').not('#invest-user,#audit-history').each(
        function () {
            var url = $(this).data('src');
            if (!url) return;

            var tabid = $(this).attr("tab");
            $(this).click(function () {
                if ($("#" + tabid).length == 0) {
                    $.get(url, function (html) {
                        $('#invest-tabs >.tab-content').append($(html).filter("#" + tabid).addClass('active'));
                        $('#invest-second-audit-panel div.ajax-file-holder').each(function () {
                            $(this).load($(this).data('src'));
                        });
                    });

                    $('#invest-tabs >.tab-content>.tab-pane').filter('.active').removeClass('active');
                }
            });

        }
        );
}(jQuery)
);

//审批panel
(function ($, panel) {

    if (!panel.length) return;

    var submitBtn = panel.find('#save-audit'),
        passBtn = $('#pass', panel).parent(),
        failBtn = $('#fail', panel).parent(),
        reasonInput = $('#audit-reason', panel),
        stage = $('input[name=Stage]', panel).val(),
        isFinalStage = stage === 'Roadshow';

    passBtn.highlight('btn-success').click(function () {
        failBtn.removeClass('btn-danger');
        if (isFinalStage) $('#audit-quota', panel).val('').slideDown();
        reasonInput.slideUp();
        submitBtn.prop('disabled', false);
    });

    failBtn.highlight('btn-danger').click(function () {
        passBtn.removeClass('btn-success');
        if (isFinalStage) $('#audit-quota', panel).slideUp();
        reasonInput.slideDown();
        submitBtn.prop('disabled', false);
    });


    $('#save-audit').click(function () {
        if ($('#fail', panel).prop('checked')) {
            if ($('textarea', reasonInput).val().trim() === '') {
                alert('请填写拒绝原因。');
                return false;
            }
        } else if ($('#pass', panel).prop('checked')) {
            $('textarea', reasonInput).val('');
            var quota = $('#audit-quota>input', panel).val();
            if (isFinalStage && !/^\+?([1-9]\d*)$/.test(quota)) {
                alert('请填写正确的审批金额。');
                return false;
            }
        } else {
            alert('请选择通过还是拒绝。');
            return false;
        }
    });

})(jQuery, jQuery('#invest-audit-panel'));


//二审文件ajax加载
(function ($, panel) {

    if (!panel.length) return;

    $('div.ajax-file-holder', panel).each(function () {
        $(this).load($(this).data('src'));
    });

})(jQuery, jQuery('#invest-second-audit-panel'));
