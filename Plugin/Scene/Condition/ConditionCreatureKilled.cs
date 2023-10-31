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
    public class ConditionCreatureKilled : SESceneCondition
    {
        public string bbCreatureId;

        public ConditionCreatureKilled()
        {
            id = "ConditionCreatureKilled";
        }

        public override void RefreshReferences()
        {
        }

        protected override NodeState Continue()
        {
            if (Scene.Blackboard.Find<GameObject>(bbCreatureId, out GameObject obj))
            {
                Creature creature = obj.GetComponentInChildren<Creature>();
                if(creature == null) return NodeState.FAILURE;
                return (creature.isKilled) ? NodeState.SUCCESS : NodeState.FAILURE;
            }

            return NodeState.FAILURE;
        }
    }
}
