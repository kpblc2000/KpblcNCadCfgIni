using System.Collections.Generic;

namespace KpblcNCadCfgIni.Data
{
    public class StartupApplicationComparer : IEqualityComparer<StartupApplication>
    {
        public bool Equals(StartupApplication x, StartupApplication y)
        {
            return x.LoaderName.Equals(y.LoaderName, System.StringComparison.InvariantCultureIgnoreCase);
        }

        public int GetHashCode(StartupApplication obj)
        {
            return obj.LoaderName.GetHashCode();
        }
    }
}
