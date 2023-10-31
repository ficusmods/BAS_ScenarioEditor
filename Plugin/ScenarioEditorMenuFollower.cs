using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThunderRoad;
using UnityEngine;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace ScenarioEditor
{
    public class ScenarioEditorMenuFollower : MonoBehaviour
    {
        bool attached = false;

        public void MoveTo(Vector3 pos, Quaternion rotation)
        {
            ScenarioEditorMenuModule.menuInstance.gameObject.transform.position = pos;
            ScenarioEditorMenuModule.menuInstance.gameObject.transform.rotation = rotation;
        }

        public void AttachTo(Transform transform)
        {
            ScenarioEditorMenuModule.menuInstance.gameObject.transform.parent = transform;
        }

        public void Detach()
        {
            ScenarioEditorMenuModule.menuInstance.gameObject.transform.parent = null;
        }

        public void Update()
        {
            if (Config.showScenarioEditorMenu && !attached)
            {
                attached = true;
                if(ScenarioEditorMenuModule.menuInstance != null)
                {
                    ScenarioEditorMenuModule.menuInstance.gameObject.SetActive(true);
                    Transform handTransform = null;
                    if (GameManager.options.uiPointerHand == Side.Right)
                    {
                        handTransform = Player.local.handLeft.transform;
                    }
                    else
                    {
                        handTransform = Player.local.handRight.transform;
                    }
                    MoveTo(handTransform.position + handTransform.forward * 0.2f,
                        Quaternion.LookRotation(handTransform.forward, Vector3.up)
                    );
                    AttachTo(handTransform);
                }

            } else if (!Config.showScenarioEditorMenu && attached)
            {
                attached = false;
                if (ScenarioEditorMenuModule.menuInstance != null)
                {
                    Detach();
                    ScenarioEditorMenuModule.menuInstance.gameObject.SetActive(false);
                }
            }
        }
    }
}
