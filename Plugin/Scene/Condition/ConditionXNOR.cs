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
    public class ConditionXNOR : SESceneBooleanFunc
    {
        public ConditionXNOR()
        {
            id = "XNOR";
        }

        protected override NodeState Continue()
        {
            NodeState stateA = nodeA.Evaluate();
            NodeState stateB = nodeB.Evaluate();
            return ((stateA == NodeState.SUCCESS && stateB == NodeState.SUCCESS)
                || (stateA == NodeState.FAILURE && stateB == NodeState.FAILURE)) ? NodeState.SUCCESS : NodeState.FAILURE;
        }
    }
}
