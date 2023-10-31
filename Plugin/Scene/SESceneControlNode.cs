using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using ThunderRoad;
using ThunderRoad.AI;
using System.Collections;

using ScenarioEditor.ExtensionMethods;
using Newtonsoft.Json;

namespace ScenarioEditor.Scene
{
    public abstract class SESceneControlNode : SESceneNode
    {
        /* Returns a list of this node's children in order */
        [JsonIgnore]
        public virtual List<SESceneNode> Children
        {
            get
            {
                List<SESceneNode> orderedChildren = new List<SESceneNode>();
                foreach(var id in ChildOrder)
                {
                    orderedChildren.Add(ChildByName[id]);
                }
                return orderedChildren;
            }
        }

        public override SESceneNode Clone()
        {
            SESceneControlNode clone = MemberwiseClone() as SESceneControlNode;
            clone.ChildByName = new Data.SEDataDictionary<SESceneNode>(ChildByName);
            clone.ChildOrder = new LinkedList<string>(ChildOrder);
            return clone;
        }

        public virtual Scene.SESceneNode AddChild(Scene.SESceneNode child)
        {
            if (child == null) return null;
            var entry = ChildByName.AddNextFree(child.id, child);
            child.id = entry.Key;
            ChildOrder.AddLast(child.id);
            child.Reparent(this);
            return child;
        }

        public virtual Scene.SESceneNode GetChild(string name) => ChildByName[name];

        public virtual Scene.SESceneNode RemoveChild(Scene.SESceneNode child)
        {
            if (child == null) return null;
            ChildByName.Remove(child.id);
            ChildOrder.Remove(child.id);
            return child;
        }

        public virtual void ChildOrderUp(Scene.SESceneNode child)
        {
            if (child == null) return;
            var curr = ChildOrder.Find(child.id);
            if (curr == null || curr.Previous == null) return;
            ChildOrder.AddBefore(curr.Previous, curr.Value);
            ChildOrder.Remove(curr);
        }

        public virtual void ChildOrderDown(Scene.SESceneNode child)
        {
            if (child == null) return;
            var curr = ChildOrder.Find(child.id);
            if (curr == null || curr.Next == null) return;
            ChildOrder.AddAfter(curr.Next, curr.Value);
            ChildOrder.Remove(curr);
        }

        public override SESceneNode RemoveChild(string id = "")
        {
            if (ChildByName.Find(id, out SESceneNode node))
            {
                return RemoveChild(node);
            }
            return null;
        }

        public override IEnumerable<SESceneNode> GetChildren()
        {
            return Children;
        }

        public Data.SEDataDictionary<Scene.SESceneNode> ChildByName = new Data.SEDataDictionary<Scene.SESceneNode>();
        public LinkedList<string> ChildOrder = new LinkedList<string>();
    }
}
