using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace ScenarioEditor
{
    public interface IBookMenu : IMenu
    {
        GameObject GetGameObject();
        void MoveTo(Vector3 pos, Quaternion rotation);
        void AttachTo(Transform transform);
        void Detach();

        void SetSize(Vector2 canvasSize, Vector3 objectScale);
    }
    
}
