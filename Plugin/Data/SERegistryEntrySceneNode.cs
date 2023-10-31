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
    public class SERegistryEntrySceneNode : ISERegistryEntry
    {
        public string Id { get => id; }
        public string PrefabLocation { get => menuPrefabAddress; }
        public GameObject LoadedPrefab { get => loadedPrefab; set => loadedPrefab = value; }

        [JsonMergeKey]
        public string id;
        public string menuPrefabAddress;
        public Type menuType;
        public Type nodeType;
        public string description = "";

        [JsonIgnore]
        public GameObject loadedPrefab;

        [JsonIgnore]
        protected bool _loaded = false;
        [JsonIgnore]
        public bool Loaded { get => _loaded; }

        public virtual bool Validate(out string errWhat)
        {
            bool valid = false;
            errWhat = null;

            if (nodeType == null)
            {
                errWhat = "Missing type for node";
            }
            else if (menuType != null && !typeof(IMenu).IsAssignableFrom(menuType))
            {
                errWhat = "Menu type doesn't satisfy ISECustomNodeEditorMenu interface";
            }
            else if (menuType != null && !Utils.IsDefaultConstructible(menuType))
            {
                errWhat = "Menu type isn't default constructible";
            }
            else if(!nodeType.IsSubclassOf(typeof(SESceneNode)))
            {
                errWhat = "Action type isn't a Scene node type";
            }
            else if(!Utils.IsDefaultConstructible(nodeType))
            {
                errWhat = "Action type isn't default constructible";
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
            if (menuPrefabAddress != null)
            {
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
            else
            {
                _loaded = true;
            }
        }

    }
}
