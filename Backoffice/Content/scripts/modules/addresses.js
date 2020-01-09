// Address module
define(["jquery", "app", "pubsub", "ajax", "bootstrap"], function ($, app, pubsub, ajax) {

    var module = {
        bind: function (context) {
            var $context = $(context);

            $('[data-region-bind]', $context).on('change', function (event, args) {

                if (args && args._init) {
                    return;
                }

                var $this = $(this),
                    country = $this.val(),
                    region = $this.data('region-bind'),
                    $region = $((!region.contains('#')) ? '#' + region : region);

                $.ajax({
                    url: app.path("app/getregions/" + country),
                    type: 'GET',
                    cache: true,
                    success: function (response) {
                        if (response.success) {

                            var html = '',
                                regions = response.regions;
                            
                            // Assemble the new region options
                            for (var i = 0, length = regions.length; i < length; i++) {
                                var region = regions[i];
                                html += '<option value="{0}">{1}</option>'.format(
                                    region.RegionCode,
                                    region.RegionName);
                            }

                            // Populate the regions
                            $region.html(html);

                            // Send a notification that this element has finished populating its regions
                            window.trigger('regionspopulated', $region);
                        }
                    }
                });
            });


            // Toggle the visibility of the Address2 field if the country is US.
            $('[data-address-bind]', $context).on('change', function (event, args) {
                var $this = $(this),
                    country = $this.val(),
                    address = $this.data('address-bind'),
                    $address = $((!address.contains('#')) ? '#' + address : address),
                    $wrapper = $address.parents('.form-group').first();

                if (country == "US") {
                    if (args && args._init) $wrapper.hide();
                    else $wrapper.slideUp('fast');
                }
                else {
                    if (args && args._init) $wrapper.show();
                    else $wrapper.slideDown('fast');
                }
            }).triggerHandler('change', [{ _init: true }]);


            // Toggle the verify address option if the country is US.
            $('[data-role="Country"]', $context).on('change', function (event, args) {
                var $this = $(this),
                    country = $this.val(),
                    $verifybuttonwrapper = $('[data-role="verify"]', $context).parents('.row').first();

                if (country == "US") {
                    if (args && args._init) $verifybuttonwrapper.show();
                    else $verifybuttonwrapper.slideDown('fast');
                }
                else {
                    if (args && args._init) $verifybuttonwrapper.hide();
                    else $verifybuttonwrapper.slideUp('fast');
                }
            }).triggerHandler('change', [{ _init: true }]);


            // Enable address verification
            $('[data-role="verify"]', $context).on('click', function (event, args) {
                var $this = $(this),
                    $verifybutton = $('[data-role="verify"]', $context);

                module.verifyAddress(context);
            });
        },
        getAddress: function (selector) {
            var $selector = $(selector);
            if ($selector.length == 0) return null;


            var result = {};

            result.$address1 = $selector.find('[data-role="Address1"]');
            result.address1 = result.$address1.val();

            result.$address2 = $selector.find('[data-role="Address2"]');
            result.address2 = result.$address2.val();

            result.$city = $selector.find('[data-role="City"]');
            result.city = result.$city.val();

            result.$state = $selector.find('[data-role="State"]');
            result.state = result.$state.val();

            result.$zip = $selector.find('[data-role="Zip"]');
            result.zip = result.$zip.val();

            result.$country = $selector.find('[data-role="Country"]');
            result.country = result.$country.val();

            return result;
        },
        clearAddress: function (selector) {
            var address = module.getAddress(selector);
            if (address == null) return;

            address.$address1.val('');
            address.$address2.val('');
            address.$city.val('');
            address.$zip.val('');
        },
        copyAddress: function (sourceSelector, targetSelector) {
            var target = module.getAddress(targetSelector);
            if (target == null) return;

            var source = module.getAddress(sourceSelector);
            if (source == null) return;

            // Change the static stuff first
            target.$address1.val(source.address1);
            target.$address2.val(source.address2);
            target.$city.val(source.city);
            target.$zip.val(source.zip);

            // Next, change the country and leave a listener for when the regions are populated if the countries don't match
            if (target.country == source.country) {
                target.$state.val(source.state);
                target.$country.val(source.country);
            }
            else {
                var onRegionsPopulated = function ($element) {
                    if ($element == target.$state) {
                        target.$state.val($element.val());
                        window.off('regionspopulated', onRegionsPopulated);
                    }
                };
                window.on('regionspopulated', function ($element) {
                    onRegionsPopulated($element);
                });

                target.$country
                    .val(source.country)
                    .triggerHandler('change');
            }
        },
        verifyAddress: function (context) {
            var address = module.getAddress(context),
                $verifybutton = $('[data-role="verify"]', $(context));

            // Ensure that we have what we need to verify the address first - stop if we don't.
            if (address.address1 == '' || address.city == '' || address.state == '' || address.zip == '' || address.country == '' || address.country != "US") return;

            // Prepare the model
            var model = {};
            for (var key in address) {
                if (key.indexOf('$') == -1) model[key] = address[key];
            }

            $verifybutton.loadingButtons();

            // Call the server
            ajax.json({
                url: app.path('app/verifyaddress'),
                data: model,
                success: function (response) {

                    // If we got a valid response, populate the new address
                    var statusHtml = "";
                    if (response.IsValid) {
                        statusHtml = '<span class="text-success verification-status" style="margin-left: 10px;"><i class="fa-check"></i> Verified!</span>';

                        var newAddress = response.VerifiedAddress;

                        address.$address1.val(newAddress.Address1);
                        address.$address2.val(newAddress.Address2);
                        address.$city.val(newAddress.City);
                        address.$state.val(newAddress.State);
                        address.$zip.val(newAddress.Zip);
                        address.$country.val(newAddress.Country);
                    }
                    else {
                        statusHtml = '<span class="text-danger verification-status" style="margin-left: 10px;"><i class="fa-warning"></i> We were unable to verify your address - try again.</span>';
                    }

                    // Insert the status message
                    $verifybutton.after(statusHtml);
                    setTimeout(function () {
                        $verifybutton.siblings('.verification-status').fadeOut('fast', function () {
                            $verifybutton.siblings('.verification-status').remove();
                        });
                    }, 3000);
                },
                complete: function () {
                    $verifybutton.buttonState('reset');
                }
            });
        }
    };

    return module;

});