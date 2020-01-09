// Trees module
define(["jquery", "app", "ajax", "guids", "bootstrap", "extensions"], function ($, app, ajax, guids) {
    $("body").popover({
        selector: '[data-treenode*="popover"][data-id]',
        html: true,
        trigger: 'hover',
        container: 'body',
        placement: 'top',
        delay: {
            hide: 500
        },
        content: function () {
            $('.popover:visible').hide();

            var customerID = $(this).data('id');
            var viewId = "view-treenode-popover-{0}".format(guids.newGuid());
            ajax.html({
                url: app.path('/popoversummary/' + customerID),
                success: function (response) {
                    $('#' + viewId).html(response);
                }
            });

            return '<div id="' + viewId + '" class="view-treenode-popover"><div class="text-center"><img src="' + app.path("Content/images/circle-24.gif") + '" style="position: relative; top: 18px;" /></div></div>';
        }
    });
});