using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;
using ThunderRoad;

using ScenarioEditor.ExtensionMethods;

namespace ScenarioEditor.Menu
{
    public class NodeGraph : SEMenu, IBookMenu
    {
        public Data.SEDataScenario Loaded { get; protected set; }

        public NodeGraph() { }
        public override IEnumerator InitCoroutine(SEMenu parent, GameObject bookObj)
        {
            this.bookObj = bookObj;
            GameObject menuObj = bookObj.GetChild("Menu");
            yield return base.InitCoroutine(null, menuObj);

            colliderObj = bookObj.GetChild("Mesh");

            labelLoading = Elements.GetChild("LabelLoading").GetComponent<Text>();
            graphViewerObj= Elements.GetChild("Scroll View");

            graphContentArea = graphViewerObj.GetChild("Viewport/Content");
            templatesRoot = graphViewerObj.GetChild("Viewport/Templates");

            templatedGraphDrawer = new TemplatedGraphDrawer(templatesRoot);
            templatesRoot.SetActive(false);

            SEEventManager.onScenarioLoaded += SEEventManager_onScenarioLoaded;
            SEEventManager.onNodeCreated += SEEventManager_onNodeCreated;
            SEEventManager.onNowEditingNodeChanged += SEEventManager_onNowEditingNodeChanged;
            SEEventManager.onNodeRemoved += SEEventManager_onNodeRemoved;
            SEEventManager.onNodeStateChanged += SEEventManager_onNodeStateChanged;

            bookObj.SetActive(true);
            colliderObj.SetActive(false);
        }

        private void SEEventManager_onNodeStateChanged(Scene.SESceneNode node)
        {
            SetNodeColorState(node);
        }

        private void SetNodeColorState(Scene.SESceneNode node)
        {
            if (GameObjectByNode.TryGetValue(node, out GameObject obj))
            {
                Image img = obj.GetChild("Node").GetComponent<Image>();
                switch (node.State)
                {
                    case Scene.SESceneNode.NodeState.RUNNING:
                        img.color = Config.ColorGraphNodeRunning;
                        break;
                    case Scene.SESceneNode.NodeState.SUCCESS:
                        img.color = Config.ColorGraphNodeSuccess;
                        break;
                    case Scene.SESceneNode.NodeState.FAILURE:
                        img.color = Config.ColorGraphNodeFail;
                        break;
                    default:
                        img.color = Config.ColorGraphNodeInit;
                        break;
                }
            }
        }

        private void SEEventManager_onNodeRemoved(Scene.SESceneNode node)
        {
            Remove(node);
        }

        private void SEEventManager_onScenarioLoaded(Data.SEDataScenario scenario)
        {
            Loaded = scenario;
        }

        private void SEEventManager_onNodeCreated(Scene.SESceneNode node)
        {
            Add(node);
        }

        private void SEEventManager_onNowEditingNodeChanged(Scene.SESceneNode node)
        {
            StartDrawCoroutineFromNode(node);
        }

        protected override void TryShow()
        {
            base.TryShow();
            if (Active)
            {
                colliderObj.SetActive(true);
                MenuObj.SetActive(true);
                if (Loaded != null)
                {
                    StartDrawCoroutineFromNode(Loaded.RootNode);
                }
            }
        }

        protected override void HideAll()
        {
            base.HideAll();
            colliderObj.SetActive(false);
            MenuObj.SetActive(false);
        }

        public void StartDrawCoroutineFromNode(Scene.SESceneNode node)
        {
            GameManager.local.StartCoroutine(DrawFromCoroutine(node));
        }

