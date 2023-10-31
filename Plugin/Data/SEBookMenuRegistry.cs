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
    public class SEBookMenuRegistry : SERegistry<SERegistryEntryBookMenu>
    {
        [JsonIgnore]
        public Dictionary<string, Data.SERegistryEntryBookMenu> Entries { get; protected set; } = new Dictionary<string, Data.SERegistryEntryBookMenu>();

        public override void OnEntriesLoaded()
        {
            Entries = loadedEntries.ToDictionary((x) => x.Id, (x) => x);
        }
    }
}
