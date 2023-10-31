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
    public class ConditionalWaitFor : SESceneConditionalNode
    {
        public ConditionalWaitFor()
        {
            id = "WaitForCondition";
        }

        public override void Reset()
        {
            base.Reset();
            conditionState = NodeState.INIT; 
        }

        protected override NodeState TryStart()
        {
            if (condition == null) return NodeState.FAILURE;
            if (child == null) return NodeState.FAILURE;
            return Continue();
        }

        protected override NodeState Continue()
        {
            if (conditionState != NodeState.SUCCESS)
            {
                conditionState = condition.Evaluate();
            }

            if (conditionState == NodeState.SUCCESS)
            {
                if (child.Evaluate() == NodeState.SUCCESS)
                {
                    return NodeState.SUCCESS;
                }
            }

            return NodeState.RUNNING;
        }

        protected NodeState conditionState = NodeState.INIT;
    }
}
