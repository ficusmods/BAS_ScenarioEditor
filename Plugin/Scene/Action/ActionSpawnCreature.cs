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
    public class ActionSpawnCreature : SESceneActionNode
    {
        public string locationId;
        public string creatureId;
        public CreatureTable.Drop.Reference creatureType;
        public string bbSpawnedId;
        public bool despawnOnReset = true;
        public ActionSetFaction.FactionType faction = ActionSetFaction.FactionType.Enemy1;

        public ActionSpawnCreature()
        {
            id = "SpawnCreature";
        }

        public override void Reset()
        {
            base.Reset();
            spawning = false;
            if(despawnOnReset && spawnedCreature != null)
            {
                spawnedCreature.Despawn();
                spawnedCreature = null;
            }
        }

        public override void RefreshReferences()
        {
            if (locationId != null && Scene.Scenario.Locations.Find(locationId, out Data.SEDataLocation loc))
            {
                location = loc;
            }
            RefreshCreature();
        }

        void RefreshCreature()
        {
            if (creatureId != null)
            {
                if (creatureType == CreatureTable.Drop.Reference.Table)
                {
                    creatureTable = Catalog.GetData<CreatureTable>(creatureId);
                }
                else if (creatureType == CreatureTable.Drop.Reference.Creature)
                {
                    creature = Catalog.GetData<CreatureData>(creatureId);
                }
            }
        }

        protected override NodeState TryStart()
        {
            if (location == null) return NodeState.FAILURE;

            CreatureData data = null;
            creatureTable?.TryPick(out data);
            if (data == null)
            {
                data = creature;
            }
            if (data == null) return NodeState.FAILURE;
            if (spawning) return NodeState.FAILURE;
            
            spawning = true;
            data.SpawnAsync(location.pos, location.rotation.y,
                callback: (creature) =>
                {
                    spawning = false;
                    Scene.Blackboard.UpdateVariable(bbSpawnedId, creature.gameObject);
                    spawnedCreature = creature;
                    creature.SetFaction((int)faction);
                });
            return NodeState.RUNNING;
        }

        protected override NodeState Continue()
        {
            if (spawning)
            {
                return NodeState.RUNNING;
            }
            else
            {
                return NodeState.SUCCESS;
            }
        }

        protected CreatureTable creatureTable;
        protected CreatureData creature;
        protected Data.SEDataLocation location;
        protected bool spawning = false;
        protected Creature spawnedCreature;
    }
}
