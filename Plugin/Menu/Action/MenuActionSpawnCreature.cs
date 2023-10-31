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
    public class MenuActionSpawnCreature : SELazyMenu
    {

        protected override IEnumerator InitLazyMenuCoroutine()
        {

            StartHorizontalGroup();
            selectorCreatureSingle = AddSelector("Select individual");
            selectorCreatureSingle.onOptionSelected += SelectorCreatureSingle_onOptionSelected;
            AddLabel("OR");
            selectorCreatureTable = AddSelector("Select table");
            selectorCreatureTable.onOptionSelected += SelectorCreatureTable_onOptionSelected;
            EndHorizontalGroup();

            StartVerticalGroup();
            selectorFaction = AddSelector("Select faction");
            selectorFaction.onOptionSelected += SelectorFaction_onOptionSelected;
            EndVerticalGroup();

            StartVerticalGroup();
            toggleDespawnOnReset = AddToggle("DespawnOnReset", (state) =>
            {
                (NodeEditorManager.NowEditing as Scene.Action.ActionSpawnCreature).despawnOnReset = state;
            });
            inputBBCreatureId = AddInputField("BB id:", (text) => {
                (NodeEditorManager.NowEditing as Scene.Action.ActionSpawnCreature).bbSpawnedId = text;
            });
            EndVerticalGroup();

            StartVerticalGroup();
            AddLabel("AT");
            selectorLocation = AddSelector("Select location");
            selectorLocation.onOptionSelected += SelectorLocation_onOptionSelected;
            EndVerticalGroup();

            StartVerticalGroup();
            labelCreatureId = AddLabel();
            labelFaction = AddLabel();
            labelLocationId = AddLabel();
            EndVerticalGroup();


            yield return new WaitForEndOfFrame();
        }

        private void SelectorFaction_onOptionSelected(string selected)
        {
            var editedNode = (NodeEditorManager.NowEditing as Scene.Action.ActionSpawnCreature);
            Enum.TryParse(selected, out editedNode.faction);
            labelFaction.text = selected;
        }

        private void SelectorLocation_onOptionSelected(string selected)
        {
            (NodeEditorManager.NowEditing as Scene.Action.ActionSpawnCreature).locationId = selected;
            UpdateLabels();
        }

        private void SelectorCreatureTable_onOptionSelected(string selected)
        {
            (NodeEditorManager.NowEditing as Scene.Action.ActionSpawnCreature).creatureId = selected;
            (NodeEditorManager.NowEditing as Scene.Action.ActionSpawnCreature).creatureType = CreatureTable.Drop.Reference.Table;
            UpdateLabels();
        }

        private void SelectorCreatureSingle_onOptionSelected(string selected)
        {
            (NodeEditorManager.NowEditing as Scene.Action.ActionSpawnCreature).creatureId = selected;
            (NodeEditorManager.NowEditing as Scene.Action.ActionSpawnCreature).creatureType = CreatureTable.Drop.Reference.Creature;
            UpdateLabels();
        }

        protected virtual void UpdateLabels()
        {
            Scene.Action.ActionSpawnCreature actionNode = (NodeEditorManager.NowEditing as Scene.Action.ActionSpawnCreature);
            labelLocationId.text = actionNode.locationId;
            labelCreatureId.text = actionNode.creatureType == CreatureTable.Drop.Reference.Table ? "(Table) " : "(Single) ";
            labelCreatureId.text += actionNode.creatureId;
            labelFaction.text = actionNode.faction.ToString();
        }

        public override void Refresh()
        {
            Scene.Action.ActionSpawnCreature actionNode = (NodeEditorManager.NowEditing as Scene.Action.ActionSpawnCreature);

            selectorLocation.RefreshContent(actionNode.Scene.Scenario.Locations.Select((entry) => entry.Key));
            selectorCreatureTable.RefreshContent(OrderedCatalog.GetDataList_IDOrder<CreatureTable>().Select((table) => table.id));
            selectorCreatureSingle.RefreshContent(OrderedCatalog.GetDataList_IDOrder<CreatureData>().Select((creature) => creature.id));
            selectorFaction.RefreshContent(Enum.GetNames(typeof(Scene.Action.ActionSetFaction.FactionType)));
            UpdateLabels();
            inputBBCreatureId.text = actionNode.bbSpawnedId;
            toggleDespawnOnReset.isOn = actionNode.despawnOnReset;
        }

        protected ContentSelectorElement selectorCreatureSingle;
        protected ContentSelectorElement selectorCreatureTable;
        protected ContentSelectorElement selectorLocation;
        protected ContentSelectorElement selectorFaction;

        protected InputField inputBBCreatureId;
        protected Toggle toggleDespawnOnReset;
        
        protected Text labelLocationId;
        protected Text labelCreatureId;
        protected Text labelFaction;

    }
}
