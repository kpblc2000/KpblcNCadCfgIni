namespace CfgIniNUnitTest
{
    public static class TestSetup
    {
        public static string ConfigFileName =>
            Path.Combine(Path.GetDirectoryName(typeof(TestSetup).Assembly.Location), "cfg.ini");

        public static string EmptyConfigFileName =>
            Path.Combine(Path.GetDirectoryName(typeof(TestSetup).Assembly.Location), "cfg_empty.ini");
    }
}
