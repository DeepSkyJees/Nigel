﻿using Newtonsoft.Json;

namespace Nigel.Basic
{
    public static class ObjectExtension
    {
        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}