using System;
using System.Web.Mvc;

namespace ExigoService
{
    public class IPaymentMethodModelBinder : DefaultModelBinder
    {
        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            if (modelType.Name != "IPaymentMethod") return base.CreateModel(controllerContext, bindingContext, modelType);

            var paymentMethodType = bindingContext.ModelName + ".PaymentMethodType";

            var rawClassName = bindingContext.ValueProvider.GetValue(paymentMethodType);
            if (rawClassName == null || string.IsNullOrEmpty(rawClassName.ToString())) throw new Exception("You cannot model-bind to a property of type {0} without passing the desired model class name through a form field named '{1}'.".FormatWith(modelType.ToString(), paymentMethodType));

            var className = rawClassName.AttemptedValue.ToString();
            modelType = Type.GetType(className);

            if (modelType.Name == "String[]") throw new Exception("You cannot pass more than one form field named '{0}' when model-binding to IPaymentMethod.".FormatWith(paymentMethodType));

            if (modelType != null)
            {
                var instance = Activator.CreateInstance(modelType);
                bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => instance, modelType);

                return instance;
            }
            else
            {
                return null;
            }
        }
    }
}