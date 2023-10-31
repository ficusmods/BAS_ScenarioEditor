using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;

namespace ScenarioEditor.Menu.Control
{
    public class MenuControl<T> : SELazyMenu
        where T: Scene.SESceneControlNode
    {

        protected override IEnumerator InitLazyMenuCoroutine()
        {
            typeSelector = AddSelector("Select type to add");
            typeSelector.onOptionSelected += (selected) => selectedType = selected;

            StartVerticalGroup();
            AddButton("Add as child", () => {
                (NodeEditorManager.NowEditing as T).AddChild(
                    NodeEditorManager.CreateNode(selectedType, NodeEditorManager.NowEditing));
                Refresh();
            });
            EndVerticalGroup();

            StartVerticalGroup();
            StartHorizontalGroup();
            childSelector = AddSelector("Select child");
            childSelector.onOptionSelected += (selected) =>
            {
                selectedChild = (NodeEditorManager.NowEditing as T).GetChild(selected);
            };
            StartVerticalGroup();
            AddButton("UP", () =>
            {
                (NodeEditorManager.NowEditing as T).ChildOrderUp(selectedChild);
                Refresh();
            });
            AddButton("DWN", () =>
            {
                (NodeEditorManager.NowEditing as T).ChildOrderDown(selectedChild);
                Refresh();
            });
            EndVerticalGroup();
            EndHorizontalGroup();

            AddButton("Edit child", () => {
                NodeEditorManager.RequestEditForNode(selectedChild);
            });
            AddButton("Remove child", () => {
                NodeEditorManager.RemoveNode(selectedChild);
                selectedChild = null;
                Refresh();
            });
            EndVerticalGroup();

            yield return new WaitForEndOfFrame();
        }

        public override void Refresh()
        {
            typeSelector.RefreshContent(NodeEditorManager.Registry.RegisteredNodeTypes);
            childSelector.ReloadContent((NodeEditorManager.NowEditing as T).Children.Select(
                (node) => node.id));
        }

        protected string selectedType;
        protected Scene.SESceneNode selectedChild;

        protected ContentSelectorElement childSelector;
        protected ContentSelectorElement typeSelector;
    }
}
