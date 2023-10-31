using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using System.Reflection;

using ScenarioEditor.ExtensionMethods;

namespace ScenarioEditor.Menu
{
    public abstract class SEMenu : IMenu
    {
        public virtual void ShowMenu() => Active = true;
        public virtual void HideMenu() => Active = false;
        public virtual void DestroyMenu() => GameObject.Destroy(MenuObj);

        public SEMenu Parent { get; protected set; }
        public GameObject MenuObj { get; protected set; }
        public GameObject Elements { get; protected set; }
        public GameObject SubMenusObj { get; protected set; }
        public List<SEMenu> SubMenus { get; protected set; } = new List<SEMenu>();

        public bool Active
        {
            get => _activePage;
            set
            {
                _activePage = value;
                HideAll();
                TryShow();
            }
        }

        public SEMenu()
        {
        }

        public virtual IEnumerator InitCoroutine(SEMenu parent, GameObject menuObj)
        {
            Parent = parent;
            MenuObj = menuObj;
            if (Parent != null)
            {
                MenuObj.transform.SetParent(Parent.MenuObj.transform);
            }

            MenuObj.transform.localPosition = Vector3.zero;
            MenuObj.transform.localRotation = Quaternion.identity;

            if (!MenuObj.GetChild("Elements", out GameObject elementsObj)) throw new DevError.InvalidMenu("Missing Elements object.");
            Elements = elementsObj;
            if (!MenuObj.GetChild("SubMenus", out GameObject subMenusObj)) throw new DevError.InvalidMenu("Missing SubMenus object.");
            SubMenusObj = subMenusObj;
            MenuObj.SetActive(true);
            Elements.SetActive(false);
            SubMenusObj.SetActive(true);
            yield return new WaitForEndOfFrame();
        }

        public IEnumerator InitCoroutine(GameObject menuObj)
        {
            yield return InitCoroutine(null, menuObj);
        }

        public virtual void Refresh() { }

        protected virtual void TryShow()
        {
            if (Active)
            {
                Elements.SetActive(true);
                Refresh();
            }

            foreach (var menu in SubMenus)
            {
                menu.TryShow();
            }
        }

        protected virtual void HideAll()
        {
            Elements.SetActive(false);
            foreach(var menu in SubMenus)
            {
                menu.HideAll();
            }
        }

        public void SwitchToParent()
        {
            if (Parent != null)
            {
                Active = false;
                Parent.Active = true;
            }
            else
            {
                throw new DevError.InvalidMenu("Menu has no parent to switch to");
            }
        }

        public void SwitchToChild(SEMenu subMenu)
        {
            if (SubMenus.Contains(subMenu))
            {
                Active = false;
                subMenu.Active = true;
            }
            else
            {
                throw new DevError.InvalidMenu("Tried to switch to menu outside the scope of this menu");
            }
        }

        protected bool _activePage = false;
    }
}
