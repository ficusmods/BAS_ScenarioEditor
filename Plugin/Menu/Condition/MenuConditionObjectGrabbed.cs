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
    public class MenuConditionObjectGrabbed : SELazyMenu
    {
        protected override IEnumerator InitLazyMenuCoroutine()
        {

            StartVerticalGroup();
            inputObjName = AddInputField("BB item name", (input) => {
                (NodeEditorManager.NowEditing as Scene.Condition.ConditionObjectGrabbed).bbObjId = input;
            });
            EndVerticalGroup();

            yield return new WaitForEndOfFrame();
        }

        public override void Refresh()
        {
            var editedNode = NodeEditorManager.NowEditing as Scene.Condition.ConditionObjectGrabbed;
            inputObjName.text = editedNode.bbObjId;
        }

        protected InputField inputObjName;
    }
}
    