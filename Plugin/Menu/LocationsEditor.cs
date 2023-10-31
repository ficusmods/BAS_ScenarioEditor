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
    public class LocationsEditor : SELazyBookMenu
    {

        public LocationsEditor() { }

        public override IEnumerator InitCoroutine(SEMenu parent, GameObject menuObj)
        {
            yield return base.InitCoroutine(parent, menuObj);

            yield return LoadGizmo();

            SEEventManager.onScenarioLoaded += EventManager_onScenarioLoaded;
            EventManager.onLevelUnload += EventManager_onLevelUnload;
        }

        private void EventManager_onLevelUnload(LevelData levelData, EventTime eventTime)
        {
            if(eventTime == EventTime.OnStart) {
                if(locationGizmo != null)
                {
                    locationGizmo.transform.parent = null;
                    SceneManager.MoveGameObjectToScene(locationGizmo.gameObject, GameManager.local.gameObject.scene);
                }
            }
        }

        protected IEnumerator LoadGizmo()
        {
            Catalog.GetData<ItemData>("SEGizmoBasic")?.SpawnAsync((item) => {
                locationGizmo = item;
                locationGizmo.disallowDespawn = true;
                GameObject.DontDestroyOnLoad(locationGizmo.gameObject);
                locationGizmo.OnUngrabEvent += delegate { StoreGizmoPosition(); };
                locationGizmo.OnTelekinesisReleaseEvent += delegate { StoreGizmoPosition(); };
            });
            yield return new WaitForEndOfFrame();
        }

        protected override IEnumerator InitLazyMenuCoroutine()
        {
            AddLabel("Locations editor").fontSize = 52;

            locationSelector = AddSelector("Locations");
            locationSelector.onOptionSelected += LocationSelector_onOptionSelected;

            StartHorizontalGroup();
            AddButton("New", () =>
            {
                if (scenario != null)
                {
                    var l = scenario.Locations.Add(new Data.SEDataLocation());
                    l.Value.pos = Player.local.transform.position + Vector3.up * 0.1f;
                    l.Value.rotation = Player.local.transform.rotation.eulerAngles;
                    selected = l.Key;
                    MoveGizmo();
                    Refresh();
                }
            });

            AddButton("Del", () =>
            {
                if (scenario != null)
                {
                    scenario.Locations.Remove(selected);
                    selected = "";
                    Refresh();
                }
            });
            EndHorizontalGroup();

            StartHorizontalGroup();
            AddButton("Gizmo", () =>
            {
                locationGizmo?.gameObject.SetActive(!locationGizmo.gameObject.activeSelf);
            });
            AddButton("Teleport", () =>
            {
                if(scenario != null && !string.IsNullOrEmpty(selected))
                {
                    TeleportTo(scenario.Locations[selected]);
                }
            });
            EndHorizontalGroup();
            /* --------------------------------------------- */


            yield return new WaitForEndOfFrame();
        }

        private void EventManager_onScenarioLoaded(Data.SEDataScenario scenario)
        {
            selected = Utils.OrEmptyString(scenario.Locations.Keys.FirstOrDefault());
            this.scenario = scenario;
        }

        private void LocationSelector_onOptionSelected(string selected)
        {
            this.selected = selected;
            MoveGizmo();
            Refresh();
        }

        protected void StoreGizmoPosition()
        {
            if (locationGizmo != null && scenario.Locations.Find(selected, out Data.SEDataLocation loc))
            {
                loc.pos = locationGizmo.transform.position;
                loc.rotation = locationGizmo.transform.rotation.eulerAngles;
            }
        }

        protected void GizmoToLocation()
        {
            if (locationGizmo != null && scenario.Locations.Find(selected, out Data.SEDataLocation loc))
            {
                locationGizmo.transform.position = loc.pos;
                locationGizmo.transform.rotation = Quaternion.Euler(loc.rotation);
            }
        }

        public virtual void MoveGizmo()
        {
            if(locationGizmo != null)
            {
                locationGizmo.gameObject.SetActive(true);
                GizmoToLocation();
            }
        }

        public override void Refresh()
        {
            if (scenario != null)
            {
                locationSelector.RefreshContent(scenario.Locations.Keys);
            }
        }

        protected override void HideAll()
        {
            base.HideAll();
            locationGizmo?.gameObject.SetActive(false);
        }

        public virtual void TeleportTo(Data.SEDataLocation location)
        {
            if (location == null) return;
            Player.local.Teleport(location.pos, Quaternion.Euler(location.rotation));
        }

        protected ContentSelectorElement locationSelector;
        protected Item locationGizmo;

        protected Data.SEDataScenario scenario;
        protected string selected = "";
    }
}
