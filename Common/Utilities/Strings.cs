using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Common
{
    public static partial class GlobalUtilities
    {
        /// <summary>
        /// Sets the value of the string to be the first non-nullable parameter found for the strings provided.
        /// </summary>
        /// <param name="strings"></param>
        /// <returns>The first non-null, non-empty string found.</returns>
        public static string Coalesce(params string[] strings)
        {
            return strings.Where(s => !string.IsNullOrEmpty(s)).FirstOrDefault();
        }

        /// <summary>
        /// Condenses the provided string to the provided max length of characters. If the content is longer than the max length, "..." will be appended to the end.
        /// </summary>
        /// <param name="content">The content to be condensed.</param>
        /// <param name="maxLength">The maximum number of allowable characters.</param>
        /// <returns>The content equal or less than the max length.</returns>
        public static string Condense(string content, int maxLength)
        {

            string contentText = Regex.Replace(content, @"<(.|\n)*?>", string.Empty);
            int length = contentText.Length;
            content = Regex.Match(contentText, @"^.{1," + (maxLength - 1) + @"}\b(?<!\s)").Value;
            if (length > maxLength) content += "...";

            return content;
        }

        /// <summary>
        /// Uppercase first letters of all words in the string.
        /// </summary>
        public static string Capitalize(string s)
        {
            return Regex.Replace(s, @"\b[a-z]\w+", delegate(Match match)
            {
                string v = match.ToString();
                return char.ToUpper(v[0]) + v.Substring(1);
            });
        }

        /// <summary>
        /// Validates the provided credit card number using a Luhn algorithm.
        /// </summary>
        /// <param name="creditCardNumber">The credit card number to validate.</param>
        /// <returns>The validity of the credit card number. True = valid card, False = invalid card.</returns>
        public static bool ValidateCreditCard(string creditCardNumber)
        {
            // this method can be called after the card has been saved and will be prefixed with asterisks, in this case it was valid to begin with so return true
            if (creditCardNumber.Contains("*")) return true;

            const string allowed = "0123456789";
            int i;

            var cleanNumber = new StringBuilder();
            for (i = 0; i < creditCardNumber.Length; i++)
            {
                if (allowed.IndexOf(creditCardNumber.Substring(i, 1)) >= 0)
                    cleanNumber.Append(creditCardNumber.Substring(i, 1));
            }
            if (cleanNumber.Length < 13 || cleanNumber.Length > 16)
                return false;

            for (i = cleanNumber.Length + 1; i <= 16; i++)
                cleanNumber.Insert(0, "0");

            int multiplier, digit, sum, total = 0;
            string number = cleanNumber.ToString();

            for (i = 1; i <= 16; i++)
            {
                multiplier = 1 + (i % 2);
                digit = int.Parse(number.Substring(i - 1, 1));
                sum = digit * multiplier;
                if (sum > 9)
                    sum -= 9;
                total += sum;
            }

            return (total % 10 == 0);
        }

        /// <summary>
        /// Format a provided amount to the provided currency code.
        /// </summary>
        /// <param name="amount">The decimal amount to format</param>
        /// <param name="currencycode">The currency code to format with</param>
        /// <returns></returns>
        public static string FormatCurrency(decimal amount, string currencycode = "usd")
        {
            var result = amount.ToString("C");

            var regionInfo = new RegionInfo(System.Threading.Thread.CurrentThread.CurrentUICulture.LCID);

            foreach (var cultureInfo in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
            {
                if (!cultureInfo.Equals(CultureInfo.InvariantCulture))
                {
                    var regionCulture = new RegionInfo(cultureInfo.LCID);
                    if (regionCulture.ISOCurrencySymbol.ToLower() == currencycode)
                    {
                        regionInfo = regionCulture;
                        return string.Format(cultureInfo, "{0:C}", amount);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Shortens the Description Inputted to a specified index and adds "..." to the end of the string
        /// </summary>
        /// <param name="description">String to be Formatted</param>
        /// <param name="cutOff">Index at which the string should stop displaying</param>
        /// <returns></returns>
        public static string DescriptionFormatter(string description, int cutOff)
        {
            cutOff = cutOff + 3;
            if (description.Length < cutOff)
            {
                return description;
            }

            //var specialCharacters = new char[] { '+', ',', '(', ')', '-', '.' };
            var toRemove = ((description.Length - 1) - cutOff);
            description = description.Remove(cutOff, toRemove);
            if (description.IndexOf(' ') > 0)
            {
                description = description.Substring(0, description.LastIndexOf(' ')).Trim();
            }
            //if (description.IndexOfAny(specialCharacters) > 0)
            //{
            //    description = description.Substring(0, description.LastIndexOfAny(specialCharacters)).Trim();
            //}

            return description + "...";
        }

        #region Merge Fields
        /// <summary>
        /// Replaces any merge fields following the syntax {{string}} with the value of any property of the same name in the provided dataSource. 
        /// If the property does not exist, the provided defaultText will be used (Defaults to "").
        /// For example: {{FirstName}} will be replaced with the value of the FirstName property of your data source if the property exists. 
        /// </summary>
        /// <param name="text">The string containing the merge fields.</param>
        /// <param name="dataSource">The object containing the values of the merge fields</param>
        /// <param name="defaultText">The text used if a property is not found in the data source. Defaults to "".</param>
        /// <returns>The flattened text with all merge fields replaced with their corresponding values.</returns>
        public static string MergeFields(string text, object dataSource, string defaultText = "")
        {
            var result = new StringBuilder(text);
            MatchCollection matches = GetMergeFieldMatches(text);
            int offset = 0;

            foreach (Match match in matches)
            {
                // Get some variables to make them easier to reference
                var mergedContent = "(N/A)";

                // Use reflection to get the matched field from the customer
                var type = dataSource.GetType();
                var field = match.Groups["field"].Value;
                var fieldformat = "";
                if (field.Contains(":"))
                {
                    var args = field.Split(':');
                    field = args[0];
                    fieldformat = args[1];
                }
                var property = type.GetProperty(field);
                if (property != null)
                {
                    mergedContent = property.GetValue(dataSource).ToString();
                    if (fieldformat.IsNotNullOrEmpty())
                    {
                        decimal decimalContent;
                        if (decimal.TryParse(mergedContent, out decimalContent))
                        {
                            mergedContent = decimalContent.ToString(fieldformat);
                        }

                        else 
                        {
                            DateTime dateContent;
                            if (DateTime.TryParse(mergedContent, out dateContent))
                            {
                                mergedContent = dateContent.ToString(fieldformat);
                            }
                        }

                        // OLD VERSION : MIKE MCBRIDE - 10/3/2014
                        //decimal decimalContent;
                        //if (decimal.TryParse(mergedContent, out decimalContent))
                        //{
                        //    mergedContent = decimalContent.ToString(fieldformat);
                        //}

                        //DateTime dateContent;
                        //if (DateTime.TryParse(mergedContent, out dateContent))
                        //{
                        //    mergedContent = dateContent.ToString(fieldformat);
                        //}

                    }
                }

                // Replace the merge field with the merged content
                result.Remove(match.Index + offset, match.Length);
                result.Insert(match.Index + offset, mergedContent);

                offset = offset + mergedContent.Length - match.Length;

            }

            return result.ToString();
        }

        /// <summary>
        /// Returns a list of matching merge fields from the provided text. Note that this is the only place that determines the merge field's syntax.
        /// </summary>
        /// <param name="text">The text containing the merge fields</param>
        /// <returns>A distinct list of the merge fields found</returns>
        private static MatchCollection GetMergeFieldMatches(string text)
        {
            // Get only the unique matches - we're going to replace all like instances each round.
            var matches = Regex.Matches(text.ToString(), "{{(?<field>[a-zA-Z0-9:]+)}}");

            return matches;
        }
        #endregion
    }
}