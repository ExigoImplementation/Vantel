using System.Web.Mvc;

namespace Common.ModelBinders
{
    public static class GlobalModelBinders
    {
        /// <summary>
        /// Globally-used model binders
        /// </summary>
        public static void Register(ModelBinderDictionary modelBinders)
        {
            modelBinders.DefaultBinder = new CustomModelBinder();
            modelBinders.Add(typeof(DateModelBinder), new DateModelBinder());
        }
    }
}