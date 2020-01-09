// Profiles module
define(["jquery", "app", "ajax", "guids"], function ($, app, ajax, guids) {
    $(document).popover({
        selector: '[data-profile*="popover"][data-id]',
        html: true,
        trigger: 'hover',
        container: 'body',
        placement: 'top',
        delay: {
            hide: 1000
        },
        content: function () {
            $('.popover:visible').hide();

            var customerID = $(this).data('id');
            var viewId = "view-profilepopover-{0}".format(guids.newGuid());

            ajax.html({
                url: app.path('profile/popoversummary/' + customerID),
                success: function (response) {
                    $('#' + viewId).html(response);
                }
            });

            return '<div id="' + viewId + '" class="view-profilepopover"><div class="text-center"><img src="' + app.path("Content/images/loading/bar-sm.gif") + '" /></div></div>';
        }
    });

    // Open Customer Details Modal
    $(document).on("click", '[data-profile*="modal"][data-id]', function (event) {
        event.preventDefault();        

        // Get the container, or create it if it doesn't yet exist
        var $modal = $('#profilemodal');
        if ($modal.length == 0) {

            // Inject the modal
            var modalHtml = [
            '<div class="modal" id="profilemodal" data-keyboard="false" tabindex="-1" role="dialog" aria-labelledby="profilemodalLabel" aria-hidden="true">',
                '<div class="modal-dialog modal-lg">',
                    '<div class="modal-content">',
                        '<div class="modal-header">',
                            '<button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>',
                            '<h4 class="modal-title" id="profilemodalLabel">Customer Details</h4>',
                        '</div>',
                        '<div class="modal-body">',
                            '<!-- Dynamically loaded content -->',
                        '</div>',
                        '<div class="modal-footer">',
                            '<a class="btn btn-default" data-dismiss="modal">Close</a>',
                        '</div>',
                    '</div>',
                '</div>',
            '</div>'].join('');
            $('body').append(modalHtml);
        }

        var customerID = $(this).data("id");
        var token = $(this).data("token");        

        $.ajax({
            url: app.path('profile/' + token),
            cache: false,
            beforeSend: function () {
                $("#profilemodal .modal-body").html('Loading ID# ' + customerID + ' - please wait...');
                $("#profilemodal").modal('show');
            },
            success: function (html) {
                $("#profilemodal .modal-body").html(html);
            }, error: function (xhr, status, error) {
                $("#profilemodal .modal-body").html('Unable to load ID# ' + customerID + ': ' + error);
                console.log(xhr);
            }
        });
    });
});