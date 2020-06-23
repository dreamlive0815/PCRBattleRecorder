﻿using System;
using System.Collections;
using System.Collections.Generic;
using DataContainerType = System.Collections.Generic.Dictionary<string, object>;
using ItemType = System.Collections.Generic.KeyValuePair<string, object>;

namespace PCRBattleRecorder.Config
{
    public abstract class Config : IEnumerable<ItemType>
    {

        protected DataContainerType container = new DataContainerType();

        public virtual object Get(string key)
        {
            if (!container.ContainsKey(key)) return string.Empty;
            return container[key];
        }

        public string GetString(string key)
        {
            var r = Get(key);
            return r?.ToString() ?? "";
        }

        public bool GetBool(string key)
        {
            var r = GetString(key);
            r = r.ToLower();
            if (r == "true" || r == "1") return true;
            return false;
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