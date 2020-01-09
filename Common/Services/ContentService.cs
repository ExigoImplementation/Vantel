using Common;
using System.Collections.Generic;
using System.Linq;
using System;
using Dapper;
using ExigoService;

namespace Common.Services
{
    public static class ContentService
    {
        #region contentItem

        /// <summary>
        /// Filters items in the ContentItems table and returns only Log-In Alerts.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private static List<ContentItem> GetContentItems(GetContentItemRequest request)
        {
            var currentSite = GetContentSites(request.SiteDescription).FirstOrDefault().SiteID.ToString().ToUpper();
            var currentEnvironment = GetCurrentContentEnvironment().EnvironmentID.ToString().ToUpper();

            // Establish the base query
            var query = "SELECT";
            query += " ContentItemID, SiteID, ViewID, ContentDescription, CountryID, LanguageID, ValidFrom, ExpirationDate, EnvironmentID";
            query += " FROM ExigoWebContext.ContentItems";

            if (!request.GetAllAlerts)
            {
                query += " Where ";
                query += " ValidFrom <= GETDATE()";
                query += " And ";
                query += " (ExpirationDate >= GETDATE() Or ExpirationDate IS NULL)";

                if (request.ContentItemIDs != null)
                {
                    query += " And ";
                    query += " ContentItemID in @contentitemid";
                }

                if (request.GetAlert)
                {
                    query += " And ";
                    query += " ViewID = 'Alert' ";
                }
            } else
            {
                query += " Where ";
                query += " ViewID = 'Alert' ";
            }

            query += " And ";
            query += " EnvironmentID = @contentenvironment";
            query += " And ";
            query += " SiteID = @contentsite";
            query += " ORDER BY ValidFrom DESC";

            var model = new List<ContentItem>();

            using (var context = ExigoDAL.Sql())
            {
                model = context.Query<ContentItem>(query, new { contentenvironment = currentEnvironment, contentsite = currentSite, contentitemid = request.ContentItemIDs }).ToList();
            }
            return model;
        }

