using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nigel.Basic.Utility
{
    public class ProcessUtility
    {
        /// <summary>
        /// 获取指定名称的进程ID
        /// </summary>
        /// <param name="processName"></param>
        /// <returns></returns>
        public static int GetProcessIdByName(string processName)
        {
            // 如果传入的是带.exe的名称，可以移除扩展名
            processName = Path.GetFileNameWithoutExtension(processName);

            var processes = Process.GetProcessesByName(processName);

            if (processes.Any())
            {
                // 返回第一个找到的进程ID
                return processes.FirstOrDefault().Id;
            }

            return -1; // 未找到进程
        }


        /// <summary>
        /// 获取指定名称的进程ID集合
        /// </summary>
        /// <param name="processName"></param>
        /// <returns></returns>
        public static List<int> GetProcessIdsByName(string processName)
        {
            // 如果传入的是带.exe的名称，可以移除扩展名
            processName = Path.GetFileNameWithoutExtension(processName);

            var processes = Process.GetProcessesByName(processName);

            if (processes.Any())
            {
                return processes.Select(p => p.Id).ToList();
            }
            return null; // 未找到进程
        }
    }
}
