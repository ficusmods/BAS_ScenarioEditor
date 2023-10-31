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

namespace ScenarioEditor.Scene
{
    public class SESceneRootNode : SESceneConditionalNode
    {
        [JsonIgnore]
        public SESceneBlackboard Blackboard { get; protected set; }

        [JsonIgnore]
        public Data.SEDataScenario Scenario { get; set; }

        public SESceneRootNode()
        {
            id = "Scene";
        }

        public override void Init(SESceneNode parent)
        {
            Parent = parent;
            if (Parent == null)
            {
                Scene = this;
                EventManager.onLevelLoad -= EventManager_onLevelLoad;
                EventManager.onLevelLoad += EventManager_onLevelLoad;
            }
            else
            {
                Scene = Parent.GetType() == typeof(SESceneRootNode) ? (SESceneRootNode)Parent : Parent.Scene;
                Scenario = Scene.Scenario;
            }
            Blackboard = new SESceneBlackboard();
            child?.Init(this);
            EventManager.onLevelLoad -= EventManager_onLevelLoad;
            EventManager.onLevelLoad += EventManager_onLevelLoad;
        }

        public override void Reparent(SESceneNode parent, bool refreshRef = false)
        {
            Parent = parent;
            if (Parent == null)
            {
                Scene = this;
            }
            else
            {
                Scene = Parent.GetType() == typeof(SESceneRootNode) ? (SESceneRootNode)Parent : Parent.Scene;
                Scenario = Scene.Scenario;
            }
            if (refreshRef) RefreshReferences();
            child?.Reparent(this, refreshRef);
        }

        public override void Reset()
        {
            base.Reset();
            Blackboard = new SESceneBlackboard();
            AddBasicObjectsToBB();
        }

        protected override NodeState TryStart()
        {
            if (child == null) return NodeState.FAILURE;

            if(condition == null || condition.Evaluate() == NodeState.SUCCESS)
            {
                return Continue();
            }

            return NodeState.INIT;
        }

        protected override NodeState Continue()
        {
            return child.Evaluate();
        }

        private void EventManager_onLevelLoad(LevelData levelData, EventTime eventTime)
        {
            if(eventTime == EventTime.OnEnd)
            {
                AddBasicObjectsToBB();
            }
        }

        protected virtual void AddBasicObjectsToBB()
        {
            Blackboard.UpdateVariable<GameObject>("Player", Player.local.gameObject);
        }
    }
}
    