using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using ThunderRoad;
using System.Collections;

using Newtonsoft.Json;

namespace ScenarioEditor.Data
{
    public class SEDataDictionary<TValue> : ISEData, IDictionary<string, TValue> where TValue : ISEData
    {
        /* Used when setting the default name for objects - e.g Location[0...MaxEntryCount].
         * If the maximum is reached then the last entry will be overridden
         * There can be more elements in the collection than this number as long as their ids are unique. */
        public string DefaultEntryId { get; set; } = "Entry";
        public int IdUpperLimit { get; set; } = 1000;

        public SEDataDictionary(string defaultEntryId, int idUpperLimit)
        {
            DefaultEntryId = defaultEntryId;
            IdUpperLimit = idUpperLimit;
            NextFreeId = DefaultEntryId;
        }

        public SEDataDictionary()
        {
            NextFreeId = DefaultEntryId;
        }

        public SEDataDictionary(IDictionary<string, TValue> dictionary, string defaultEntryId="Entry")
        {
            _map = new SortedDictionary<string, TValue>(dictionary);
            DefaultEntryId = defaultEntryId;
            NextFreeId = DefaultEntryId + Math.Min(IdUpperLimit, _map.Count);
        }

        public int Count => _map.Count();

        public bool IsReadOnly => false;

        public ICollection<string> Keys => _map.Keys;

        public ICollection<TValue> Values => _map.Values;

        public TValue this[string key] { get => _map[key]; set => _map[key] = value; }

        public bool Find(string id, out TValue data) => _map.TryGetValue(id, out data);

        public virtual KeyValuePair<string,TValue> Add(TValue item)
        {
            string id = FindNextFreeId(DefaultEntryId);
            _map[id] = item;
            return new KeyValuePair<string, TValue>(id, item);
        }

        public virtual void Add(string id, TValue item)
        {
            AddNextFree(id, item);
        }

        public virtual KeyValuePair<string, TValue> AddNextFree(string id, TValue item)
        {
            string newId = FindNextFreeId(id);
            _map.Add(newId, item);
            return new KeyValuePair<string, TValue>(newId, item);
        }

        public virtual string FindNextFreeId(string id)
        {
            int idx = 0;
            string found = id;
            while(_map.ContainsKey(found) && idx < IdUpperLimit)
            {
                found = id + idx;
                idx++;
            }
            return found;
        }

        public virtual bool Rename(string id, string newId)
        {
            if(ContainsKey(id) && !ContainsKey(newId))
            {
                var entry = _map[id];
                _map.Remove(id);
                _map.Add(newId, entry);
                return true;
            }
            return false;
        }

        public virtual void RefreshReferences()
        {
            foreach (TValue child in _map.Values)
            {
                child.RefreshReferences();
            }
        }

        public virtual IEnumerator RefreshReferencesCoroutine()
        {
            foreach (TValue child in _map.Values)
            {
                yield return child.RefreshReferencesCoroutine();
            }
        }

        public void Clear() => _map.Clear();

        public bool ContainsKey(string key)
        {
            return _map.ContainsKey(key);
        }

        public bool Remove(string key)
        {
            return _map.Remove(key);
        }

        public bool TryGetValue(string key, out TValue value)
        {
            return _map.TryGetValue(key, out value);
        }

        public void Add(KeyValuePair<string, TValue> item)
        {
            ((ICollection<KeyValuePair<string, TValue>>)_map).Add(item);
        }

        public bool Contains(KeyValuePair<string, TValue> item)
        {
            return ((ICollection<KeyValuePair<string, TValue>>)_map).Contains(item);
        }

        public void CopyTo(KeyValuePair<string, TValue>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<string, TValue>>)_map).CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<string, TValue> item)
        {
            return ((ICollection<KeyValuePair<string, TValue>>)_map).Remove(item);
        }

        IEnumerator<KeyValuePair<string, TValue>> IEnumerable<KeyValuePair<string, TValue>>.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<string, TValue>>)_map).GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)_map).GetEnumerator();
        }

        protected SortedDictionary<string, TValue> _map = new SortedDictionary<string, TValue>();
        protected string NextFreeId { get; set; }
    }
}