        protected IEnumerator DrawFromCoroutine(Scene.SESceneNode node)
        {
            if (node.GetType() != typeof(Scene.SESceneRootNode))
            {
                node = node.Scene;
            }

            ClearGraphViewer();// TODO cache already processed scenes
            labelLoading.gameObject.SetActive(true);
            graphViewerObj.gameObject.SetActive(false);

            if (node.Scene != node) // drawing from a sub scene
            {
                Add(node.Scene);
                SetNodeColorState(node.Scene);
                AddSubScene(node);
                SetNodeColorState(node);
            }
            else
            {
                Add(node);
                SetNodeColorState(node);
            }

            GameObject start = GameObjectByNode[node]?.GetChild("Children");
            if (start != null) /* ignore drawing for nodes created outside the graph, i.e. not using UI | TODO in the future. */ 
            {
                yield return templatedGraphDrawer.WalkThrough(node, start, (dict) => {
                    foreach (var entry in dict)
                    {
                        GameObjectByNode[entry.Key] = entry.Value;
                        entry.Value.GetComponentInChildren<Button>().onClick.AddListener(() =>
                        {
                            NodeEditorManager.RequestEditForNode(entry.Key);
                        });
                        SetNodeColorState(entry.Key);
                    }
                    labelLoading.gameObject.SetActive(false);
                    graphViewerObj.gameObject.SetActive(true);
                });
            }
        }

        protected void ClearGraphViewer()
        {
            foreach (GameObject obj in GameObjectByNode.Values)
            {
                GameObject.Destroy(obj);
            }
            GameObjectByNode.Clear();
        }

        public void Remove(Scene.SESceneNode node)
        {
            templatedGraphDrawer.Remove(node, GameObjectByNode[node]);
            GameObjectByNode.Remove(node);
        }

        public void Add(Scene.SESceneNode node)
        {
            GameObject nodeObj;
            if (node.Parent == null)
            {
                nodeObj = graphContentArea; // root scene node
            }
            else
            {
                nodeObj = GameObjectByNode[node.Parent]?.GetChild("Children");
            }

            GameObject added = templatedGraphDrawer.Add(node, nodeObj);
            GameObjectByNode[node] = added;
            added.GetComponentInChildren<Button>().onClick.AddListener(() =>
            {
                NodeEditorManager.RequestEditForNode(node);
            });
        }

        public void AddSubScene(Scene.SESceneNode node)
        {
            GameObject nodeObj;
            if (node.GetType() == typeof(Scene.SESceneRootNode))
            {
                nodeObj = GameObjectByNode[node.Scene]?.GetChild("Children");
                GameObject added = templatedGraphDrawer.Add(node, nodeObj);
                GameObjectByNode[node] = added;
                added.GetComponentInChildren<Button>().onClick.AddListener(() =>
                {
                    NodeEditorManager.RequestEditForNode(node);
                });
            }
        }

        public override void Refresh()
        {
        }

        public void MoveTo(Vector3 pos, Quaternion rotation)
        {
            bookObj.transform.position = pos;
            bookObj.transform.rotation = rotation;
        }

        public void AttachTo(Transform transform)
        {
            bookObj.transform.parent = transform;
        }

        public GameObject GetGameObject()
        {
            return bookObj;
        }

        public void Detach()
        {
            bookObj.transform.parent = null;
        }

        public void SetSize(Vector2 canvasSize, Vector3 objectScale)
        {
            RectTransform rect = MenuObj.GetComponent<RectTransform>();
            rect.sizeDelta = canvasSize;
            colliderObj.transform.localScale = new Vector3(canvasSize.x, canvasSize.y, 2.0f);
            bookObj.transform.localScale = objectScale;
            rect.ForceUpdateRectTransforms();
        }

        protected GameObject bookObj;
        protected GameObject colliderObj;

        protected Text labelLoading;
        protected GameObject graphViewerObj;
        protected GameObject graphContentArea;
        protected GameObject templatesRoot;

        TemplatedGraphDrawer templatedGraphDrawer;

        Dictionary<Scene.SESceneNode, GameObject> GameObjectByNode = new Dictionary<Scene.SESceneNode, GameObject>();
    }
}
