using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;
using ThunderRoad;
using ThunderRoad.AI;

namespace ScenarioEditor.Menu.Action
{
    public class MenuActionMoveCreature : SELazyMenu
    {

        protected override IEnumerator InitLazyMenuCoroutine()
        {
            inputCreatureId = AddInputField("BB id:", (text) => {
                (NodeEditorManager.NowEditing as Scene.Action.ActionMoveCreature).bbCreatureId = text;
            });

            selectorLocation = AddSelector("Select location");
            selectorLocation.onOptionSelected += SelectorLocation_onOptionSelected;
            labelLocationId = AddLabel();

            yield return new WaitForEndOfFrame();
        }

        private void SelectorLocation_onOptionSelected(string selected)
        {
            var editedNode = (NodeEditorManager.NowEditing as Scene.Action.ActionMoveCreature);
            editedNode.locationId = selected;
            labelLocationId.text = selected;
        }

        public override void Refresh()
        {
            var editedNode = (NodeEditorManager.NowEditing as Scene.Action.ActionMoveCreature);

            selectorLocation.RefreshContent(editedNode.Scene.Scenario.Locations.Select((entry) => entry.Key));
            inputCreatureId.text = editedNode.bbCreatureId;
            labelLocationId.text = editedNode.locationId;
        }

        protected InputField inputCreatureId;
        protected ContentSelectorElement selectorLocation;
        protected Text labelLocationId;

    }
}
