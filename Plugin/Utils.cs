using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

using System.Reflection;
using ThunderRoad;

namespace ScenarioEditor
{
    public class Utils
    {
        public static LinkedList<T> CreateLinkedList<T>(IEnumerator<T> enumerator)
        {
            LinkedList<T> retList = new LinkedList<T>();
            while(enumerator != null)
            {
                retList.AddLast(enumerator.Current);
                enumerator.MoveNext();
            }
            return retList;
        }

        public static bool HasSameParameterList(MethodBase method1, MethodBase method2)
        {
            return method1.GetParameters() == method2.GetParameters();
        }

        public static bool HasSameConstructor(Type t1, Type t2)
        {
            ConstructorInfo c1 = t1?.GetConstructors()?.FirstOrDefault();
            ConstructorInfo c2 = t2?.GetConstructors()?.FirstOrDefault();
            if (c1 == null || c2 == null) return false;
            return HasSameParameterList(c1, c2);
        }

        public static bool IsDefaultConstructible(Type t1)
        {
            return t1.GetConstructor(Type.EmptyTypes) != null;
        }

        public static bool HasMethodWithParams(Type t1, string name, Type[] types)
        {
            return t1.GetMethod(name, types) != null;
        }

        public static string OrEmptyString(string str)
        {
            return str + "";
        }

        public static bool isDungeonLevel(LevelData level)
        {
            var sandbox = level.modes.Find(x => x.name.Equals("Sandbox"));
            if (sandbox != null)
            {
                var areaModule = sandbox.modules.Find(x => x.type == typeof(LevelAreaModule));
                return areaModule != null;
            }
            return false;
        }
    }
}
