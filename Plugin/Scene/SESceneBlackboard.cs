using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using ThunderRoad;

namespace ScenarioEditor.Scene
{
    public class SESceneBlackboard : ThunderRoad.AI.Blackboard
    {
        public Dictionary<int, SESceneSignal> Signals { get; protected set; } = new Dictionary<int, SESceneSignal>();

        public SESceneBlackboard()
        {
            dictionaries.Add(Signals);
        }

        public bool Find<T>(string id, out T outData)
        {
            outData = Find<T>(id);
            if (outData == null) return false;
            return true;
        }

        public bool SignalActive(string id)
        {
            if (id != null && Signals.TryGetValue(id.GetHashCode(), out SESceneSignal signal))
            {
                return signal.state;
            }

            return false;
        }

        public void SetSignalLevel(string id, bool high)
        {
            if (id != null)
            {
                Signals[id.GetHashCode()] = new SESceneSignal { state = high };
            }
        }

        public void ResetSignals()
        {
            Signals.Clear();
        }

        public void Clean()
        {
            foreach(var dictionary in dictionaries)
            {
                dictionary.Clear();
            }
            dictionaries.Clear();
        }
    }
}
