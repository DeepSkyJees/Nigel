using System;
using Newtonsoft.Json;

namespace Nigel.Basic.JsonConverters
{
    public class JsonDateConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime?);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value != null)
            {
                string timeStr = reader.Value.ToString();
                var dt = timeStr.ToDateTime();
                return dt;
            }
            return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            DateTime? dt = null;
            if (value != null)
            {
                dt = value.ToString().ToDateTime();
                dt = Convert.ToDateTime(Convert.ToDateTime(value).ToString("yyyy-MM-dd HH:mm:ss.fff")).ToUniversalTime().AddHours(8);
            }
            writer.WriteValue(dt);
        }
    }
}