using NUnit.Framework;

namespace CfgIniNUnitTest
{
    public class FileExistTest
    {
        [Test, Order(0)]
        public void CheckFileExistTest()
        {
            Assert.IsTrue(File.Exists(TestSetup.ConfigFileName));
        }

        [Test, Order(0)]
        public void CheckEmptyFileExistTest()
        {
            Assert.IsTrue(File.Exists(TestSetup.EmptyConfigFileName));
        }
    }
}
