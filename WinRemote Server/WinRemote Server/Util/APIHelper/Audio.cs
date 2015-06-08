using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinRemote_Server.Util.APIHelper
{
    static class Audio
    {
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

        public static bool GetSystemMuted()
        {
            MMDeviceEnumerator deviceEnum = new MMDeviceEnumerator();
            MMDevice device = deviceEnum.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            if (device.State == DeviceState.Active)
            {
                return device.AudioEndpointVolume.Mute;
            }
            else
            {
                Logger.Log("NAudio", "Getting the muted state was not successful, device state: " + device.State);
                return false;
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
