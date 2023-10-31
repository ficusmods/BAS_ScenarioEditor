using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;

namespace ScenarioEditor.Menu.Condition
{
    public class MenuConditionObjectCloseToLocation : SELazyMenu
    {
        protected override IEnumerator InitLazyMenuCoroutine()
        {
            selectorLocation = AddSelector("Select location");
            selectorLocation.onOptionSelected += SelectorLocation_onOptionSelected;

            StartVerticalGroup();
            inputObjName = AddInputField("Blackboard object name", (input) => {
                (NodeEditorManager.NowEditing as Scene.Condition.ConditionObjectCloseToLocation).bbObjId = input;
            });

            inputDistance = AddInputField("distance <", (input) => {
                (NodeEditorManager.NowEditing as Scene.Condition.ConditionObjectCloseToLocation).distance = float.Parse(input);
            });
            labelLocationId = AddLabel();
            EndVerticalGroup();

            yield return new WaitForEndOfFrame();
        }

        private void SelectorLocation_onOptionSelected(string selected)
        {
            var closeToNode = NodeEditorManager.NowEditing as Scene.Condition.ConditionObjectCloseToLocation;
            closeToNode.locationId = selected;
            labelLocationId.text = selected;
        }

        public override void Refresh()
        {
            var closeToNode = NodeEditorManager.NowEditing as Scene.Condition.ConditionObjectCloseToLocation;
            selectorLocation.RefreshContent(closeToNode.Scene.Scenario.Locations.Select((entry) => entry.Key));
            labelLocationId.text = closeToNode.locationId;
            inputObjName.text = closeToNode.bbObjId;
            inputDistance.text = closeToNode.distance.ToString("0.00");
        }

        protected InputField inputObjName;
        protected InputField inputDistance;
        protected ContentSelectorElement selectorLocation;
        protected Text labelLocationId;
    }
}
    