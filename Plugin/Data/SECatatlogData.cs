using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using ThunderRoad;
using System.Collections;
using Newtonsoft.Json;

namespace ScenarioEditor.Data
{
    public class SECatalogData : CustomData, ISEData
    {
        public string Id { get => id; set => id = value; }

        [JsonIgnore]
        public string saveFolder;

        public virtual void RefreshReferences()
        {}

        public virtual IEnumerator RefreshReferencesCoroutine()
        {
            yield return new WaitForEndOfFrame();
        }
    }
}
