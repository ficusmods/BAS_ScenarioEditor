using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;

using ThunderRoad;

namespace ScenarioEditor.Menu.Action
{
    public class MenuActionLoadDungeon : SELazyMenu
    {
        protected override IEnumerator InitLazyMenuCoroutine()
        {
            levelSelector = AddSelector("Choose dungeon");
            levelSelector.onOptionSelected += TypeSelector_onOptionSelected;

            StartHorizontalGroup();
            inputSeed = AddInputField("Seed", (input) =>
            {
                var editedNode = (NodeEditorManager.NowEditing as Scene.Action.ActionLoadDungeon);
                editedNode.seedStr = input;
            });
            AddButton("Current", () =>
            {
                inputSeed.text = Level.seed.ToString();
            });
            EndHorizontalGroup();

            yield return new WaitForEndOfFrame();
        }

        private void TypeSelector_onOptionSelected(string selected)
        {
            var editedNode = (NodeEditorManager.NowEditing as Scene.Action.ActionLoadDungeon);
            editedNode.levelId = Catalog.GetData<LevelData>(selected)?.id;
        }

        public override void Refresh()
        {
            var editedNode = (NodeEditorManager.NowEditing as Scene.Action.ActionLoadDungeon);
            levelSelector.RefreshContent(Catalog.GetDataList<LevelData>().Where((x) => Utils.isDungeonLevel(x)).Select((x) => x.id));
            inputSeed.text = editedNode.seedStr;
        }

        protected ContentSelectorElement levelSelector;
        protected InputField inputSeed;
    }
}
