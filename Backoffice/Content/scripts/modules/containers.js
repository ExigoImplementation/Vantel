// Containers module
define(["jquery", "pubsub"], function ($, pubsub) {

    $(function () {

        // Bind expandable containers
        $(document).on({
            'expand.app.container': function (event) {
                $('.container-expandable').addClass('expanded');

                setTimeout(function () {
                    $(window).triggerHandler('resize');
                    $(document).trigger('expanded.app.container');
                }, 500);
            },
            'compress.app.container': function (event) {
                $('.container-expandable').removeClass('expanded');

                setTimeout(function () {
                    $(window).triggerHandler('resize');
                    $(document).trigger('compressed.app.container');
                }, 500);
            }
        });
        $('[data-toggle="container"]').on('click', function () {
            if ($(this).hasClass('expanded')) {
                $(this).removeClass('expanded');

                $('[data-toggle="container"] i').removeClass().addClass('fa-expand');

                $(document).trigger('compress.app.container');
            }
            else {
                $(this).addClass('expanded');

                $('[data-toggle="container"] i').removeClass().addClass('fa-compress');
                $(document).trigger('expand.app.container');
            }
        });
    });

});