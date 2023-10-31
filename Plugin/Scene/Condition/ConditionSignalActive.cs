using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using ThunderRoad;
using ThunderRoad.AI;
using System.Collections;

namespace ScenarioEditor.Scene.Condition
{
    public class ConditionSignalActive : SESceneCondition
    {
        public ConditionSignalActive()
        {
            id = "ConditionSignalActive";
        }
        protected override NodeState TryStart()
        {
            if (bbSignalId == null) return NodeState.FAILURE;
            return Continue();
        }

        protected override NodeState Continue()
        {
            return Scene.Blackboard.SignalActive(bbSignalId) ? NodeState.SUCCESS : NodeState.FAILURE;
        }

        public string bbSignalId;
    }
}
