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
    public class ActionSetCreatureEquipment : SESceneActionNode
    {
        public string containerId;
        public string bbCreatureName;

        public ActionSetCreatureEquipment()
        {
            id = "SetCreatureEquipment";
        }

        public override void RefreshReferences()
        {
            if(containerId != null)
            {
                containerData = Catalog.GetData<ContainerData>(containerId);
            }
        }

        protected override NodeState TryStart()
        {
            if (containerData == null) return NodeState.FAILURE;
            return Continue();
        }

        protected override NodeState Continue()
        {
            if(Scene.Blackboard.Find(bbCreatureName, out GameObject obj))
            {
                Creature creature = obj.GetComponentInChildren<Creature>();
                if (creature == null) return NodeState.FAILURE;
                if (creature.loaded == false) return NodeState.RUNNING;
                foreach (var holder in creature.holders)
                {
                    var items = holder.items.ToList();
                    holder.UnSnapAll();
                    foreach (var item in items)
                    {
                        item.Despawn();
                    }
                }
                creature.equipment.UnequipAllWardrobes();
                creature.container.loadContent = Container.LoadContent.ContainerID;
                creature.container.containerID = containerId;
                creature.container.Load();
                creature.mana.Load();
                creature.equipment.EquipAllWardrobes(false, false);
                return NodeState.SUCCESS;
            }
            else
            {
                return NodeState.FAILURE;
            }
        }

        protected ContainerData containerData;
    }
}
