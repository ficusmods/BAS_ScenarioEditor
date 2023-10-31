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
    public class NodeTreeEditor : SELazyBookMenu
    {
        public string Selected { get; protected set; } = "";
        public Data.SEDataScenario Loaded { get; protected set; }

        public NodeTreeEditor() { }

        public override IEnumerator InitCoroutine(SEMenu parent, GameObject menuObj)
        {
            yield return base.InitCoroutine(parent, menuObj);

            nodeRegistry = Catalog.GetData<Data.SESceneNodeRegistry>("SceneNodeRegistry");
            nodeRegistry.onRefresh += NodeRegistry_onRefresh;

            SEEventManager.onScenarioLoaded += SEEventManager_onScenarioLoaded;
            SEEventManager.onNowEditingNodeChanged += SEEventManager_onNowEditingNodeChanged;
        }

        protected override IEnumerator InitLazyMenuCoroutine()
        {
            SetElementsBoundary(RectTransform.Edge.Bottom, 0.7f);
            SetSubmenusBoundary(RectTransform.Edge.Top, 0.3f);

            labelLoading = AddLabel("LOADING...");
            labelLoading.fontSize = 64;
            labelLoading.gameObject.SetActive(false);

            StartVerticalGroup();
            labelNodeName = AddLabel("Node name");
            labelNodeType = AddLabel("Node type");
            labelNodePath = AddLabel("Path");
            labelNodeName.fontSize = 42;
            labelNodeType.fontSize = 32;
            labelNodePath.fontSize = 32;

            labelNodeDesc = AddLabel("Description");
            labelNodeDesc.fontSize = 32;
            StartHorizontalGroup();
            AddButton("Move up", () =>
            {
                if (NodeEditorManager.NowEditing.Parent != null)
                {
                    NodeEditorManager.RequestEditForNode(NodeEditorManager.NowEditing.Parent);
                }
            });
            EndHorizontalGroup();

            EndVerticalGroup();

            yield return new WaitForEndOfFrame();
        }

        private void NodeRegistry_onRefresh(EventTime time)
        {
            if(time == EventTime.OnEnd)
            {
                GameManager.local.StartCoroutine(InitCustomEditors());
            }
        }

        protected virtual IEnumerator InitCustomEditors()
        {
            Elements.SetChildrenActive(false);
            labelLoading.gameObject.SetActive(true);
            SubMenusObj.DestroyChildren();
            nodeEditorMenus.Clear();
            Logger.Basic("Initializing custom menus based on node registry");
            foreach (var entry in nodeRegistry.RegisteredNodeTypes)
            {
                Logger.Detailed("    Found entry: {0}", entry);
            }

            foreach (var nodeEntry in nodeRegistry.EntryByTypeName.Values)
            {
                if (nodeEntry.menuType != null)
                {
                    Logger.Detailed("Initializing menu for {0}", nodeEntry.id);
                    IMenu editorMenu = (IMenu)Activator.CreateInstance(nodeEntry.menuType);
                    if (nodeEntry.loadedPrefab != null)
                    {
                        yield return editorMenu.InitCoroutine(GameObject.Instantiate(nodeEntry.loadedPrefab, SubMenusObj.transform));
                        nodeEditorMenus.Add(nodeEntry.nodeType, editorMenu);
                        editorMenu.HideMenu();
                    }
                    else
                    {
                        Logger.Basic("Prefab for menu {0} is null. Skipping...", nodeEntry.id);
                    }
                }
                else
                {
                    Logger.Detailed("No menu registered for {0}", nodeEntry.id);
                }
            }

            Elements.SetChildrenActive(true);
            labelLoading.gameObject.SetActive(false);
        }

        private void SEEventManager_onScenarioLoaded(Data.SEDataScenario scenario)
        {
            Loaded = scenario;
            NodeEditorManager.RequestEditForNode(scenario.RootNode);
        }

        private void SEEventManager_onNowEditingNodeChanged(Scene.SESceneNode node)
        {
            Refresh();
        }

        public override void Refresh()
        {
            if (Loaded != null)
            {
                labelNodeName.text = "Node: " + NodeEditorManager.NowEditing.id;
                labelNodeType.text = "Type: " + NodeEditorManager.Registry.EntryByType[NodeEditorManager.NowEditingType].id;
                labelNodePath.text = "Path: " + NodeEditorManager.NowEditing.GetFullPath();
                labelNodeDesc.text = NodeEditorManager.Registry.EntryByType[NodeEditorManager.NowEditingType].description;
                foreach (var menu in nodeEditorMenus.Values)
                {
                    menu.HideMenu();
                }

                if (nodeEditorMenus.TryGetValue(NodeEditorManager.NowEditingType, out IMenu currentMenu))
                {
                    currentMenu.ShowMenu();
                }
            }
        }

        protected Data.SESceneNodeRegistry nodeRegistry;
        protected Dictionary<Type, IMenu> nodeEditorMenus = new Dictionary<Type, IMenu>();

        protected Text labelLoading;
        protected Text labelNodeName;
        protected Text labelNodeType;
        protected Text labelNodePath;
        protected Text labelNodeDesc;
    }
}
