using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using ThunderRoad;
using ThunderRoad.AI;
using System.Collections;

namespace ScenarioEditor.Scene.Control
{
    public class ControlParallel : SESceneControlNode
    {
        public ControlParallel()
        {
            id = "Parallel";
        }

        public override void RefreshReferences()
        {
            base.RefreshReferences();
            childrenAsList = Children;
        }

        protected override NodeState TryStart()
        {
            if (childrenAsList.Count == 0) return NodeState.FAILURE;
            return Continue();
        }

        protected override NodeState Continue()
        {

            NodeState[] childStates = new NodeState[childrenAsList.Count];
            for(int i = 0; i < childrenAsList.Count; i++)
            {

                NodeState childState = childrenAsList[i].State;
                if(childState == NodeState.INIT || childState == NodeState.RUNNING)
                {
                    childState = childrenAsList[i].Evaluate();
                }
                childStates[i] = childState;
            }

            if(childStates.Any((x) => x == NodeState.RUNNING))
            {
                return NodeState.RUNNING;
            }
            else
            {
                return NodeState.SUCCESS;
            }
        }

        protected List<SESceneNode> childrenAsList;
    }
}
