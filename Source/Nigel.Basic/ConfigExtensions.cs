using Microsoft.Extensions.Configuration;
using Nigel.Basic.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nigel.Basic
{
    public static class ConfigExtensions
    {
        public static string GetEnvConfigValue(this IConfiguration config, string name)
        {
            string envValue = Environment.GetEnvironmentVariable(name);
            if (envValue.IsNoneValue())
            {
                envValue = config.GetSection(name).Value;
                if (envValue.IsNoneValue())
                {
                    throw new ConfigExceprion("配置异常：{0}配置无效，请重新处理", name);
                }
            }
            return envValue;
        }

        public static Dictionary<string, string> GetEnvConfigListValue(this IConfiguration config, List<string> nameList)
        {
            Dictionary<string, string> configDic = new Dictionary<string, string>();
            nameList.Distinct().ToList().ForEach(name =>
            {
                var value = config.GetEnvConfigValue(name);
                configDic.Add(name, value);
            });
            return configDic;
        }
    }
}