using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using ThunderRoad;
using ThunderRoad.AI;
using System.Collections;

namespace ScenarioEditor.Scene.Condition
{
    public class ConditionTriggerActive : SESceneCondition
    {
        public string triggerId;
        public string bbCount;
        public bool inverted;
        public HashSet<string> filterNames = new HashSet<string>();

        public ConditionTriggerActive()
        {
            id = "ConditionTriggerActive";
        }

        public override void Reset()
        {
            base.Reset();
            count = 0;
            triggerActive = false;
        }

        public override void RefreshReferences()
        {
            if (triggerId != null && Scene.Scenario.Triggers.Find(triggerId, out trigger))
            {
                trigger.onTriggerEnter -= Trigger_onTriggerEnter;
                trigger.onTriggerEnter += Trigger_onTriggerEnter;
                trigger.onTriggerExit -= Trigger_onTriggerExit;
                trigger.onTriggerExit += Trigger_onTriggerExit;
            }

            filters.Clear();
            foreach(var filterName in filterNames)
            {
                var registry = Catalog.GetData<Data.SEFilterRegistry>("FilterRegistry");
                if(registry.FilterByName.TryGetValue(filterName, out Data.SERegistryEntryFilter filterEntry))
                {
                    filters.Add((Filter.IFilter)Activator.CreateInstance(filterEntry.filterType));
                }
            }
        }

        private void Trigger_onTriggerExit(Collider other)
        {
            bool filtered = false;
            if(inverted)
            {
                if (filters.Any((f) => !f.Check(other.attachedRigidbody.gameObject)))
                {
                    filtered = true;
                }
            }
            else
            {
                if (filters.Any((f) => f.Check(other.attachedRigidbody.gameObject)))
                {
                    filtered = true;
                }
            }

            if(!filtered)
            {
                count = count == 0 ? 0 : count-1;
                if (count == 0)
                {
                    triggerActive = false;
                }
            }
        }

        private void Trigger_onTriggerEnter(Collider other)
        {
            bool filtered = false;
            if (inverted)
            {
                if (filters.Any((f) => !f.Check(other.attachedRigidbody.gameObject)))
                {
                    filtered = true;
                }
            }
            else
            {
                if (filters.Any((f) => f.Check(other.attachedRigidbody.gameObject)))
                {
                    filtered = true;
                }
            }

            if (!filtered)
            {
                count += 1;
                triggerActive = true;
            }
        }

        protected override NodeState TryStart()
        {
            if (trigger == null) return NodeState.FAILURE;
            return Continue();
        }

        protected override NodeState Continue()
        {
            if(!String.IsNullOrEmpty(bbCount))
            {
                Scene.Blackboard.UpdateVariable(bbCount, count);
            }
            return triggerActive ? NodeState.SUCCESS : NodeState.FAILURE;
        }

        protected Data.SEDataTrigger trigger;
        protected List<Filter.IFilter> filters = new List<Filter.IFilter>();
        protected int count;
        protected bool triggerActive = false;
    }
}
