// AJAX module
define(["jquery"], function ($) {

    var module = {
        json: function (request) {
            if (!request.url) {
                alert('Missing JSON Ajax URL. Request: ', request);
                return;
            }

            return $.ajax({
                url: request.url,
                type: 'POST',
                cache: request.cache || false,
                dataType: 'json',
                contentType: request.contentType || "application/json; charset=utf-8",
                data: (request.data) ? JSON.stringify(request.data) : null,
                dataFilter: function (data) {
                    return data.d || data;
                },
                headers: request.headers || { "__RequestVerificationToken": $('[name=__RequestVerificationToken]').val() },
                beforeSend: function () {
                    if (request.beforeSend) request.beforeSend();
                },
                success: function (data) {
                    data = data.d || data;
                    if (request.success) request.success(data);
                },
                error: function (xhr, status, error) {
                    var response = xhr.responseText;
                    try { response = $.parseJSON(response); }
                    catch (error) { }

                    if (request.error) request.error(response, status, error);

                    if (request.maxAttempts !== "undefined" && request.interval !== "undefined") {
                        if (request.maxAttempts == 0) {
                            console.warn("The Ajax module cannot make infinite repeating attempts. Changing to 3 attempts...");
                            request.maxAttempts = 3;
                        }

                        var repeatOptionDefaults = {
                            _attempts: 0,
                            maxAttempts: 1,
                            interval: 3000,
                            fail: function (attempts) {
                                console.error('Repeating calls failed');
                            }
                        };
                        request = $.extend(repeatOptionDefaults, request, {});
                        request._attempts = request._attempts + 1;
                        if (request._attempts < request.maxAttempts) {
                            setTimeout(function () {
                                if (request.repeat) request.repeat(request._attempts, request.maxAttempts);
                                module.json(request);
                            }, request.interval);
                        }
                        else {
                            request.fail(xhr, status, error);
                        }
                    }
                },
                complete: function () {
                    if (request.complete) request.complete();
                }
            });
        },
        post: function (request) {
            if (!request.url) {
                alert('Missing JSON Ajax URL. Request: ', request);
                return;
            }

            return $.ajax({
                url: request.url,
                type: 'POST',
                cache: request.cache || false,
                dataType: 'json',
                contentType: request.contentType || "application/x-www-form-urlencoded; charset=UTF-8",
                data: (request.data) ? request.data : null,
                dataFilter: function (data) {
                    return data.d || data;
                },
                headers: request.headers || { "__RequestVerificationToken": $('[name=__RequestVerificationToken]').val() },
                beforeSend: function () {
                    if (request.beforeSend) request.beforeSend();
                },
                success: function (data) {
                    data = data.d || data;
                    if (request.success) request.success(data);
                },
                error: function (xhr, status, error) {
                    var response = xhr.responseText;
                    try { response = $.parseJSON(response); }
                    catch (error) { }

                    if (request.error) request.error(response, status, error);

                    if (request.maxAttempts !== "undefined" && request.interval !== "undefined") {
                        if (request.maxAttempts == 0) {
                            console.warn("The Ajax module cannot make infinite repeating attempts. Changing to 3 attempts...");
                            request.maxAttempts = 3;
                        }

                        var repeatOptionDefaults = {
                            _attempts: 0,
                            maxAttempts: 1,
                            interval: 3000,
                            fail: function (attempts) {
                                console.error('Repeating calls failed');
                            }
                        };
                        request = $.extend(repeatOptionDefaults, request, {});
                        request._attempts = request._attempts + 1;
                        if (request._attempts < request.maxAttempts) {
                            setTimeout(function () {
                                if (request.repeat) request.repeat(request._attempts, request.maxAttempts);
                                module.json(request);
                            }, request.interval);
                        }
                        else {
                            request.fail(xhr, status, error);
                        }
                    }
                },
                complete: function () {
                    if (request.complete) request.complete();
                }
            });
        },
        html: function (request) {
            if (!request.url) {
                alert('Missing HTML Ajax URL. Request: ', request);
                return;
            }

            return $.ajax({
                url: request.url,
                type: 'GET',
                cache: request.cache || true,
                dataType: 'html',
                contentType: "application/x-www-form-urlencoded; charset=UTF-8",
                data: (request.data) ? request.data : null,
                dataFilter: function (data) {
                    return data.d || data;
                },
                beforeSend: function () {
                    if (request.beforeSend) request.beforeSend();
                },
                success: function (data) {
                    data = data.d || data;
                    if (request.success) request.success(data);
                },
                error: function (xhr, status, error) {
                    var response = xhr.responseText;
                    try { response = $.parseJSON(response); }
                    catch (error) { }

                    if (request.error) request.error(response, status, error);

                    if (request.maxAttempts !== "undefined" && request.interval !== "undefined") {
                        if (request.maxAttempts == 0) {
                            console.warn("The Ajax module cannot make infinite repeating attempts. Changing to 3 attempts...");
                            request.maxAttempts = 3;
                        }

                        var repeatOptionDefaults = {
                            _attempts: 0,
                            maxAttempts: 1,
                            interval: 3000,
                            fail: function (attempts) {
                                console.error('Repeating calls failed');
                            }
                        };
                        request = $.extend(repeatOptionDefaults, request, {});
                        request._attempts = request._attempts + 1;
                        if (request._attempts < request.maxAttempts) {
                            setTimeout(function () {
                                if (request.repeat) request.repeat(request._attempts, request.maxAttempts);
                                module.html(request);
                            }, request.interval);
                        }
                        else {
                            request.fail(xhr, status, error);
                        }
                    }
                },
                complete: function () {
                    if (request.complete) request.complete();
                }
            });
        }
    };

    return module;

});