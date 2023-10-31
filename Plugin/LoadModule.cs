using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

using ThunderRoad;
using UnityEngine;
using UnityEngine.UI;
using ScenarioEditor.Menu;

namespace ScenarioEditor
{
    public class LoadModule : ThunderScript
    {
        public string mod_version = "0.1";
        public string mod_name = "ScenarioEditor";
        public string logger_level = "Detailed";

        public override void ScriptEnable()
        {
            base.ScriptEnable();
            Logger.init(mod_name, mod_version, logger_level);
            Logger.Basic("Loading {0}", mod_name);

            EventManager.onLevelLoad += EventManager_onLevelLoad;
        }

        private IEnumerator CreateSEMenu()
        {
            Logger.Basic("Creating SE main menu");
            var data = Catalog.GetData(Category.Menu, "ScenarioEditor");
            MenuData menuData = (MenuData)data;
            yield return Catalog.InstantiateCoroutine(menuData.prefabLocation, delegate (UIMenu value)
            {
                Logger.Basic("Loaded prefab for SEMenu");
                ScenarioEditorMenuModule.menuInstance = value;
            }, "ScenarioEditorMenu");

            if (ScenarioEditorMenuModule.menuInstance != null)
            {
                Logger.Basic("Initializing SE main menu");
                menuData.module?.Init(menuData, ScenarioEditorMenuModule.menuInstance);
                GameObject.DontDestroyOnLoad(ScenarioEditorMenuModule.menuInstance.gameObject);
                ScenarioEditorMenuModule.menuInstance.gameObject.SetActive(false);
            } else
            {
                Logger.Basic("Failed to load SE main menu");
            }
        }

        private void EventManager_onLevelLoad(LevelData levelData, EventTime eventTime)
        {
            if(eventTime == EventTime.OnEnd)
            {
                if (ScenarioEditorMenuModule.menuInstance == null)
                {
                    GameManager.local.StartCoroutine(CreateSEMenu());
                }

                if(Player.local?.gameObject.GetComponent<ScenarioEditorMenuFollower>() == null) {
                    Logger.Basic("Menu follower added to player");
                    Player.local.gameObject.AddComponent<ScenarioEditorMenuFollower>();
                }
            }
        }
    }
}
