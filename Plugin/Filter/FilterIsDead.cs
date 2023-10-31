using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using ThunderRoad;
using ThunderRoad.AI;
using System.Collections;

namespace ScenarioEditor.Filter
{
    public class FilterIsDead : IFilter
    {
        public bool Check(GameObject obj)
        {
            var creature = obj.GetComponentInChildren<Creature>();
            return creature != null && creature.isKilled;
        }
    }
}
