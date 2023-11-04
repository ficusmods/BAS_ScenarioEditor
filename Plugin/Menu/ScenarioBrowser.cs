using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;

namespace ScenarioEditor.Menu
{
    public class ScenarioBrowser : SELazyBookMenu
    {
        public string Selected { get; protected set; } = "";

        public ScenarioBrowser() { }

        public override IEnumerator InitCoroutine(SEMenu parent, GameObject menuObj)
        {
            yield return base.InitCoroutine(parent, menuObj);

            yield return AddLazySubMenuCoroutine<ScenarioEdit>((menu) =>
            {
                menuEdit = menu;
            });

            SEEventManager.onScenarioLoaded += SEEventManager_onScenarioLoaded;
        }

        private void SEEventManager_onScenarioLoaded(Data.SEDataScenario scenario)
        {
            SwitchToChild(menuEdit);
        }

        protected override IEnumerator InitLazyMenuCoroutine()
        {
            AddLabel("Scenario selector").fontSize = 52;

            scenarioSelector = AddSelector("Scenarios");
            scenarioSelector.onOptionSelected += ScenarioSelector_onOptionSelected;

            labelSelectedScenario = AddLabel();

            StartHorizontalGroup();
            AddButton("New", () =>
            {
                Selected = ScenarioManager.CreateScenario()?.id;
                Refresh();
            });

            AddButton("Load", () =>
            {
                ScenarioManager.LoadScenario(Selected);
            });
            EndHorizontalGroup();


            yield return new WaitForEndOfFrame();
        }

        private void ScenarioSelector_onOptionSelected(string selected)
        {
            Selected = selected;
            labelSelectedScenario.text = "Scenario: " + Selected;
        }

        public override void Refresh()
        {
            scenarioSelector.RefreshContent(ScenarioManager.Scenarios.Keys);
            labelSelectedScenario.text = "Scenario: " + Selected;
        }

        protected ScenarioEdit menuEdit;

        protected ContentSelectorElement scenarioSelector;
        protected Text labelSelectedScenario;
    }
}
