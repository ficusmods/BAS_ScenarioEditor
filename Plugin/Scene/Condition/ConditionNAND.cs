﻿using System;
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
    public class ConditionNAND : SESceneBooleanFunc
    {
        public ConditionNAND()
        {
            id = "NAND";
        }

        protected override NodeState Continue()
        {
            NodeState stateA = nodeA.Evaluate();
            NodeState stateB = nodeB.Evaluate();
            return ((stateA == NodeState.FAILURE && stateB == NodeState.FAILURE)
                || (stateA == NodeState.SUCCESS && stateB == NodeState.FAILURE)
                || (stateA == NodeState.FAILURE  && stateB == NodeState.SUCCESS)) ? NodeState.SUCCESS : NodeState.FAILURE;
        }
    }
}
