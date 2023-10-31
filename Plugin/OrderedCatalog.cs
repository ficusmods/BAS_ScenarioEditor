using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

using ThunderRoad;

namespace ScenarioEditor
{
    public class OrderedCatalog
    {
        internal OrderedCatalog() { }
        public static List<T> GetDataList_IDOrder<T>() where T : CatalogData
        {
            List<T> data = Catalog.GetDataList<T>();
            data.Sort(delegate (T data1, T data2) {
                return String.Compare(data1.id, data2.id, StringComparison.Ordinal);
            });
            return data;
        }
        public static List<CatalogData> GetDataList_IDOrder(Category category)
        {
            List<CatalogData> data = Catalog.GetDataList(category);
            data.Sort(delegate (CatalogData data1, CatalogData data2) {
                return String.Compare(data1.id, data2.id, StringComparison.Ordinal);
            });
            return data;
        }
    }
}
