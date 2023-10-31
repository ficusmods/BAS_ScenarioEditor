using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using ThunderRoad;

namespace ScenarioEditor.Data
{
    public class SEDataScenarios : SEDataDictionary<SEDataScenario>
    {
        public SEDataScenarios()
            : base("Scenario", 1000)
        {
        }
        public SEDataScenarios(IDictionary<string, SEDataScenario> dictionary)
            : base(dictionary, "Scenario")
        {
        }

        public override KeyValuePair<string, SEDataScenario> Add(SEDataScenario item)
        {
            var added = base.Add(item);
            added.Value.id = added.Key;
            return added;
        }

        public override void Add(string id, SEDataScenario item)
        {
            var added = AddNextFree(id, item);
            added.Value.id = added.Key;
        }

        public override KeyValuePair<string, SEDataScenario> AddNextFree(string id, SEDataScenario item)
        {
            var added = base.AddNextFree(id, item);
            added.Value.id = added.Key;
            return added;
        }
    }
    public class SEDataScenario : SECatalogData, ISEData
    {
        public int DataFormatVersion = 1;

        public SEDataLocations Locations
        {
            get;
            protected set;
        } = new SEDataLocations();

        public SEDataTriggers Triggers
        {
            get;
            protected set;
        } = new SEDataTriggers();

        public Scene.SESceneRootNode RootNode
        {
            get;
            protected set;
        } = new Scene.SESceneRootNode();

        public SEDataScenario()
        {
            id = "DefaultScenario";
            saveFolder = "Scenarios";
        }

        public override void RefreshReferences()
        {
            Locations.RefreshReferences();
            Triggers.RefreshReferences();
            RootNode.RefreshReferences();
        }

        public override IEnumerator RefreshReferencesCoroutine()
        {
            yield return Locations.RefreshReferencesCoroutine();
            yield return Triggers.RefreshReferencesCoroutine();
            yield return RootNode.RefreshReferencesCoroutine();
        }

        public void Save()
        {
            PersistentDataSaver.Save(this);
        }

    }
}
