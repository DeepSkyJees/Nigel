using Newtonsoft.Json;
using Nigel.Basic.Jsons;
using System;
using System.Text.Json;

namespace Nigel.Basic
{
    public static class ObjectExtension
    {
        /// <summary>
        /// CamelCasePropertyNamesContractResolver
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="jsonConverters">The json converters.</param>
        /// <returns></returns>
        public static string ToJson(this object obj, JsonConverter[] jsonConverters)
        {
            var setting = new JsonSerializerSettings
            {
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
                Converters = jsonConverters ??= new JsonConverter[]
                {
                    new JsonBoolConverter(),
                    new JsonDateConverter()
                }
            };
            return JsonConvert.SerializeObject(obj, setting);
        }

        /// <summary>
        /// CamelCasePropertyNamesContractResolver
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static string ToJson(this object obj)
        {
            var setting = new JsonSerializerSettings
            {
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
               
            };
            return JsonConvert.SerializeObject(obj, setting);
        }
    }
}