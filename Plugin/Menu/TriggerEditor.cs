using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;

using ThunderRoad;
using UnityEngine.SceneManagement;

namespace ScenarioEditor.Menu
{
    public class TriggerEditor : SELazyBookMenu
    {
        public TriggerEditor() { }

        public override IEnumerator InitCoroutine(SEMenu parent, GameObject menuObj)
        {
            yield return base.InitCoroutine(parent, menuObj);

            yield return LoadGizmo();

            SEEventManager.onScenarioLoaded += EventManager_onScenarioLoaded;
            EventManager.onLevelUnload += EventManager_onLevelUnload;
        }

        private void EventManager_onLevelUnload(LevelData levelData, EventTime eventTime)
        {
            if (eventTime == EventTime.OnStart)
            {
                if (drawerGizmo != null)
                {
                    drawerGizmo.transform.parent = null;
                    SceneManager.MoveGameObjectToScene(drawerGizmo.gameObject, GameManager.local.gameObject.scene);
                }
            }
        }

        protected IEnumerator LoadGizmo()
        {
            Catalog.GetData<ItemData>("SEGizmoBasic")?.SpawnAsync((item) => {
                drawerGizmo = item;
                drawerGizmo.disallowDespawn = true;
                GameObject.DontDestroyOnLoad(drawerGizmo.gameObject);
                drawer = drawerGizmo.gameObject.AddComponent<TriggerDrawer>();
                drawer.onDrawFinished += Drawer_onDrawFinished;
            });

            yield return new WaitForEndOfFrame();
        }

        protected override IEnumerator InitLazyMenuCoroutine()
        {
            AddLabel("Trigger editor").fontSize = 52;

            triggerSelector = AddSelector("Triggers");
            triggerSelector.onOptionSelected += TriggerSelector_onOptionSelected;

            StartHorizontalGroup();
            AddButton("New", () =>
            {
                if (scenario != null)
                {
                    selected = scenario.Triggers.Add(new Data.SEDataTrigger()).Key;
                    StartTriggerEdit();
                    Refresh();
                }
            });

            AddButton("Del", () =>
            {
                if (scenario != null)
                {
                    if (scenario.Triggers.Find(this.selected, out Data.SEDataTrigger activeTrigger))
                    {
                        activeTrigger.Delete();
                    }
                    scenario.Triggers.Remove(selected);
                    selected = "";
                    Refresh();
                }
            });
            EndHorizontalGroup();

            StartHorizontalGroup();
            AddButton("Redraw", () =>
            {
                StartTriggerEdit();
            });
            EndHorizontalGroup();

            yield return new WaitForEndOfFrame();
        }

        private void EventManager_onScenarioLoaded(Data.SEDataScenario scenario)
        {
            selected = Utils.OrEmptyString(scenario.Locations.Keys.FirstOrDefault());
            this.scenario = scenario;
        }

        private void TriggerSelector_onOptionSelected(string selected)
        {
            if (scenario.Triggers.Find(this.selected, out Data.SEDataTrigger activeTrigger))
            {
                activeTrigger.Hide();
            }

            this.selected = selected;
            if(scenario.Triggers.Find(selected, out Data.SEDataTrigger currentTrigger))
            {
                currentTrigger.Show();
            }
        }

        protected void StartTriggerEdit()
        {
            if (drawerGizmo != null)
            {
                drawerGizmo.gameObject.SetActive(true);
                drawer.StartDraw();
            }
        }

        private void Drawer_onDrawFinished(Vector3 begin, Vector3 end)
        {
            drawerGizmo.gameObject.SetActive(false);
            if (scenario.Triggers.Find(this.selected, out Data.SEDataTrigger trigger))
            {
                trigger.start = begin;
                trigger.end = end;
                trigger.SetTrigger(begin, end);
                trigger.Show();
            }
        }

        public override void Refresh()
        {
            if (scenario != null)
            {
                triggerSelector.RefreshContent(scenario.Triggers.Keys);
            }
        }

        protected override void HideAll()
        {
            base.HideAll();
            if (scenario != null && scenario.Triggers.Find(this.selected, out Data.SEDataTrigger activeTrigger))
            {
                activeTrigger.Hide();
            }
            drawerGizmo?.gameObject.SetActive(false);
        }

        public virtual void TeleportTo(Data.SEDataTrigger trigger)
        {
            if (trigger == null) return;
            Player.local.Teleport(trigger.start, Quaternion.identity);
        }

        protected ContentSelectorElement triggerSelector;
        protected Item drawerGizmo;
        protected TriggerDrawer drawer;

        protected Data.SEDataScenario scenario;
        protected string selected = "";
    }
}
