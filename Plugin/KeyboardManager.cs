using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;

using ScenarioEditor.Scene;
using ThunderRoad;

namespace ScenarioEditor
{
    public static class KeyboardManager
    {
        public static void RequestKeyboard(InputField field)
        {
            keyboard?.SetField(field);
        }

        public static void RegisterKeyboard(IKeyboard keyboard)
        {
            KeyboardManager.keyboard = keyboard;
        }

        static IKeyboard keyboard;
    }
}
