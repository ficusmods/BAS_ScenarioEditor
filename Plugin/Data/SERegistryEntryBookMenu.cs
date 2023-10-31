using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using ThunderRoad;

using Newtonsoft.Json;

using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;

namespace ScenarioEditor.Data
{
    public class SERegistryEntryBookMenu : ISERegistryEntry
    {
        public string Id { get => id; }
        public string PrefabLocation { get => menuPrefabAddress; }
        public GameObject LoadedPrefab { get => loadedPrefab; set => loadedPrefab = value; }

        [JsonMergeKey]
        public string id;

        public string menuPrefabAddress;
        public Type menuType;
        public Vector2 size = new Vector2(1200.0f, 1600.0f);
        public Vector3 scale = new Vector3(0.000415f, 0.000415f, 0.000415f);
        public float distance = 0.85f;

        [NonSerialized]
        public GameObject loadedPrefab;

        [JsonIgnore]
        protected bool _loaded = false;
        [JsonIgnore]
        public bool Loaded { get => _loaded; }


        public virtual bool Validate(out string errWhat)
        {
            bool valid = false;
            errWhat = null;

            if (String.IsNullOrEmpty(menuPrefabAddress))
            {
                errWhat = "Missing prefab address for menu";
            }
            else if (menuType == null)
            {
                errWhat = "Missing type for menu";
            }
            else if(!(typeof(IBookMenu).IsAssignableFrom(menuType)))
            {
                errWhat = "Menu type doesn't satisfy ISEBookMenu interface";
            }
            else if (!Utils.IsDefaultConstructible(menuType))
            {
                errWhat = "Menu type isn't default constructible";
            }
            else
            {
                valid = true;
            }

            return valid;
        }

        public virtual void Init()
        {
            _loaded = false;
        }

        public virtual void Refresh()
        {
        }

        public virtual IEnumerator RefreshCoroutine()
        {
            _loaded = false;
            loadedPrefab = null;
            AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(menuPrefabAddress);
            yield return handle;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                Logger.Basic("Prefab for {0} loaded ({1})", id, PrefabLocation);
                loadedPrefab = handle.Result;
                _loaded = true;
            }
            else
            {
                Logger.Basic("Prefab for {0} failed to load ({1})", id, PrefabLocation);
            }
        }
    }
}
