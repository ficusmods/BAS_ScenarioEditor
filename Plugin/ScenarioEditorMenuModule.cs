using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ThunderRoad;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


using ScenarioEditor.ExtensionMethods;

namespace ScenarioEditor
{
    public class ScenarioEditorMenuModule : MenuModule
    {
        public static UIMenu menuInstance;

        class MenuFollowStatus
        {
            public enum Status
            {
                Hidden,
                Attached,
                Detached
            }

            public void Next()
            {
                var values = typeof(Status).GetEnumValues();
                idx++;
                if(idx >= values.Length)
                {
                    idx = 0;
                }
                current = (Status)idx;
            }

            public Status current = Status.Hidden;
            public float distance = 0.8f;
            int idx = 0;
        }

        public override void Init(MenuData menuData, UIMenu menu)
        {
            base.Init(menuData, menu);

            scrollContent = menu.GetCustomReference("ContentArea").gameObject;
            templateMenuButton = menu.GetCustomReference("ButtonTemplate").gameObject;
            UpdateVersionInfo(menu.GetCustomReference("pageRight").gameObject);

            ScenarioManager.ReloadScenarios();
            EventManager_onGameLoad();

            EventManager.onGameLoad += EventManager_onGameLoad;
            EventManager.onLevelUnload += EventManager_onLevelUnload;
        }

        private void EventManager_onLevelUnload(LevelData levelData, EventTime eventTime)
        {
            if(eventTime == EventTime.OnStart)
            {
                foreach(var menu in menuStatus.Keys)
                {
                    menu.Detach();
                    menu.AttachTo(null);
                    menu.HideMenu();
                    menuStatus[menu].current = MenuFollowStatus.Status.Hidden;
                    SceneManager.MoveGameObjectToScene(menu.GetGameObject(), GameManager.local.gameObject.scene);
                }
                menuInstance.gameObject.transform.SetParent(null, false);
                menuInstance.gameObject.SetActive(false);
                SceneManager.MoveGameObjectToScene(menuInstance.gameObject, GameManager.local.gameObject.scene);
            }
        }

        private void EventManager_onGameLoad()
        {
            // Refresh isn't called for merged catalog entries so we need to call it manually.
            Data.SEBookMenuRegistry menuRegistry = Catalog.GetData<Data.SEBookMenuRegistry>("BookMenuRegistry");
            menuRegistry.onRefresh += 
                (time) => {
                if (time == EventTime.OnEnd)
                {
                    GameManager.local.StartCoroutine(InitMenusCoroutine());
                }
            };
            menuRegistry.OnCatalogRefresh();

            Data.SESceneNodeRegistry nodeRegistry = Catalog.GetData<Data.SESceneNodeRegistry>("SceneNodeRegistry");
            nodeRegistry.onRefresh +=
                (time) => {
                if (time == EventTime.OnEnd)
                {
                    NodeEditorManager.Registry = Catalog.GetData<Data.SESceneNodeRegistry>("SceneNodeRegistry");
                }
            };
            nodeRegistry.OnCatalogRefresh();
        }

        private void MenuCycleState(IBookMenu menu)
        {
            menuStatus[menu].Next();
            if(menuStatus[menu].current == MenuFollowStatus.Status.Hidden)
            {
                menu.HideMenu();
            }
            else if (menuStatus[menu].current == MenuFollowStatus.Status.Attached)
            {
                menu.ShowMenu();
                Transform headTransform = Player.local.head.transform;
                menu.MoveTo(
                    headTransform.position + headTransform.forward*0.8f,
                    Quaternion.LookRotation(headTransform.forward, Vector3.up)
                );
                menu.AttachTo(headTransform);
            }
            else
            {
                menu.Detach();
            }
        }
        private IEnumerator CreateBookMenu(Data.SERegistryEntryBookMenu menuEntry, Action<IBookMenu> callback)
        {
            GameObject instance = GameObject.Instantiate(menuEntry.loadedPrefab);
            IBookMenu created = (IBookMenu)Activator.CreateInstance(menuEntry.menuType);
            yield return created.InitCoroutine(instance);
            GameObject.DontDestroyOnLoad(created.GetGameObject());
            callback(created);
        }

        private IEnumerator InitMenusCoroutine()
        {
            Data.SEBookMenuRegistry menuRegistry = Catalog.GetData<Data.SEBookMenuRegistry>("BookMenuRegistry");

            scrollContent.DestroyChildren();

            menuStatus.Clear();
            foreach (Data.ISERegistryEntry entry in menuRegistry.Entries.Values)
            {
                Data.SERegistryEntryBookMenu menuEntry = (Data.SERegistryEntryBookMenu)entry;
                UnityEngine.UI.Button button = GameObject.Instantiate(templateMenuButton, scrollContent.transform).GetComponent<UnityEngine.UI.Button>();
                yield return CreateBookMenu(menuEntry, (menu) =>
                {
                    menuStatus.Add(menu, new MenuFollowStatus { distance = menuEntry.distance }) ;
                    button.GetComponentInChildren<Text>().text = menuEntry.id;
                    button.onClick.AddListener(() =>
                    {
                        MenuCycleState(menu);
                    });
                    menu.SetSize(menuEntry.size, menuEntry.scale);
                    menu.HideMenu();
                });
                yield return new WaitForEndOfFrame();
            }
        }

        private void UpdateVersionInfo(GameObject page)
        {
            Text version = page.GetChild("VersionInfo/Version").GetComponent<Text>();
            version.text = Logger.ModVersion;
        }

        GameObject scrollContent;
        GameObject templateMenuButton;

        Dictionary<IBookMenu, MenuFollowStatus> menuStatus = new Dictionary<IBookMenu, MenuFollowStatus>();
    }
}
