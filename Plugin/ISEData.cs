using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScenarioEditor
{
    public interface ISEData
    {
        void RefreshReferences();

        IEnumerator RefreshReferencesCoroutine();
    }
}
