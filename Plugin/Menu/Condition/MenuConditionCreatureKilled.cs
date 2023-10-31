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
    public class MenuConditionCreatureKilled : SELazyMenu
    {
        protected override IEnumerator InitLazyMenuCoroutine()
        {

            StartVerticalGroup();
            inputCreatureName = AddInputField("BB creature name", (input) => {
                (NodeEditorManager.NowEditing as Scene.Condition.ConditionCreatureKilled).bbCreatureId = input;
            });
            EndVerticalGroup();

            yield return new WaitForEndOfFrame();
        }

        public override void Refresh()
        {
            var editedNode = NodeEditorManager.NowEditing as Scene.Condition.ConditionCreatureKilled;
            inputCreatureName.text = editedNode.bbCreatureId;
        }

        protected InputField inputCreatureName;
    }
}
    