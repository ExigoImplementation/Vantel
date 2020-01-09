namespace Common.Api.ExigoWebService
{
    public partial class GetCompanyNewsItemResponse
    {
        public static explicit operator ExigoService.CompanyNewsItem(GetCompanyNewsItemResponse newsItem)
        {
            var model = new ExigoService.CompanyNewsItem();
            if (newsItem == null) return model;

            model.NewsID  = newsItem.NewsID;
            model.Description       = newsItem.Description;
            model.Content        = newsItem.Content;
            model.CreatedDate = newsItem.CreatedDate;
            model.WebSettings = newsItem.WebSettings;
            model.Departments = newsItem.Departments;

            return model;
        }
    }
}