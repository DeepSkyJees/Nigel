using Microsoft.Extensions.Configuration;
using Nigel.Basic.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nigel.Basic
{
    public static class ConfigExtensions
    {
        /// <summary>
        /// 请用"_"代替AppSettings里面的":"
        /// AppSetting(Mysql:Host)
        /// 环境变量(Mysql_Host)
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        /// <exception cref="Nigel.Basic.Exceptions.ConfigException">配置异常：{0}配置无效，请重新处理</exception>
        public static string GetEnvConfigValue(this IConfiguration config, string name)
        {
            string envValue = Environment.GetEnvironmentVariable(name.Replace(":","_"));
            if (envValue.IsNoneValue())
            {
                envValue = config.GetSection(name.Replace("_",":")).Value;
                if (envValue.IsNoneValue())
                {
                    throw new ConfigException("{0}配置无效，请重新处理".Format(name));
                }
            }
            return envValue;
        }

        /// <summary>
        /// 获取链接字符串
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        /// <exception cref="Nigel.Basic.Exceptions.ConfigException">配置异常：{0}配置无效，请重新处理</exception>
        public static string GetConnectionStringValue(this IConfiguration config, string name)
        {
            string envValue = Environment.GetEnvironmentVariable(name);
            if (envValue.IsNoneValue())
            {
                envValue = config.GetConnectionString(name);
                if (envValue.IsNoneValue())
                {
                    throw new ConfigException("{0}配置无效，请重新处理".Format(name));
                }
            }
            return envValue;
        }

        /// <summary>
        /// 批量获取环境变量
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <param name="nameList">The name list.</param>
        /// <returns></returns>
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