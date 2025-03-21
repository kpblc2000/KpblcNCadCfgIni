using HostMgd.ApplicationServices;
using Microsoft.Win32;
using Teigha.DatabaseServices;
using Teigha.Runtime;

namespace AutoloadCommands.Infrastructure
{
    internal class NanoCadConfig
    {
        //public NanoCadConfig()
        //{


        //    Document doc = Application.DocumentManager.MdiActiveDocument;
        //    if (doc != null)
        //    {
        //        dynamic comDoc = doc.AcadDocument;
        //        ConfigFileByDrRaz = Path.Combine(comDoc.Application.CurUserAppData, _cfgFileName);
        //    }
        //}
        public string ConfigFileFullName
        {
            get
            {
                RegistryKey curUserKey = Registry.CurrentUser;
                RegistryKey startupKey = curUserKey.OpenSubKey(HostApplicationServices.Current.UserRegistryProductRootKey);
                string res = Path.Combine(startupKey.GetValue("UserDataDir").ToString(), $@"Config\{_cfgFileName}");
                startupKey.Close();
                return res;
            }
        }
        //public string ConfigFileByDrRaz { get; }

        private readonly string _cfgFileName = "cfg.ini";
        
    }
}
