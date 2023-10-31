using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using ThunderRoad;

using Newtonsoft.Json;

using ScenarioEditor.Scene;
using System.Collections;

using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;

namespace ScenarioEditor.Data
{
    public class SERegistryEntryFilter : ISERegistryEntry
    {
        public string Id { get => id; }

        [JsonMergeKey]
        public string id;
        public Type filterType;

        [JsonIgnore]
        protected bool _loaded = false;
        [JsonIgnore]
        public bool Loaded { get => _loaded; }

        public virtual bool Validate(out string errWhat)
        {
            bool valid = false;
            errWhat = null;

            if (filterType == null)
            {
                errWhat = "Missing type for filter";
            }
            else if (filterType != null && !typeof(Filter.IFilter).IsAssignableFrom(filterType))
            {
                errWhat = "Filter type doesn't satisfy IFilter interface";
            }
            else if (filterType != null && !Utils.IsDefaultConstructible(filterType))
            {
                errWhat = "Filter type isn't default constructible";
            }
            else
            {
                valid = true;
            }

            return valid;
        }

        public virtual void Init()
        {
            _loaded = true;
        }

        public virtual void Refresh()
        {
        }

        public virtual IEnumerator RefreshCoroutine()
        {
            yield return new WaitForEndOfFrame();
        }

    }
}
