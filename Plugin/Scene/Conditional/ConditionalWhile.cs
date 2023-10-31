using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using ThunderRoad;
using ThunderRoad.AI;
using System.Collections;

namespace ScenarioEditor.Scene.Conditional
{
    public class ConditionalWhile : SESceneConditionalNode
    {
        public ConditionalWhile()
        {
            id = "While";
        }

        protected override NodeState Continue()
        {
            NodeState conditionState = condition.Evaluate();
            if(conditionState == NodeState.SUCCESS)
            {
                child?.Evaluate();
                return NodeState.RUNNING;
            }

            return conditionState;
        }
    }
}
