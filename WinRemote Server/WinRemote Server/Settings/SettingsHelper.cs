using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinRemote_Server.Settings
{
    class SettingsHelper
    {
        private const string FILE_NAME = "settings.ini";

        public static void Init()
        {
            LoadedSettings.initialized = true;
            if (!File.Exists(FILE_NAME)) 
            {
                File.Create(FILE_NAME).Close();
                Logger.Log("Settings", "Created new settings file with the name: " + FILE_NAME);
            }
            else
            {
                ReadSettings();
            }
        }

        public static void ReadSettings()
        {
            string[] fileLines = File.ReadAllLines(FILE_NAME, Encoding.UTF8);
            foreach (string line in fileLines)
            {
                string[] splitResult = line.Split(new char[] { '=', ' ' });
                if (splitResult.Length != 2)
                {
                    Logger.Log("Settings", "Read invalid settings entry \"" + line + "\". Restored defaults.");
                    continue;
                }
                else
                {
                    LoadedSettings.WriteValue(splitResult[0], splitResult[1]);
                }
            }
        }

        public static void SaveSettings()
        {
            Logger.Log("Saving", "Start saving all files ...");
            long startTime = DateTime.Now.Ticks;
            string[] lines = LoadedSettings.GetSaveFileLines();
            File.WriteAllLines(FILE_NAME, lines, Encoding.UTF8);
            double msTaken = (double)(DateTime.Now.Ticks - startTime) / 10000d;
            Logger.Log("Saving", "Saving took " + msTaken + " ms.");
        }

    }
}
