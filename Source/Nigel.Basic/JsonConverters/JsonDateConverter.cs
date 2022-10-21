using System;
using Newtonsoft.Json;

namespace Nigel.Basic.JsonConverters
{
    public class JsonDateConverter: JsonConverter
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
                long timeTicks = 0L;
                if (long.TryParse(timeStr, out timeTicks))
                {
                    System.DateTime dt = TimeZoneInfo.ConvertTime(new System.DateTime(1970, 1, 1), TimeZoneInfo.Local);
                    try
                    {
                        dt = dt.AddSeconds(timeTicks / 1000);
                    }
                    catch (Exception)
                    {

                        dt = dt.AddMilliseconds(timeTicks / 1000);
                    }

                    //time /= 1000;
                    dt = Convert.ToDateTime(dt.ToString("yyyy-MM-dd HH:mm:ss")).ToUniversalTime().AddHours(8);
                    return dt;
                }
                else
                {
                    //Convert.ToDateTime(dateTime.ToString("yyyy-MM-dd HH:mm:ss")).ToUniversalTime().AddHours(8);
                    var dt = Convert.ToDateTime(Convert.ToDateTime(timeStr).ToString("yyyy-MM-dd HH:mm:ss")).ToUniversalTime().AddHours(8);
                    return dt;
                }
            }
            return null;


        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            DateTime? dt = null;
            if (value != null)
            {
                dt = Convert.ToDateTime(Convert.ToDateTime(value).ToString("yyyy-MM-dd HH:mm:ss")).ToUniversalTime().AddHours(8);
            }
            writer.WriteValue(dt);

        }
    }
}
