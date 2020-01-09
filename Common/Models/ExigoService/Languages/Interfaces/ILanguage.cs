namespace ExigoService
{
    public interface ILanguage
    {
        int LanguageID { get; set; }
        string LanguageDescription { get; set; }
        string CultureCode { get; set; }
    }
}