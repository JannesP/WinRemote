using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;

namespace WinRemote_Server.Util
{
    static class WindowsHelper
    {
        public enum ShutdownParameter
        {
            LOG_OFF = 0x0, SHUTDOWN = 0x1, REBOOT = 0x2, FORCED = 0x4, POWER_OFF = 0x8
        }

        /// <summary>
        /// Shuts down the computer without any question. Use with caution!
        /// </summary>
        /// <param name="param">A param for the shutdown (WindowsHelper.ShutdownParameter)</param>
        public static void Shutdown(int param)
        {
            ManagementBaseObject mboShutdown = null;
            ManagementClass mcWin32 = new ManagementClass("Win32_OperatingSystem");
            mcWin32.Get();

            // You can't shutdown without security privileges
            mcWin32.Scope.Options.EnablePrivileges = true;
            ManagementBaseObject mboShutdownParams =
                     mcWin32.GetMethodParameters("Win32Shutdown");

            // Flag 1 means we want to shut down the system. Use "2" to reboot.
            mboShutdownParams["Flags"] = param;
            mboShutdownParams["Reserved"] = "0";
            foreach (ManagementObject manObj in mcWin32.GetInstances())
            {
                mboShutdown = manObj.InvokeMethod("Win32Shutdown", mboShutdownParams, null);
            }
        }
    }
}
