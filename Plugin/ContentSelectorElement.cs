using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ThunderRoad;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace ScenarioEditor
{
    public class ContentSelectorElement
    {
        public GameObject GObject
        {
            get;
            protected set;
        }

        public Text Label
        {
            get;
            protected set;
        }

        public delegate void OptionSelected(string selected);
        public event OptionSelected onOptionSelected;

        public ContentSelectorElement(GameObject root, GameObject contentObj, GameObject templateObj, Text label)
        {
            GObject = root;
            entryTemplate = templateObj;
            content = contentObj;
            Label = label;
        }

        public void RefreshContent(IEnumerable<string> options)
        {
            entryTemplate.SetActive(true);
            HashSet<string> deletedEntries = gameObjects.Keys.ToHashSet();
            foreach (string option in options)
            {
                deletedEntries.Remove(option);
                if (gameObjects.ContainsKey(option)) continue;

                GameObject currEntry = GameObject.Instantiate(entryTemplate, content.transform);
                gameObjects.Add(option, currEntry);
                Button button = currEntry.GetComponentInChildren<Button>(true);
                Text label = currEntry.GetComponentInChildren<Text>(true);
                label.text = option;
                button.onClick.AddListener(() => { onOptionSelected?.Invoke(option); });
            }

            foreach (string deletedId in deletedEntries)
            {
                GameObject currObj = null;
                if (gameObjects.TryGetValue(deletedId, out currObj))
                {
                    gameObjects.Remove(deletedId);
                    GameObject.Destroy(currObj);
                }
            }
            entryTemplate.SetActive(false);
        }

        public void ReloadContent(IEnumerable<string> options)
        {
            GameObject[] objects = gameObjects.Values.ToArray();
            gameObjects.Clear();
            foreach (GameObject obj in objects)
            {
                GameObject.Destroy(obj);
            }
            RefreshContent(options);
        }

        public void ClearContent()
        {
            ReloadContent(new string[] { });
        }

        protected GameObject entryTemplate;
        protected GameObject content;
        protected Dictionary<string, GameObject> gameObjects = new Dictionary<string, GameObject>();
    }
}
