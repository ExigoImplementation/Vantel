using System.Collections.Generic;
using System.Linq;
using System;
using Dapper;


namespace ExigoService
{
    public static partial class ExigoDAL
    {
        #region ResourceItems

        public static List<ResourceItem> GetResourceItems(GetResourcesRequest request)
        {
            // Establish the base query
            var query = "SELECT ItemID, typeID, Title, Url, UrlThumbnail, CreatedDate, PostDate, ItemDescription, StatusID, LanguageID FROM ExigoWebContext.ResourceItems";

            // Apply any filters
            var filters = 0;
            if (request.ItemID != Guid.Empty)
            {
                query += " WHERE ItemID = @itemid";
                filters++;
            }

            if (request.TypeID != Guid.Empty)
            {
                var filterText = (filters > 0) ? "AND" : "WHERE";

                query += String.Format(" {0} TypeID = @typeid", filterText);
                filters++;
            }

            if (request.ItemIDs != null && request.ItemIDs.Count() > 0)
            {
                var filterText = (filters > 0) ? "AND" : "WHERE";

                query += String.Format(" {0} ItemID in @itemidlist", filterText);
                filters++;
            }

            if (request.SearchFilter.IsNotNullOrEmpty())
            {
                var filterText = (filters > 0) ? "AND" : "WHERE";

                query += String.Format(" {0} Title LIKE @searchfilter", filterText);
                filters++;
            }




            var model = new List<ResourceItem>();

            using (var context = ExigoDAL.Sql())
            {
                model = context.Query<ResourceItem>(query, new { itemid = request.ItemID, typeid = request.TypeID, itemidlist = request.ItemIDs, searchfilter = request.SearchFilter }).ToList();
            }
            return model;
        }

        public static List<ResourceItem> GetResourceItemsByCategory(Guid categoryID)
        {
            var resources = new List<ResourceItem>();
            using (var context = ExigoDAL.Sql())
            {
                resources = context.Query<ResourceItem>(@"
                    SELECT *

                    FROM ExigoWebContext.ResourceItems i
	                    INNER JOIN ExigoWebContext.ResourceCategoryItems c
		                    ON c.ItemID = i.ItemID
	                                
                    WHERE c.CategoryID = @categoryID
                    ", new
                {
                    categoryID = categoryID
                }).ToList();
            }

            return resources;
        }

