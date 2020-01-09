// Cart Item model
define(["guids"], function (guids) {
    
    var model = function (product, quantity) {
        product = product || {};
        product.Quantity = quantity;

        if (product.ID == "00000000-0000-0000-0000-000000000000") product.ID = guids.newGuid();

        product.serialize = function () {
            return {
                ItemCode: this.itemcode,
                Quantity: this.quantity,
                ParentItemCode: this.parentitemcode
            };
        };

        return product;
    };

    return model;

});