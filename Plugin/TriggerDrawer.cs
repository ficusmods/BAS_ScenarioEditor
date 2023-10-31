using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

using ThunderRoad;

using ScenarioEditor.ExtensionMethods;
using UnityEngine.SceneManagement;

namespace ScenarioEditor
{
    public class TriggerDrawer : MonoBehaviour
    {
        public delegate void DrawFinished(Vector3 begin, Vector3 end);
        public event DrawFinished onDrawFinished;

        enum ActionState
        {
            ToPresent,
            ToSet,
            Set,
            Finished
        }

        enum PointState
        {
            P1,
            P2
        }

        public void Awake()
        {
            drawerGizmo = gameObject.GetComponent<Item>();
            drawerGizmo.OnUngrabEvent += delegate { HandleUngrab(); };
            drawerGizmo.OnTelekinesisReleaseEvent += delegate { HandleUngrab(); };
            GameManager.local.StartCoroutine(LoadTriggerPrefab());
            EventManager.onLevelUnload += EventManager_onLevelUnload;
        }

        private void EventManager_onLevelUnload(LevelData levelData, EventTime eventTime)
        {
            if (eventTime == EventTime.OnStart)
            {
                if (triggerTemplate != null)
                {
                    triggerTemplate.transform.parent = null;
                    SceneManager.MoveGameObjectToScene(triggerTemplate, GameManager.local.gameObject.scene);
                }
            }
        }

        private IEnumerator LoadTriggerPrefab()
        {
            var handle = Addressables.InstantiateAsync(Config.TriggerPrefabLocation);
            yield return handle;
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                triggerTemplate = handle.Result;
                GameObject.DontDestroyOnLoad(triggerTemplate);
                triggerTemplate.SetActive(false);
            }
        }

        void HandleUngrab()
        {
            StoreResult();
            actionState = ActionState.Set;
        }

        public void StartDraw()
        {
            if (actionState == ActionState.Finished)
            {
                actionState = ActionState.ToPresent;
                pointState = PointState.P1;
            }
        }

        void PresentGizmo()
        {
            Transform headTransform = Player.local.head.transform;
            drawerGizmo.gameObject.transform.position = headTransform.position + headTransform.forward * 0.5f;
            drawerGizmo.gameObject.transform.rotation = Quaternion.LookRotation(headTransform.forward, Vector3.up);
        }

        void StoreResult()
        {
            if(pointState == PointState.P1)
            {
                currentStart = drawerGizmo.gameObject.transform.position;
            }
            else if (pointState == PointState.P2)
            {
                currentEnd = drawerGizmo.gameObject.transform.position;
            }
        }

        void OnDisable()
        {
            currentStart = Vector3.zero;
            currentEnd = Vector3.zero;
            triggerTemplate?.SetActive(false);
            actionState = ActionState.Finished;
        }

        void FixedUpdate()
        {
            if (triggerTemplate != null)
            {
                if (actionState == ActionState.ToPresent)
                {
                    PresentGizmo();
                    actionState = ActionState.ToSet;
                }
                else if (actionState == ActionState.ToSet)
                {
                    if (pointState == PointState.P2)
                    {
                        triggerTemplate.SetActive(true);
                        Vector3 diagonal = drawerGizmo.gameObject.transform.position - currentStart;
                        triggerTemplate.transform.localScale = new Vector3(Mathf.Abs(diagonal.x), Mathf.Abs(diagonal.y), Mathf.Abs(diagonal.z));
                        triggerTemplate.transform.localPosition = currentStart + diagonal / 2;
                    }
                }
                else if (actionState == ActionState.Set)
                {
                    if (pointState == PointState.P1)
                    {
                        pointState = PointState.P2;
                        actionState = ActionState.ToPresent;
                    }
                    else
                    {
                        triggerTemplate.SetActive(false);
                        onDrawFinished?.Invoke(currentStart, currentEnd);
                        actionState = ActionState.Finished;
                    }
                }
            }
        }

        Item drawerGizmo;
        ActionState actionState = ActionState.Finished;
        PointState pointState;
        GameObject triggerTemplate;
        Vector3 currentStart;
        Vector3 currentEnd;
    }
}
