using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;

namespace ScenarioEditor.Menu.Conditional
{
    public class MenuConditional<T> : SELazyMenu
        where T: Scene.SESceneConditionalNode
    {
        protected override IEnumerator InitLazyMenuCoroutine()
        {
            StartHorizontalGroup();

            StartVerticalGroup();
            typeSelector = AddSelector("Choose child type");
            typeSelector.onOptionSelected += TypeSelector_onOptionSelected;

            childName = AddLabel("Child name:");
            childType = AddLabel("Child type:");

            AddButton("Edit child", () =>
            {
                NodeEditorManager.RequestEditForNode((NodeEditorManager.NowEditing as T).child);
            });
            EndVerticalGroup();

            StartVerticalGroup();
            conditionSelector = AddSelector("Choose condition");
            conditionSelector.onOptionSelected += ConditionSelector_onOptionSelected;

            conditionName = AddLabel("Condition name:");
            conditionType = AddLabel("Condition type:");

            AddButton("Edit condition", () =>
            {
                NodeEditorManager.RequestEditForNode((NodeEditorManager.NowEditing as T).condition);
            });
            EndVerticalGroup();

            EndHorizontalGroup();

            yield return new WaitForEndOfFrame();
        }

        private void TypeSelector_onOptionSelected(string selected)
        {
            var editedNode = NodeEditorManager.NowEditing as T;
            NodeEditorManager.RemoveNode(editedNode.child);
            var newNode = NodeEditorManager.CreateNode(selected, NodeEditorManager.NowEditing);
            editedNode.SetChild(newNode);
            childName.text = "Child name: " + editedNode.child.id;
            childType.text = "Child type:" + selected;
        }

        private void ConditionSelector_onOptionSelected(string selected)
        {
            var editedNode = NodeEditorManager.NowEditing as T;
            var newNode = NodeEditorManager.CreateNode(selected, NodeEditorManager.NowEditing);
            NodeEditorManager.RemoveNode(editedNode.condition);
            editedNode.SetCondition(newNode);
            conditionName.text = "Condition name: " + editedNode.condition.id;
            conditionType.text = "Condition type:" + selected;
        }

        public override void Refresh()
        {
            var editedNode = NodeEditorManager.NowEditing as T;
            typeSelector.RefreshContent(NodeEditorManager.Registry.RegisteredNodeTypes);
            childName.text = "Child name: " + editedNode.child?.id;
            childType.text = "Child type:" + editedNode.child?.GetType().Name;
            conditionSelector.RefreshContent(NodeEditorManager.Registry.RegisteredNodeTypes);
            conditionName.text = "Condition name: " + editedNode.condition?.id;
            conditionType.text = "Condition type:" + editedNode.condition?.GetType().Name;
        }

        protected Text childName;
        protected Text childType;
        protected ContentSelectorElement typeSelector;

        protected Text conditionName;
        protected Text conditionType;
        protected ContentSelectorElement conditionSelector;
    }
}
    