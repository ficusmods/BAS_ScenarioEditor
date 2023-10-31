using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LazyInputField : InputField
{
    public delegate void InputStart();
    public event InputStart onInputStart;

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        onInputStart?.Invoke();
    }
}
