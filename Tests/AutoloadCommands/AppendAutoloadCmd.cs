using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoloadCommands.Infrastructure;
using HostMgd.ApplicationServices;
using Teigha.Runtime;

namespace AutoloadCommands
{
    public static class AppendAutoloadCmd
    {
        [CommandMethod("append-autoload-kpblc")]
        public static void AppendAutoloadByKpblcCommand()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            NanoCadConfig cfg = new NanoCadConfig();
            doc.Editor.WriteMessage($"\nKpblc : {cfg.ConfigFileByKpblc}; {File.Exists(cfg.ConfigFileByKpblc)}");
            doc.Editor.WriteMessage($"\nDrRaz : {cfg.ConfigFileByDrRaz}; {File.Exists(cfg.ConfigFileByDrRaz)}");
        }
    }
}
