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
    public class ConditionAllCreaturesDead : SESceneCondition
    {
        public ConditionAllCreaturesDead()
        {
            id = "ALL_DEAD";
        }

        protected override NodeState Continue()
        {
            return Creature.allActive.All((c) => c.isPlayer || c.isKilled) ? NodeState.SUCCESS : NodeState.FAILURE;
        }
    }
}
