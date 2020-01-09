// Handlebars helpers and extensions
define(["jquery", "extensions"], function ($) {
    Handlebars.render = function (selector, data) {
        var $source = $(selector),
            source = $source.html(),
            $target = $($source.attr('data-target')),
            template = Handlebars.compile(source),
            html = template(data);

        // Confirm that the target exists
        if ($target.length == 0) {

            // Create a new ID for our new target element
            var newTargetID = "{0}-target".format($source.attr('id'));

            // Create the new target, since we didn't have one before
            $target = $('<div id="{0}" />'.format(newTargetID));

            // Insert the new target element and set the target attribute of the template source
            $source.before($target).attr('data-target', newTargetID);
        }

        // Insert the HTML
        $target.html(html);
    };

    // jQuery extension that makes it easier to bind Handlebars templates and data
    $.fn.template = function (data, template, context) {
        var template = Handlebars.compile($(template, context).html());
        var html = template(data);

        $(this).html(html);
    }

    // Common handlebars extensions
    Handlebars.registerHelper('number', function (number) {
        return number.format("N");
    });
    Handlebars.registerHelper('numberorblank', function (number, decimals) {
        if (isNaN(decimals)) decimals = 2;
        if (number == 0) {
            return '-';
        }
        else {
            return number.formatMoney(decimals);
        }
    });
    Handlebars.registerHelper('intorblank', function (number) {
        if (number == 0) {
            return '-';
        }
        else {
            return number;
        }
    });
    Handlebars.registerHelper('int', function (number) {
        return Number(number).format("N0");
    });
    Handlebars.registerHelper('money', function (number) {
        return Number(number).format("C");
    });
    Handlebars.registerHelper('shortdate', function (text) {
        var date = new Date(text);

        var response = "{0}/{1}/{2}".format(
            date.getMonth() + 1,
            date.getDate(),
            date.getFullYear()
        );

        return response;;
    });
    Handlebars.registerHelper('shortjsondate', function (jsondate) {
        var date = new Date(parseInt(jsondate.substr(6)));

        var response = "{0}/{1}/{2}".format(
            date.getMonth() + 1,
            date.getDate(),
            date.getFullYear()
        );

        return response;;
    });
    Handlebars.registerHelper('longdate', function (text) {
        var date = new Date(text);

        var response = "{0}, {1} {2}, {3}".format(
            date.getDayName(),
            date.getMonthName(),
            date.getDate(),
            date.getFullYear()
        );

        return response;;
    });
    Handlebars.registerHelper('longjsondate', function (jsondate) {
        var date = new Date(parseInt(jsondate.substr(6)));

        var response = "{0}, {1} {2}, {3}".format(
            date.getDayName(),
            date.getMonthName(),
            date.getDate(),
            date.getFullYear()
        );

        return response;;
    });
    Handlebars.registerHelper('fulldate', function (text) {
        var date = new Date(text);
        return date.toLocaleString();
    });

    return Handlebars;
});