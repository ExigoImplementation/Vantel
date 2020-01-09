using Newtonsoft.Json;
using System;

namespace ExigoService
{
    public class PaymentMethodJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(IPaymentMethod));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            object result = null;

            if (result == null) result = serializer.Deserialize<CreditCard>(reader);
            if (result == null) result = serializer.Deserialize<BankAccount>(reader);

            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}