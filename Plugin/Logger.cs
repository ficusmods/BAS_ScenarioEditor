using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace ScenarioEditor
{
    public class Logger
    {
        public enum Level
        {
            None = 0,
            Basic = 1,
            Detailed = 2
        }

        public static string ModName { get; protected set; } = "UnnamedMod";
        public static string ModVersion { get; protected set; } = "0.0";
        public static Level LoggerLevel { get; protected set; } = Level.Basic;

        public static void init(string _name, string _version, string _level)
        {
            Logger.ModName = _name;
            Logger.ModVersion = _version;
            Logger.LoggerLevel = Level.Basic;
            if (!Enum.TryParse(_level, out Level level))
            {
                LoggerLevel = level;
            }
        }

        public static void Basic(object msg, params object[] values)
        {
            if (LoggerLevel >= Level.Basic)
                Debug.Log(String.Format($"{ModName} v{ModVersion} (Basic) | " + msg, values));
        }
        public static void Detailed(object msg, params object[] values)
        {
            if (LoggerLevel >= Level.Detailed)
                Debug.Log(String.Format($"{ModName} v{ModVersion} (Detailed) | " + msg, values));
        }
    }
}
