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
    public class SESceneNodeRegistry : SERegistry<SERegistryEntrySceneNode>
    {
        [JsonIgnore]
        public List<string> RegisteredNodeTypes { get; protected set; } = new List<string>();

        [JsonIgnore]
        public Dictionary<string, Data.SERegistryEntrySceneNode> EntryByTypeName { get; protected set; } = new Dictionary<string, Data.SERegistryEntrySceneNode>();

        [JsonIgnore]
        public Dictionary<Type, Data.SERegistryEntrySceneNode> EntryByType { get; protected set; } = new Dictionary<Type, Data.SERegistryEntrySceneNode>();

        public override void OnEntriesLoaded()
        {
            EntryByTypeName = loadedEntries.ToDictionary((x) => x.id, (x) => x);
            EntryByType = loadedEntries.ToDictionary((x) => x.nodeType, (x) => x);
            RegisteredNodeTypes = EntryByTypeName.Keys.ToList();
            RegisteredNodeTypes.Sort();
        }

    }
}
