using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace ScenarioEditor
{
    public interface IMenu
    {
        IEnumerator InitCoroutine(GameObject menuObj);
        void ShowMenu();
        void HideMenu();
        void DestroyMenu();
    }
}
