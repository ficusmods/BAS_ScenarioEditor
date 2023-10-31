using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using ThunderRoad;
using ThunderRoad.AI;

using Newtonsoft.Json;

namespace ScenarioEditor.Scene.Action
{
    public class ActionSetHealth : SESceneActionNode
    {
        public int health;
        public string bbCreatureId;

        public ActionSetHealth()
        {
            id = "SetHealth";
        }

        public override void RefreshReferences()
        {
            
        }

        protected override NodeState TryStart()
        {
            return Continue();
        }

        protected override NodeState Continue()
        {

            if (Scene.Blackboard.Find<GameObject>(bbCreatureId, out GameObject obj))
            {
                Creature creature = obj.GetComponentInChildren<Creature>();
                if (creature == null) return NodeState.FAILURE;
                if (creature.isPlayer) return NodeState.FAILURE;

                creature.maxHealth = health;
                creature.currentHealth = health;

                return NodeState.SUCCESS;
            }

            return NodeState.RUNNING;
        }

    }
}
