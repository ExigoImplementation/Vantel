// Custom sort module
define(["jquery", "jquery-ui", "app", "ajax", "toastr", "bootstrap"], function ($, jqueryui, app, ajax, toastr, bootstrap) {
    
    var module = {
        CustomSort: function (data) {

            var sortGroupID = data.sortGroup,
                sortURL = data.sortUrl,
                saveURL = data.saveUrl,
                parentID = data.parentId,
                customerid = data.customerid,
                axis = data.axis,
                containment = data.containment,
                placeholderClass = data.placeholderClass,
                sortGroupContainer = $('[data-sort-group="' + sortGroupID + '"]'),
                sortGroupNodes = sortGroupContainer.find("[data-sort-node]");

            if (parentID != "" && parentID != undefined && parentID != null) {
                sortGroupContainer = $('[data-parent-ID="' + parentID + '"]');
                sortGroupNodes = sortGroupContainer.find('[data-sort-node]');
            }

            if (placeholderClass == "" || placeholderClass == undefined || placeholderClass == null) {
                placeholderClass = false;
            }

            if (axis == "" || axis == undefined || axis == null) {
                axis = false;
            }

            if (containment == "" || containment == undefined || containment == null) {
                containment = false;
            }

            var sortOptions = {
                axis: axis,
                containment: containment,
                tolerance: "pointer",
                revert: "invalid",
                handle: ".sortable-handle",
                placeholder: placeholderClass,
                stop: function (event, ui) {

                    var sortedNodes = sortGroupContainer.find("[data-sort-node]");                    
                    var sortTarget = sortGroupID;

                    if (parentID != "" && parentID != undefined && parentID != null) {
                        sortTarget = parentID;
                    }                   

                    var btnSave = $('[data-sort-target="' + sortTarget + '"]');
                    btnSave.addClass("activated");

                        btnSave.off("click").on("click", function () {
                            var sortValues = [];
                            var counter = 1;
                            sortedNodes.each(function () {
                                var nodeID = $(this).data("sortNode"),
                                    sortIndex = counter++;
                                sortValues.push({nodeID:nodeID,sortIndex:sortIndex});
                            });

                            ajax.json({
                                url: saveURL,
                                contentType: 'application/json',
                                data: {
                                    customerID: customerid,
                                    groupID: sortGroupID,
                                    parentID: parentID,
                                    sortValues: sortValues
                                },
                                success: function (response) {
                                    if (response.success) {
                                        toastr.success("Your changes have been saved.");
                                        btnSave.removeClass("activated");
                                    } else {
                                        toastr.error("Your changes could not be saved at this time. Please try again later.");                                        
                                    }
                                }
                            });
                        });

                }
            };

            // Make the items in the sortable container draggable and sortable
            sortGroupContainer.sortable(sortOptions);

            // Sort the elements on the stage according to the sort order determined in the extended tables
            this.initializeSort = function () {
                ajax.json({
                    url: sortURL,
                    data: {
                        groupID: sortGroupID,
                        customerID: customerid
                    },
                    success: function(response) {
                        if (response.success) {
                            var nodes = sortGroupContainer.find('[data-sort-node]');

                            // Create array to store nodes in their preferred order
                            var sortedNodes = [];
                            for (var i = 1, nodeCount = response.sortedNodes.length; i <= nodeCount; i++) {
                                sortedNodes.push(sortGroupContainer.find('[data-sort-node="' + response.sortedNodes[i - 1].NodeID + '"]'));
                            }

                            // Move nodes into place, one at a time, using the sortedNodes array.
                            // $previousNode represents the last node placed.
                            // The first node is place before all existing nodes,
                            // then each successive node is placed after the last item placed.
                            var $previousNode = sortedNodes[0];
                            for (var i = 0; i < response.sortedNodes.length; i++) {
                                if (i == 0) {
                                    $previousNode.insertBefore(nodes.first());
                                } else {
                                    var $nextNode = sortedNodes[i];
                                    $nextNode.insertAfter($previousNode);
                                    $previousNode = $nextNode;
                                }
                            }
                        }
                    },
                    complete: function () {
                        // Once loading is complete, show the nodes in the browser
                        sortGroupContainer.find("[data-sort-node]").css("opacity", 1);
                    }
                });
            }
            if (sortURL != "" && sortURL != undefined && sortURL != null) {
                this.initializeSort();
            }
        },

        init: function (sortGroups) {
            sortGroups.each(function () {
                var $data = $(this).data();
                var sort = module.CustomSort($data);
            });
        }
    };

    // Get all containers targeted for custom sorting
    var $dataSortGroups = $("[data-sort-group]");
    module.init($dataSortGroups);

    return module;
});