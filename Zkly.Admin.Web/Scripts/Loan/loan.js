(function ($, panel) {
    //panel.find("label").each(function () {
    //    $(this).on('click', function () {
    //        var val = $(this).find('input').val();
    //        $('#audit-remark').toggleClass('hidden', val);
    //    });
    //});

    if (!panel.length) return;

    var passBtn = $('#pass', panel).parent(),
        failBtn = $('#fail', panel).parent(),
        submitBtn = panel.find('#save-audit'),
         reasonInput = $('#audit-reason', panel);

    passBtn.highlight('btn-success').click(function () {
        failBtn.removeClass('btn-danger');
        reasonInput.slideUp();
        submitBtn.prop('disabled', false);
    });

    failBtn.highlight('btn-danger').click(function () {
        passBtn.removeClass('btn-success');
        reasonInput.slideDown();
        submitBtn.prop('disabled', false);
    });


    $('#save-audit').click(function () {
        if ($('#fail', panel).prop('checked')) {
            if ($('textarea', reasonInput).val().trim() === '') {
                alert('请填写拒绝原因。');
                return false;
            }
        }
    });
}(jQuery, jQuery('#loan-audit-panel')));


(function ($, panel) {
    
    if (!panel.length) return;
   
    
    var firstAffix = $('div[data-spy="affix"]:first', panel),
        affixWidth = firstAffix.width();
    firstAffix.width(affixWidth);
   
    $('>ul>li>a', panel).each(function () {
        var url = $(this).data('src');
        if (!url) return;
        $.get(url, function (html) {
            $('>.tab-content', panel).append(html).find('div[data-spy="affix"]:last').affix().width(affixWidth);
        });
    });

    $('>.tab-content>.tab-pane', panel).each(function () {
        $(this).load($(this).data('src'));
    });

})(jQuery, jQuery('#loan-tabs'));

(function ($, panel) {

    if (!panel.length) return;

    $('div.ajax-file-holder', panel).each(function () {
        $(this).load($(this).data('src'));
    });

})(jQuery, jQuery('#loan-audit-panel'));