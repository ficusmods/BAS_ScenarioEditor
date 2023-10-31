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
    public class MenuConditionCount : SELazyMenu
    {
        protected override IEnumerator InitLazyMenuCoroutine()
        {
            StartVerticalGroup();
            inputTargetCount = AddInputField("Count", (input) => {
                if(int.TryParse(input, out int result)) {
                    (NodeEditorManager.NowEditing as Scene.Condition.ConditionCount).targetCount = result;
                }
            });
            EndVerticalGroup();

            yield return new WaitForEndOfFrame();
        }

        public override void Refresh()
        {
            var editedNode = (NodeEditorManager.NowEditing as Scene.Condition.ConditionCount);
            inputTargetCount.text = editedNode.targetCount.ToString();
        }

        protected InputField inputTargetCount;
    }
}
    