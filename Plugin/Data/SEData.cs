using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace ScenarioEditor.Data
{
    public class SEData : ISEData
    {
        public virtual void RefreshReferences() {}
        public virtual IEnumerator RefreshReferencesCoroutine() { yield return new WaitForEndOfFrame(); }
    }
}
