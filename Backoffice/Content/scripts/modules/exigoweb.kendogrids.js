// Kendo Grids module
define(["jquery", "pubsub", "kendo"], function ($, pubsub) {

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
                        url: window.location.href,
                        type: "POST",
                        data: function () {
                            return {
                                total: this.total || 0,
                                "__RequestVerificationToken": $("[name='__RequestVerificationToken']").val()
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
            groupable: false,
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
            pdf: {
                allPages: true
            },
            dataBound: function (e) {
                var dataSource = e.sender.dataSource;
                var cache = e.sender.dataSource.transport.options.read;
                var filterCollection = dataSource._filter;
                var lastfilters = cache.lastfilters || [];
                var currentfilters = cache.lastfilters = (filterCollection) ? filterCollection.filters : [];

                var resettotal = false;
                if (dataSource.refresh) {
                    resettotal = true;
                    dataSource.refresh = false;
                }
                else if (($(lastfilters).not(currentfilters).length != 0 || $(currentfilters).not(lastfilters).length != 0) && currentfilters.length != 0) resettotal = true;

                cache.total = (resettotal) ? 0 : dataSource.total();

                if (typeof $(document).trigger !== "function") {
                    pubsub.create(document);
                }

                $(document).trigger('dataBound.kendo.grid', e);
            }
        },
        create: function (selector, options) {
            return $(selector).kendoGrid(module.options(options));
        },
        options: function (options) {
            options = options || {};

            // Extend the settings
            var settings = $.extend(true, {}, options, module.defaults);

            // Overwrite some special settings
            if (options.url) settings.dataSource.transport.read.url = options.url;
            if (options.sort) settings.dataSource.sort = options.sort;
            if (options.filter) settings.dataSource.filter = options.filter;

            if (options.collapsed) {
                $(document).on('dataBound.kendo.grid', function (event, grid) {
                    var $grid = $(grid.sender.element);
                    $grid.find(".k-grouping-row").each(function () {
                        $grid.data('kendoGrid').collapseGroup(this);
                    });
                });
            }

            // Write our overrides in 
            recursivelyPopulateObject(settings, options);

            return settings;
        },


        // Grid Templates
        templates: {
            level: function (field) {
                return function (model) {
                    var html = '';
                    var max = Math.min(10, model[field]);
                    for (var i = max; i > 0; i--) {
                        html += '.';
                    }
                    html += model[field];
                    return html;
                }
            },
            customer: function (customerID, firstName, lastName, token) {
                var id = "#:" + customerID + "#";
                var token = "#:" + token + "#";
                var name = (lastName)
                    ? "#:" + firstName + "# #:" + lastName + "#"
                    : "#:" + firstName + "#";

                var html = [
                    '<div class="media clickable" data-profile="modal" data-id="' + id + '" data-token="' + token + '">',
                        '<div class="media-left pull-left">',
                            '<img class="media-object avatar avatar-sm" style="max-height: 35px;" src="/profiles/avatar/' + id + '" />',
                        '</div>',
                        '<div class="media-body">',
                            name + '<br /> ',
                            '<small class="text-muted">ID ' + id + '</small>',
                        '</div>',
                    '</div>'
                ].join('');

                return kendo.template(html);
            }
        }
    };

    return module;

});