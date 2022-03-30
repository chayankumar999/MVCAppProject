$(document).ready(function () {
    var minimized_elements = $('a.minimize');

    minimized_elements.each(function () {
        var t = $(this).text();
        if (t.length < 10) return;

        $(this).html(
            t.slice(0, 10) + '</span><a href="#" class="more"><span><i class="bi bi-arrow-down-short"></i></a>' +
            '<span style="display:none;">' + t.slice(10, t.length) + ' <a href="#" class="less"><i class="bi bi-arrow-up-short"></i></a></span>'
        );
    });

    $('a.more', minimized_elements).click(function (event) {
        event.preventDefault();
        $(this).hide().prev().hide();
        $(this).next().show();
    });

    $('a.less', minimized_elements).click(function (event) {
        event.preventDefault();
        $(this).parent().hide().prev().show().prev().show();
    });
});