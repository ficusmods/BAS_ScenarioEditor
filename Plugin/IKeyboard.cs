using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;

namespace ScenarioEditor
{
    public interface IKeyboard : IBookMenu
    {
        void SetField(InputField field);
    }
    
}