        public static void ModifyResourceItem(ResourceItem item)
        {
            using (var context = ExigoDAL.Sql())
            {
                context.Execute(@"
                UPDATE ExigoWebContext.ResourceItems
                SET
                TypeID = @typeid,
                Title = @title,
                Url = @url,
                UrlThumbnail = @urlthumbnail,
                CreatedDate = @createddate,
                PostDate = @postdate,
                ItemDescription = @description,
                StatusID = @statusid,
                Language = @language
                WHERE
                ItemID = @itemid
                ", new { typeid = item.TypeID, title = item.Title, url = item.Url, urlthumbnail = item.UrlThumbnail, createddate = item.CreatedDate, postdate = item.PostDate, description = item.ItemDescription, statusid = item.StatusID, itemid = item.ItemID, language = item.Language });
            }
        }

        public static void CreateResourceItem(ResourceItem item)
        {
            using (var context = ExigoDAL.Sql())
            {
                context.Execute(@"
                INSERT INTO
                    ExigoWebContext.ResourceItems
                    (ItemID, TypeID, Title, Url, UrlThumbnail, CreatedDate, PostDate, StatusID, ItemDescription, LanguageID)
                VALUES
                    (@itemid, @typeid, @title, @url, @urlthumb, @create, @post, @statusid, @description, @languageid)

                ", new { itemid = item.ItemID, typeid = item.TypeID, title = item.Title, url = item.Url, urlthumb = item.UrlThumbnail, create = item.CreatedDate, post = item.PostDate, statusid = item.StatusID, description = item.ItemDescription, languageid = item.LanguageID });
            }
        }

        public static void DeleteResourceItem(ResourceItem item)
        {
            using (var context = ExigoDAL.Sql())
            {
                context.Execute(@"
                    DELETE FROM
                        ExigoWebContext.ResourceItems
                    WHERE
                        ItemID = @item


                ", new { item = item.ItemID });
            }
        }

        #endregion

        #region Resource Categories
        public static List<ResourceCategory> GetResourceCategories(GetResourceCategoriesRequest request)
        {
            var query = "SELECT CategoryDescription, CategoryID, CategoryOrder, ParentID FROM ExigoWebContext.ResourceCategories";

            // Apply the filters
            var filters = 0;

            if (request.CategoryID != null && request.CategoryID != Guid.Empty)
            {
                query += " WHERE CategoryID = @catid";
                filters++;
            }

            if (request.CategoryIDs != null && request.CategoryIDs.Count() > 0)
            {
                var filterText = (filters > 0) ? "AND" : "WHERE";

                query += String.Format(" {0} CategoryID in @catidlist", filterText);
                filters++;
            }

            if (request.CategoryOrder != 0)
            {
                var filterText = (filters > 0) ? "AND" : "WHERE";

                query += String.Format(" {0} CategoryOrder > @categoryorder", filterText);
                filters++;
            }

            if (request.ParentID != null && request.ParentID != Guid.Empty)
            {
                var filterText = (filters > 0) ? "AND" : "WHERE";

                query += String.Format(" {0} ParentID = @parentid", filterText);
                filters++;
            }


            var model = new List<ResourceCategory>();
            using (var context = ExigoDAL.Sql())
            {
                model = context.Query<ResourceCategory>(query, new { catid = request.CategoryID, catidlist = request.CategoryIDs, categoryorder = request.CategoryOrder, parentid = request.ParentID }).ToList();
            };

            return model;
        }

        public static void AddResourceCategory(ResourceCategory newCategory)
        {
            using (var context = ExigoDAL.Sql())
            {
                context.Execute(@"
                    INSERT 
                            ExigoWebContext.ResourceCategories
                            (CategoryID, CategoryDescription, CategoryOrder, ParentID)
                    VALUES
                            (@categoryid, @categorydescription, @categoryorder, @parentid)
                                                        

                ", new { categoryid = newCategory.CategoryID, categorydescription = newCategory.CategoryDescription, categoryorder = newCategory.CategoryOrder, parentid = newCategory.ParentID });
            }

        }

        public static void ModifyResourceCategory(ResourceCategory category)
        {
            using (var context = ExigoDAL.Sql())
            {
                context.Execute(@"
               
                UPDATE ExigoWebContext.ResourceCategories
                SET CategoryDescription = @description, ParentID = @parentid
                WHERE CategoryID = @id 

            ", new { description = category.CategoryDescription, id = category.CategoryID, parentid = category.ParentID });
            }
        }

        public static void ModifyResourceCategoryDescription(ResourceCategory category)
        {
            using (var context = ExigoDAL.Sql())
            {
                context.Execute(@"
               
                UPDATE ExigoWebContext.ResourceCategories
                SET CategoryDescription = @description
                WHERE CategoryID = @id 

            ", new { description = category.CategoryDescription, id = category.CategoryID });
            }
        }

        public static void ModifyResourceCategoryOrder(ResourceCategory category)
        {
            using (var context = ExigoDAL.Sql())
            {
                context.Execute(@"
               
                UPDATE ExigoWebContext.ResourceCategories
                SET CategoryOrder = @order, ParentID = @parentid
                WHERE CategoryID = @id 

            ", new { order = category.CategoryOrder, id = category.CategoryID, parentid = category.ParentID });
            }
        }

        public static void DeleteResourceCategory(ResourceCategory category)
        {
            using (var context = ExigoDAL.Sql())
            {
                context.Execute(@"
                    DELETE FROM
                        ExigoWebContext.ResourceCategories
                    WHERE   
                        CategoryID = @category


            ", new { category = category.CategoryID });
            }
        }
        #endregion

        #region Resource Category Items
        public static List<ResourceCategoryItem> GetResourceCategoryItems(GetResourceCategoryItemsRequest request)
        {
            // Establish the base query
            var query = "SELECT ItemID, CategoryID, ItemOrder FROM ExigoWebContext.ResourceCategoryItems";

            // Apply any filters
            var filters = 0;
            if (request.ItemID != Guid.Empty)
            {
                query += " WHERE ItemID = @itemid";
                filters++;
            }

            if (request.CategoryID != Guid.Empty)
            {
                var filterText = (filters > 0) ? "AND" : "WHERE";

                query += String.Format(" {0} CategoryID = @catid", filterText);
                filters++;
            }

            if (request.ItemIDs != null && request.ItemIDs.Count() > 0)
            {
                var filterText = (filters > 0) ? "AND" : "WHERE";

                query += String.Format(" {0} ItemID in @itemidlist", filterText);
                filters++;
            }

            if (request.CategoryIDs != null && request.CategoryIDs.Count() > 0)
            {
                var filterText = (filters > 0) ? "AND" : "WHERE";

                query += String.Format(" {0} CategoryID in @categorylist", filterText);
                filters++;
            }

            if (request.ItemOrder != 0)
            {
                var filterText = (filters > 0) ? "AND" : "WHERE";

                query += String.Format(" {0} ItemOrder > @itemOrder", filterText);
                filters++;
            }

            var model = new List<ResourceCategoryItem>();
            using (var context = ExigoDAL.Sql())
            {
                model = context.Query<ResourceCategoryItem>(query, new { itemid = request.ItemID, catid = request.CategoryID, itemidlist = request.ItemIDs, itemOrder = request.ItemOrder, categorylist = request.CategoryIDs }).ToList();
            }
            return model;
        }

        public static void CreateResourceCategoryItem(ResourceCategoryItem item)
        {
            using (var context = ExigoDAL.Sql())
            {
                context.Execute(@"
                INSERT
                    ExigoWebContext.ResourceCategoryItems
                    (ItemID, CategoryID, ItemOrder)
                VALUES
                    (@itemid, @categoryid, @itemorder)

                ", new { itemid = item.ItemID, categoryid = item.CategoryID, itemorder = item.ItemOrder });
            }
        }

        public static void DeleteResourceCategoryItems(Guid itemID, List<Guid> categoryID)
        {
            using (var context = ExigoDAL.Sql())
            {
                context.Execute(@"
                    DELETE FROM 
                        ExigoWebContext.ResourceCategoryItems
                    WHERE ItemID = @id
                    AND CategoryID IN @categoryids
                
                ", new {
                    id = itemID,
                    categoryids = categoryID
                });
            }
        }

        public static void ModifyResourceCategoryItemOrder(ResourceCategoryItem categoryItem)
        {
            using (var context = ExigoDAL.Sql())
            {
                context.Execute(@"
               
                UPDATE ExigoWebContext.ResourceCategoryItems
                SET ItemOrder = @neworder
                WHERE ItemID = @itemid
                AND CategoryID = @categoryid

            ", new { neworder = categoryItem.ItemOrder, itemid = categoryItem.ItemID, categoryid = categoryItem.CategoryID });
            }
        }


        #endregion

        #region Resource Translations
        public static List<ResourceTranslatedCategoryItem> GetCategoryTranslations(GetTranslatedCategoryRequest request)
        {
            // Establish the base query
            var query = "SELECT Language, CategoryID, TranslatedCategoryDescription, TranslatedCategoryID FROM ExigoWebContext.ResourceTranslatedCategoryItems";

            // Apply any filters
            var filters = 0;
            if (request.CategoryID != Guid.Empty)
            {
                query += " WHERE CategoryID = @catid";
                filters++;
            }

            if (request.Language != null && request.Language.Count() > 0)
            {
                var filterText = (filters > 0) ? "AND" : "WHERE";

                query += String.Format(" {0} Language = @lang", filterText);
                filters++;
            }

            if (request.TranslatedCategoryID != Guid.Empty)
            {
                var filterText = (filters > 0) ? "AND" : "WHERE";

                query += String.Format(" {0} TranslatedCategoryID = @tcid", filterText);
                filters++;
            }

            if (request.CategoryIDs != null && request.CategoryIDs.Count() > 0)
            {
                var filterText = (filters > 0) ? "AND" : "WHERE";

                query += String.Format(" {0} CategoryID in @catidlist", filterText);

                filters++;
            }

            // Run the query
            var model = new List<ResourceTranslatedCategoryItem>();
            using (var context = ExigoDAL.Sql())
            {
                model = context.Query<ResourceTranslatedCategoryItem>(query, new { catid = request.CategoryID, lang = request.Language, tcid = request.TranslatedCategoryID, catidlist = request.CategoryIDs }).ToList();
            }
            return model;
        }

        public static void ModifyResourceTranslationDescription(ResourceTranslatedCategoryItem resource)
        {
            using (var context = ExigoDAL.Sql())
            {
                context.Execute(@"
               
                UPDATE ExigoWebContext.ResourceTranslatedCategoryItems
                SET TranslatedCategoryDescription = @description
                WHERE TranslatedCategoryID = @id 

            ", new { description = resource.TranslatedCategoryDescription, id = resource.TranslatedCategoryID });
            }
        }

        public static void AddCategoryTranslation(ResourceTranslatedCategoryItem rtcItems)
        {
            using (var context = ExigoDAL.Sql())
            {
                context.Execute(@"
                    INSERT 
                            ExigoWebContext.ResourceTranslatedCategoryItems
                            (TranslatedCategoryID, CategoryID, Language, TranslatedCategoryDescription)
                    VALUES
                            (@translatedcategoryid, @categoryid, @language, @categorydescription)
                    
                                                        

                ", new { translatedcategoryid = rtcItems.TranslatedCategoryID, categoryid = rtcItems.CategoryID, language = rtcItems.Language, categorydescription = rtcItems.TranslatedCategoryDescription });
            }
        }

        #endregion

        #region Resource Types
        public static List<ResourceType> GetResourceTypes(GetResourceTypeRequest request)
        {
            var query = "SELECT TypeID, TypeDescription, SortOrder  FROM ExigoWebContext.ResourceTypes";

            // Apply any filters
            var filters = 0;
            if (request.TypeID != Guid.Empty)
            {
                query += " WHERE TypeID = @id";
                filters++;
            }

            if (request.TypeDescription != null && request.TypeDescription != "")
            {
                var filterText = (filters > 0) ? "AND" : "WHERE";

                query += String.Format(" {0} TypeDescription = @description", filterText);
                filters++;
            }

            if (request.TypeDescriptions != null && request.TypeDescriptions.Count() > 0)
            {
                var filterText = (filters > 0) ? "AND" : "WHERE";

                query += String.Format(" {0} TypeDescription IN @typeDescriptions", filterText);
                filters++;
            }

            var model = new List<ResourceType>();
            using (var context = ExigoDAL.Sql())
            {
                model = context.Query<ResourceType>(query, new { id = request.TypeID, description = request.TypeDescription, typedescriptions = request.TypeDescriptions }).ToList();
            }
            return model;
        }

        #endregion

        #region Resource Market Availabilities
        public static List<ResourceAvailability> GetResourceAvailabilities(GetResourceAvailabilitiesRequest request)
        {
            var query = "SELECT AvailabilityID, LanguageID, ItemID, CountryCode FROM ExigoWebContext.ResourceAvailabilities";

            // Apply any filters
            var filters = 0;
            if (request.AvailabilityID != Guid.Empty)
            {
                query += " WHERE AvailabilityID = @availabilityid";
                filters++;
            }

            if (request.ItemID != Guid.Empty)
            {
                var filterText = (filters > 0) ? "AND" : "WHERE";

                query += String.Format(" {0} ItemID = @itemid", filterText);
                filters++;
            }

            if (request.LanguageID != null)
            {
                var filterText = (filters > 0) ? "AND" : "WHERE";

                query += String.Format(" {0} LanguageID = @languageid", filterText);
                filters++;
            }

            if (request.CountryCode != null && request.CountryCode != "")
            {
                var filterText = (filters > 0) ? "AND" : "WHERE";

                query += String.Format(" {0} CountryCode = @countrycode", filterText);
                filters++;
            }

            var model = new List<ResourceAvailability>();
            using (var context = ExigoDAL.Sql())
            {
                model = context.Query<ResourceAvailability>(query, new { availabilityid = request.AvailabilityID, itemid = request.ItemID, languageid = request.LanguageID, countrycode = request.CountryCode }).ToList();
            }
            return model;
        }

        public static void CreateResourceAvailabilities(ResourceAvailability resource)
        {
            using (var context = ExigoDAL.Sql())
            {
                context.Execute(@"
                INSERT INTO
                    ExigoWebContext.ResourceAvailabilities
                    (AvailabilityID, LanguageID, ItemID, CountryCode)
                VALUES
                    (@availabilityid, @languageid, @itemid, @countrycode)
                ", new { availabilityid = resource.AvailabilityID, languageid = resource.LanguageID, itemid = resource.ItemID, countrycode = resource.CountryCode });
            }
        }

        public static void DeleteResourceAvailabilities(List<Guid> availableID)
        {
            using (var context = ExigoDAL.Sql())
            {
                context.Execute(@"
                    DELETE FROM 
                        ExigoWebContext.ResourceAvailabilities
                    WHERE AvailabilityID IN @availabilities
                
                ", new { availabilities = availableID });
            }
        }
        #endregion

        #region Resource Item Tags

        public static List<ResourceItemTag> GetResourceItemTags(GetResourceItemTagsRequest request)
        {
            var query = "SELECT TagID, ItemID FROM ExigoWebContext.ResourceItemTags";

            // Apply any filters
            var filters = 0;
            if (request.TagID != Guid.Empty)
            {
                query += " WHERE TagID = @tagid";
                filters++;
            }

            if (request.ItemID != Guid.Empty)
            {
                var filterText = (filters > 0) ? "AND" : "WHERE";

                query += String.Format(" {0} ItemID = @itemid", filterText);
                filters++;
            }

            if (request.TagIDs != null && request.TagIDs.Count > 0)
            {
                var filterText = (filters > 0) ? "AND" : "WHERE";
                query += String.Format(" {0} TagID IN @tagids", filterText);
                filters++;
            }

            var model = new List<ResourceItemTag>();
            using (var context = ExigoDAL.Sql())
            {
                model = context.Query<ResourceItemTag>(query, new { tagid = request.TagID, itemid = request.ItemID, request.TagIDs }).ToList();
            }
            return model;
        }

        public static void CreateResourceItemTag(ResourceItemTag tag)
        {
            using (var context = ExigoDAL.Sql())
            {
                context.Execute(@"
                INSERT
                    ExigoWebContext.ResourceItemTags
                    (TagID, ItemID)
                VALUES
                    (@tagid, @itemid)

                ", new { tagid = tag.TagID, itemid = tag.ItemID });
            }
        }

        public static void DeleteResourceItemTags(List<Guid> tagids)
        {
            using (var context = ExigoDAL.Sql())
            {
                context.Execute(@"
                    DELETE FROM
                        ExigoWebContext.ResourceItemTags
                    WHERE
                        TagID IN @tags


                ", new { tags = tagids });
            }
        }
        #endregion

        #region Resource Status
        public static List<ResourceStatus> GetResourceStatuses()
        {
            var query = "SELECT StatusID, StatusDescription FROM ExigoWebContext.ResourceStatuses";


            var model = new List<ResourceStatus>();
            using (var context = ExigoDAL.Sql())
            {
                model = context.Query<ResourceStatus>(query).ToList();
            }
            return model;
        }

        public static class ResourceStatuses
        {
            public static Guid Active
            {
                get
                {
                    return Guid.Parse("691F9CFD-502C-4B69-A2CB-C2286B7CB8FD");
                }
            }

            public static Guid Pending
            {
                get
                {
                    return Guid.Parse("5B426376-2C5E-4506-B3C6-F21ADFC2D17E");
                }
            }
        }
        #endregion

        #region Tags
        public static List<Tag> GetTagsForResources(GetTagsForResourcesRequest request)
        {
            // Set the SQL Command
            var query = @" SELECT TagID, Name FROM ExigoWebContext.Tags ";

            // Apply any filters
            var filters = 0;
            if (request.TagID != Guid.Empty)
            {
                query += " WHERE TagID = @tagid";
                filters++;
            }

            if (request.Name != null && request.Name != "")
            {
                var filterText = (filters > 0) ? "AND" : "WHERE";

                query += String.Format(" {0} Name = @name", filterText);
                filters++;
            }

            if (request.Names != null && request.Names.Count() > 0)
            {
                var filterText = (filters > 0) ? "AND" : "WHERE";

                query += String.Format(" {0} Name in @names", filterText);
                filters++;
            }

            // Establish a SQL Command
            var model = new List<Tag>();
            using (var context = ExigoDAL.Sql())
            {
                // Execute the SQL Command and set the results to the Collection constructed above
                model = context.Query<Tag>(query, new { tagid = request.TagID, name = request.Name, names = request.Names }).ToList();
            }

            // Return the collection of Tag Name
            return model;
        }

        public static void CreateTag(Tag tag)
        {
            using (var context = ExigoDAL.Sql())
            {
                context.Execute(@"
                INSERT
                    ExigoWebContext.Tags
                    (TagID, Name)
                VALUES
                    (@tagid, @name)
                ", new { tagid = tag.TagID, name = tag.Name });
            }
        }
        #endregion
    }
}