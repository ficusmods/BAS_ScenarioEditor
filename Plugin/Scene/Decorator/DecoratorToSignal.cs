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
    public class DecoratorToSignal : SESceneDecoratorNode
    {
        public DecoratorToSignal()
        {
            id = "ToSignal";
        }
        protected override NodeState TryStart()
        {
            if (bbSignalId == null) return NodeState.FAILURE;
            if (child == null) return NodeState.FAILURE;
            return Continue();
        }

        protected override NodeState Continue()
        {
            NodeState childState = child.Evaluate();
            Scene.Blackboard.SetSignalLevel(bbSignalId, (childState == NodeState.SUCCESS));
            return childState;
        }

        public string bbSignalId;
    }
}
