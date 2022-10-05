using Newtonsoft.Json;

namespace Nigel.Basic
{
    public static class ObjectExtension
    {
        /// <summary>
        /// CamelCasePropertyNamesContractResolver
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static string ToJson(this object obj)
        {
            var setting = new JsonSerializerSettings
            {
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
            };
            return JsonConvert.SerializeObject(obj, setting);
        }
    }
}