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
    public class MenuConditionCompare : SELazyMenu
    {
        protected override IEnumerator InitLazyMenuCoroutine()
        {
            ddOperation = AddDropdown(
                Enum.GetNames(typeof(Scene.Condition.ConditionCompare.Operation)).Select((v) => new Dropdown.OptionData(v)).ToList(),
                (s) => {
                    (NodeEditorManager.NowEditing as Scene.Condition.ConditionCompare).operation = (Scene.Condition.ConditionCompare.Operation)s;
            });

            inputBBValue1 = AddInputField("Value1 (BB name)", (input) => {
                (NodeEditorManager.NowEditing as Scene.Condition.ConditionCompare).bbValue1 = input;
            });

            StartVerticalGroup();
            inputBBValue2 = AddInputField("Value2 (BB name)", (input) => {
                (NodeEditorManager.NowEditing as Scene.Condition.ConditionCompare).bbValue2 = input;
            });
            AddLabel("OR");
            inputValue2 = AddInputField("Value2 (constant)", (input) => {
                if (int.TryParse(input, out int result))
                {
                    (NodeEditorManager.NowEditing as Scene.Condition.ConditionCompare).value2 = result;
                }
                else
                {
                    inputValue2.text = "";
                }
            });
            EndVerticalGroup();

            yield return new WaitForEndOfFrame();
        }

        public override void Refresh()
        {
            var editedNode = (NodeEditorManager.NowEditing as Scene.Condition.ConditionCompare);
            inputBBValue1.text = editedNode.bbValue1;
            inputBBValue2.text = editedNode.bbValue2;
            inputValue2.text = editedNode.value2.ToString();
        }

        protected Dropdown ddOperation;
        protected InputField inputBBValue1;
        protected InputField inputBBValue2;
        protected InputField inputValue2;
    }
}
    