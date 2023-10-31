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

namespace ScenarioEditor.Scene.Condition
{
    public class ConditionObjectGrabbed : SESceneCondition
    {
        public string bbObjId;

        public ConditionObjectGrabbed()
        {
            id = "ConditionObjectGrabbed";
        }

        public override void RefreshReferences()
        {
        }

        protected override NodeState Continue()
        {
            if (Scene.Blackboard.Find<GameObject>(bbObjId, out GameObject obj))
            {
                Item item = obj.GetComponentInChildren<Item>();
                if(item == null) return NodeState.FAILURE;
                return (item.handlers.Count > 0) ? NodeState.SUCCESS : NodeState.FAILURE;
            }

            return NodeState.FAILURE;
        }
    }
}
