using Common.Services;

namespace ExigoService
{
    public class Language : ILanguage
    {
        public Language() { }
        public Language(int languageID, string cultureCode)
        {
            this.LanguageID = languageID;
            this.CultureCode = cultureCode;
            this.LanguageDescription = GetLanguageDescription();
        }

        public int LanguageID { get; set; }
        public string LanguageDescription { get; set; }
        public string CultureCode { get; set; }

        /// <summary>
        /// Gets the language's description from the Languages global resource file
        /// </summary>
        public string GetLanguageDescription()
        {
            var result = this.LanguageDescription;

            try
            {
                var resourceValue = CommonResources.Languages(this.LanguageID);
                if (resourceValue != null)
                {
                    result = (string)resourceValue;
                    this.LanguageDescription = result;
                }
            }
            catch { }

            return result;
        }
    }
}