        /// <summary>
        ///  Creates a content item into the ContentItems table with the fields passed to it in the object.
        /// </summary>
        /// <param name="item"></param>
        private static ContentItem SetContentItems(ContentItem content)
        {
            if (content.ContentItemID == null) {
                content.ContentItemID = Guid.NewGuid();
                using (var context = ExigoDAL.Sql())
                {
                    var currentSite = GetContentSites(content.SiteDescription).FirstOrDefault().SiteID.ToString();
                    var currentEnvironment = GetCurrentContentEnvironment().EnvironmentID.ToString();

                    context.Execute(@"
                    INSERT INTO
                        ExigoWebContext.ContentItems
                        (ContentItemID, SiteID, ViewID, ContentDescription, CountryID, LanguageID, ValidFrom, ExpirationDate, EnvironmentID)
                    VALUES
                        (@contentitemid, @siteid, @viewid, @contentdescription, @countryid, @languageid, @validfrom, @expirationdate, @environmentid)

                    ", new { contentitemid = content.ContentItemID.ToString(),
                        siteid = currentSite,
                        viewid = content.ViewID,
                        contentdescription = content.ContentDescription,
                        countryid = content.CountryID,
                        languageid = content.LanguageID,
                        validfrom = content.ValidFrom,
                        expirationdate = content.ExpirationDate,
                        environmentid = currentEnvironment
                    });
                }
            }
            else
            {
                using (var context = ExigoDAL.Sql())
                {
                    context.Execute(@"
                UPDATE ExigoWebContext.ContentItems
                SET
                ContentDescription = @contentdescription,
                ValidFrom = @validfrom,
                ExpirationDate = @expirationdate,
                LanguageID = @languageid
                WHERE
                ContentItemID = @itemid
                ", new { itemid = content.ContentItemID,
                        contentdescription = content.ContentDescription,
                        validfrom = content.ValidFrom,
                        expirationdate = content.ExpirationDate,
                        languageid = content.LanguageID });
                }
            }
            return content;
        }

        /// <summary>
        /// Deletes content item from ContentItems table where ContentItemID matches the string passed as a parameter.
        /// </summary>
        /// <param name="contentItemID"></param>
        private static void DeleteContentItems(string contentItemID)
        {
            using (var context = ExigoDAL.Sql())
            {
                context.Execute(@"
                    DELETE FROM
                        ExigoWebContext.ContentItems
                    WHERE
                        ContentItemID = @contentItemID


                ", new { contentItemID = contentItemID });
            }
        }

        #endregion
        #region contentItemCountryLanguage

        /// <summary>
        /// Gets all content from ContentItemCountryLanguages table and returns them in a list.
        /// </summary>
        /// <param name="contentItemID">Optional parameter. If passed it will only retrieve that specific contnent item.</param>
        /// <returns></returns>
        private static List<ContentItem> GetContentItemCountryLanguages(GetContentItemCountryLanguagesRequest request)
        {
            // Establish the base query
            var query = "SELECT";
            query += " ContentItemCountryLanguageID, ContentItemID, CountryID, LanguageID, Content";
            query += " FROM ExigoWebContext.ContentItemCountryLanguages";

            if (request.CountryCode != "" && request.LanguageID != null)
            {
                query += " Where CountryID = @countrycode";
                query += " And LanguageID = @languageid";

                if (request.ContentItemIDs.Any())
                {
                    query += " And ContentItemID in @contentitemids";
                }
            }

            else if (request.ContentItemIDs.Any())
            {
                query += " Where ContentItemID in @contentitemids";
            }

            var model = new List<ContentItem>();

            using (var context = ExigoDAL.Sql())
            {
                model = context.Query<ContentItem>(query, new { countrycode = request.CountryCode, languageid = request.LanguageID, contentitemids = request.ContentItemIDs }).ToList();
            }

            if (model.Any() && request.ContentItems.Any())
            {
                foreach (var requestContentItem in request.ContentItems)
                {
                    var contentItem = model.Where(c => c.ContentItemID == requestContentItem.ContentItemID).FirstOrDefault();
                    if (contentItem != null)
                    {
                        model.Where(c => c.ContentItemID == requestContentItem.ContentItemID).FirstOrDefault().ContentDescription = requestContentItem.ContentDescription;
                    }
                }
            }

            return model;
        }

        /// <summary>
        /// Updates content from ContentItemCountryLanguages table with fields passed in request.
        /// </summary>
        /// <param name="request"></param>
        private static void SetContentItemCountryLanguages(ContentItem item)
        {
            if(item.CountryLanguageList == null)
            {
                // Check to see if instances of the object exist in the database.
                var query = "SELECT COUNT (*)";
                query += " FROM ExigoWebContext.ContentItemCountryLanguages";
                query += " WHERE ";
                query += "     ContentItemID = @itemid ";

                var instanceCount = 0;
                using (var context = ExigoDAL.Sql())
                {
                    instanceCount = context.ExecuteScalar<int>(query, new { itemid = item.ContentItemID });
                }
                if (instanceCount == 0) return;

                // Update content for specific country/language in ContentItemCountryLanguages
                var recordsAffected = 0;
                using (var context = ExigoDAL.Sql())
                {
                    recordsAffected = context.Execute(@"
                        UPDATE ExigoWebContext.ContentItemCountryLanguages
                        SET
                        Content = @content
                        WHERE
                        ContentItemID = @itemid
                        AND
                        CountryID = @countryid
                        AND
                        LanguageID = @languageid

                    ", new { itemid = item.ContentItemID.ToString(), countryid = item.CountryID, languageid = item.LanguageID, content = item.Content });
                }

                if ( recordsAffected == 0 ) {
                    item.ContentItemCountryLanguageID = Guid.NewGuid();
                    // If content does not exist for specified country/language, we create it.
                    using (var context = ExigoDAL.Sql())
                    {
                        context.Execute(@"
                            INSERT INTO
                                ExigoWebContext.ContentItemCountryLanguages
                                (ContentItemCountryLanguageID, ContentItemID, CountryID, LanguageID, Content)
                            VALUES
                                (@ContentItemCountryLanguageid, @contentitemid, @countryid, @languageid, @content)

                            ", new { ContentItemCountryLanguageid = item.ContentItemCountryLanguageID.ToString(), contentitemid = item.ContentItemID.ToString(), countryid = item.CountryID, languageid = item.LanguageID, content = item.Content });
                    }
                }
            }
            else
            {
                // If request is an alert, we do the following:
                // For each country/language set, create one entry in the ContentItemCountryLanguage DB.
                foreach (var countryLanguage in item.CountryLanguageList)
                {
                    // Split country/language string into 'country' and 'language'
                    string[] countryLanguageSet = countryLanguage.Split('/');
                    // Set country and language.
                    var country = countryLanguageSet[0];
                    var language = int.Parse(countryLanguageSet[1]);

                    // Fill out new ContentItem with same content for every country/language but different CountryID and LanguageID.
                    ContentItem contentItemCountryLanguage = new ContentItem()
                    {
                        ContentItemCountryLanguageID = Guid.NewGuid(),
                        ContentItemID = item.ContentItemID,
                        Content = item.Content,
                        CountryID = GlobalSettings.Markets.AvailableMarkets.Where(c => c.CookieValue.ToString() == country).FirstOrDefault().CookieValue,
                        LanguageID = GlobalSettings.Markets.AvailableMarkets.Where(c => c.CookieValue.ToString() == country).FirstOrDefault().AvailableLanguages.Where(c => c.LanguageID == language).FirstOrDefault().LanguageID
                    };

                    using (var context = ExigoDAL.Sql())
                    {
                        context.Execute(@"
                INSERT INTO
                    ExigoWebContext.ContentItemCountryLanguages
                    (ContentItemCountryLanguageID, ContentItemID, CountryID, LanguageID, Content)
                VALUES
                    (@ContentItemCountryLanguageid, @contentitemid, @countryid, @languageid, @content)

                ", new { ContentItemCountryLanguageid = contentItemCountryLanguage.ContentItemCountryLanguageID, contentitemid = contentItemCountryLanguage.ContentItemID, countryid = contentItemCountryLanguage.CountryID, languageid = contentItemCountryLanguage.LanguageID, content = contentItemCountryLanguage.Content });
                    }
                };
            }
        }

        /// <summary>
        ///  Deletes a content into the ContentItemCountryLanguage table with the fields passed to it in the object.
        /// </summary>
        /// <param name="item"></param>
        private static void DeleteContentItemCountryLanguages(string contentItemID)
        {
            using (var context = ExigoDAL.Sql())
            {
                context.Execute(@"
                    DELETE FROM
                        ExigoWebContext.ContentItemCountryLanguages
                    WHERE
                        ContentItemID = @contentItemID


                ", new { contentItemID = contentItemID });
            }
        }

        #endregion
        #region contentEnvironment

        /// <summary>
        /// Gets all content from ContentEnvironments table and returns them in a list.
        /// </summary>
        /// <param name="contentItemID">Optional parameter. If passed it will only retrieve that specific contnent item.</param>
        /// <returns></returns>
        private static List<ContentEnvironment> GetContentEnvironments(string description = "")
        {
            // Establish the base query
            var query = "SELECT";
            query += " EnvironmentID, Description";
            query += " FROM ExigoWebContext.ContentEnvironments";

            if (description != "") { 
                query += " Where Description = @description";
            }

            var model = new List<ContentEnvironment>();

            using (var context = ExigoDAL.Sql())
            {
                model = context.Query<ContentEnvironment>(query, new { description = description }).ToList();
            }

            return model;
        }

        #endregion
        #region contentSite

        /// <summary>
        /// Gets all content from ContentSites table and returns them in a list.
        /// </summary>
        /// <param name="contentItemID">Optional parameter. If passed it will only retrieve that specific contnent item.</param>
        /// <returns></returns>
        private static List<ContentSite> GetContentSites(string description = "")
        {
            // Establish the base query
            var query = "SELECT";
            query += " SiteID, Description";
            query += " FROM ExigoWebContext.ContentSites";

            if (description != "")
            {
                query += " Where Description = @description";
            }

            var model = new List<ContentSite>();

            using (var context = ExigoDAL.Sql())
            {
                model = context.Query<ContentSite>(query, new { description = description }).ToList();
            }

            return model;
        }

        #endregion
        #region Helper Methods

        /// <summary>
        /// Gets current ContentEnvironment from the GetContentEnvironments private method by checking wether or not sandbox is on.
        /// </summary>
        /// <returns></returns>
        public static ContentEnvironment GetCurrentContentEnvironment()
        {
            var contentEnvironmentDescription = "";
            contentEnvironmentDescription = "Production";
            var currentContentEnvironment = GetContentEnvironments(contentEnvironmentDescription);
            return currentContentEnvironment.FirstOrDefault();
        }

        /// <summary>
        /// Gets a list of all available markets followed by their perspective available languages.
        /// </summary>
        public static void GetCountryAndLanguageSets(out List<string> availableCountries, out List<int> availableLanguages, out List<string> availableCountryAndLanguageSets)
        {
            // Get a list of all available markets for the Backoffice.
            var markets = GlobalSettings.Markets.AvailableMarkets.ToList();

            availableCountries =  new List<string>();
            availableLanguages = new List<int>();
            availableCountryAndLanguageSets = new List<string>();

            // For each available market and available language for that market, add to list in the format 'country/language'.
            foreach (var market in markets)
            {
                foreach (var language in market.AvailableLanguages)
                {
                    availableCountries.Add(market.CookieValue);
                    availableLanguages.Add(language.LanguageID);
                    availableCountryAndLanguageSets.Add(market.Description.ToString() + "/" + language.LanguageDescription.ToString());
                }
            }
        }

        /// <summary>
        /// Returns a list of all available languages for specific market.
        /// </summary>
        /// <returns></returns>
        public static List<Language> GetLanguages(string market)
        {
            // Empty string of Language to store available languages for this market.
            var availableLanguages = new List<Language>();

            // Add each available language of available market where the CookieValue matches the string passed as a parameter.
            foreach (var language in GlobalSettings.Markets.AvailableMarkets.Where(c => c.CookieValue == market).FirstOrDefault().AvailableLanguages)
            {
                availableLanguages.Add(language);
            }

            return availableLanguages;
        }

        /// <summary>
        /// Retreives alert from ContentItem database table.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static List<ContentItem> GetAlert(GetContentItemRequest content)
        {
            return ContentService.GetContentItems(content);
        }

        /// <summary>
        /// Retrieves alert from ContentItemCountryLanguages database table.
        /// </summary>
        /// <param name="contentItemCountryLanguagesRequest"></param>
        /// <returns></returns>
        public static List<ContentItem> GetAlert(GetContentItemCountryLanguagesRequest contentItemCountryLanguagesRequest)
        {
            return ContentService.GetContentItemCountryLanguages(contentItemCountryLanguagesRequest);
        }

        /// <summary>
        /// Gets content from the ContentItemCountryLanguages table according user location/language.
        /// </summary>
        /// <param name="request"></param>
        public static List<ContentItem> GetContentBlock(GetContentItemRequest request)
        {
            return ContentService.GetContentItems(request);
        }

        /// <summary>
        /// Gets content from the ContentItemCountryLanguages table according user location/language.
        /// </summary>
        /// <param name="request"></param>
        public static List<ContentItem> GetContentBlock(GetContentItemCountryLanguagesRequest request)
        {
            var content = ContentService.GetContentItemCountryLanguages(request);
            // Default to US English if the content block does not exist for specified country/language.
            if (!content.Any())
            {
                request.CountryCode = CountryCodes.UnitedStates;
                request.LanguageID = Languages.English;
                content = ContentService.GetContentItemCountryLanguages(request);
            }
            return content;
        }

        /// <summary>
        /// Updates content block in ContentItemCountryLanguages table and deletes from cache.
        /// </summary>
        /// <param name="contentBlock"></param>
        public static void SetContentBlock(ContentItem contentBlock)
        {
            ContentService.SetContentItemCountryLanguages(contentBlock);

            //Deletes content item ID from cache after modifying block, so that page will reload from DB instead of pulling from cache
            GlobalUtilities.DeleteFromCache(contentBlock.ContentItemID.ToString().ToUpper() + contentBlock.CountryID + contentBlock.LanguageID.ToString());
        }

        /// <summary>
        /// Creates and edits an alert.
        /// </summary>
        public static void SetAlert(ContentItem content)
        {
            // Create alert, else edit alert.
            if(content.ContentItemID == null)
            {
                content.CountryID = content.CountryIDList != null ? String.Join(", ", content.CountryIDList.ToArray()) : "";
                // Send item to CreateContentItem Exigo service to create alert in database.
                var contentItem = ContentService.SetContentItems(content);

                // For each country/language set, create one entry in the ContentItemCountryLanguage DB.
                ContentService.SetContentItemCountryLanguages(contentItem);
            }
            else
            {
                // Send ContentItem to EditContentItems exigo service.
                ContentService.SetContentItems(content);

                // Delete ContentItem from ContentItemCountryLanguage DB since we will re-set the country/language availability.
                ContentService.DeleteContentItemCountryLanguages(content.ContentItemID.ToString());

                //// Create one entry for each country/language selected.
                ContentService.SetContentItemCountryLanguages(content);

                //Deletes content item ID from cache after modifying block, so that page will reload from DB instead of pulling from cache.
                GlobalUtilities.DeleteFromCache(content.ContentItemID.ToString());
            }
        }

        /// <summary>
        /// Deletes alert from ContentItem and ContentItemCountryLanguages tables.
        /// </summary>
        /// <param name="contentItemID"></param>
        public static void DeleteAlert(string contentItemID)
        {
            // Delete the item from the ContentItem table.
            ContentService.DeleteContentItems(contentItemID);

            // Delete all content from the ContentItemCountryLanguage where the contentItemID matches that being passed.
            ContentService.DeleteContentItemCountryLanguages(contentItemID);
        }

        #endregion
    }
}