using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;

using ThunderRoad;

namespace ScenarioEditor.Menu.Condition
{
    public class MenuConditionTriggerActive : SELazyMenu
    {
        protected override IEnumerator InitLazyMenuCoroutine()
        {
            StartVerticalGroup();
            selectorTrigger = AddSelector("Trigger id");
            selectorTrigger.onOptionSelected += SelectorLocation_onOptionSelected;

            StartHorizontalGroup();
            StartVerticalGroup();
            availableFilterSelector = AddSelector("Available filters");
            availableFilterSelector.onOptionSelected += AvailableFilterSelector_onOptionSelected;
            AddButton("Add filter", () =>
            {
                (NodeEditorManager.NowEditing as Scene.Condition.ConditionTriggerActive).filterNames.Add(selectedAvailableFilter);
                Refresh();
            });
            EndVerticalGroup();
            StartVerticalGroup();
            filterSelector = AddSelector("Assigned filters");
            filterSelector.onOptionSelected += FilterSelector_onOptionSelected;
            AddButton("Remove filter", () =>
            {
                (NodeEditorManager.NowEditing as Scene.Condition.ConditionTriggerActive).filterNames.Remove(selectedFilter);
                Refresh();
            });
            EndVerticalGroup();
            EndHorizontalGroup();
            isInverted = AddToggle("Inverted", (state) => {
                (NodeEditorManager.NowEditing as Scene.Condition.ConditionTriggerActive).inverted = state;
                Refresh();
            });
            EndVerticalGroup();

            yield return new WaitForEndOfFrame();
        }

        private void FilterSelector_onOptionSelected(string selected)
        {
            selectedFilter = selected;
        }

        private void AvailableFilterSelector_onOptionSelected(string selected)
        {
            selectedAvailableFilter = selected;
        }

        private void SelectorLocation_onOptionSelected(string selected)
        {
            (NodeEditorManager.NowEditing as Scene.Condition.ConditionTriggerActive).triggerId = selected;
        }

        public override void Refresh()
        {
            var editedNode = NodeEditorManager.NowEditing as Scene.Condition.ConditionTriggerActive;
            selectorTrigger.RefreshContent(editedNode.Scene.Scenario.Triggers.Keys);

            var registry = Catalog.GetData<Data.SEFilterRegistry>("FilterRegistry");
            availableFilterSelector.RefreshContent(registry.RegisteredFilterNames);
            filterSelector.ReloadContent(editedNode.filterNames);
            isInverted.isOn = editedNode.inverted;
        }

        protected ContentSelectorElement selectorTrigger;
        protected ContentSelectorElement availableFilterSelector;
        protected ContentSelectorElement filterSelector;
        protected Toggle isInverted;

        protected string selectedAvailableFilter;
        protected string selectedFilter;
    }
}
    