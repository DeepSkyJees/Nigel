using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using Nigel.Basic.Utility;

public class ProcessManager
{

    public static void Main(string[] args)
    {
        // 假设你要查找名为 'nginx' 的进程的 PID
        string targetProcessName = args.Any() ? args[0] : "nginx";
        var (pid, startName) = ProcessUtility.GetProcessIdAndStartTimeByName(targetProcessName);

        if (pid != -1)
        {
            Console.WriteLine($"找到进程 '{targetProcessName}' 的 PID: {pid}, 启动时间: {startName}");
        }
        else
        {
            Console.WriteLine($"未找到进程 '{targetProcessName}' 或执行命令失败。");
        }
    }
}
