using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Nigel.Basic.Utility
{
    public class ProcessUtility
    {
        /// 获取指定名称的进程ID和启动时间
        /// </summary>
        /// <param name="processName"></param>
        /// <returns></returns>
        public static List<int> GetProcessIds(string processName)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var processes = Process.GetProcessesByName(processName);

                return processes.Select(x => x.Id).ToList();
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return GetLinuxProcessIds(processName);
            }
            return new List<int>();
        }

        /// 获取指定名称的进程ID和启动时间
        /// </summary>
        /// <param name="processName"></param>
        /// <returns></returns>
        public static (int, DateTime) GetProcessIdAndStartTime(string processName)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return GetWindowsProcessIdAndStartTime(processName);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return GetLinuxProcessIdAndStartTime(processName);
            }
            return (-1, DateTime.MinValue);
        }




        /// <summary>
        /// 获取指定名称的进程ID(默认返回第一个匹配结果)
        /// </summary>
        /// <param name="processName"></param>
        /// <returns></returns>
        public static (int, DateTime) GetWindowsProcessIdAndStartTime(string processName)
        {
            var processes = Process.GetProcessesByName(processName);

            if (processes.Any())
            {
                var first = processes.FirstOrDefault();
                // 返回第一个找到的进程ID
                return (first.Id, first.StartTime);
            }

            return (-1, DateTime.Parse("1900-01-01")); // 未找到进程
        }
        /// <summary>
        /// 获取指定名称的进程ID（Linux）
        /// </summary>
        /// <param name="processName"></param>
        /// <returns></returns>
        public static int GetLinuxProcessId(string processName)
        {
            var pIds = GetLinuxProcessIds(processName);
            if (pIds.Any())
            {
                return pIds.First();
            }
            return -1;

        }


        /// <summary>
        /// 获取指定名称的进程ID（Linux）
        /// </summary>
        /// <param name="processName"></param>
        /// <returns></returns>
        public static List<int> GetLinuxProcessIds(string processName)
        {
            // 构建 pgrep 命令，查找指定进程名的 PID
            // -f 选项匹配完整的命令行，-x 匹配精确的进程名，可以根据需求调整
            string command = $"pgrep -f {processName}";

            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "/bin/bash", // 在 Linux 上使用 bash 或 sh
                Arguments = $"-c \"{command}\"",
                RedirectStandardOutput = true, // 重定向标准输出
                RedirectStandardError = true,  // 重定向标准错误 (可选)
                UseShellExecute = false,       // 不使用系统 shell 执行 (更安全、高效)
                CreateNoWindow = true          // 不创建新窗口
            };

            try
            {
                using (Process process = Process.Start(psi))
                {
                    // 读取所有输出
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit(); // 等待命令执行完毕

                    if (process.ExitCode == 0 && !string.IsNullOrWhiteSpace(output))
                    {
                        // 返回找到的第一个 PID (pgrep 可能会返回多个)
                        // 也可以用 Split('\n') 遍历所有 PID
                        Console.WriteLine(output.Trim());
                        return output.Trim().Split('\n').Select(int.Parse).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"执行命令时发生错误: {ex.Message}");
            }
            return new List<int>(); // 未找到进程

        }


        /// <summary>


        /// <summary>
        /// 获取指定名称的进程ID和启动时间（Linux）
        /// </summary>
        /// <param name="processName"></param>
        /// <returns></returns>
        public static (int, DateTime) GetLinuxProcessIdAndStartTime(string processName)
        {
            var processId = GetLinuxProcessId(processName);
            var startTime = GetProcessStartTime(processId);
            return (processId, startTime);
        }


        /// <summary>
        /// 获取进程启动时间
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        public static DateTime GetProcessStartTime(int processId)
        {
            // Linux/macOS: 使用 ps 命令的 lstart 格式
            // lstart 返回标准格式如: Mon Jan  5 10:30:45 2026
            string psCommand = $"ps -p {processId} -o lstart="; // 加等号可去掉标题行
            return GetStartTimeFromCommand(psCommand);
        }




        private static DateTime GetStartTimeFromCommand(string command)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "/bin/sh",
                    Arguments = $"-c \"{command}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process process = Process.Start(psi))
                {
                    string output = process.StandardOutput.ReadToEnd().Trim();
                    process.WaitForExit();

                    if (string.IsNullOrEmpty(output)) return DateTime.MinValue;

                    // 解析 Linux/macOS 格式: "Mon Jan  6 15:40:22 2026"
                    // ps -o lstart 输出通常是这种标准固定的字符串格式
                    string cleanOutput = output.Replace("  ", " "); // 处理日期中个位数的双空格
                    return ParsePsLstartFormat(cleanOutput);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"获取进程时间失败: {ex.Message}");
            }

            return DateTime.MinValue;
        }

        /// <summary>
        /// 解析ps lstart格式的时间字符串
        /// </summary>
        private static DateTime ParsePsLstartFormat(string lstartString)
        {
            // 格式: "ddd MMM dd HH:mm:ss yyyy"
            // 例如: "Fri May 10 14:30:00 2024"

            // 移除多余的空白字符
            lstartString = System.Text.RegularExpressions.Regex.Replace(
                lstartString, @"\s+", " ").Trim();

            // 解析月份缩写
            var monthAbbreviations = new Dictionary<string, string>
    {
        {"Jan", "01"}, {"Feb", "02"}, {"Mar", "03"}, {"Apr", "04"},
        {"May", "05"}, {"Jun", "06"}, {"Jul", "07"}, {"Aug", "08"},
        {"Sep", "09"}, {"Oct", "10"}, {"Nov", "11"}, {"Dec", "12"}
    };

            var parts = lstartString.Split(' ');
            if (parts.Length >= 5)
            {
                // parts[0]: 星期缩写 (忽略)
                string monthAbbr = parts[1];  // "May"
                string day = parts[2].PadLeft(2, '0');  // "10"
                string time = parts[3];  // "14:30:00"
                string year = parts[4];  // "2024"

                if (monthAbbreviations.TryGetValue(monthAbbr, out string monthNumber))
                {
                    string dateString = $"{year}-{monthNumber}-{day} {time}";

                    if (DateTime.TryParse(dateString, out DateTime result))
                    {
                        return result;
                    }
                }
            }

            // 如果解析失败，尝试直接解析
            if (DateTime.TryParse(lstartString, out DateTime fallbackResult))
            {
                return fallbackResult;
            }

            throw new FormatException($"无法解析ps lstart格式: {lstartString}");
        }
    }
}
