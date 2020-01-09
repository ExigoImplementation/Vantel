/* PubSub
***********************************************************
    This library injects a simple PubSub framework into any object. 

    Sample Use:
        // Inject PubSub into your object
        var myObject = {};
        PubSub(myObject);

        // Add an event and define your callback
        myObject.on('someevent', function(args) {
           // Do something
        });

        // Add an event and get the token representing this callback
        myObject.on('someotherevent', 'my token', function(args) {
           // Do something
        });

        // Trigger the event
        myObject.trigger('someevent', { somekey: 'somevalue' });


        // Remove a specific event by it's token
        myObject.off('my token string');
*/

define(function () {

    var module = {
        create: function (obj) {
            "use strict";

            // Check to see if we have already made this object a PubSub object before. 
            // If so, don't do it again. Return the object we started with.
            if (typeof obj.pubsubEnabled !== "undefined") return obj;

            // Create our variables
            var p = obj || {},
                topics = {},
                lastUid = -1;

            p.pubsubEnabled = true;

            var publish = function (topic, data) {
                if (!topics.hasOwnProperty(topic)) {
                    return false;
                }

                var notify = function () {
                    var subscribers = topics[topic],
                        throwException = function (e) {
                            return function () {
                                throw e;
                            };
                        };

                    for (var i = 0, j = subscribers.length; i < j; i++) {
                        try {
                            subscribers[i].func(data);
                        } catch (e) {
                            setTimeout(throwException(e), 0);
                        }
                    }
                };

                setTimeout(notify, 0);
                return true;
            };

            /**
             *  Publishes the topic, passing the data to it's subscribers
             *  @@topic (String): The topic to publish
             *   @@data: The data to pass to subscribers
             **/
            p.trigger = function (topic, data) {
                return publish(topic, data, false);
            };

            /**
             *  Subscribes the passed function to the passed topic.
             *  Every returned token is unique and should be stored if you need to unsubscribe
             *  @@topic (String): The topic to subscribe to
             *  @@token (String): The token to use when subscribing
             *  @@func (Function): The function to call when a new topic is published
             **/
            p.on = function (topic, token, func) {
                // topic is not registered yet
                if (!topics.hasOwnProperty(topic)) {
                    topics[topic] = [];
                }

                var callback = func;
                var providedToken = (typeof token !== "function");


                if (!providedToken) {
                    callback = token;
                    token = (++lastUid).toString();
                }

                // If we pass in our own token, replace any instances with the same token
                if (providedToken) {
                    for (var t = 0, max = topics[topic].length; t < max; t++) {
                        if (topics[topic][t].token == token) {
                            topics[topic].splice(t, 1);
                        }
                    }
                }

                topics[topic].push({
                    token: token,
                    func: callback
                });

                // return token for unsubscribing
                return token;

            };

            /**
             *  Unsubscribes a specific subscriber from a specific topic using the unique token
             *  @@token (String): The token of the function to unsubscribe
             **/
            p.off = function (token) {
                for (var m in topics) {
                    if (topics.hasOwnProperty(m)) {
                        for (var i = 0, j = topics[m].length; i < j; i++) {
                            if (topics[m][i].token === token) {
                                topics[m].splice(i, 1);
                                return token;
                            }
                        }
                    }
                }
                return false;
            };

            return p;
        }
    };

    module.create(window);

    return module;
});