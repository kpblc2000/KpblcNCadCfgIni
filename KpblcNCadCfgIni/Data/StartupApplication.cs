using KpblcNCadCfgIni.Enums;

namespace KpblcNCadCfgIni.Data
{
    public class StartupApplication
    {
        public StartupApplication()
        {
            Enabled = true;
        }
        public string LoaderName { get; set; }
        public AppTypeEnum AppType { get; set; }
        public bool Enabled { get; set; }
        public int LoadOrder { get; set; }
    }
}
