using Common.Api.ExigoWebService;
using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExigoService
{
    public static partial class ExigoDAL
    {
        public static List<CompanyNewsItem> GetCompanyNews(GetCompanyNewsRequest request)
        {

            var api = ExigoDAL.WebService();
            CompanyNewsResponse[] newsResponse;
            var newsItems = new List<CompanyNewsItem>();

            foreach (var department in request.NewsDepartments)
            {
                newsResponse = (api.GetCompanyNews(new Common.Api.ExigoWebService.GetCompanyNewsRequest
                {
                    DepartmentType = department
                }).CompanyNews);

                //Convert to our model 
                newsItems = newsResponse.Select(apiItem => (CompanyNewsItem)apiItem)
                    .Where(newsItem => (NewsWebSettings)newsItem.WebSettings == NewsWebSettings.AccessAvailable).ToList(); //and filter out anything not flagged as available in the backoffice


                // If they requested the content, then loop through all the articles and add the content to each news item
                var tasks = new List<Task>();
                foreach (var item in newsItems)
                {
                    tasks.Add(Task.Factory.StartNew(() =>
                    {
                        var newsItemResponse = (api.GetCompanyNewsItem(new Common.Api.ExigoWebService.GetCompanyNewsItemRequest
                        {
                            NewsID = item.NewsID
                        }));

                        //Add the content to the item
                        item.Content = newsItemResponse.Content;
                    }));

                    Task.WaitAll(tasks.ToArray());
                    tasks.Clear();
                }
            }

            return newsItems;
        }

        public static CompanyNewsItem GetCompanyNewsItem(GetCompanyNewsItemRequest request)
        {

            var api = ExigoDAL.WebService();
            CompanyNewsItem response;
            var newsItemResponse = (api.GetCompanyNewsItem(new Common.Api.ExigoWebService.GetCompanyNewsItemRequest
            {
                NewsID = request.NewsID
            }));

            //Convert to our model 
            CompanyNewsItem newsItem = (CompanyNewsItem)newsItemResponse;

            //Only respond if the article is flagged to backoffice visbility
            if (newsItem.WebSettings == NewsWebSettings.AccessAvailable)
            {
                response = newsItem;
            }

            return newsItem;

        }

        public static List<CompanyNewsItem> GetCompanyNewsSQL(GetCompanyNewsRequest request)
        {
            var NewsItems = new List<CompanyNewsItem>();
            string takeSkipQuery = string.Empty;
            string departmentsQuery = "WHERE cnd.DepartmentID IN @departments";

            // Ensure that we have the minimum requirements
            var hasRequestedDepartments = (request.NewsDepartments != null && request.NewsDepartments.Length > 0);
            var hasRequestedNewsItems = (request.NewsItemIDs != null && request.NewsItemIDs.Length > 0);


            if (!hasRequestedDepartments || request.NewsDepartments.Any(n => n == 0))
            {
                departmentsQuery = "";
            }

            //if (!hasRequestedDepartments && !hasRequestedNewsItems)
            //{
            //    return NewsItems;
            //}

            if (hasRequestedDepartments && request.RowCount > 0)
            {
                //NewsItems = NewsItems.Skip(request.Skip).Take(request.Take).ToList();
                takeSkipQuery = string.Format("OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", request.Skip, request.Take);
            }

            if (hasRequestedNewsItems)
            {
                using (var ctx = ExigoDAL.Sql())
                {
                    NewsItems = ctx.Query<CompanyNewsItem>(@"
                    SELECT 
	                      DISTINCT(cnd.NewsID) AS Department
	                    , cn.CompanyNewsID AS NewsID
	                    , cn.Title AS Description
	                    , cn.Content AS Content
	                    , cn.CreatedDate
                    FROM CompanyNewsDepartments cnd
                      INNER JOIN CompanyNews cn
                      ON cn.CompanyNewsID = cnd.NewsID
                      AND cn.AvailableOnWeb = 1
                    WHERE cn.CompanyNewsID IN @newsitems
                    ORDER BY cn.CreatedDate DESC
                    " + takeSkipQuery + @"
                ", new { newsitems = request.NewsItemIDs }).ToList();
                }
            }
            else
            {
                using (var ctx = ExigoDAL.Sql())
                {
                    NewsItems = ctx.Query<CompanyNewsItem>(@"
                    SELECT 
	                      DISTINCT(cnd.NewsID) AS Department
	                    , cn.CompanyNewsID AS NewsID
	                    , cn.Title AS Description
	                    , cn.Content AS Content
	                    , cn.CreatedDate
                    FROM CompanyNewsDepartments cnd
                      INNER JOIN CompanyNews cn
                      ON cn.CompanyNewsID = cnd.NewsID
                      AND cn.AvailableOnWeb = 1
                    " + departmentsQuery + @"
                    ORDER BY cn.CreatedDate DESC
                    " + takeSkipQuery + @"
                ", new { departments = request.NewsDepartments }).ToList();
                }
            }

            return NewsItems;
        }

        public static List<int> GetDepartments()
        {
            var newsDepartments = new List<int>();

            using (var context = ExigoDAL.Sql())
            {
                newsDepartments = context.Query<int>(@"
                            SELECT
                                d.DepartmentID
                            FROM
                                Departments d
                    ").ToList();
            };

            return newsDepartments;
        }
    }
}