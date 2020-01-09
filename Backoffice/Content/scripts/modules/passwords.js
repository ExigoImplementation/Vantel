define(["jquery"], function (jQuery) {

    // Hide/Show Passwords
    (function ($) {

        var dataKey = 'plugin_hideShowPassword',
            shorthandArgs = ['show', 'innerToggle'],
            SPACE = 32,
            ENTER = 13;

        var canSetInputAttribute = (function () {
            var body = document.body,
                input = document.createElement('input'),
                result = true;
            if (!body) {
                body = document.createElement('body');
            }
            input = body.appendChild(input);
            try {
                input.setAttribute('type', 'text');
            } catch (e) {
                result = false;
            }
            body.removeChild(input);
            return result;
        }());

        var defaults = {
            // Visibility of the password text. Can be true, false, 'toggle'
            // or 'infer'. If 'toggle', it will be the opposite of whatever
            // it currently is. If 'infer', it will be based on the input
            // type (false if 'password', otherwise true).
            show: 'infer',

            // Set to true to create an inner toggle for this input. Can
            // also be sent to an event name to delay visibility of toggle
            // until that event is triggered on the input element.
            innerToggle: false,

            // If false, the plugin will be disabled entirely. Set to
            // the outcome of a test to insure input attributes can be
            // set after input has been inserted into the DOM.
            enable: canSetInputAttribute,

            // Class to add to input element when the plugin is enabled.
            className: 'hideShowPassword-field',

            // Event to trigger when the plugin is initialized and enabled.
            initEvent: 'hideShowPasswordInit',

            // Event to trigger whenever the visibility changes.
            changeEvent: 'passwordVisibilityChange',

            // Properties to add to the input element.
            props: {
                autocapitalize: 'off',
                autocomplete: 'off',
                autocorrect: 'off',
                spellcheck: 'false'
            },

            // Options specific to the inner toggle.
            toggle: {
                // The element to create.
                element: '<button type="button">',
                // Class name of element.
                className: 'hideShowPassword-toggle',
                // Whether or not to support touch-specific enhancements.
                // Defaults to the value of Modernizr.touch if available,
                // otherwise false.
                touchSupport: (typeof Modernizr === 'undefined') ? false : Modernizr.touch,
                // Non-touch event to bind to.
                attachToEvent: 'click',
                // Event to bind to when touchSupport is true.
                attachToTouchEvent: 'touchstart mousedown',
                // Key event to bind to if attachToKeyCodes is an array
                // of at least one keycode.
                attachToKeyEvent: 'keyup',
                // Key codes to bind the toggle event to for accessibility.
                // If false, this feature is disabled entirely.
                // If true, the array of key codes will be determined based
                // on the value of the element option.
                attachToKeyCodes: true,
                // Styles to add to the toggle element. Does not include
                // positioning styles.
                styles: { position: 'absolute' },
                // Styles to add only when touchSupport is true.
                touchStyles: { pointerEvents: 'none' },
                // Where to position the inner toggle relative to the
                // input element. Can be 'right', 'left' or 'infer'. If
                // 'infer', it will be based on the text-direction of the
                // input element.
                position: 'infer',
                // Where to position the inner toggle on the y-axis
                // relative to the input element. Can be 'top', 'bottom'
                // or 'middle'.
                verticalAlign: 'middle',
                // Amount by which to "offset" the toggle from the edge
                // of the input element.
                offset: 0,
                // Attributes to add to the toggle element.
                attr: {
                    role: 'button',
                    'aria-label': 'Show Password',
                    tabIndex: 0
                }
            },

            // Options specific to the wrapper element, created
            // when the innerToggle is initialized to help with
            // positioning of that element.
            wrapper: {
                // The element to create.
                element: '<div>',
                // Class name of element.
                className: 'hideShowPassword-wrapper',
                // If true, the width of the wrapper will be set
                // unless it is already the same width as the inner
                // element. If false, the width will never be set. Any
                // other value will be used as the width.
                enforceWidth: true,
                // Styles to add to the wrapper element. Does not
                // include inherited styles or width if enforceWidth
                // is not false.
                styles: { position: 'relative' },
                // Styles to "inherit" from the input element, allowing
                // the wrapper to avoid disrupting page styles.
                inheritStyles: [
                    'display',
                    'verticalAlign',
                    'marginTop',
                    'marginRight',
                    'marginBottom',
                    'marginLeft'
                ],
                // Styles for the input element when wrapped.
                innerElementStyles: {
                    marginTop: 0,
                    marginRight: 0,
                    marginBottom: 0,
                    marginLeft: 0
                }
            },

            // Options specific to the 'shown' or 'hidden'
            // states of the input element.
            states: {
                shown: {
                    className: 'hideShowPassword-shown',
                    changeEvent: 'passwordShown',
                    props: { type: 'text' },
                    toggle: {
                        className: 'hideShowPassword-toggle-hide',
                        content: 'Hide',
                        attr: { 'aria-pressed': 'true' }
                    }
                },
                hidden: {
                    className: 'hideShowPassword-hidden',
                    changeEvent: 'passwordHidden',
                    props: { type: 'password' },
                    toggle: {
                        className: 'hideShowPassword-toggle-show',
                        content: 'Show',
                        attr: { 'aria-pressed': 'false' }
                    }
                }
            }

        };

        function HideShowPassword(element, options) {
            this.element = $(element);
            this.wrapperElement = $();
            this.toggleElement = $();
            this.init(options);
        }

        HideShowPassword.prototype = {
            init: function (options) {
                if (this.update(options, defaults)) {
                    this.element.addClass(this.options.className);
                    if (this.options.innerToggle) {
                        this.wrapElement(this.options.wrapper);
                        this.initToggle(this.options.toggle);
                        if (typeof this.options.innerToggle === 'string') {
                            this.toggleElement.hide();
                            this.element.one(this.options.innerToggle, $.proxy(function () {
                                this.toggleElement.show();
                            }, this));
                        }
                    }
                    this.element.trigger(this.options.initEvent, [this]);
                }
            },

            update: function (options, base) {
                this.options = this.prepareOptions(options, base);
                if (this.updateElement()) {
                    this.element
                        .trigger(this.options.changeEvent, [this])
                        .trigger(this.state().changeEvent, [this]);
                }
                return this.options.enable;
            },

            toggle: function (showVal) {
                showVal = showVal || 'toggle';
                return this.update({ show: showVal });
            },

            prepareOptions: function (options, base) {
                var keyCodes = [], testElement;
                base = base || this.options;
                options = $.extend(true, {}, base, options);
                if (options.enable) {
                    if (options.show === 'toggle') {
                        options.show = this.isType('hidden', options.states);
                    } else if (options.show === 'infer') {
                        options.show = this.isType('shown', options.states);
                    }
                    if (options.toggle.position === 'infer') {
                        options.toggle.position = (this.element.css('text-direction') === 'rtl') ? 'left' : 'right';
                    }
                    if (!$.isArray(options.toggle.attachToKeyCodes)) {
                        if (options.toggle.attachToKeyCodes === true) {
                            testElement = $(options.toggle.element);
                            switch (testElement.prop('tagName').toLowerCase()) {
                                case 'button':
                                case 'input':
                                    break;
                                case 'a':
                                    if (testElement.filter('[href]').length) {
                                        keyCodes.push(SPACE);
                                        break;
                                    }
                                default:
                                    keyCodes.push(SPACE, ENTER);
                                    break;
                            }
                        }
                        options.toggle.attachToKeyCodes = keyCodes;
                    }
                }
                return options;
            },

            updateElement: function () {
                if (!this.options.enable || this.isType()) return false;
                this.element
                    .prop($.extend({}, this.options.props, this.state().props))
                    .addClass(this.state().className)
                    .removeClass(this.otherState().className);
                this.updateToggle();
                return true;
            },

            isType: function (comparison, states) {
                states = states || this.options.states;
                comparison = comparison || this.state(undefined, undefined, states).props.type;
                if (states[comparison]) {
                    comparison = states[comparison].props.type;
                }
                return this.element.prop('type') === comparison;
            },

            state: function (key, invert, states) {
                states = states || this.options.states;
                if (key === undefined) {
                    key = this.options.show;
                }
                if (typeof key === 'boolean') {
                    key = key ? 'shown' : 'hidden';
                }
                if (invert) {
                    key = (key === 'shown') ? 'hidden' : 'shown';
                }
                return states[key];
            },

            otherState: function (key) {
                return this.state(key, true);
            },

            wrapElement: function (options) {
                var enforceWidth = options.enforceWidth, targetWidth;
                if (!this.wrapperElement.length) {
                    targetWidth = this.element.outerWidth();
                    $.each(options.inheritStyles, $.proxy(function (index, prop) {
                        options.styles[prop] = this.element.css(prop);
                    }, this));
                    this.element.css(options.innerElementStyles).wrap(
                        $(options.element).addClass(options.className).css(options.styles)
                    );
                    this.wrapperElement = this.element.parent();
                    if (enforceWidth === true) {
                        enforceWidth = (this.wrapperElement.outerWidth() === targetWidth) ? false : targetWidth;
                    }
                    if (enforceWidth !== false) {
                        this.wrapperElement.css('width', enforceWidth);
                    }
                }
                return this.wrapperElement;
            },

            initToggle: function (options) {
                if (!this.toggleElement.length) {
                    // Create element
                    this.toggleElement = $(options.element)
                        .attr(options.attr)
                        .addClass(options.className)
                        .css(options.styles)
                        .appendTo(this.wrapperElement);
                    // Update content/attributes
                    this.updateToggle();
                    // Position
                    this.positionToggle(options.position, options.verticalAlign, options.offset);
                    // Events
                    if (options.touchSupport) {
                        this.toggleElement.css(options.touchStyles);
                        this.element.on(options.attachToTouchEvent, $.proxy(this.toggleTouchEvent, this));
                    } else {
                        this.toggleElement.on(options.attachToEvent, $.proxy(this.toggleEvent, this));
                    }
                    if (options.attachToKeyCodes.length) {
                        this.toggleElement.on(options.attachToKeyEvent, $.proxy(this.toggleKeyEvent, this));
                    }
                }
                return this.toggleElement;
            },

            positionToggle: function (position, verticalAlign, offset) {
                var styles = {};
                styles[position] = offset;
                switch (verticalAlign) {
                    case 'top':
                    case 'bottom':
                        styles[verticalAlign] = offset;
                        break;
                    case 'middle':
                        styles['top'] = '50%';
                        styles['marginTop'] = this.toggleElement.outerHeight() / -2;
                        break;
                }
                return this.toggleElement.css(styles);
            },

            updateToggle: function (state, otherState) {
                var paddingProp, targetPadding;
                if (this.toggleElement.length) {
                    paddingProp = 'padding-' + this.options.toggle.position;
                    state = state || this.state().toggle;
                    otherState = otherState || this.otherState().toggle;
                    this.toggleElement
                        .attr(state.attr)
                        .addClass(state.className)
                        .removeClass(otherState.className)
                        .html(state.content);
                    targetPadding = this.toggleElement.outerWidth() + (this.options.toggle.offset * 2);
                    if (this.element.css(paddingProp) !== targetPadding) {
                        this.element.css(paddingProp, targetPadding);
                    }
                }
                return this.toggleElement;
            },

            toggleEvent: function (event) {
                event.preventDefault();
                this.toggle();
            },

            toggleKeyEvent: function (event) {
                $.each(this.options.toggle.attachToKeyCodes, $.proxy(function (index, keyCode) {
                    if (event.which === keyCode) {
                        this.toggleEvent(event);
                        return false;
                    }
                }, this));
            },

            toggleTouchEvent: function (event) {
                var toggleX = this.toggleElement.offset().left,
                    eventX,
                    lesser,
                    greater;
                if (toggleX) {
                    eventX = event.pageX || event.originalEvent.pageX;
                    if (this.options.toggle.position === 'left') {
                        toggleX += this.toggleElement.outerWidth();
                        lesser = eventX;
                        greater = toggleX;
                    } else {
                        lesser = toggleX;
                        greater = eventX;
                    }
                    if (greater >= lesser) {
                        this.toggleEvent(event);
                    }
                }
            }

        };

        $.fn.hideShowPassword = function () {
            var options = {};
            $.each(arguments, function (index, value) {
                var newOptions = {};
                if (typeof value === 'object') {
                    newOptions = value;
                } else if (shorthandArgs[index]) {
                    newOptions[shorthandArgs[index]] = value;
                } else {
                    return false;
                }
                $.extend(true, options, newOptions);
            });
            return this.each(function () {
                var $this = $(this), data = $this.data(dataKey);
                if (data) {
                    data.update(options);
                } else {
                    $this.data(dataKey, new HideShowPassword(this, options));
                }
            });
        };

        $.each({ 'show': true, 'hide': false, 'toggle': 'toggle' }, function (verb, showVal) {
            $.fn[verb + 'Password'] = function (innerToggle, options) {
                return this.hideShowPassword(showVal, innerToggle, options);
            };
        });

    })(jQuery);


    /* Intelligent Web NameSpace */
    var IW = IW || {};

    /**
     * Password validator logic
     */
    (function (IW) {

        var secondsInADay = 86400;

        function PasswordValidator() {
        }

        /**
         * How long a password can be expected to last
         */
        PasswordValidator.prototype.passwordLifeTimeInDays = 365;

        /**
         * An estimate of how many attempts could be made per second to guess a password
         */
        PasswordValidator.prototype.passwordAttemptsPerSecond = 500;

        /**
         * An array of regular expressions to match against the password. Each is associated
         * with the number of unique characters that each expression can match.
         * @param password
         */
        PasswordValidator.prototype.expressions = [
            {
                regex: /[A-Z]+/,
                uniqueChars: 26
            },
            {
                regex: /[a-z]+/,
                uniqueChars: 26
            },
            {
                regex: /[0-9]+/,
                uniqueChars: 10
            },
            {
                regex: /[!\?.;,\\@$£#*()%~<>{}\[\]]+/,
                uniqueChars: 17
            }
        ];

        /**
         * Checks the supplied password
         * @param {String} password
         * @return The predicted lifetime of the password, as a percentage of the defined password lifetime.
         */
        PasswordValidator.prototype.checkPassword = function (password) {

            password = password || "";

            var
                    expressions = this.expressions,
                    i,
                    l = expressions.length,
                    expression,
                    possibilitiesPerLetterInPassword = 0;

            for (i = 0; i < l; i++) {

                expression = expressions[i];

                if (expression.regex.exec(password)) {
                    possibilitiesPerLetterInPassword += expression.uniqueChars;
                }

            }

            var totalCombinations = Math.pow(possibilitiesPerLetterInPassword, password.length),
                // how long, on average, it would take to crack this (@ 200 attempts per second)
                    crackTime = ((totalCombinations / this.passwordAttemptsPerSecond) / 2) / secondsInADay,
                // how close is the time to the projected time?
                    percentage = crackTime / this.passwordLifeTimeInDays;

            return Math.min(Math.max(password.length * 5, percentage * 100), 100);

        };

        IW.PasswordValidator = new PasswordValidator();

    })(IW);

    /**
     * jQuery plugin which allows you to add password validation to any
     * form element.
     */
    (function (IW, jQuery) {

        function updatePassword() {

            var
                    percentage = IW.PasswordValidator.checkPassword(this.val()),
                    progressBar = this.parent().find(".passwordStrengthBar div");

            progressBar
                    .removeClass("strong medium weak useless")
                    .stop()
                    .animate({ "width": percentage + "%" });

            if (percentage > 90) {
                progressBar.addClass("strong");
            } else if (percentage > 50) {
                progressBar.addClass("medium")
            } else if (percentage > 10) {
                progressBar.addClass("weak");
            } else {
                progressBar.addClass("useless");
            }
        }

        jQuery.fn.passwordValidate = function () {

            this
                    .bind('keyup', jQuery.proxy(updatePassword, this))
                    .after("<div class='passwordStrengthBar'>" +
                    "<div></div>" +
                    "</div>");

            updatePassword.apply(this);

            if (this.val() != '') {
                this.trigger('keyup');
            }

            return this; // for chaining

        }

    })(IW, jQuery);
});
