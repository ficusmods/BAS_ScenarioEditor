using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using ThunderRoad;
using ThunderRoad.AI;
using System.Collections;

using Newtonsoft.Json;

namespace ScenarioEditor.Scene.Action
{
    public class ActionLoadLevel : SESceneDecoratorNode
    {
        public string levelId;

        public ActionLoadLevel()
        {
            id = "ActionLoadLevel";
        }

        public override void RefreshReferences()
        {
            if (levelId != null)
            {
                level = Catalog.GetData<LevelData>(levelId);
            }
        }

        public override void Reset()
        {
            base.Reset();
            EventManager.onLevelLoad -= EventManager_onLevelLoad;
            levelLoaded = false;
        }

        protected override NodeState TryStart()
        {
            if (level == null) return NodeState.FAILURE;
            if (Utils.isDungeonLevel(level)) return NodeState.FAILURE;

            if (Level.current.data.id == level.id)
            {
                levelLoaded = true;
            }
            else
            {
                levelLoaded = false;
                EventManager.onLevelLoad -= EventManager_onLevelLoad;
                EventManager.onLevelLoad += EventManager_onLevelLoad;
                LevelManager.LoadLevel(level.id, "Sandbox");
            }

            return Continue();
        }

        protected override NodeState Continue()
        {
            return levelLoaded ? NodeState.SUCCESS : NodeState.RUNNING;
        }

        private void EventManager_onLevelLoad(LevelData levelData, EventTime eventTime)
        {
            if (eventTime == EventTime.OnEnd)
            {
                levelLoaded = true;
            }
        }

        protected LevelData level;
        protected bool levelLoaded = false;
    }
}
    