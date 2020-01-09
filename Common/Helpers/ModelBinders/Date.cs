using System;
using System.ComponentModel;
using System.Web.Mvc;

namespace Common.ModelBinders
{
    public class DateModelBinder : DefaultModelBinder
    {
        protected override void BindProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, PropertyDescriptor propertyDescriptor)
        {
            if (propertyDescriptor.PropertyType == typeof(DateTime?) || propertyDescriptor.PropertyType == typeof(DateTime))
            {
                var request = controllerContext.HttpContext.Request;
                var propertyName = propertyDescriptor.Name;

                var year = GetValue<int>(bindingContext, string.Format("{0}.Year", propertyName));
                var month = GetValue<int>(bindingContext, string.Format("{0}.Month", propertyName));
                var day = GetValue<int>(bindingContext, string.Format("{0}.Day", propertyName));


                if (year != null && month != null && day != null)
                {
                    var date = new DateTime((int)year, (int)month, (int)day);
                    base.SetProperty(controllerContext, bindingContext, propertyDescriptor, date);
                }
            }

            base.BindProperty(controllerContext, bindingContext, propertyDescriptor);
        }

        private Nullable<T> GetValue<T>(ModelBindingContext bindingContext, string key) where T : struct
        {
            if (String.IsNullOrEmpty(key)) return null;
            ValueProviderResult valueResult;

            //Try it with the prefix...
            valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName + "." + key);

            //Didn't work? Try without the prefix if needed...
            if (valueResult == null && bindingContext.FallbackToEmptyPrefix == true)
            {
                valueResult = bindingContext.ValueProvider.GetValue(key);
            }
            if (valueResult == null)
            {
                return null;
            }
            return (Nullable<T>)valueResult.ConvertTo(typeof(T));
        }
    }
}