using System;
using System.Collections;
using System.Collections.Generic;
using DataContainerType = System.Collections.Generic.Dictionary<string, object>;
using ItemType = System.Collections.Generic.KeyValuePair<string, object>;

namespace PCRBattleRecorder.Config
{
    public abstract class Config : IEnumerable<ItemType>
    {

        protected DataContainerType container = new DataContainerType();

        public virtual bool HasKey(string key)
        {
            return container.ContainsKey(key);
        }

        public virtual object Get(string key)
        {
            if (!container.ContainsKey(key)) return null;
            return container[key];
        }

        public string GetString(string key)
        {
            return GetString(key, string.Empty);
        }

        public string GetString(string key, string defaultStr)
        {
            var r = Get(key);
            return r?.ToString() ?? defaultStr;
        }

        public bool GetBool(string key)
        {
            return GetBool(key, false);
        }

        public bool GetBool(string key, bool defaultBool)
        {
            var r = Get(key);
            if (r == null) return defaultBool;
            var lower = r.ToString().ToLower();
            if (lower == "true" || lower == "1") return true;
            return false;
        }

        public double GetDouble(string key)
        {
            return GetDouble(key, 0);
        }

        public double GetDouble(string key, double defaultDouble)
        {
            var r = Get(key);
            if (r == null) return defaultDouble;
            double num;
            if (double.TryParse(r.ToString(), out num))
                return num;
            else
                return defaultDouble;
        }

        public virtual void Set(string key, object value)
        {
            container[key] = value;
        }

        public abstract void Save();

        public virtual DataContainerType AsDictionary()
        {
            return container;
        }

        public virtual IEnumerator<ItemType> GetEnumerator()
        {
            return container.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
