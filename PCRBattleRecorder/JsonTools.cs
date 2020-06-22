using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PCRBattleRecorder
{
    class JsonTools
    {
        private static JsonTools instance;

        public static JsonTools GetInstance()
        {
            if (instance == null)
            {
                instance = new JsonTools();
            }
            return instance;
        }

        private JsonTools()
        {

        }

        public string Encode(object obj)
        {
            var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            return json;
        }

        public JToken Decode(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                json = "{}";
            }
            var jObj = JToken.Parse(json);
            return jObj;
        }

        public T Decode<T>(string json)
        {
            var obj = JsonConvert.DeserializeObject<T>(json);
            return obj;
        }
    }
}
