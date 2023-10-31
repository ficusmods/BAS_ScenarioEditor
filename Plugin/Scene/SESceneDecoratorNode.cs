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
    public abstract class SESceneDecoratorNode : SESceneNode
    {
        public override IEnumerable<SESceneNode> GetChildren()
        {
            if (child != null)
            {
                return EmptyChildren.Append(child);
            }
            return EmptyChildren;
        }

        public override SESceneNode RemoveChild(string id = "")
        {
            if (child != null && child.id == id)
            {
                SESceneNode node = child;
                child = null;
                return node;
            }
            return null;
        }

        public virtual void SetChild(SESceneNode node)
        {
            child = node;
            node.Reparent(this);
        }

        protected override NodeState TryStart()
        {
            if (child == null) return NodeState.FAILURE;
            return Continue();
        }

        protected override NodeState Continue()
        {
            return child.Evaluate();
        }

        public SESceneNode child;
    }
}
