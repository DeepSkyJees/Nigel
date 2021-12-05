using Microsoft.Extensions.Configuration;
using Nigel.Basic.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nigel.Basic
{
    public static class ConfigExtensions
    {
        public static Task<string> GetEnvConfigValue(this IConfiguration config, string name)
        {
            string envValue = Environment.GetEnvironmentVariable(name);
            if (envValue.IsNoneValue())
            {
                envValue = config.GetSection(name).Value;
                if (envValue.IsNoneValue())
                {
                    throw new ConfigExceprion("配置异常：{0}", nameof(name));
                }
            }
            return Task.FromResult(envValue);

        }

        public static  Task<Dictionary<string,string >> GetEnvConfigListValue(this IConfiguration config, List<string> nameList)
        {
            Dictionary<string, string> configDic = new Dictionary<string, string>();
            nameList.Distinct().ToList().ForEach(async name =>
            {
                var value = await config.GetEnvConfigValue(name);
                configDic.Add(name, value);
            });
            return Task.FromResult(configDic);

        }
    }
}
