/* ========================================================================
 * ExigoPayments: v1.0.0
  * ========================================================================
 * Copyright 2015 Exigo Payments, Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * ======================================================================== */


var ExigoPayments = function () {
    return {
        tokenize: function (options) {

            //force options to be an object
            options = options || {};

            //--> Tokenize Card
            $.ajax({
                url: "https://token.nuvei.com/2.0/token/jsonp/createcreditcardtoken",
                type: 'GET',
                dataType: "jsonp",
                data: {
                    credential: options.credential,
                    creditCardNumber: options.card,
                    expirationMonth: options.month,
                    expirationYear: options.year
                },
                success: function (res) {
                    if (res.success) {
                        options.success(res.creditCardToken);
                    } else {
                        options.failure(res.errorMessage);
                    }
                },
                error: function (xhr, status, error) {
                    options.failure("Connection Error");
                }
            });
        }
    }
}();