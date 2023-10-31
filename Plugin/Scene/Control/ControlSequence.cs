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
    public class ControlSequence : SESceneControlNode
    {
        public ControlSequence()
        {
            id = "Sequence";
        }

        public override void Reset()
        {
            base.Reset();
            currentIdx = 0;
        }

        public override void RefreshReferences()
        {
            base.RefreshReferences();
            childrenAsList = Children;
        }

        protected override NodeState TryStart()
        {
            currentIdx = 0;
            if (currentIdx >= childrenAsList.Count) return NodeState.FAILURE;
            return Continue();
        }

        protected override NodeState Continue()
        {
            if (currentIdx >= childrenAsList.Count) return NodeState.SUCCESS;

            NodeState childState = childrenAsList[currentIdx].Evaluate();
            while (childState == NodeState.SUCCESS && ++currentIdx < childrenAsList.Count)
            {
                childState = childrenAsList[currentIdx].Evaluate();
            }

            return childState;
        }

        protected int currentIdx;
        protected List<SESceneNode> childrenAsList;
    }
}
