using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinRemote_Server.Settings
{
    static class Settings
    {
        public const string KEY_BINDING_ADDRESS = "bindingAddress";
        public const string KEY_USE_BINDING_ADDRESS = "useBindingAddress";
        public const string KEY_PORT = "port";


        private static Dictionary<string, string> settings = new Dictionary<string, string>();
        public static bool initialized = false;

        public static void WriteValue(string key, string value)
        {
            CheckForInit();
            if (settings.ContainsKey(key))
            {
                settings[key] = value;
            }
            else
            {
                settings.Add(key, value);
            }
        }

        internal static bool GetBool(string key)
        {
            bool res = false;
            bool succeeded = bool.TryParse(GetString(key), out res);
            if (succeeded)
            {
                return res;
            }
            else
            {
                Logger.Log("Settings", "The value " + GetString(key) + " of the key " + key + " couldn't be parsed to a bool. Using default.");
                RestoreDefault(key);
                return GetBool(key);
            }
        }

        public static string GetString(string key)
        {
            CheckForInit();
            string result = "";
            bool foundVal = settings.TryGetValue(key, out result);
            if (foundVal)
            {
                return result;
            }
            else
            {
                settings.Add(key, Defaults.GetValue(key));
                return GetString(key);
            }
        }

        public static int GetNumber(string key)
        {
            int res = -1;
            bool succeeded = int.TryParse(GetString(key), out res);
            if (succeeded)
            {
                return res;
            }
            else
            {
                Logger.Log("Settings", "The value " + GetString(key) + " of the key " + key + " couldn't be parsed to an int. Using default.");
                RestoreDefault(key);
                return GetNumber(key);
            }
        }

        public static void RestoreDefault(string key)
        {
            WriteValue(key, Defaults.GetValue(key));
        }

        public static void CheckForInit()
        {
            if (!initialized)
            {
                Logger.Log("WARNING", "Settings didn't get initialized before accessing them!");
                initialized = true;
            }
        }

        public static string[] GetSaveFileLines()
        {
            string[] result = new string[settings.Count];
            for (int i = 0; i < settings.Count; i++)
            {
                KeyValuePair<string, string> kvp = settings.ElementAt(i);
                result[i] = kvp.Key + "=" + kvp.Value;
            }
            
            return result;
        }

        public static class Defaults
        {
            private static Dictionary<string, string> defaults = new Dictionary<string, string>();

            static Defaults()
            {
                defaults.Add("port", "5555");
                defaults.Add("bindingAddress", "");
                defaults.Add("useBindingAddress", "false");
            }

            public static string GetValue(string key)
            {
                string returnValue = "";
                bool foundVal = defaults.TryGetValue(key, out returnValue);
                if (foundVal)
                {
                    return returnValue;
                }
                else
                {
                    throw new MissingFieldException("The requested key: " + key + " is missing in the default config! Try deleting settings file.");
                }
            }

        }
    }
}
