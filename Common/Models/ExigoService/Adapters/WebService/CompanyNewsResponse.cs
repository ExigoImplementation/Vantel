namespace Common.Api.ExigoWebService
{
    public partial class CompanyNewsResponse
    {
        public static explicit operator ExigoService.CompanyNewsItem(CompanyNewsResponse newsItem)
        {
            var model = new ExigoService.CompanyNewsItem();
            if (newsItem == null) return model;

            model.NewsID  = newsItem.NewsID;
            model.Description       = newsItem.Description;
            model.Content        = string.Empty;
            model.CreatedDate = newsItem.CreatedDate;
            model.WebSettings = newsItem.WebSettings;
            model.CompanySettings = newsItem.CompanySettings;
            return model;
        }
    }
}