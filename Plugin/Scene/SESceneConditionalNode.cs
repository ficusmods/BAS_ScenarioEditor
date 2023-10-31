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
    public abstract class SESceneConditionalNode : SESceneDecoratorNode
    {
        public virtual void SetCondition(SESceneNode condition)
        {
            this.condition = condition;
            this.condition.Reparent(this);
        }

        public override IEnumerable<SESceneNode> GetChildren()
        {
            var nodes = base.GetChildren();
            if(condition != null)
            {
                return nodes.Append(condition);
            }
            return nodes;
        }

        public override SESceneNode RemoveChild(string id = "")
        {
            var node = base.RemoveChild(id);
            if(node == null) // base didn't remove a child
            {
                if (condition.id == id)
                {
                    node = condition;
                    condition = null;
                }
            }
            
            return node;
        }

        protected override NodeState TryStart()
        {
            if (condition == null) return NodeState.FAILURE;
            return Continue();
        }

        public SESceneNode condition;
    }
}
