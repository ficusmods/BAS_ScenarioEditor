using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using ScenarioEditor.Scene;
using ThunderRoad;

namespace ScenarioEditor
{
    public static class ScenarioManager
    {
        public static bool Running { get; private set; } = false;

        static bool _paused;
        public static bool Paused
        {
            get => _paused;
            set 
            {
                _paused = value;
                SEEventManager.InvokeScenarioPaused(_paused);
            }
        }
        public static bool ToStop { get; set; } = false;
        public static bool ToReset { get; set; } = false;

        public static Data.SEDataScenarios Scenarios { get; private set; }
        public static void ReloadScenarios()
        {
            Scenarios = new Data.SEDataScenarios(
                Catalog.GetDataList<Data.SEDataScenario>()
                .Where(
                    (data) => Config.DataFormatCompatibility.Contains(data.DataFormatVersion))
                .ToDictionary(
                    (data) => data.id,
                    (data) => data));
            SEEventManager.InvokeScenariosReloaded();
        }

        public static Data.SEDataScenario CreateScenario()
        {
            Data.SEDataScenario s = Scenarios.Add(new Data.SEDataScenario()).Value;
            s.RootNode.Init(null);
            s.RootNode.Scenario = s;
            Logger.Basic("Created new scenario: {0}", s.id);
            return s;
        }

        public static void LoadScenario(string id)
        {
            if (!Running)
            {
                if (Scenarios.Find(id, out Data.SEDataScenario scenario))
                {
                    scenario.RootNode.Scenario = scenario;
                    scenario.RootNode.Init(null);
                    scenario.RefreshReferences();
                    GameManager.local.StartCoroutine(LoadScenarioCoroutine(scenario));
                }
                else
                {
                    Logger.Basic("Couldn't load scenario {0} (not found)", id);
                }
            }
            else
            {
                Logger.Basic("Tried to load scenario while already running one");
            }
        }

        static IEnumerator LoadScenarioCoroutine(Data.SEDataScenario scenario)
        {
            yield return scenario.RefreshReferencesCoroutine();
            Logger.Basic("Loaded scenario: {0}", scenario.id);
            SEEventManager.InvokeScenarioLoaded(scenario);
        }

        public static void SaveScenario(string id)
        {
            if (Scenarios.Find(id, out Data.SEDataScenario s))
            {
                SaveScenario(s);
            }
            else
            {
                Logger.Basic("Couldn't load save scenario {0} (not found)", s.id);
            }
        }

        public static void SaveScenario(Data.SEDataScenario scenario)
        {
            if (!Running)
            {
                scenario.RootNode.Reset();
                scenario.Save();
            }
            else
            {
                Logger.Basic("Tried to save scenario while already running one");
            }
        }

        public static void StartScenario(Data.SEDataScenario scenario)
        {
            StartScenario(scenario, () => { return new WaitForSeconds(0.1f); });
        }

        public static void StartScene(Data.SEDataScenario scenario, Scene.SESceneRootNode root)
        {
            StartScenario(scenario, () => { return new WaitForSeconds(0.1f); }, root);
        }

        public static void StartScenario(Data.SEDataScenario scenario, Func<YieldInstruction> yielder, Scene.SESceneRootNode root = null)
        {
            if (!Running)
            {
                EventManager.onLevelLoad -= EventManager_onLevelLoad;
                EventManager.onLevelLoad += EventManager_onLevelLoad;
                EventManager.onLevelUnload -= EventManager_onLevelUnload;
                EventManager.onLevelUnload += EventManager_onLevelUnload;

                Logger.Basic("Starting scenario: {0}", scenario.id);

                try
                {
                    Running = true;
                    Paused = false;
                    ToStop = false;

                    var rootNode = root != null ? root : scenario.RootNode;

                    rootNode.Reset();
                    rootNode.RefreshReferences();
                    GameManager.local.StartCoroutine(RunScenario(scenario, yielder, root));
                }
                catch (Exception e)
                {
                    Logger.Basic("Exception occured during the start of the scenario\n{0}", e.ToString());
                    Running = false;
                    Paused = false;
                    ToStop = true;
                }
            }
            else
            {
                Logger.Basic("Tried to start scenario while already running one");
            }
        }

        private static void EventManager_onLevelUnload(LevelData levelData, EventTime eventTime)
        {
            if(eventTime == EventTime.OnStart)
            {
                Paused = true;
            }
        }

        private static IEnumerator RunScenario(Data.SEDataScenario scenario, Func<YieldInstruction> yielder, Scene.SESceneRootNode root = null)
        {
            yield return scenario.RefreshReferencesCoroutine();
            SEEventManager.InvokeScenarioStarted(scenario);

            SESceneNode.NodeState currState = SESceneNode.NodeState.RUNNING;

            var rootNode = root != null ? root : scenario.RootNode;

            while (currState == SESceneNode.NodeState.RUNNING && ToStop == false)
            {
                try
                {
                    if (ToReset)
                    {
                        rootNode.Reset();
                        ToReset = false;
                    }

                    if (!Paused)
                    {
                        currState = rootNode.Evaluate();
                    }
                }
                catch (Exception e)
                {
                    Logger.Basic("Exception occured during the run of the scenario\n{0}", e.ToString());
                    Running = false;
                    Paused = false;
                    ToStop = true;
                }
                yield return yielder();
            }

            Running = false;
            SEEventManager.InvokeScenarioEnded();
        }

        private static void EventManager_onLevelLoad(LevelData levelData, EventTime eventTime)
        {
            if (eventTime == EventTime.OnStart)
            {
                Paused = true;
            }
            else
            {
                Paused = false;
            }
        }
    }
}
