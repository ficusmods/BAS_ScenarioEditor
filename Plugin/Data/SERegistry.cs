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
    public interface ISERegistryEntry
    {
        string Id { get; }
        bool Loaded { get; }
        bool Validate(out string errWhat);
        void Init();
        void Refresh();
        IEnumerator RefreshCoroutine();
    }

    public abstract class SERegistry<T> : CustomData
        where T: ISERegistryEntry
    {
        public delegate void OnRefresh(EventTime time);
        public event OnRefresh onRefresh;

        public List<T> entries = new List<T>();

        public abstract void OnEntriesLoaded();

        private void Refresh()
        {
            validEntries.Clear();
            loadedEntries.Clear();
            onRefresh?.Invoke(EventTime.OnStart);
            foreach (var entry in entries)
            {
                if (entry.Validate(out string errWhat))
                {
                    entry.Init();
                    validEntries.Add(entry);
                }
                else
                {
                    Logger.Basic("Invalid registry entry {0} dropped from registry {1}. {2}", entry.Id, id, errWhat);
                }
            }

            foreach (ISERegistryEntry entry in validEntries)
            {
                entry.Refresh();
            }
            GameManager.local.StartCoroutine(RefreshCoroutine());
        }

        private IEnumerator RefreshCoroutine()
        {
            yield return BatchRunChildCoroutines();
            loadedEntries = validEntries.Where((x) => x.Loaded).ToList();
            OnEntriesLoaded();
            onRefresh?.Invoke(EventTime.OnEnd);
        }

        private IEnumerator BatchRunChildCoroutines()
        {
            int idx = 0;
            var batches = validEntries.GroupBy((x) => { return idx++ / Config.RegistryRefreshBatchCount; });
            foreach (var batch in batches)
            {
                foreach(var coroutine in batch.Select((x) => x.RefreshCoroutine()).ToList())
                {
                    yield return coroutine;
                }
            }
        }

        public sealed override void OnCatalogRefresh()
        {
            Refresh();
        }

        public sealed override IEnumerator OnCatalogRefreshCoroutine()
        {
            /* Not used because we have to validate and load the entries
             *  and the order of calls to OnCatalogRefresh
             *  and OnCatalogRefreshCoroutine isn't guaranteed
             */
            yield return new WaitForEndOfFrame();
        }

        protected List<T> validEntries = new List<T>();
        protected List<T> loadedEntries = new List<T>();
    }
}
