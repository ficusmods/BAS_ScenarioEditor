using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;

namespace ScenarioEditor.Menu.Decorator
{
    public class MenuDecorator<T> : SELazyMenu
        where T: Scene.SESceneDecoratorNode
    {
        protected override IEnumerator InitLazyMenuCoroutine()
        {
            typeSelector = AddSelector("Choose child type");
            typeSelector.onOptionSelected += TypeSelector_onOptionSelected;

            childName = AddLabel("Child name:");
            childType = AddLabel("Child type:");

            AddButton("Edit child", () =>
            {
                NodeEditorManager.RequestEditForNode((NodeEditorManager.NowEditing as T).child);
            });

            yield return new WaitForEndOfFrame();
        }

        private void TypeSelector_onOptionSelected(string selected)
        {
            var editedNode = NodeEditorManager.NowEditing as T;
            NodeEditorManager.RemoveNode(editedNode.child);
            editedNode.SetChild(NodeEditorManager.CreateNode(selected, NodeEditorManager.NowEditing));
            childName.text = "Child name: " + editedNode.child.id;
            childType.text = "Child type:" + selected;
        }

        public override void Refresh()
        {
            var editedNode = NodeEditorManager.NowEditing as T;
            typeSelector.RefreshContent(NodeEditorManager.Registry.RegisteredNodeTypes);
            childName.text = "Child name: " + editedNode.child?.id;
            childType.text = "Child type:" + editedNode.child?.GetType().Name;
        }

        protected Text childName;
        protected Text childType;
        protected ContentSelectorElement typeSelector;
    }
}
    