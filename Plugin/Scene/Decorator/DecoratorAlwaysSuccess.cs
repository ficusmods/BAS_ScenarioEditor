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
    public class DecoratorAlwaysSuccess : SESceneDecoratorNode
    {
        public DecoratorAlwaysSuccess()
        {
            id = "AlwaysSuccess";
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
            return NodeState.SUCCESS;
        }
    }
}
