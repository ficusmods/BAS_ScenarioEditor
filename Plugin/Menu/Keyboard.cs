using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;

using ScenarioEditor.ExtensionMethods;

namespace ScenarioEditor.Menu
{
    public class Keyboard : SEMenu, IKeyboard
    {
        protected class KBButton
        {
            public GameObject obj;
            public Button button;
            public Text label;
            public bool isSymbol = true;
            public string symbol;
        }

        public override IEnumerator InitCoroutine(SEMenu parent, GameObject bookObj)
        {
            this.bookObj = bookObj;
            GameObject menuObj = bookObj.GetChild("Menu");
            yield return base.InitCoroutine(null, menuObj);

            colliderObj = bookObj.GetChild("Mesh");

            bookObj.SetActive(true);
            colliderObj.SetActive(false);

            _activePage = true;

            InitKeys();

            KeyboardManager.RegisterKeyboard(this);
        }

        protected void InitKeys()
        {
            buttonMap.Clear();
            GameObject keyboardRoot = Elements.GetChild("Keyboard");

            var keys = new Dictionary<string, string[]>
            {
                {"Numbers/0123456789", new string[]{ "0","1","2","3","4","5","6","7","8","9" } },
                {"Letters/qwertyuiop", new string[]{ "q","w","e","r","t","y","u","i","o","p" } },
                {"Letters/asdfghjkl", new string[]{ "a","s","d","f","g","h","j","k","l" } },
                {"Letters/zxcvbnm", new string[]{ "z","x","c","v","b","n","m" } },
                { "Symbols", new string[] { "_", "." } },
                { "Control", new string[] { "uppercase", "space", "backspace" } }
            };

            foreach(var entry in keys)
            {
                foreach (var input in entry.Value)
                {
                    string path = string.Format("{0}/{1}", entry.Key, input);
                    GameObject buttonRoot = keyboardRoot.GetChild(path);
                    Button button = buttonRoot.GetComponentInChildren<Button>();
                    buttonMap.Add(input, new KBButton
                    {
                        obj = buttonRoot,
                        button = button,
                        label = button.GetComponentInChildren<Text>(),
                        symbol = input
                    });

                    if(entry.Key != "Control")
                    {
                        buttonMap[input].isSymbol = true;
                        buttonMap[input].button.onClick.AddListener(delegate
                        { 
                            TypeChar(buttonMap[input].symbol);
                        });
                    }
                    else
                    {
                        buttonMap[input].isSymbol = false;
                    }
                }
            }

            buttonMap["space"].button.onClick.AddListener(delegate
            {
                TypeChar(" ");
            });

            buttonMap["uppercase"].button.onClick.AddListener(delegate
            {
                SwitchCase();
            });

            buttonMap["backspace"].button.onClick.AddListener(delegate
            {
                Backspace();
            });
        }

        protected void TypeChar(string character)
        {
            if(activeField != null)
            {
                activeField.text += character;
            }
        }

        protected void Backspace()
        {
            if (activeField != null && activeField.text.Length > 0)
            {
                string newStr = activeField.text.Substring(0, activeField.text.Length - 1);
                activeField.text = newStr;
            }
        }

        protected void SwitchCase()
        {
            isUpperCase = !isUpperCase;
            foreach(var entry in buttonMap.Values)
            {
                if(entry.isSymbol)
                {
                    if (isUpperCase)
                    {
                        entry.symbol = entry.symbol.ToUpper();
                    }
                    else
                    {
                        entry.symbol = entry.symbol.ToLower();
                    }
                    entry.label.text = entry.symbol;
                }
            }
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

        public void Detach()
        {
            bookObj.transform.parent = null;
        }
        
        public GameObject GetGameObject()
        {
            return bookObj;
        }

        public void SetSize(Vector2 canvasSize, Vector3 objectScale)
        {
            RectTransform rect = MenuObj.GetComponent<RectTransform>();
            rect.sizeDelta = canvasSize;
            colliderObj.transform.localScale = new Vector3(canvasSize.x, canvasSize.y, 2.0f);
            bookObj.transform.localScale = objectScale;
            rect.ForceUpdateRectTransforms();
        }

        public override void ShowMenu()
        {
            colliderObj.SetActive(true);
            MenuObj.SetActive(true);
            HideAll();
            TryShow();
        }

        public override void HideMenu()
        {
            colliderObj.SetActive(false);
            MenuObj.SetActive(false);
            HideAll();
        }

        public override void DestroyMenu()
        {
            GameObject.Destroy(bookObj);
        }

        public void SetField(InputField field)
        {
            activeField = field;
        }

        protected GameObject bookObj;
        protected GameObject colliderObj;

        protected InputField activeField;

        protected Dictionary<string, KBButton> buttonMap = new Dictionary<string, KBButton>();
        bool isUpperCase = false;
    }
}
