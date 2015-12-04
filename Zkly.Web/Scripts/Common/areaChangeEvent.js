(function($) {
    _init_area();
    $('#s_county').on('change', function () {
        $('#area').val($('#s_province').val() + $('#s_city').val() + $('#s_county').val());
    });
}(jQuery))