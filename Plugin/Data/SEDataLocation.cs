using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using ThunderRoad;

namespace ScenarioEditor.Data
{
    public class SEDataLocations : SEDataDictionary<SEDataLocation>
    {
        public SEDataLocations()
            : base("Location", 1000)
        {
        }
    }
    public class SEDataLocation : SEData
    {
        public Vector3 pos;
        public Vector3 rotation;
    }
}
