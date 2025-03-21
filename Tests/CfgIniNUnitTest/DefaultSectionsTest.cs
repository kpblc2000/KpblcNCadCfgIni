﻿using KpblcNCadCfgIni;
using NUnit.Framework;

namespace CfgIniNUnitTest
{
    public class DefaultSectionsTest
    {
        [Test, Order(1)]
        public void CheckUnnamedSection()
        {
            NCadConfig config = new NCadConfig(TestSetup.ConfigFileName);
            Assert.IsNotNull(config.UnnamedSection);
            Assert.AreEqual(config.UnnamedSection.Name, "\\");

            Assert.AreEqual(config.UnnamedSection.Data.Count, 1);

            Assert.AreEqual(config.UnnamedSection.Data[0].Key, "@");
            Assert.AreEqual(config.UnnamedSection.Data[0].Value, "e");
        }

        [Test, Order(2)]
        public void CheckConfigFoldersSection()
        {
            NCadConfig config = new NCadConfig(TestSetup.ConfigFileName);
            Assert.IsNotNull(config.IncludeConfigurationFolders);
            Assert.AreEqual(config.IncludeConfigurationFolders.Name, "IncCfgDirs");
            Assert.AreEqual(config.IncludeConfigurationFolders.Data.Count, 1);

            Assert.AreEqual(config.IncludeConfigurationFolders.Data[0].Key, @"C:/Users/kpblc/AppData/Roaming/Nanosoft/nanoCAD x64 23.1/Config/");
            Assert.AreEqual(config.IncludeConfigurationFolders.Data[0].Value, 0);
        }

        [Test, Order(3)]
        public void CheckEmptyUnnamedSection()
        {
            NCadConfig config = new NCadConfig(TestSetup.EmptyConfigFileName);
            Assert.IsNotNull(config.UnnamedSection);
            Assert.AreEqual(config.UnnamedSection.Name, "\\");

            Assert.AreEqual(config.UnnamedSection.Data.Count, 1);

            Assert.AreEqual(config.UnnamedSection.Data[0].Key, "@");
            Assert.AreEqual(config.UnnamedSection.Data[0].Value, "e");
        }

        [Test, Order(4)]
        public void CheckEmptyConfigFoldersSection()
        {
            NCadConfig config = new NCadConfig(TestSetup.EmptyConfigFileName);
            Assert.IsNotNull(config.IncludeConfigurationFolders);
            Assert.AreEqual(config.IncludeConfigurationFolders.Name, "IncCfgDirs");
            Assert.AreEqual(config.IncludeConfigurationFolders.Data.Count, 1);

            Assert.AreEqual(config.IncludeConfigurationFolders.Data[0].Key, @"C:/Users/razygraevaa/AppData/Roaming/Nanosoft/nanoCAD x64 23.1/Config/");
            Assert.AreEqual(config.IncludeConfigurationFolders.Data[0].Value, 0);
        }
    }
}
