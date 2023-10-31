using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using ThunderRoad;
using System.Collections;

using Newtonsoft.Json;

namespace ScenarioEditor.Scene
{
    public abstract class SESceneNode : ISEData
    {
        public enum NodeState
        {
            INIT,
            SUCCESS,
            FAILURE,
            RUNNING
        }

        public string id { get; set; } = "Node";

        [JsonIgnore]
        public SESceneNode Parent
        {
            get;
            protected set;
        }

        [JsonIgnore]
        public SESceneRootNode Scene
        {
            get;
            protected set;
        }

        protected NodeState _state = NodeState.INIT;
        [JsonIgnore]
        public NodeState State
        {
            get => _state;
            protected set
            {
                if (_state != value)
                {
                    _state = value;
                    SEEventManager.InvokeNodeStateChanged(this);
                }
            }
        }

        public virtual NodeState Evaluate()
        {
            if (State == NodeState.RUNNING)
            {
                return State = Continue();
            }
            else
            {
                return State = TryStart();
            }
        }

        public virtual void Reset()
        {
            State = NodeState.INIT;
            foreach (var child in GetChildren())
            {
                child.Reset();
            }
        }

        public virtual void Init(SESceneNode parent)
        {
            Parent = parent;
            Scene = Parent?.GetType() == typeof(SESceneRootNode) ? (SESceneRootNode)Parent : Parent.Scene;
            foreach (var child in GetChildren())
            {
                child.Init(this);
            }
        }

        public virtual string GetFullPath()
        {
            if (Parent == null) return id;
            return Parent.GetFullPath() + "/" + id;
        }
        public virtual void Reparent(SESceneNode parent, bool refreshRef = false)
        {
            if (parent == null) throw new DevError.InvalidNode("Non-scene nodes can not be root.");
            Parent = parent;
            Scene = Parent?.GetType() == typeof(SESceneRootNode) ? (SESceneRootNode)Parent : Parent.Scene;
            if (refreshRef) RefreshReferences();
            foreach (var child in GetChildren())
            {
                child.Reparent(this, refreshRef);
            }
        }

        public virtual void OnVisualization()
        {
            foreach (var child in GetChildren())
            {
                child.OnVisualization();
            }
        }

        public virtual IEnumerable<SESceneNode> GetChildren() => EmptyChildren;
        public virtual SESceneNode RemoveChild(string id = "") { return null; }

        public virtual void RefreshReferences()
        {
            foreach(var child in GetChildren())
            {
                child.RefreshReferences();
            }
        }

        public virtual IEnumerator RefreshReferencesCoroutine() { yield return BatchRunChildCoroutines(); }
        public virtual IEnumerator BatchRunChildCoroutines()
        {
            int idx = 0;
            var batches = GetChildren().GroupBy((x) => { return idx++ / Config.NodeRefreshConcurrentCount; });
            foreach (var batch in batches)
            {
                foreach (var coroutine in batch.Select((x) => x.RefreshReferencesCoroutine()).ToList())
                {
                    yield return coroutine;
                }
            }
        }

        public virtual SESceneNode Clone()
        {
            return MemberwiseClone() as SESceneNode;
        }

        protected virtual NodeState TryStart() => State = Continue();
        protected virtual NodeState Continue() => State = NodeState.SUCCESS;

        protected static List<SESceneNode> EmptyChildren = new List<SESceneNode>();
    }
}
