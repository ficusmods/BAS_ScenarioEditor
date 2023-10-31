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
    public class MenuConditionSignalActive : SELazyMenu
    {
        protected override IEnumerator InitLazyMenuCoroutine()
        {
            StartVerticalGroup();
            inputSignalName = AddInputField("Signal name", (input) => {
                (NodeEditorManager.NowEditing as Scene.Condition.ConditionSignalActive).bbSignalId = input;
            });
            EndVerticalGroup();

            yield return new WaitForEndOfFrame();
        }

        public override void Refresh()
        {
            var editedNode = (NodeEditorManager.NowEditing as Scene.Condition.ConditionSignalActive);
            inputSignalName.text = editedNode.bbSignalId;
        }

        protected InputField inputSignalName;
    }
}
    