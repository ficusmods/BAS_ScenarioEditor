using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using ThunderRoad;

using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;

using ScenarioEditor.ExtensionMethods;

namespace ScenarioEditor.Menu
{
    public abstract class SELazyMenu : SEMenu
    {
        protected class ShotTimer
        {
            public float timeBetweenShots = 1.0f;
            public float lastShot = 0.0f;
        }

        static GameObject prefabObject;

        public static IEnumerator NewPrefabCoroutine(GameObject parent, Action<GameObject> callback)
        {
            if (prefabObject == null)
            {
                AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(Config.LazyMenuPrefabLocation);
                yield return handle;

                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    Logger.Basic("Loaded LazyMenu prefab from {0}", Config.LazyMenuPrefabLocation);
                    prefabObject = handle.Result;
                }
            }

            GameObject obj = GameObject.Instantiate(prefabObject, parent.transform);
            callback(obj);
        }

        public static IEnumerator MenuFromPrefabCoroutine<T>(string name, SEMenu parentMenu, GameObject parentObj, Action<T> callback) where T: SELazyMenu
        {
            if (!Utils.IsDefaultConstructible(typeof(T))) throw new DevError.InvalidMenu("Menu isn't default constructible");
            GameObject prefabObj = null;

            yield return SELazyMenu.NewPrefabCoroutine(parentObj, (obj) => {
                prefabObj = obj;
            });
            prefabObj.SetActive(true);
            prefabObj.transform.SetParent(parentObj.transform, false);
            T menu = (T)Activator.CreateInstance(typeof(T));
            yield return menu.InitCoroutine(parentMenu, prefabObj);
            callback(menu);
        }

        public override IEnumerator InitCoroutine(SEMenu parent, GameObject menuObj)
        {
            yield return base.InitCoroutine(parent, menuObj);

            templatesRoot = MenuObj.GetChild("Templates");
            tHorizontalGroup = templatesRoot.GetChild("HorizontalGroup");
            tVerticalGroup = templatesRoot.GetChild("VerticalGroup");
            tLabel = templatesRoot.GetChild("Label");
            tButton = templatesRoot.GetChild("Button");
            tInputField = templatesRoot.GetChild("InputField");
            tToggle = templatesRoot.GetChild("Toggle");
            tDropdown = templatesRoot.GetChild("Dropdown");
            tSelector = templatesRoot.GetChild("Selector");
            scrollContentArea = Elements.GetChild("Scroll View/Viewport/Content");

            CurrentGroup = scrollContentArea.GetComponent<HorizontalOrVerticalLayoutGroup>();

            StartMenu();
            yield return InitLazyMenuCoroutine();
            EndMenu();
        }

        protected abstract IEnumerator InitLazyMenuCoroutine();

        protected void SetElementsBoundary(RectTransform.Edge edge, float offset)
        {
            SetTransformBoundary(Elements.GetComponent<RectTransform>(), edge, offset);
        }

        protected void SetSubmenusBoundary(RectTransform.Edge edge, float offset)
        {
            SetTransformBoundary(SubMenusObj.GetComponent<RectTransform>(), edge, offset);
        }

        protected void SetTransformBoundary(RectTransform rectTransform, RectTransform.Edge edge, float offset)
        {
            switch (edge)
            {
                case RectTransform.Edge.Bottom:
                    rectTransform.anchorMin = new Vector2(rectTransform.anchorMin.x, offset);
                    break;
                case RectTransform.Edge.Left:
                    rectTransform.anchorMin = new Vector2(offset, rectTransform.anchorMin.y);
                    break;
                case RectTransform.Edge.Top:
                    rectTransform.anchorMax = new Vector2(rectTransform.anchorMax.x, 1.0f - offset);
                    break;
                case RectTransform.Edge.Right:
                    rectTransform.anchorMax = new Vector2(1.0f - offset, rectTransform.anchorMax.y);
                    break;
            }
        }

        void StartMenu()
        {
            templatesRoot.SetActive(true);
        }

        void EndMenu()
        {
            templatesRoot.SetActive(false);
        }

        protected virtual HorizontalOrVerticalLayoutGroup StartHorizontalGroup()
        {
            return CurrentGroup = CreateSubElem<HorizontalLayoutGroup>(tHorizontalGroup).GetComponent<HorizontalLayoutGroup>();
        }

        protected virtual HorizontalOrVerticalLayoutGroup StartVerticalGroup()
        {
            return CurrentGroup = CreateSubElem<VerticalLayoutGroup>(tVerticalGroup).GetComponent<VerticalLayoutGroup>();
        }

        protected virtual void EndGroup()
        {
            HorizontalOrVerticalLayoutGroup parentGroup = CurrentGroup.transform.parent.GetComponent<HorizontalOrVerticalLayoutGroup>();
            if(parentGroup == null)
            {
                throw new DevError.InvalidMenu("Trying to end non-existent group");
            }
            CurrentGroup = parentGroup;
        }

        protected virtual void EndHorizontalGroup()
        {
            if (!CurrentGroup.GetType().IsAssignableFrom(typeof(HorizontalLayoutGroup)))
                throw new DevError.InvalidMenu("Wrong layout settings. Trying to end a horizontal group when a vertical was started");
            EndGroup();
        }

