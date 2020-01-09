using Common.Filters;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;

namespace Common.ModelBinders
{
    public class CustomModelBinder : DefaultModelBinder
    {
        protected override void BindProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, System.ComponentModel.PropertyDescriptor propertyDescriptor)
        {
            // Check if the property has the PropertyBinderAttribute, meaning it's specifying a different binder to use.

           
            var propertyBinderAttribute = TryFindPropertyBinderAttribute(propertyDescriptor);
            if (propertyBinderAttribute != null)
            {
                var binder = CreateBinder(propertyBinderAttribute);
                var value = binder.BindModel(controllerContext, bindingContext);
                try
                {
                    propertyDescriptor.SetValue(bindingContext.Model, value);
                }
                catch 
                {
                    base.BindProperty(controllerContext, bindingContext, propertyDescriptor);
                }
            }
            else // revert to the default behavior.
            {
                base.BindProperty(controllerContext, bindingContext, propertyDescriptor);
            }
        }

        IModelBinder CreateBinder(PropertyBinderAttribute propertyBinderAttribute)
        {
            return (IModelBinder)DependencyResolver.Current.GetService(propertyBinderAttribute.BinderType);
        }

        PropertyBinderAttribute TryFindPropertyBinderAttribute(PropertyDescriptor propertyDescriptor)
        {
            return propertyDescriptor.Attributes
              .OfType<PropertyBinderAttribute>()
              .FirstOrDefault();
        }
    }
}