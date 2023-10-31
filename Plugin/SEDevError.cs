using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using ThunderRoad;

namespace ScenarioEditor.DevError
{
    public class MissingObject : Exception
    {
        public MissingObject()
        {
        }

        public MissingObject(string message)
            : base(message)
        {
        }

        public MissingObject(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    public class InvalidNode : Exception
    {
        public static string tip = " Recheck yout scenario data.";
        public InvalidNode()
            : base(tip)
        {
        }

        public InvalidNode(string message)
            : base(message + tip)
        {
        }

        public InvalidNode(string message, Exception inner)
            : base(message + tip, inner)
        {
        }
    }

    public class InvalidMenu : Exception
    {
        public static string tip = " Recheck your Menu prefab and code.";
        public InvalidMenu()
            : base(tip)
        {
        }

        public InvalidMenu(string message)
            : base(message + tip)
        {
        }

        public InvalidMenu(string message, Exception inner)
            : base(message + tip, inner)
        {
        }
    }
}
