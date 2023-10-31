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
    public class MenuActionSetHeight : SELazyMenu
    {

        protected override IEnumerator InitLazyMenuCoroutine()
        {
            inputCreatureId = AddInputField("BB id:", (text) => {
                (NodeEditorManager.NowEditing as Scene.Action.ActionSetHeight).bbCreatureId = text;
            });

            inputHeight = AddInputField("height:", (text) => {
                if(float.TryParse(text, out float height)) {
                    (NodeEditorManager.NowEditing as Scene.Action.ActionSetHeight).height = height;
                }
            });

            yield return new WaitForEndOfFrame();
        }

        public override void Refresh()
        {
            var editedNode = (NodeEditorManager.NowEditing as Scene.Action.ActionSetHeight);

            inputCreatureId.text = editedNode.bbCreatureId;
            inputHeight.text = editedNode.height.ToString();
        }

        protected InputField inputCreatureId;
        protected InputField inputHeight;

    }
}
