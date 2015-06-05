using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using NAudio.CoreAudioApi;

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

        public static Dictionary<int, string> GetSoundDevices()
        {
            Process process = new Process();
            process.StartInfo.FileName = Path.Combine(new string[] { System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "EndPointController.exe" });
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            process.WaitForExit();
            string output = process.StandardOutput.ReadToEnd().Trim();
            string[] lines = output.Split(new char[] { '\n' });
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = lines[i].Trim();
            }

            Dictionary<int, string> devices = new Dictionary<int, string>();
            foreach (string deviceLine in lines)
            {
                string num = "";
                int currIndex = -1;
                char nextChar = '0';
                while (nextChar != ' ')
                {
                    num += nextChar;
                    nextChar = deviceLine[++currIndex];
                }
                devices.Add(int.Parse(num), deviceLine.Substring(currIndex + 1));
            }

            return devices;
        }

        public static void SetSoundDevice(int id)
        {
            Process process = new Process();
            process.StartInfo.FileName = Path.Combine(new string[] { System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "EndPointController.exe" });
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.Arguments = id.ToString();
            process.StartInfo.CreateNoWindow = true;
            process.Start();
        }

        public static int GetSystemMuted()
        {
            MMDeviceEnumerator deviceEnum = new MMDeviceEnumerator();
            MMDevice device = deviceEnum.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            if (device.State == DeviceState.Active)
            {
                return device.AudioEndpointVolume.Mute ? 1 : 0;
            }
            else
            {
                Logger.Log("NAudio", "Getting the muted state was not successful, device state: " + device.State);
                return 0;
            }
        }

        public static int GetSystemVolume()
        {
            MMDeviceEnumerator deviceEnum = new MMDeviceEnumerator();
            MMDevice device = deviceEnum.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            if (device.State == DeviceState.Active)
            {
                return (int)(device.AudioEndpointVolume.MasterVolumeLevelScalar * 100f);
            }
            else
            {
                Logger.Log("NAudio", "Getting the system volume was not successful, device state: " + device.State);
                return 0;
            }
        }

        public static void SetSystemMute(bool mute)
        {
            MMDeviceEnumerator deviceEnum = new MMDeviceEnumerator();
            MMDevice device = deviceEnum.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            if (device.State == DeviceState.Active)
            {
                device.AudioEndpointVolume.Mute = mute;
            }
            else
            {
                Logger.Log("NAudio", "Setting the system mute was not successful, device state: " + device.State);
            }
        }

        public static void SetSystemVolume(int volume)
        {
            MMDeviceEnumerator deviceEnum = new MMDeviceEnumerator();
            MMDevice device = deviceEnum.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            if (device.State == DeviceState.Active)
            {
                device.AudioEndpointVolume.MasterVolumeLevelScalar = volume / 100f;
            }
            else
            {
                Logger.Log("NAudio", "Setting the master volume was not successful, device state: " + device.State);
            }
        }
    }
}
