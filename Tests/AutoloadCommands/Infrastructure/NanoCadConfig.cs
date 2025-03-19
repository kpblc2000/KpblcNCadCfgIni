using HostMgd.ApplicationServices;
using Microsoft.Win32;
using Teigha.DatabaseServices;
using Teigha.Runtime;

namespace AutoloadCommands.Infrastructure
{
    internal class NanoCadConfig
    {
        public NanoCadConfig()
        {
            RegistryKey curUserKey = Registry.CurrentUser;
            RegistryKey startupKey = curUserKey.OpenSubKey(HostApplicationServices.Current.UserRegistryProductRootKey);
            ConfigFileByKpblc = Path.Combine(startupKey.GetValue("UserDataDir").ToString(), $@"Config\{_cfgFileName}");
            startupKey.Close();

            Document doc = Application.DocumentManager.MdiActiveDocument;
            if (doc != null)
            {
                dynamic comDoc = doc.AcadDocument;
                ConfigFileByDrRaz = Path.Combine( comDoc.Application.CurUserAppData, _cfgFileName);
            }
        }
        public string ConfigFileByKpblc { get; }
        public string ConfigFileByDrRaz { get; }

        private readonly string _cfgFileName = "cfg.ini";
    }
}
