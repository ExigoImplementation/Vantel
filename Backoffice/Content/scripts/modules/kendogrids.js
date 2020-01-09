// AJAX module
define(["jquery", "urls", "pubsub", "kendo"], function ($, url) {

    function recursivelyPopulateObject(settings, options) {
        for (var prop in options) {
            if (typeof options[prop] === "object") {
                recursivelyPopulateObject(settings[prop], options[prop]);
            }
            else {
                settings[prop] = options[prop];
            }
        }
    }

    var module = {
        defaults: {
            dataSource: {
                transport: {
                    read: {
                        url: url.current.path,
                        type: "POST",
                        data: function () {
                            return {
                                total: this.total || 0
                            };
                        }
                    }
                },
                schema: {
                    data: "data",
                    total: "total"
                },
                serverPaging: true,
                serverSorting: true,
                serverFiltering: true
            },
            height: kendo.support.mobileOS.wp ? "24em" : 630,
            navigatable: false,
            columnMenu: false,
            groupable: true,
            sortable: true,
            reorderable: true,
            selectable: false,
            resizable: true,
            filterable: {
                extra: false
            },
            pageable: {
                pageSize: 25,
                pageSizes: [10, 25, 50],
                numeric: true,
                refresh: true,
                input: false,
                info: true,
                buttonCount: 5
            },
            dataBound: function (e) {
                var dataSource = e.sender.dataSource;
                var cache = e.sender.dataSource.transport.options.read;
                var filterCollection = dataSource._filter;

                var lastfilters = cache.lastfilters || [];
                var currentfilters = cache.lastfilters = (filterCollection) ? filterCollection.filters : [];

                var resettotal = false;
                if (($(lastfilters).not(currentfilters).length != 0 || $(currentfilters).not(lastfilters).length != 0) && currentfilters.length != 0) resettotal = true;

                cache.total = (resettotal) ? 0 : dataSource.total();

                $(document).trigger('dataBound.kendo.grid');
            }
        },

        options: function (options) {
            options = options || {};

            // Extend the settings
            var settings = $.extend(true, {}, options, module.defaults);

            // Overwrite some special settings
            if (options.url) settings.dataSource.transport.read.url = options.url;

            // Write our overrides in 
            recursivelyPopulateObject(settings, options);

            return settings;
        }
    };

    return module;

});