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
    public class MenuActionSetHealth : SELazyMenu
    {

        protected override IEnumerator InitLazyMenuCoroutine()
        {
            inputCreatureId = AddInputField("BB id:", (text) => {
                (NodeEditorManager.NowEditing as Scene.Action.ActionSetHealth).bbCreatureId = text;
            });

            inputHealth = AddInputField("health:", (text) => {
                if(int.TryParse(text, out int health)) {
                    (NodeEditorManager.NowEditing as Scene.Action.ActionSetHealth).health = health;
                }
            });

            yield return new WaitForEndOfFrame();
        }

        public override void Refresh()
        {
            var editedNode = (NodeEditorManager.NowEditing as Scene.Action.ActionSetHealth);

            inputCreatureId.text = editedNode.bbCreatureId;
            inputHealth.text = editedNode.health.ToString();
        }

        protected InputField inputCreatureId;
        protected InputField inputHealth;

    }
}
