using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

using ThunderRoad;

using ScenarioEditor.ExtensionMethods;

namespace ScenarioEditor.Data
{
    public class SEDataTriggers : SEDataDictionary<SEDataTrigger>
    {
        public SEDataTriggers()
            : base("Trigger", 1000)
        {
        }
    }
    public class SEDataTrigger : SEData
    {
        public Vector3 start;
        public Vector3 end;

        public delegate void TriggerEnter(Collider other);
        public delegate void TriggerStay(Collider other);
        public delegate void TriggerEnterExit(Collider other);
        public event TriggerEnter onTriggerEnter;
        public event TriggerStay onTriggerStay;
        public event TriggerEnterExit onTriggerExit;

        public override void RefreshReferences()
        {
            RefreshTriggerLocation();
        }

        public virtual void Show()
        {
            if (triggerObj != null)
            {
                triggerRenderer.enabled = true;
            }
        }

        public virtual void Hide()
        {
            if (triggerObj != null)
            {
                triggerRenderer.enabled = false;
            }
        }

        public virtual void Delete()
        {
            if (triggerObj != null)
            {
                GameObject.Destroy(triggerObj);
                triggerRenderer = null;
                triggerWatch = null;
            }
        }

        public virtual void RefreshTriggerLocation()
        {
            if (triggerObj != null)
            {
                Vector3 diagonal = end - start;
                triggerObj.transform.localScale = new Vector3(Mathf.Abs(diagonal.x), Mathf.Abs(diagonal.y), Mathf.Abs(diagonal.z));
                triggerObj.transform.localPosition = start + diagonal / 2;
            }
        }

        public virtual void SetTrigger(GameObject triggerObj)
        {
            if (triggerObj != null)
            {
                this.triggerObj = triggerObj;
                triggerObj.SetActive(true);
                triggerRenderer = triggerObj.GetComponentInChildren<MeshRenderer>();
                triggerWatch = triggerObj.GetComponentInChildren<TriggerWatch>();
                triggerWatch.onTriggerEnter += (other) => { onTriggerEnter?.Invoke(other); };
                triggerWatch.onTriggerExit += (other) => { onTriggerExit?.Invoke(other); };
                triggerWatch.onTriggerStay += (other) => { onTriggerStay?.Invoke(other); };
                GameObject.DontDestroyOnLoad(triggerObj);
                RefreshTriggerLocation();
                Hide();
            }
        }
        public virtual void SetTrigger(Vector3 start, Vector3 end)
        {
            this.start = start;
            this.end = end;
            if (triggerObj == null)
            {
                var handle = Addressables.InstantiateAsync(Config.TriggerPrefabLocation);
                handle.Completed += TriggerPrefabLoad_Completed;
            }
            else
            {
                RefreshTriggerLocation();
            }
        }

        private void TriggerPrefabLoad_Completed(AsyncOperationHandle<GameObject> obj)
        {
            if(obj.Status == AsyncOperationStatus.Succeeded)
            {
                SetTrigger(obj.Result);
            }
        }

        public override IEnumerator RefreshReferencesCoroutine()
        {
            if (triggerObj == null)
            {
                var handle = Addressables.InstantiateAsync(Config.TriggerPrefabLocation);
                yield return handle;
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    SetTrigger(handle.Result);
                }
                else
                {
                    Logger.Basic("Failed to load trigger prefab {0}", Config.TriggerPrefabLocation);
                }
            }
        }

        protected GameObject triggerObj;
        protected MeshRenderer triggerRenderer;
        protected TriggerWatch triggerWatch;
    }
}
