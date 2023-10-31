using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using ThunderRoad;
using ThunderRoad.AI;
using System.Collections;

namespace ScenarioEditor.Scene
{
    public abstract class SESceneBooleanFunc : SESceneCondition
    {
        public SESceneBooleanFunc()
        {
            id = "BOOLEAN";
        }

        public virtual void SetNodeA(SESceneNode node)
        {
            nodeA = node;
            nodeA.Reparent(this);
        }

        public virtual void SetNodeB(SESceneNode node)
        {
            nodeB = node;
            nodeB.Reparent(this);
        }

        public override IEnumerable<SESceneNode> GetChildren()
        {
            var nodes = new List<SESceneNode>();
            if (nodeA != null) nodes.Add(nodeA);
            if (nodeB != null) nodes.Add(nodeB);
            return nodes;
        }

        protected override NodeState TryStart()
        {
            if (nodeA == null || nodeB == null) return NodeState.FAILURE;
            return Continue();
        }

        protected abstract override NodeState Continue();

        public SESceneNode nodeA;
        public SESceneNode nodeB;
    }
}
