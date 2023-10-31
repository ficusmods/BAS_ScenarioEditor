using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using ThunderRoad;
using ThunderRoad.AI;
using System.Collections;

namespace ScenarioEditor.Scene.Decorator
{
    public class DecoratorNot : SESceneDecoratorNode
    {
        public DecoratorNot()
        {
            id = "NOT";
        }
        protected override NodeState TryStart()
        {
            return Continue();
        }

        protected override NodeState Continue()
        {
            NodeState childState = child.Evaluate();
            if (childState == NodeState.SUCCESS)
            {
                return NodeState.FAILURE;
            }
            else if (childState == NodeState.FAILURE)
            {
                return NodeState.SUCCESS;
            }
            else
            {
                return childState;
            }
        }
    }
}
