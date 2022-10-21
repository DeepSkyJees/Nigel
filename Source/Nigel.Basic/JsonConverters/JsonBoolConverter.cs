using System;
using Newtonsoft.Json;

namespace Nigel.Basic.JsonConverters
{
    public class JsonBoolConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(bool);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader?.Value?.ToString() == "1" || string.Equals(reader?.Value?.ToString(), "True", StringComparison.OrdinalIgnoreCase);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(((bool)value) ? 1 : 0);
        }
    }
}
