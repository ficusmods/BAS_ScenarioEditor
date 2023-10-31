using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using ThunderRoad;
using ThunderRoad.AI;
using System.Collections;

namespace ScenarioEditor.Scene
{
    public abstract class SESceneCondition : SESceneNode
    {
        public override void Init(SESceneNode parent)
        {
            base.Init(parent);
        }

        protected override NodeState TryStart()
        {
            return Continue();
        }

    }
}
