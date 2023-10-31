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
    public class MenuConditionTimer : SELazyMenu
    {
        protected override IEnumerator InitLazyMenuCoroutine()
        {
            StartVerticalGroup();
            inputTime = AddInputField("Time to wait", (input) => {
                (NodeEditorManager.NowEditing as Scene.Condition.ConditionTimer).seconds = float.Parse(input);
            });
            EndVerticalGroup();

            yield return new WaitForEndOfFrame();
        }

        public override void Refresh()
        {
            var editedNode = (NodeEditorManager.NowEditing as Scene.Condition.ConditionTimer);
            inputTime.text = editedNode.seconds.ToString("0.00");
        }

        protected InputField inputTime;
    }
}
    