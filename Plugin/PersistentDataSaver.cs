using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using ThunderRoad;
using UnityEngine;
using Newtonsoft.Json;

namespace ScenarioEditor
{
    public static class PersistentDataSaver
    {
        public static void Save(Data.SECatalogData data)
        {
            if (string.IsNullOrEmpty(data.filePath))
            {
                /* New data was created */
                string dirPath = FileManager.GetFullPath(FileManager.Type.JSONCatalog, FileManager.Source.Mods, string.Format("ScenarioEditor/{0}", data.saveFolder));
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }
                string fullPath = string.Format("{0}/{1}.json", dirPath, data.id);
                File.WriteAllText(fullPath, JsonConvert.SerializeObject(data, typeof(CatalogData), Catalog.jsonSerializerSettings));
                Logger.Basic("Persistent data {0} saved to <Mod folder>/ScenarioEditor/{1}", data.id, data.saveFolder);
            }
            else
            {
                File.WriteAllText(data.filePath, JsonConvert.SerializeObject(data, typeof(CatalogData), Catalog.jsonSerializerSettings));
                Logger.Basic("Persistent data {0} saved to {1}", data.id, data.filePath);
            }
        }
        public static void Delete(Data.SECatalogData data)
        {
            if (!string.IsNullOrEmpty(data.filePath))
            {
                string dirPath = FileManager.GetFullPath(FileManager.Type.JSONCatalog, FileManager.Source.Mods, string.Format("ScenarioEditor/{0}", data.saveFolder));
                string fullPath = string.Format("{0}/{1}.json", dirPath, data.id);
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    Logger.Basic("Persistent data {0} deleted from {1}", data.id, fullPath);
                }
            }
        }
    }
}
