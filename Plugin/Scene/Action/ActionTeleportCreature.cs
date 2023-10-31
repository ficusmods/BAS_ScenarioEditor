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
    public class ActionTeleportCreature : SESceneActionNode
    {
        public string locationId;
        public string bbCreatureId;

        public ActionTeleportCreature()
        {
            id = "TeleportCreature";
        }

        public override void RefreshReferences()
        {
            if(locationId != null && Scene.Scenario.Locations.Find(locationId, out Data.SEDataLocation location))
            {
                this.location = location;
            }
        }

        protected override NodeState TryStart()
        {
            if (location == null) return NodeState.FAILURE;
            return Continue();
        }

        protected override NodeState Continue()
        {

            if (Scene.Blackboard.Find<GameObject>(bbCreatureId, out GameObject obj))
            {
                Creature creature = obj.GetComponentInChildren<Creature>();
                if (creature == null) return NodeState.FAILURE;
                if (creature.isPlayer)
                {
                    creature.player.Teleport(location.pos, Quaternion.Euler(location.rotation));
                }
                else
                {
                    creature.Teleport(location.pos, Quaternion.Euler(location.rotation));
                }
                return NodeState.SUCCESS;
            }

            return NodeState.RUNNING;
        }

        protected Data.SEDataLocation location;
    }
}
