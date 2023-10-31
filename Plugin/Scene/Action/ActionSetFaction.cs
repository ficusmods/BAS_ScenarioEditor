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
    public class ActionSetFaction : SESceneActionNode
    {
        public enum FactionType
        {
            Passive = -1,
            None,
            Ignored,
            Friendly,
            Enemy1,
            Enemy2,
            Enemy3,
        }

        public FactionType faction;
        public string bbCreatureId;

        public ActionSetFaction()
        {
            id = "SetFaction";
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

                creature.SetFaction((int)faction);

                return NodeState.SUCCESS;
            }

            return NodeState.RUNNING;
        }

    }
}
