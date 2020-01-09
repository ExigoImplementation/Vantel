//Debug Module
define(["jquery", "cookies", "forms", "bootstrap"], function ($, cookies) {
    var module = {
        console: {
            log: function (request) {
                if (module.debugIsEnabled()) {
                    return console.log(request);
                }
                this.clear();
                return console.warn("Debug is NOT Enabled");
            },
            clear: function () {
                return console.clear();
            }

        },
        toBoolean: function (request) {
            switch (request.toLowerCase()) {
                case "yes":
                    return true;
                case "true":
                    return true;
                case "t":
                    return true;
                case "1":
                    return true;
                case "no":
                    return false;
                case "false":
                    return false;
                case "f":
                    return false;
                case "0":
                    return false;
                default:
                    return false;
            }
        },
        debugIsEnabled: function () {
            //JS, 09-15-2015, Add The Debug Cookie's Name Here
            var debugCookie = cookies.get("exigodemo_DebugMode");
            if (debugCookie != null) {
                return this.toBoolean(debugCookie);
            }
            return false;
        },
        ControlPanelControls: function (context) {
            var $context = $(context);

            $('[data-toggle="tooltip"]').tooltip();

            function getRandomString(length) {
                length = length || 8;
                var text = "";
                var possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

                for (var i = 0; i < length; i++)
                    text += possible.charAt(Math.floor(Math.random() * possible.length));

                return text;
            };
            function getRandomNumber(length) {
                length = length || 10;
                var text = "";
                var possible = "0123456789";

                for (var i = 0; i < length; i++)
                    text += possible.charAt(Math.floor(Math.random() * possible.length));

                return text;
            };
            $('[data-role="validate"]', $context).on('click', function () {
                $('form', $context).valid();
            });
            $('[data-role="basic-info-fill"]', $context).on('click', function () {
                $('input[type="text"]:visible').each(function () {
                    $(this).val(getRandomString());
                });
                //Sets Email To Random Exigo Test Email Account That Does Not Exist
                $('input[type="email"]:visible').each(function () {
                    $(this).val(getRandomString(10) + "@exigo.test");
                });
                //Set Telephone Number to Random Number
                $('input[type="tel"]:visible').each(function () {
                    $(this).val(getRandomNumber());
                });
                //Sets TaxID to a Random Number
                $('input[data-restrict-input="taxid"]:visible').each(function () {
                    $(this).val(getRandomNumber(9));
                });
                //Sets Password to Specific Code for Easy Sign-In
                $('input[type="password"]:visible').each(function () {
                    $(this).val("exigotest12345");
                });

                $(':input:visible:first').triggerHandler('change');
            });
            function testInfoFill() {
                $('input[type="text"][data-restrict-input!="password"][data-restrict-input!="taxid"]:visible').not('input[name*="CVV"]:visible').not('input[name*="CardNumber"]:visible').each(function () {
                    $(this).val(getRandomString());
                });
                //Sets Email To Random Exigo Test Email Account That Does Not Exist
                $('input[type="email"]:visible').each(function () {
                    $(this).val(getRandomString(10) + "@exigo.test");
                });
                //Set Telephone Number to Random Number
                $('input[type="tel"]:visible').each(function () {
                    $(this).val(getRandomNumber());
                });
                //Sets TaxID to a Random Number
                $('input[data-restrict-input="taxid"]:visible').each(function () {
                    $(this).val(getRandomNumber(9));
                });
                //Sets Password to Specific Code for Easy Sign-In
                $('input[type="password"]:visible').each(function () {
                    $(this).val("exigotest12345");
                });

                $(':input:visible:first').triggerHandler('change');
            };
            $('[data-role="valid-address-fill"]', $context).on('click', function () {
                //Sets Address to Exigo Office
                //This May or May Not Work Dependent on the Form and Will Require Fine Tuning
                $('select[name*="Country"]:visible').each(function () {
                    $(this).val("US")
                });
                $('input[name*="Address1"]:visible').each(function () {
                    $(this).val("8130 John Carpenter Fwy.");
                });
                $('input[name*="City"]:visible').each(function () {
                    $(this).val("Dallas");
                });
                $('select[name*="State"]:visible').each(function () {
                    $(this).val("TX")
                });
                $('input[name*="Zip"]:visible').each(function () {
                    $(this).val("75247");
                });
            });
            $('[data-role="name-fill"]', $context).on('click', function () {
                //Sets Name Fields to Exigo Test
                //This May or May Not Work Dependent on the Form and Will Require Fine Tuning
                $('input[name*="FirstName"]:visible').each(function () {
                    $(this).val("Exigo")
                });
                $('input[name*="MiddleName"]:visible').each(function () {
                    $(this).val("Sample");
                });
                $('input[name*="LastName"]:visible').each(function () {
                    $(this).val("Test");
                });
            });
            $('[data-role="credit-card-fill"]', $context).on('click', function () {
                //Sets Payment Fields to Exigo Test Credit Card and Random CVV
                //This May or May Not Work Dependent on the Form and Will Require Fine Tuning
                //Sets Card Number to Test Card
                $('input[name*="CardNumber"]:visible').each(function () {
                    $(this).val("9696969696969696");
                });
                //Sets CVV to a Random Number
                $('input[name*="CVV"]:visible').each(function () {
                    $(this).val(getRandomNumber(3));
                });
                //Sets Expiration Year To Ensure Validity
                $('select[name*="ExpirationYear"]:visible').each(function () {
                    $(this).val((new Date().getFullYear() + 1).toString());
                });
            });
            $('[data-role="complete-auto-fill"]', $context).on('click', function () {
                //This will attempt to change the form using all available options
                //This May or May Not Work Dependent on the Form and Will Require Fine Tuning
                testInfoFill();
                $("[data-role='credit-card-fill']").triggerHandler("click");
                $("[data-role='name-fill']").triggerHandler("click");
                $("[data-role='valid-address-fill']").triggerHandler("click");

            });
            //Opens and Closes the Control Panel
            $('.fa-chevron-left', ".debug-cp").on('click', function () {
                $(".debug-cp").animate({ right: '-5px' });
                $(this).addClass("hidden").parents().find(".fa-chevron-right").removeClass("hidden").addClass(".block");
            });
            $('.fa-chevron-right', ".debug-cp").on('click', function () {
                $(".debug-cp").animate({ right: '-265px' }, function () {
                    $(".debug-cp").removeAttr("style");
                });
                $(this).addClass("hidden").parents().find(".fa-chevron-left").removeClass("hidden").addClass(".block");
            });
            $('.fa-chevron-left', ".debug-serial-cp").on('click', function () {
                $(".debug-serial-cp").animate({ left: '-265px' }, function () {
                    $(".debug-serial-cp").removeAttr("style");
                });
                $(this).addClass("hidden").parents().find(".fa-chevron-right").removeClass("hidden").addClass(".block");
            });
            $('.fa-chevron-right', ".debug-serial-cp").on('click', function () {
                $(".debug-serial-cp").animate({ left: '15px' });
                $(this).addClass("hidden").parents().find(".fa-chevron-left").removeClass("hidden").addClass(".block");
            });
            $('[data-role="add-serialized"]', '.debug-serial-cp').on('click', function () {
                var selected = $(this).parents(".debug-serial-cp").find("select option:checked").val();
                $("#" + selected + "").removeClass("hidden");
            });
            $('[data-role="go-to-serialized"]', '.debug-serial-cp').on('click', function () {
                var selected = $(this).parents(".debug-serial-cp").find("select option:checked").val();
                $(".debug-serial-cp").find("pre").not(".hidden").addClass("hidden");
                $("#" + selected + "").removeClass("hidden");
            });
        }
    }

    return module;
});