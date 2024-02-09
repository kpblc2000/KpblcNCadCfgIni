using NUnit.Framework;
using System.IO;
using System.Linq;

namespace CfgIniNUnitTest
{
    public class FileExistTest
    {
        [Test, Order(0)]
        public void CheckFileExistTest()
        {
            Assert.IsTrue(File.Exists(TestSetup.ConfigFileName));
        }
    }
}
