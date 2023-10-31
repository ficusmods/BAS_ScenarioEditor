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
    public class MenuActionSpawnItem : SELazyMenu
    {

        protected override IEnumerator InitLazyMenuCoroutine()
        {
            StartHorizontalGroup();
            selectorItemType = AddSelector("Select category");
            selectorItemType.onOptionSelected += SelectorCategory_onOptionSelected;

            selectorItem = AddSelector("Select item");
            selectorItem.onOptionSelected += SelectorItem_onOptionSelected;
            EndHorizontalGroup();

            StartVerticalGroup();
            inputBBItemId = AddInputField("BB id:", (text) => {
                (NodeEditorManager.NowEditing as Scene.Action.ActionSpawnItem).bbSpawnedId = text;
            });

            isStatic = AddToggle("Static", (active) =>
            {
                (NodeEditorManager.NowEditing as Scene.Action.ActionSpawnItem).staticObj = active;
            });
            EndVerticalGroup();

            StartVerticalGroup();
            AddLabel("AT");
            selectorLocation = AddSelector("Select location");
            selectorLocation.onOptionSelected += SelectorLocation_onOptionSelected;
            EndVerticalGroup();

            StartVerticalGroup();
            labelItemId = AddLabel();
            labelLocationId = AddLabel();
            EndVerticalGroup();


            yield return new WaitForEndOfFrame();
        }

        private void SelectorCategory_onOptionSelected(string selected)
        {
            Enum.TryParse(selected, out selectedItemType);
            Refresh();
        }

        private void SelectorItem_onOptionSelected(string selected)
        {
            var spawnItem = (NodeEditorManager.NowEditing as Scene.Action.ActionSpawnItem);
            spawnItem.itemId = selected;
            UpdateLabels();
        }

        private void SelectorLocation_onOptionSelected(string selected)
        {
            var spawnItem = (NodeEditorManager.NowEditing as Scene.Action.ActionSpawnItem);
            spawnItem.locationId = selected;
            UpdateLabels();
        }

        protected virtual void UpdateLabels()
        {
            var spawnItem = (NodeEditorManager.NowEditing as Scene.Action.ActionSpawnItem);
            labelLocationId.text = spawnItem.locationId;
            labelItemId.text = spawnItem.itemId;
        }

        public override void Refresh()
        {
            Scene.Action.ActionSpawnItem actionNode = (NodeEditorManager.NowEditing as Scene.Action.ActionSpawnItem);

            selectorItemType.RefreshContent(Enum.GetNames(typeof(ItemData.Type)));
            selectorLocation.RefreshContent(actionNode.Scene.Scenario.Locations.Select((entry) => entry.Key));
            selectorItem.RefreshContent(OrderedCatalog.GetDataList_IDOrder<ItemData>()
                .Where((item) => item.type == selectedItemType)
                .Select((table) => table.id));
            UpdateLabels();
            inputBBItemId.text = actionNode.bbSpawnedId;
            isStatic.isOn = actionNode.staticObj;
        }


        protected ContentSelectorElement selectorItemType;
        protected ContentSelectorElement selectorItem;
        protected ContentSelectorElement selectorLocation;

        protected InputField inputBBItemId;
        protected Toggle isStatic;
        
        protected Text labelLocationId;
        protected Text labelItemId;

        protected ItemData.Type selectedItemType = ItemData.Type.Prop;

    }
}
