
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;

namespace PCRBattleRecorder.Config
{
    public class JsonConfig : Config
    {
        private JsonTools jsonTools = JsonTools.GetInstance();
        private string filePath;

        public static JsonConfig FromString(string json)
        {
            var obj = new JsonConfig();
            obj.LoadFromString(json);
            return obj;
        }

        public static JsonConfig FromFile(string filePath)
        {
            var obj = new JsonConfig();
            obj.LoadFromFile(filePath);
            return obj;
        }

        public static JsonConfig FromFileOrEmpty(string filePath)
        {
            var obj = new JsonConfig();
            if (File.Exists(filePath))
                obj.LoadFromFile(filePath);
            return obj;
        }

        private JsonConfig()
        {
        }

        private void LoadFromString(string json)
        {
            var jObj = jsonTools.Decode(json);
            foreach (var token in jObj)
            {
                if (!(token is JProperty)) continue;
                var jPro = token as JProperty;
                var key = jPro.Name;
                var valToken = jPro.Value;
                object val;
                if (valToken.Type == JTokenType.String)
                {
                    val = valToken.ToObject<string>();
                }
                else
                {
                    val = valToken;
                }
                Set(key, val);
            }
        }

        private void LoadFromFile(string filePath)
        {
            this.filePath = filePath;
            var json = File.ReadAllText(filePath);
            LoadFromString(json);
        }

        public void SaveAsFile(string filePath)
        {
            var dictionary = AsDictionary();
            var json = jsonTools.Encode(dictionary);
            File.WriteAllText(filePath, json);
        }

        public override void Save()
        {
            if (File.Exists(filePath))
            {
                SaveAsFile(filePath);
            }
        }
    }
}
