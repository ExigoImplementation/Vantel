// Cart model
define(["jquery", "app", "ajax"], function ($, app, ajax) {
    
    var model = function (settings) {

        // Settings
        var defaults = {
            replaceExistingItemQuantities: true
        };
        this.options = $.extend(defaults, settings, {});

        // Properties
        this.items = [];

        // Item Management
        this.getItem = function (id) {
            for (var i = 0, max = this.items.length; i < max; i++) {
                if (this.items[i].ID === id) {
                    return this.items[i];
                }
            }
        };
        this.addItem = function (item) {

            // Update any existing items
            var itemsToUpdate = [];


            // If the item is not new, check by it's ID
            for (var i = 0, max = this.items.length; i < max; i++) {
                if (this.items[i].ID === item.ID) {
                    itemsToUpdate.push(item);
                }
            }

            // If the item is new, check by it's properties.
            if (itemsToUpdate.length == 0) {
                for (var i = 0, max = this.items.length; i < max; i++) {
                    if (this.items[i].ItemCode === item.ItemCode) {
                        itemsToUpdate.push(this.items[i]);
                    }
                }
            }

            // Add or update the item
            if (itemsToUpdate.length == 0) {
                this.items.push(item);
            }
            else {
                for (var x = 0, max = itemsToUpdate.length; x < max; x++) {
                    if (this.options.replaceExistingItemQuantities) itemsToUpdate[x].Quantity = item.Quantity;
                    else itemsToUpdate[x].Quantity += item.Quantity;
                }
            }


            // Cleanup the items
            this._cleanup();
        };
        this.removeItem = function (item) {
            for (var i = 0, max = this.items.length; i < max; i++) {
                if (this.items[i].ID == item.ID) {
                    this.items[i].Quantity = 0;
                }
            }

            // Cleanup the items
            this._cleanup();
        };
        this._cleanup = function () {

            // Remove all items with a quantity of 0
            for (var i = 0, max = this.items.length; i < max; i++) {
                var item = this.items[i];
                if (item.Quantity <= 0) {
                    this.items.splice(i, 1);
                    max--;
                    i--;
                }
            }
        };

        // Calculation
        this.subtotal = function () {
            var subtotal = 0;

            for (var i = 0, max = this.items.length; i < max; i++) {
                var item = this.items[i];
                subtotal += (item.Price * item.Quantity);
            }

            return subtotal;
        };
        this.calculate = function (data) {
            data = data || {};
            data.items = data.items || this.items;

            if (data.items.length == 0) return { then: function () { console.warn("Could not calculate (no items in cart)"); } };

            return ajax.json({
                url: app.path('shopping/calculateorder'),
                data: data
            });
        }

        return this;
    };

    return model;

});