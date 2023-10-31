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
    public class MenuActionLoadLevel : SELazyMenu
    {
        protected override IEnumerator InitLazyMenuCoroutine()
        {
            levelSelector = AddSelector("Choose level");
            levelSelector.onOptionSelected += TypeSelector_onOptionSelected;

            levelLabel = AddLabel("Level");

            yield return new WaitForEndOfFrame();
        }

        private void TypeSelector_onOptionSelected(string selected)
        {
            (NodeEditorManager.NowEditing as Scene.Action.ActionLoadLevel).levelId = Catalog.GetData<LevelData>(selected)?.id;
        }

        public override void Refresh()
        {
            var nowEditing = (NodeEditorManager.NowEditing as Scene.Action.ActionLoadLevel);
            levelSelector.RefreshContent(Catalog.GetDataList<LevelData>().Select((x) => x.id));

            levelLabel.text = nowEditing.levelId;
        }

        protected ContentSelectorElement levelSelector;
        protected Text levelLabel;
    }
}
