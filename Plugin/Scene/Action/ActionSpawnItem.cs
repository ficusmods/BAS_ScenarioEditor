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
    public class ActionSpawnItem : SESceneActionNode
    {
        public string locationId;
        public string itemId;
        public string bbSpawnedId;
        public bool staticObj;

        public ActionSpawnItem()
        {
            id = "SpawnItem";
        }

        public override void RefreshReferences()
        {
            if (locationId != null && Scene.Scenario.Locations.Find(locationId, out Data.SEDataLocation loc))
            {
                location = loc;
            }

            if (itemId != null)
            {
                itemData = Catalog.GetData<ItemData>(itemId);
            }
        }

        public override void Reset()
        {
            base.Reset();
            spawning = false;
            if(spawnedItem != null)
            {
                spawnedItem.Despawn();
                spawnedItem = null;
            }
        }

        protected override NodeState TryStart()
        {
            if (location == null) return NodeState.FAILURE;
            if (itemData == null) return NodeState.FAILURE;
            if (spawning) return NodeState.FAILURE;
            
            spawning = true;
            itemData.SpawnAsync(callback: (item) =>
            {
                spawning = false;
                Scene.Blackboard.UpdateVariable(bbSpawnedId, item.gameObject);
                spawnedItem = item;
                if(staticObj)
                {
                    foreach(var handle in item.handles)
                    {
                        handle.SetTelekinesis(false);
                    }
                    var rb = item.gameObject.GetComponent<Rigidbody>();
                    if(rb)
                    {
                        rb.isKinematic = true;
                    }
                    item.disallowDespawn = true;
                }
            }, location.pos, Quaternion.Euler(location.rotation));
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

        protected bool spawning = false;
        protected Data.SEDataLocation location;
        protected ItemData itemData;
        protected Item spawnedItem;
    }
}
