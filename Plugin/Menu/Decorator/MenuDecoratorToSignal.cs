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
    public class MenuDecoratorToSignal : SELazyMenu
    {
        protected override IEnumerator InitLazyMenuCoroutine()
        {
            typeSelector = AddSelector("Choose child type");
            typeSelector.onOptionSelected += TypeSelector_onOptionSelected;

            childName = AddLabel("Child name:");
            childType = AddLabel("Child type:");

            AddButton("Edit child", () =>
            {
                NodeEditorManager.RequestEditForNode((NodeEditorManager.NowEditing as Scene.Decorator.DecoratorToSignal).child);
            });

            inputSignalName = AddInputField("Signal name", (input) => {
                (NodeEditorManager.NowEditing as Scene.Decorator.DecoratorToSignal).bbSignalId = input;
            });

            yield return new WaitForEndOfFrame();
        }

        private void TypeSelector_onOptionSelected(string selected)
        {
            var editedNode = NodeEditorManager.NowEditing as Scene.Decorator.DecoratorToSignal;
            NodeEditorManager.RemoveNode(editedNode.child);
            editedNode.SetChild(NodeEditorManager.CreateNode(selected, NodeEditorManager.NowEditing));
            childName.text = "Child name: " + editedNode.child.id;
            childType.text = "Child type:" + selected;
        }

        public override void Refresh()
        {
            var editedNode = NodeEditorManager.NowEditing as Scene.Decorator.DecoratorToSignal;
            typeSelector.RefreshContent(NodeEditorManager.Registry.RegisteredNodeTypes);
            childName.text = "Child name: " + editedNode.child?.id;
            childType.text = "Child type:" + editedNode.child?.GetType().Name;
        }

        protected InputField inputSignalName;
        protected Text childName;
        protected Text childType;
        protected ContentSelectorElement typeSelector;
    }
}
    