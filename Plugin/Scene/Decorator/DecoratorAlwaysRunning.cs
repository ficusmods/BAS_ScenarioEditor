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
    public class DecoratorAlwaysRunning : SESceneDecoratorNode
    {
        public DecoratorAlwaysRunning()
        {
            id = "AlwaysRunning";
        }
        protected override NodeState TryStart()
        {
            return Continue();
        }

        protected override NodeState Continue()
        {
            if (child != null)
            {
                child.Evaluate();
            }
            return NodeState.RUNNING;
        }
    }
}
