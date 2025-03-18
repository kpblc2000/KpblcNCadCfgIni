using KpblcNCadCfgIni.Data;
using KpblcNCadCfgIni;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KpblcNCadCfgIni.Enums;

namespace CfgIniNUnitTest
{
    public class ConfigurationTest
    {
        [Test, Order(0)]
        public void CheckConfigRangeAndBase()
        {
            NCadConfig config = new NCadConfig(TestSetup.ConfigFileName);
            Assert.IsNotNull(config.ConfigurationList);
            Assert.AreEqual(config.ConfigurationList.Count, 3);

            NCadConfiguration cadConfig = config.ConfigurationList.Find(o => o.ConfigurationName == "");
            Assert.IsNotNull(cadConfig);
            Assert.AreEqual(cadConfig.CfgFileName, "nanoCAD.cfg");
            Assert.AreEqual(cadConfig.PgpFileName, "nCad.pgp");
            Assert.AreEqual(cadConfig.Plat, "nPlatComp");

            cadConfig = config.ConfigurationList.Find(o => o.ConfigurationName == "Mech");
            Assert.IsNotNull(cadConfig);
            Assert.AreEqual(cadConfig.CfgFileName, "Mech.cfg");
            Assert.AreEqual(cadConfig.PgpFileName, "Mech.pgp");
            Assert.AreEqual(cadConfig.Plat, "nMechComp");

            cadConfig = config.ConfigurationList.Find(o => o.ConfigurationName == "SPDS");
            Assert.IsNotNull(cadConfig);
            Assert.AreEqual(cadConfig.CfgFileName, "nanoCAD.cfg");
            Assert.AreEqual(cadConfig.PgpFileName, "nCad.pgp");
            Assert.AreEqual(cadConfig.Plat, "nSPDSComp");
        }

        [Test, Order(1)]
        public void CheckStartupApplications()
        {
            NCadConfig config = new NCadConfig(TestSetup.ConfigFileName);
            Assert.IsNotNull(config.ConfigurationList);
            Assert.AreEqual(config.ConfigurationList.Count, 3);

            NCadConfiguration cadConfig = config.ConfigurationList.Find(o => o.ConfigurationName == "");
            Assert.IsNotNull(cadConfig);
            Assert.AreEqual(cadConfig.StartupApplicationList.Count, 1);

            cadConfig = config.ConfigurationList.Find(o => o.ConfigurationName == "Mech");
            Assert.IsNotNull(cadConfig);
            Assert.AreEqual(cadConfig.StartupApplicationList.Count, 1);

            cadConfig = config.ConfigurationList.Find(o => o.ConfigurationName == "SPDS");
            Assert.IsNotNull(cadConfig);
            Assert.AreEqual(cadConfig.StartupApplicationList.Count, 2);
        }

        [Test, Order(2)]
        public void AddStartupApplication()
        {
            NCadConfig start = new NCadConfig(TestSetup.ConfigFileName);

            NCadConfiguration config = start.ConfigurationList
                .FirstOrDefault(o => string.IsNullOrWhiteSpace(o.ConfigurationName));
            int startRange = config.StartupApplicationList.Count();

            StartupApplication app = new StartupApplication()
            {
                LoadOrder = 0,
                LoaderName = @"c:\1\2.dll",
                AppType = AppTypeEnum.MGD
            }
                ;
            config.StartupApplicationList.Add(app);
            start.Save();

            NCadConfig reader = new NCadConfig(TestSetup.ConfigFileName);
            Assert.AreEqual(reader.ConfigurationList
                .FirstOrDefault(o => string.IsNullOrWhiteSpace(o.ConfigurationName)).StartupApplicationList.Count,
                startRange + 1);
        }

        [Test, Order(3)]
        public void RemoveStartupApplication()
        {
            NCadConfig start = new NCadConfig(TestSetup.ConfigFileName);

            NCadConfiguration config = start.ConfigurationList
                .FirstOrDefault(o => string.IsNullOrWhiteSpace(o.ConfigurationName));

            config.StartupApplicationList.Clear();
            start.Save();

            NCadConfig reader = new NCadConfig(TestSetup.ConfigFileName);
            Assert.AreEqual(reader.ConfigurationList
                .FirstOrDefault(o => string.IsNullOrWhiteSpace(o.ConfigurationName)).StartupApplicationList.Count,
                0);
        }
    }
}
