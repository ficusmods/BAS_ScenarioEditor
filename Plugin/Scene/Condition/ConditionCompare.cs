using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using ThunderRoad;
using ThunderRoad.AI;
using System.Collections;

using Newtonsoft.Json;

namespace ScenarioEditor.Scene.Condition
{
    public class ConditionCompare : SESceneCondition
    {
        public enum Operation
        {
            GreaterThan,
            LessThan,
            Equal
        }

        public string bbValue1;
        public string bbValue2;
        public int value2;
        public Operation operation;

        public ConditionCompare()
        {
            id = "ConditionCompare";
        }

        public override void RefreshReferences()
        {
        }

        protected override NodeState TryStart()
        {
            if(!String.IsNullOrEmpty(bbValue1))
            {
                return NodeState.FAILURE;
            }
            else
            {
                return Continue();
            }
        }

        protected override NodeState Continue()
        {
            if (Scene.Blackboard.Find<int>(bbValue1, out int val1))
            {
                if (!String.IsNullOrEmpty(bbValue2))
                {
                    if (Scene.Blackboard.Find<int>(bbValue2, out int val2))
                    {
                        return Compare(val1, val2) ? NodeState.SUCCESS : NodeState.FAILURE;
                    }
                    return NodeState.FAILURE;
                }
                else
                {
                    return Compare(val1, value2) ? NodeState.SUCCESS : NodeState.FAILURE;
                }
            }

            return NodeState.FAILURE;
        }

        protected bool Compare(int val1, int val2)
        {
            switch(operation)
            {
                case Operation.Equal:
                    return val1 == val2;
                case Operation.GreaterThan:
                    return val1 > val2;
                case Operation.LessThan:
                    return val1 < val2;
                default:
                    return false;
            }
        }
    }
}
