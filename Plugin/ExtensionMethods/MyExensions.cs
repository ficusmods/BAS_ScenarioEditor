using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace ScenarioEditor.ExtensionMethods
{
    public static class MyExensions
    {
        public static void MoveFirstTo<T>(this LinkedList<T> list1, LinkedList<T> list2)
        {
            var node = list1.First;
            if (node == null) return;
            T val = node.Value;
            list1.Remove(node);
            list2.AddLast(val);
        }

        public static bool GetChild(this GameObject obj, string name, out GameObject outObj)
        {
            outObj = obj?.transform.Find(name)?.gameObject;
            return outObj != null;
        }

        public static GameObject GetChild(this GameObject obj, string name)
        {
            GameObject outObj = obj?.transform.Find(name)?.gameObject;
            return outObj;
        }

        public static void DestroyChildren(this GameObject obj)
        {
            int count = obj.transform.childCount;
            for (int i = 0; i < count; i++)
            {
                GameObject.Destroy(obj.transform.GetChild(i).gameObject);
            }
        }

        public static void SetChildrenActive(this GameObject obj, bool status)
        {
            int count = obj.transform.childCount;
            for (int i = 0; i < count; i++)
            {
                obj.transform.GetChild(i).gameObject.SetActive(status);
            }
        }
    }
}
