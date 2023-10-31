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
    public class SEFilterRegistry : SERegistry<SERegistryEntryFilter>
    {
        [JsonIgnore]
        public List<string> RegisteredFilterNames { get; protected set; } = new List<string>();

        [JsonIgnore]
        public Dictionary<string, Data.SERegistryEntryFilter> FilterByName { get; protected set; } = new Dictionary<string, Data.SERegistryEntryFilter>();

        [JsonIgnore]
        public Dictionary<Type, Data.SERegistryEntryFilter> FilterByType { get; protected set; } = new Dictionary<Type, Data.SERegistryEntryFilter>();

        public override void OnEntriesLoaded()
        {
            FilterByName = loadedEntries.ToDictionary((x) => x.id, (x) => x);
            FilterByType = loadedEntries.ToDictionary((x) => x.filterType, (x) => x);
            RegisteredFilterNames = FilterByName.Keys.ToList();
            RegisteredFilterNames.Sort();
        }

    }
}
