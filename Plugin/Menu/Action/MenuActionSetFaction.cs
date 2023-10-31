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
    public class MenuActionSetFaction : SELazyMenu
    {

        protected override IEnumerator InitLazyMenuCoroutine()
        {
            inputCreatureId = AddInputField("BB id:", (text) => {
                (NodeEditorManager.NowEditing as Scene.Action.ActionTeleportCreature).bbCreatureId = text;
            });

            selectorFaction = AddSelector("Select faction");
            selectorFaction.onOptionSelected += SelectorFaction_onOptionSelected;
            labelFaction = AddLabel();

            yield return new WaitForEndOfFrame();
        }

        private void SelectorFaction_onOptionSelected(string selected)
        {
            var editedNode = (NodeEditorManager.NowEditing as Scene.Action.ActionSetFaction);
            Enum.TryParse(selected, out editedNode.faction);
            labelFaction.text = selected;
        }

        public override void Refresh()
        {
            var editedNode = (NodeEditorManager.NowEditing as Scene.Action.ActionSetFaction);

            selectorFaction.RefreshContent(Enum.GetNames(typeof(Scene.Action.ActionSetFaction.FactionType)));
            inputCreatureId.text = editedNode.bbCreatureId;
            labelFaction.text = editedNode.faction.ToString();
        }

        protected InputField inputCreatureId;
        protected ContentSelectorElement selectorFaction;
        protected Text labelFaction;

    }
}
