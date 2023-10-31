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
    public class ConditionalWhen : SESceneConditionalNode
    {
        public ConditionalWhen()
        {
            id = "When";
        }

        protected override NodeState TryStart()
        {
            if (condition == null) return NodeState.FAILURE;
            if (child == null) return NodeState.FAILURE;
            return Continue();
        }

        protected override NodeState Continue()
        {
            if (condition.Evaluate() == NodeState.SUCCESS)
            {
                child.Evaluate();
            }

            return NodeState.RUNNING;
        }
    }
}
