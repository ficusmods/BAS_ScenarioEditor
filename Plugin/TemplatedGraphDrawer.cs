using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;

using ScenarioEditor.ExtensionMethods;

namespace ScenarioEditor
{
    public class TemplatedGraphDrawer
    {

        public TemplatedGraphDrawer(GameObject templatesRoot)
        {
            this.templatesRoot = templatesRoot;
            templateNodeGroup = templatesRoot.GetChild("NodeGroup");
            templateNode = templatesRoot.GetChild("Node");
            templateNodeChildren = templatesRoot.GetChild("Children");
        }

        public virtual IEnumerator WalkThrough(Scene.SESceneNode startNode, GameObject startObj, Action<Dictionary<Scene.SESceneNode, GameObject>> callback)
        {
            Dictionary<Scene.SESceneNode, GameObject> createdObjects = new Dictionary<Scene.SESceneNode, GameObject>();
            IEnumerable<Scene.SESceneNode> children = startNode.GetChildren();
            if(children != null)
            {
                yield return WalkThrough_Impl(startNode, startObj, createdObjects);
            }
            callback(createdObjects);
        }

        protected virtual IEnumerator WalkThrough_Impl(Scene.SESceneNode currentNode, GameObject currentObj, Dictionary<Scene.SESceneNode, GameObject> createdObjects)
        {
            yield return new WaitForEndOfFrame();
            
            if (currentObj.transform.childCount > 0)
            {
                currentObj.DestroyChildren();
            }

            if (currentNode.GetChildren() != null)
            {
                foreach (var child in currentNode.GetChildren())
                {
                    GameObject childObj = Add(child, currentObj);
                    createdObjects.Add(child, childObj);
                    if (child.GetChildren() != null && child.GetType() != typeof(Scene.SESceneRootNode))
                    {
                        yield return WalkThrough_Impl(child, childObj.GetChild("Children"), createdObjects);
                    }
                }
            }
        }

        public virtual GameObject Add(Scene.SESceneNode node, GameObject obj)
        {
            templatesRoot.SetActive(true);
            GameObject groupObj = GameObject.Instantiate(templateNodeGroup, obj.transform);
            GameObject nodeObj = GameObject.Instantiate(templateNode, groupObj.transform);
            nodeObj.transform.name = "Node";
            GameObject.Instantiate(templateNodeChildren, groupObj.transform).transform.name = "Children";
            nodeObj.GetComponentInChildren<Text>().text = node.id;
            templatesRoot.SetActive(false);
            return groupObj;
        }

        public virtual void Remove(Scene.SESceneNode node, GameObject obj)
        {
            GameObject.Destroy(obj);
        }

        protected GameObject templatesRoot;
        protected GameObject templateNodeGroup;
        protected GameObject templateNode;
        protected GameObject templateNodeChildren;
    }
}
