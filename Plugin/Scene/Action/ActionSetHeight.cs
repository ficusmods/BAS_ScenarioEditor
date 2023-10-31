using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using ThunderRoad;
using ThunderRoad.AI;

using Newtonsoft.Json;

namespace ScenarioEditor.Scene.Action
{
    public class ActionSetHeight : SESceneActionNode
    {
        public float height;
        public string bbCreatureId;

        public ActionSetHeight()
        {
            id = "SetHeight";
        }

        public override void RefreshReferences()
        {
            
        }

        protected override NodeState TryStart()
        {
            return Continue();
        }

        protected override NodeState Continue()
        {

            if (Scene.Blackboard.Find<GameObject>(bbCreatureId, out GameObject obj))
            {
                Creature creature = obj.GetComponentInChildren<Creature>();
                if (creature == null) return NodeState.FAILURE;
                if (creature.isPlayer) return NodeState.FAILURE;

                creature.SetHeight(height);

                return NodeState.SUCCESS;
            }

            return NodeState.RUNNING;
        }

    }
}
