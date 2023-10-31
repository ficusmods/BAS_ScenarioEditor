using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using ThunderRoad;
using ThunderRoad.AI;
using System.Collections;

using Newtonsoft.Json;

namespace ScenarioEditor.Scene.Action
{
    public class ActionClearCreatures : SESceneDecoratorNode
    {
        public ActionClearCreatures()
        {
            id = "ActionClearCreatures";
        }

        protected override NodeState TryStart()
        {
            if (Level.current == null) return NodeState.FAILURE;
            return Continue();
        }

        protected override NodeState Continue()
        {
            if(AreaManager.Instance != null)
            {
                foreach(var room in AreaManager.Instance.CurrentTree)
                {
                    var area = room.SpawnedArea;
                    var creatures = new List<Creature>(area.creatures);
                    foreach (var creature in creatures)
                    {
                        if (!creature.isPlayer)
                        {
                            creature.Despawn();
                        }
                    }
                }
            }
            else
            {
                var creatures = Creature.allActive.ToList();
                foreach (var creature in creatures)
                {
                    creature.Despawn();
                }
            }
            return NodeState.SUCCESS;
        }
    }
}
    