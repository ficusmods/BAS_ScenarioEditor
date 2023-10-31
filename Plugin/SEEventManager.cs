using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ThunderRoad;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace ScenarioEditor
{
    public class SEEventManager
    {
        public delegate void ScenariosReloaded();
        public static event ScenariosReloaded onScenariosReloaded;

        public delegate void ScenarioLoaded(Data.SEDataScenario scenario);
        public static event ScenarioLoaded onScenarioLoaded;

        public delegate void ScenarioStarted(Data.SEDataScenario scenario);
        public static event ScenarioStarted onScenarioStarted;

        public delegate void ScenarioPaused(bool paused);
        public static event ScenarioPaused onScenarioPaused;

        public delegate void ScenarioEnded();
        public static event ScenarioEnded onScenarioEnded;

        public delegate void NowEditingNodeChanged(Scene.SESceneNode node);
        public static event NowEditingNodeChanged onNowEditingNodeChanged;

        public delegate void NodeCreated(Scene.SESceneNode node);
        public static event NodeCreated onNodeCreated;

        public delegate void NodeRemoved(Scene.SESceneNode node);
        public static event NodeRemoved onNodeRemoved;

        public delegate void NodeStateChanged(Scene.SESceneNode node);
        public static event NodeStateChanged onNodeStateChanged;

        public static void InvokeScenariosReloaded()
        {
            onScenariosReloaded?.Invoke();
        }

        public static void InvokeScenarioLoaded(Data.SEDataScenario scenario)
        {
            onScenarioLoaded?.Invoke(scenario);
        }

        public static void InvokeScenarioStarted(Data.SEDataScenario scenario)
        {
            onScenarioStarted?.Invoke(scenario);
        }

        public static void InvokeScenarioEnded()
        {
            onScenarioEnded?.Invoke();
        }

        public static void InvokeScenarioPaused(bool paused)
        {
            onScenarioPaused?.Invoke(paused);
        }

        public static void InvokeNowEditingNodeChanged(Scene.SESceneNode node)
        {
            onNowEditingNodeChanged?.Invoke(node);
        }

        public static void InvokeNodeCreated(Scene.SESceneNode node)
        {
            onNodeCreated?.Invoke(node);
        }

        public static void InvokeNodeRemoved(Scene.SESceneNode node)
        {
            onNodeRemoved?.Invoke(node);
        }

        public static void InvokeNodeStateChanged(Scene.SESceneNode node)
        {
            onNodeStateChanged?.Invoke(node);
        }
    }
}
