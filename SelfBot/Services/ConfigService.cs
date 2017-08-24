using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SelfBot.Services
{
    public static class ConfigService
    {
        private static JsonSerializer JsonSerializer = new JsonSerializer();
        private static Dictionary<string, string> _configDict = new Dictionary<string, string>();

        public static void InitializeLoader()
        {
            JsonSerializer.Converters.Add(new JavaScriptDateTimeConverter());
            JsonSerializer.NullValueHandling = NullValueHandling.Ignore;
        }

        public static bool LoadConfig()
        {
            if (!File.Exists("config.json"))
                return false;
            
            using (StreamReader sr = File.OpenText("config.json"))
            using (JsonReader reader = new JsonTextReader(sr))
            {
                _configDict = JsonSerializer.Deserialize<Dictionary<string, string>>(reader);
            }
            return true;
        }

        public static void SaveConfig(Dictionary<string, string> configDict)
        {
            using (StreamWriter sw = File.CreateText("config.json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                JsonSerializer.Serialize(writer, configDict);
            }
            Console.WriteLine("Created new config.json with entered values.");
        }

        public static string GetConfigData(string key)
        {
            string result = "";
            _configDict.TryGetValue(key, out result);
            return result;
        }

        public static Dictionary<string, string> GetConfig()
        {
            return _configDict;
        }

    }
}