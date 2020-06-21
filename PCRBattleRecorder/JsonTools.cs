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
            var json = JsonConvert.SerializeObject(obj);
            return json;
        }

        public object Decode(string json)
        {
            var obj = JsonConvert.DeserializeObject(json);
            return obj;
        }

        public T Decode<T>(string json)
        {
            var obj = JsonConvert.DeserializeObject<T>(json);
            return obj;
        }
    }
}
