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
    public class ConditionCount : SESceneCondition
    {
        public int targetCount = 0;

        public ConditionCount()
        {
            id = "COUNT";
        }

        public override void Reset()
        {
            base.Reset();
            currentCount = 0;
        }

        protected override NodeState Continue()
        {
            NodeState state = NodeState.FAILURE;
            if(currentCount < targetCount)
            {
                state = NodeState.SUCCESS;
            }
            currentCount++;
            return state;
        }

        protected int currentCount;
    }
}
