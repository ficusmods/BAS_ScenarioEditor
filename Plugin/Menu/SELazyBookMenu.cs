using ScenarioEditor.Scene;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

using ScenarioEditor.ExtensionMethods;

namespace ScenarioEditor.Menu
{
    public abstract class SELazyBookMenu : SELazyMenu, IBookMenu
    {
        public override IEnumerator InitCoroutine(SEMenu parent, GameObject bookObj)
        {
            this.bookObj = bookObj;
            GameObject menuObj = bookObj.GetChild("Menu");
            yield return base.InitCoroutine(null, menuObj);

            colliderObj = bookObj.GetChild("Mesh");

            bookObj.SetActive(true);
            colliderObj.SetActive(false);

            _activePage = true;
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

        protected GameObject bookObj;
        protected GameObject colliderObj;
    }
}
