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
    public class ActionMoveCreature : SESceneActionNode
    {
        public string locationId;
        public string bbCreatureId;

        public ActionMoveCreature()
        {
            id = "MoveCreature";
        }

        public override void Reset()
        {
            base.Reset();
            moveToNode = null;
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
                Creature creature = obj.GetComponent<Creature>();
                if (creature == null) return NodeState.FAILURE;
                if (creature.isKilled) return NodeState.FAILURE;

                if (moveToNode == null)
                {
                    moveToNode = new ThunderRoad.AI.Action.MoveTo();

                    Blackboard adapterBoard = new Blackboard();
                    moveToNode.moveTarget = ThunderRoad.AI.Action.MoveTo.MoveTarget.InputPosition;
                    moveToNode.turnTarget = ThunderRoad.AI.Action.MoveTo.TurnTarget.MoveDirection;
                    moveToNode.inputMoveTargetVariableName = "moveTarget";
                    moveToNode.strafeAroundTarget = false;
                    moveToNode.targetMaxRadius = 1f;
                    adapterBoard.UpdateVariable("moveTarget", location.pos);
                    moveToNode.Init(creature, adapterBoard);
                    moveToNode.Evaluate();

                }
                else
                {
                    var moveNodeEval = moveToNode.Evaluate();
                    if (moveNodeEval == ThunderRoad.AI.State.SUCCESS)
                    {
                        return NodeState.SUCCESS;
                    }
                    else if(moveNodeEval == ThunderRoad.AI.State.FAILURE)
                    {
                        return NodeState.FAILURE;
                    }
                }
            }

            return NodeState.RUNNING;
        }

        protected Data.SEDataLocation location;
        protected ThunderRoad.AI.Action.MoveTo moveToNode;
    }
}
