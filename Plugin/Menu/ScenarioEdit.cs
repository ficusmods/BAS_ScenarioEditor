using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;

namespace ScenarioEditor.Menu
{
    public class ScenarioEdit : SELazyMenu
    {
        public Data.SEDataScenario Loaded { get; protected set; }

        public ScenarioEdit() { }

        public override IEnumerator InitCoroutine(SEMenu parent, GameObject menuObj)
        {
            yield return base.InitCoroutine(parent, menuObj);
            SEEventManager.onScenarioLoaded += SEEventManager_onScenarioLoaded;
            SEEventManager.onScenarioStarted += SEEventManager_onScenarioStarted;
            SEEventManager.onScenarioPaused += SEEventManager_onScenarioPaused;
            SEEventManager.onScenarioEnded += SEEventManager_onScenarioEnded;
        }

        protected override IEnumerator InitLazyMenuCoroutine()
        {
            labelScenario = AddLabel();

            StartVerticalGroup();
            buttonPlay = AddButton("Play", () =>
            {
                if (!ScenarioManager.Running)
                {
                    ScenarioManager.StartScenario(Loaded);
                }
                else
                {
                    ScenarioManager.ToStop = true;
                }
            });

            buttonPlayScene = AddButton("Play selected scene", () =>
            {
                if (!ScenarioManager.Running)
                {
                    var currentScene = (NodeEditorManager.NowEditing.GetType() == typeof(Scene.SESceneRootNode)) ? NodeEditorManager.NowEditing : NodeEditorManager.NowEditing.Scene;
                    ScenarioManager.StartScene(Loaded, (Scene.SESceneRootNode)currentScene);
                }
            });

            buttonPause = AddButton("Pause", () =>
            {
                ScenarioManager.Paused = !ScenarioManager.Paused;
            });

            buttonReset = AddButton("Reset", () =>
            {
                if (ScenarioManager.Running)
                {
                    ScenarioManager.ToReset = true;
                }
                else
                {
                    Loaded.RootNode.Reset();
                }
            });

            EndVerticalGroup();

            StartVerticalGroup();
            buttonSave = AddButton("Save scenario", () =>
            {
                ScenarioManager.SaveScenario(Loaded);
            });

            buttonBack = AddButton("Back", () =>
            {
                SwitchToParent();
            });
            EndVerticalGroup();

            buttonPause.interactable = false;

            yield return new WaitForEndOfFrame();
        }

        private void SEEventManager_onScenarioStarted(Data.SEDataScenario scenario)
        {
            Refresh();
        }

        private void SEEventManager_onScenarioEnded()
        {
            Refresh();
        }

        private void SEEventManager_onScenarioPaused(bool paused)
        {
            Refresh();
        }

        private void SEEventManager_onScenarioLoaded(Data.SEDataScenario scenario)
        {
            Loaded = scenario;
            labelScenario.text = scenario.id;
            Refresh();
        }

        public override void Refresh()
        {
            if(ScenarioManager.Running)
            {
                buttonPlay.GetComponentInChildren<Text>().text = "Stop";
                buttonPlayScene.interactable = false;
                buttonPause.interactable = true;
                if (ScenarioManager.Paused)
                {
                    buttonPause.GetComponentInChildren<Text>().text = "Continue";
                }
                else
                {
                    buttonPause.GetComponentInChildren<Text>().text = "Pause";
                }
                buttonBack.interactable = false;
                buttonSave.interactable = false;
            }
            else
            {
                buttonPlay.GetComponentInChildren<Text>().text = "Play";
                buttonPlayScene.interactable = true;
                buttonPause.interactable = false;
                buttonBack.interactable = true;
                buttonSave.interactable = true;
            }
        }

        protected Text labelScenario;
        protected Button buttonPlay;
        protected Button buttonPlayScene;
        protected Button buttonPause;
        protected Button buttonReset;
        protected Button buttonSave;
        protected Button buttonBack;
    }
}
