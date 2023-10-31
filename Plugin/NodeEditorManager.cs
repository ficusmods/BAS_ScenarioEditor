using ScenarioEditor.Data;
using ScenarioEditor.Scene;
using System;
using ThunderRoad;

namespace ScenarioEditor
{
    public static class NodeEditorManager
    {
        public static Data.SESceneNodeRegistry Registry { get; set; }
        public static SESceneNode NowEditing { get; private set; }
        public static Type NowEditingType { get; private set; }

        public static void RequestEditForNode(Scene.SESceneNode toEdit)
        {
            if (toEdit == null) return;
            SESceneNodeRegistry nodeRegistry = Catalog.GetData<SESceneNodeRegistry>("SceneNodeRegistry");
            if (!nodeRegistry.EntryByType.TryGetValue(toEdit.GetType(), out SERegistryEntrySceneNode entry))
            {
                Logger.Basic("Failed to edit the node {0}. Couldn't find in registry.", toEdit.id);
            }
            else
            {
                NowEditing = toEdit;
                NowEditingType = toEdit.GetType();
                SEEventManager.InvokeNowEditingNodeChanged(toEdit);
            }
        }

        public static Scene.SESceneNode CreateNode(string nodeTypeId, Scene.SESceneNode parent)
        {
            SESceneNodeRegistry nodeRegistry = Catalog.GetData<SESceneNodeRegistry>("SceneNodeRegistry");
            if (!nodeRegistry.EntryByTypeName.TryGetValue(nodeTypeId, out SERegistryEntrySceneNode entry))
            {
                Logger.Basic("Failed creating {0} node for parent {1}", nodeTypeId, parent?.id);
            }
            else
            {
                return CreateNode(entry, parent);
            }
            return null;
        }

        public static Scene.SESceneNode CreateNode(Type nodeType, Scene.SESceneNode parent)
        {
            SESceneNodeRegistry nodeRegistry = Catalog.GetData<SESceneNodeRegistry>("SceneNodeRegistry");
            if (!nodeRegistry.EntryByType.TryGetValue(nodeType, out SERegistryEntrySceneNode entry))
            {
                Logger.Basic("Failed creating {0} node for parent {1}", nodeType.Name, parent?.id);
            }
            else
            {
                return CreateNode(entry, parent);
            }
            return null;
        }

        private static Scene.SESceneNode CreateNode(SERegistryEntrySceneNode nodeEntry, Scene.SESceneNode parent)
        {
            var node = (Scene.SESceneNode)Activator.CreateInstance(nodeEntry.nodeType);
            node.id = nodeEntry.id;
            node.Init(parent);
            node.RefreshReferences();
            SEEventManager.InvokeNodeCreated(node);
            return node;
        }

        public static Scene.SESceneNode RemoveNode(Scene.SESceneNode node)
        {
            if (node == null) return null;
            if (node.Parent == null) return null;

            Scene.SESceneNode removed = node.Parent.RemoveChild(node.id);
            SEEventManager.InvokeNodeRemoved(node);
            return removed;
        }

    }
}
