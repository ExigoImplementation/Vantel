using Newtonsoft.Json;
using System;

namespace ExigoService
{
    public class AddressJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(IAddress));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            object result = serializer.Deserialize<ShippingAddress>(reader);

            if (result == null) result = serializer.Deserialize<Address>(reader);

            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}