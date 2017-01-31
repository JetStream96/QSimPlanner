using System.IO;
using System.Reflection;

namespace IntegrationTest.Util
{
    public static class CommonUtil
    {
        public static string AssemblyDirectory()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }
    }
}