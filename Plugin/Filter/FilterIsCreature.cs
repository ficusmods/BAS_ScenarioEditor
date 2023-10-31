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
    public class FilterIsCreature : IFilter
    {
        public bool Check(GameObject obj)
        {
            return obj.GetComponentInChildren<Creature>() != null;
        }
    }
}
