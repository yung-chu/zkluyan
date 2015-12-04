//user details page
(function ($, panel) {

    if (!panel.length) return;

    $('input[name=LockoutEndDateUtc]', panel).popover();

})(jQuery, jQuery('#user-details-panel'));


//user create page
(function ($, panel) {

    if (!panel.length) return;

    $('input[data-content]', panel).attr({
        "data-toggle": "popover",
        "data-container": "body",
        "data-placement": "top",
        "data-trigger": "focus"
    }).popover();

    var role = $('#user-role-group').data('active'),
        radioBtn = $('#user-role-group>label>input[value="' + role + '"]').parent();
    radioBtn.click();

})(jQuery, jQuery('#create-user'));