        protected virtual void EndVerticalGroup()
        {
            if (!CurrentGroup.GetType().IsAssignableFrom(typeof(VerticalLayoutGroup)))
                throw new DevError.InvalidMenu("Wrong layout settings. Trying to end a vertical group when a horizontal was started");
            EndGroup();
        }

        protected virtual T CreateSubElem<T>(GameObject template) where T:MonoBehaviour
        {
            T ret = GameObject.Instantiate(template, CurrentGroup.transform).GetComponent<T>();
            return ret;
        }

        protected virtual Text AddLabel()
        {
            return CreateSubElem<Text>(tLabel);
        }

        protected virtual Text AddLabel(string label)
        {
            Text labelComp = AddLabel();
            labelComp.text = label;
            return labelComp;
        }

        protected virtual Button AddButton()
        {
            Button button =  CreateSubElem<Button>(tButton);
            buttonShotTimers.Add(button, new ShotTimer());
            return button;
        }

        protected virtual Button AddButton(string label, System.Action callback, float timeBetweenShots = 1.0f)
        {
            Button button = AddButton();
            buttonShotTimers[button].timeBetweenShots = timeBetweenShots;
            button.GetComponentInChildren<Text>().text = label;
            button.onClick.AddListener( delegate {
                float dt = Time.time - buttonShotTimers[button].lastShot;
                if(dt > buttonShotTimers[button].timeBetweenShots)
                {
                    buttonShotTimers[button].lastShot = Time.time;
                    callback();
                }
            });
            return button;
        }

        protected virtual InputField AddInputField()
        {
            LazyInputField field = CreateSubElem<LazyInputField>(tInputField);

            field.onInputStart += delegate {
                KeyboardManager.RequestKeyboard(field);
            };

            return field;
        }

        protected virtual InputField AddInputField(string placeHolder, Action<string> callback)
        {
            InputField field = AddInputField();
            field.placeholder.GetComponent<Text>().text = placeHolder;
            field.onValueChanged.AddListener(new UnityEngine.Events.UnityAction<string>(callback));
            return field;
        }

        protected virtual Toggle AddToggle()
        {
            return CreateSubElem<Toggle>(tToggle);
        }
        protected virtual Toggle AddToggle(string label, Action<bool> callback)
        {
            Toggle toggle = AddToggle();
            toggle.GetComponentInChildren<Text>().text = label;
            toggle.onValueChanged.AddListener(new UnityEngine.Events.UnityAction<bool>(callback));
            return toggle;
        }

        protected virtual Dropdown AddDropdown()
        {
            return CreateSubElem<Dropdown>(tDropdown);
        }

        protected virtual Dropdown AddDropdown(List<Dropdown.OptionData> options, Action<int> callback)
        {
            Dropdown dropdown = AddDropdown();
            dropdown.options = options;
            dropdown.onValueChanged.AddListener(new UnityEngine.Events.UnityAction<int>(callback));
            return dropdown;
        }

        protected virtual ContentSelectorElement AddSelector(string label)
        {
            GameObject root =  GameObject.Instantiate(tSelector, CurrentGroup.transform);
            GameObject contentObj = root.GetChild("Scroll View/Viewport/Content");
            GameObject templateObj = root.GetChild("EntryTemplate");
            Text labelComp = root.GetChild("LabelDescription").GetComponent<Text>();
            labelComp.text = label;
            return new ContentSelectorElement(root, contentObj, templateObj, labelComp);
        }
        protected virtual IEnumerator AddSubMenuCoroutine<TMenu>(GameObject menuObj, Action<TMenu> callback) where TMenu : SEMenu
        {
            if (!Utils.IsDefaultConstructible(typeof(TMenu))) throw new DevError.InvalidMenu("Menu isn't default constructible");
            menuObj.transform.SetParent(SubMenusObj.transform, false);
            TMenu menu = (TMenu)Activator.CreateInstance(typeof(TMenu));
            yield return menu.InitCoroutine(this, menuObj);
            SubMenus.Add(menu);
            callback(menu);
        }

        protected virtual IEnumerator AddLazySubMenuCoroutine<TMenu>(Action<TMenu> callback) where TMenu : SELazyMenu
        {
            GameObject loaded = null;
            yield return SELazyMenu.NewPrefabCoroutine(SubMenusObj, (lazyPrefabObj) => {
                loaded = lazyPrefabObj;
            });

            yield return AddSubMenuCoroutine<TMenu>(loaded, (menu) => callback(menu));
        }

        protected GameObject tHorizontalGroup { get; set; }
        protected GameObject tVerticalGroup { get; set; }
        protected GameObject tLabel { get; set; }
        protected GameObject tButton { get; set; }
        protected GameObject tInputField { get; set; }
        protected GameObject tToggle { get; set; }
        protected GameObject tDropdown { get; set; }
        protected GameObject tSelector { get; set; }
        protected GameObject scrollContentArea { get; set; }

        protected GameObject templatesRoot { get; set; }
        protected HorizontalOrVerticalLayoutGroup CurrentGroup { get; set; }

        protected Dictionary<Button, ShotTimer> buttonShotTimers = new Dictionary<Button, ShotTimer>();
    }
}
