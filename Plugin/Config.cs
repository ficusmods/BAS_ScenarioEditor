using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;
using ThunderRoad;

namespace ScenarioEditor
{
    public class Config
    {
        public static string LazyMenuPrefabLocation = "ficus.SEMenu.LazyMenu";
        public static string TriggerPrefabLocation = "ficus.SETrigger";
        public static int ScenarioStopWaitTimeout = 180;
        public static int RegistryRefreshBatchCount = 10;
        public static int NodeRefreshConcurrentCount = 5; // TODO do something about depth
        public static int RegistryRefreshConcurrentCount = 50;

        public static Color ColorGraphNodeInit = new Color(0.27f, 0.521f, 0.533f); // blue
        public static Color ColorGraphNodeRunning = new Color(0.843f, 0.475f, 0.129f); // orange
        public static Color ColorGraphNodeSuccess = new Color(0.557f, 0.753f, 0.486f); // green
        public static Color ColorGraphNodeFail = new Color(0.686f, 0.227f, 0.012f); // red

        public static HashSet<int> DataFormatCompatibility = new HashSet<int>() { 1, };

        [ModOption(name: "Show MainMenu", saveValue = false)]
        [ModOptionButton(interactionType = ModOption.InteractionType.ButtonList)]
        public static bool showScenarioEditorMenu = false;
    }
}
