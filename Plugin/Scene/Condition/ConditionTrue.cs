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
    public class ConditionTrue : SESceneCondition
    {
        public ConditionTrue()
        {
            id = "TRUE";
        }
        protected override NodeState TryStart()
        {
            return Continue();
        }

        protected override NodeState Continue()
        {
            return NodeState.SUCCESS;
        }
    }
}
