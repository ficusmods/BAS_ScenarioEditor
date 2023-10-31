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
    public class MenuConditonBooleanFunc<T> : SELazyMenu
        where T: Scene.SESceneBooleanFunc
    {
        protected override IEnumerator InitLazyMenuCoroutine()
        {

            StartHorizontalGroup();
            StartVerticalGroup();
            selectorNodeA = AddSelector("Choose node A");
            selectorNodeA.onOptionSelected += SelectorNodeA_onOptionSelected;
            AddButton("Edit child", () =>
            {
                NodeEditorManager.RequestEditForNode((NodeEditorManager.NowEditing as T).nodeA);
            });
            EndVerticalGroup();

            StartVerticalGroup();
            selectorNodeB = AddSelector("Choose node B");
            selectorNodeB.onOptionSelected += SelectorNodeB_onOptionSelected;
            AddButton("Edit child", () =>
            {
                NodeEditorManager.RequestEditForNode((NodeEditorManager.NowEditing as T).nodeB);
            });
            EndVerticalGroup();
            EndHorizontalGroup();

            yield return new WaitForEndOfFrame();
        }

        private void SelectorNodeA_onOptionSelected(string selected)
        {
            var editedNode = NodeEditorManager.NowEditing as T;
            NodeEditorManager.RemoveNode(editedNode.nodeA);
            editedNode.SetNodeA(NodeEditorManager.CreateNode(selected, NodeEditorManager.NowEditing));
        }

        private void SelectorNodeB_onOptionSelected(string selected)
        {
            var editedNode = NodeEditorManager.NowEditing as T;
            NodeEditorManager.RemoveNode(editedNode.nodeB);
            editedNode.SetNodeB(NodeEditorManager.CreateNode(selected, NodeEditorManager.NowEditing));
        }

        protected ContentSelectorElement selectorNodeA;
        protected ContentSelectorElement selectorNodeB;
    }
}
    