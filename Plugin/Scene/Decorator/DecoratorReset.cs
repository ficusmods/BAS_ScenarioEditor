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
    public class DecoratorReset : SESceneDecoratorNode
    {
        public DecoratorReset()
        {
            id = "DecoratorReset";
        }

        protected override NodeState Continue()
        {
            NodeState childState = child.Evaluate();

            if(childState != NodeState.RUNNING)
            {
                child.Reset();
                return NodeState.INIT;
            }

            return childState;
        }
    }
}
