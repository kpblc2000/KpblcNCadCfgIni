using AutoloadCommands.Infrastructure;
using HostMgd.ApplicationServices;
using HostMgd.EditorInput;
using KpblcNCadCfgIni;
using KpblcNCadCfgIni.Data;
using KpblcNCadCfgIni.Enums;
using System.ComponentModel;
using Teigha.Runtime;

namespace AutoloadCommands
{
    public static class AppendAutoloadCmd
    {
        [CommandMethod("-autoload-add")]
        [Description("Дополнение автозагрузки. Тип приложения определяется по расширению выбранного файла. Наличие файла не проверяется. Дубликаты игнорируются. Приложение добавляется в конец общего списка")]
        public static void AppendAutoloadCommand()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null)
            {
                return;
            }

            Editor ed = doc.Editor;

            PromptStringOptions appNameOptions = new PromptStringOptions("\nВведите полный путь к добавляемому приложению");
            appNameOptions.AllowSpaces = true;
            PromptResult appNameResult = ed.GetString(appNameOptions);

            if (appNameResult.Status != PromptStatus.OK)
            {
                return;
            }

            StartupApplication startupApp = CreateStartupApplication(appNameResult.StringResult);
            if (startupApp == null)
            {
                ed.WriteMessage("\nНе удалось определить тип приложения. Выход");
                return;
            }

            NanoCadConfig cfg = new NanoCadConfig();
            NCadConfig ncConfig = new NCadConfig(cfg.ConfigFileFullName);
            string profileName = Application.GetSystemVariable("cprofile").ToString();
            NCadConfiguration? curProfileData = ncConfig.ConfigurationList.FirstOrDefault(x => x.ConfigurationName == profileName);

            if (curProfileData == null)
            {
                curProfileData = new NCadConfiguration()
                {
                    ConfigurationName = profileName,
                    StartupApplicationList = new List<StartupApplication>(),
                };
            }

            curProfileData.StartupApplicationList.Add(startupApp);
            ncConfig.Save();
        }

        private static StartupApplication CreateStartupApplication(string appFullFileName)
        {
            StartupApplication startupApp = new StartupApplication()
            {
                Enabled = true,
                LoaderName = appFullFileName,
            };

            if (Path.GetExtension(appFullFileName).Equals(".lsp", StringComparison.InvariantCultureIgnoreCase))
            {
                startupApp.AppType = AppTypeEnum.LISP;
            }
            else if (Path.GetExtension(appFullFileName).Equals(".dll", StringComparison.InvariantCultureIgnoreCase))
            {
                startupApp.AppType = AppTypeEnum.MGD;
            }
            else if (Path.GetExtension(appFullFileName).Equals(".package", StringComparison.InvariantCultureIgnoreCase))
            {
                startupApp.AppType = AppTypeEnum.APP_PACKAGE;
            }
            else if (Path.GetExtension(appFullFileName).Equals(".nrx", StringComparison.InvariantCultureIgnoreCase))
            {
                startupApp.AppType = AppTypeEnum.NRX;
            }

            if (startupApp.AppType != AppTypeEnum.Unknown)
            {
                return startupApp;
            }

            return null;
        }
    }
}
