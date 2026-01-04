#nullable enable

using Newtonsoft.Json;
using Nigel.Basic.JsonConverters;
using MessagePack;

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

        public static string ToMsgPackJson(this object obj, MessagePackSerializerOptions options = null)
        {

            if (options == null)
            {
                options = MessagePack.Resolvers.ContractlessStandardResolver.Options;
            }
            var blob = MessagePackSerializer.Serialize(obj, options);
            return MessagePackSerializer.ConvertToJson(blob, options);
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