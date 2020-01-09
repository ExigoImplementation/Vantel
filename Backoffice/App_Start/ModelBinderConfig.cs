using Common.ModelBinders;
using System.Web.Mvc;

namespace Backoffice
{
    public class ModelBinderConfig
    {
        public static void RegisterModelBinders(ModelBinderDictionary modelBinders)
        {
            GlobalModelBinders.Register(modelBinders);
        }
    }
}