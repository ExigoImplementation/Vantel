define(function () {

    var module = {
        popup: function (request) {
            // Private functions
            function getOption(option, defaultValue) {
                var result = defaultValue;

                if (typeof option !== "undefined") {
                    result = (option == true) ? "yes" : "no";
                }

                return result;
            }

            // Determine the variables
            var width = request.width || 1020,
                height = request.height || 600,
                screenX = typeof window.screenX != 'undefined' ? window.screenX : window.screenLeft,
                screenY = typeof window.screenY != 'undefined' ? window.screenY : window.screenTop,
                outerWidth = typeof window.outerWidth != 'undefined' ? window.outerWidth : document.documentElement.clientWidth,
                outerHeight = typeof window.outerHeight != 'undefined' ? window.outerHeight : (document.documentElement.clientHeight - 22),
                monitorX = (screenX < 0) ? window.screen.width + screenX : screenX,
                left = parseInt(monitorX + ((outerWidth - width) / 2), 10),
                top = parseInt(screenY + ((outerHeight - height) / 3), 10),
                popup = {
                    url: request.url || '',
                    name: request.name || 'PopupWindow',
                    specs: [
                        'width={0}'.format(width),
                        'height={0}'.format(height),
                        'top={0}'.format(top),
                        'left={0}'.format(left),
                        'location={0}'.format(getOption(request.location, 'no')),
                        'directories={0}'.format(getOption(request.directories, 'no')),
                        'fullscreen={0}'.format(getOption(request.fullscreen, 'no')),
                        'menubar={0}'.format(getOption(request.menubar, 'no')),
                        'resizable={0}'.format(getOption(request.resizable, 'yes')),
                        'scrollbars={0}'.format(getOption(request.scrollbars, 'yes')),
                        'status={0}'.format(getOption(request.status, 'no')),
                        'toolbar={0}'.format(getOption(request.toolbar, 'no')),
                        'copyhistory={0}'.format(getOption(request.copyhistory, 'no'))
                    ].join(','),
                    replace: request.replace || true
                };

            // Check for queries
            if (request.query) {
                // Assemble the query
                var query = '';
                var separator = (popup.url.contains('?') ? '&' : '?');
                if (request.query) {
                    for (var prop in request.query) {
                        query += '{0}{1}={2}'.format(separator, prop, encodeURIComponent(request.query[prop]));
                        separator = '&';
                    }
                }

                popup.url += query;
            }

            // Open the popup window
            window.open(popup.url, popup.name, popup.specs, popup.replace);
        }
    };

    return module;
});