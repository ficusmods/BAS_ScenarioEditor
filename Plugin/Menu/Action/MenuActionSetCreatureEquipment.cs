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
    public class MenuActionSetCreatureEquipment : SELazyMenu
    {

        protected override IEnumerator InitLazyMenuCoroutine()
        {

            selectorContainer = AddSelector("Select container");
            selectorContainer.onOptionSelected += SelectorContainer_onOptionSelected; ;

            inputBBCreatureName = AddInputField("BB creature:", (text) => {
                (NodeEditorManager.NowEditing as Scene.Action.ActionSetCreatureEquipment).bbCreatureName = text;
            });

            yield return new WaitForEndOfFrame();
        }

        private void SelectorContainer_onOptionSelected(string selected)
        {
            var editedNode = (NodeEditorManager.NowEditing as Scene.Action.ActionSetCreatureEquipment);
            editedNode.containerId = selected;
        }


        public override void Refresh()
        {
            var editedNode = (NodeEditorManager.NowEditing as Scene.Action.ActionSetCreatureEquipment);

            selectorContainer.RefreshContent(OrderedCatalog.GetDataList_IDOrder<ContainerData>().Select((container) => container.id));
            inputBBCreatureName.text = editedNode.bbCreatureName;
        }


        protected ContentSelectorElement selectorContainer;
        protected InputField inputBBCreatureName;

    }
}
