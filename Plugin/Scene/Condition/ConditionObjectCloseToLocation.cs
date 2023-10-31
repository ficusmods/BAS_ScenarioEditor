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

namespace ScenarioEditor.Scene.Condition
{
    public class ConditionObjectCloseToLocation : SESceneCondition
    {
        public string bbObjId;
        public float distance = 2.0f;
        public string locationId;

        public ConditionObjectCloseToLocation()
        {
            id = "CLOSE_TO";
        }

        public override void RefreshReferences()
        {
            if (locationId != null)
            {
                Scene.Scenario.Locations.Find(locationId, out location);
            }
        }

        protected override NodeState TryStart()
        {
            if (location == null) return NodeState.FAILURE;
            return Continue();
        }

        protected override NodeState Continue()
        {
            if (Scene.Blackboard.Find<GameObject>(bbObjId, out GameObject obj))
            {
                return (Vector3.Distance(obj.transform.position, location.pos) < distance) ? NodeState.SUCCESS : NodeState.FAILURE;
            }

            return NodeState.FAILURE;
        }

        protected Data.SEDataLocation location;
    }
}
