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
    public class ConditionTimer : SESceneCondition
    {
        public float seconds = 1.0f;

        public ConditionTimer()
        {
            id = "ConditionTimer";
        }

        public override void Reset()
        {
            base.Reset();
            lastTime = 0.0f;
        }

        protected override NodeState TryStart()
        {
            lastTime = Time.time;
            return Continue();
        }

        protected override NodeState Continue()
        {
            NodeState retState = NodeState.FAILURE;
            if(Time.time - lastTime > seconds)
            {
                retState = NodeState.SUCCESS;
            }
            lastTime = Time.time;

            return retState;
        }

        protected float lastTime = 0.0f;
    }
}
