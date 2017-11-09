using System.Reflection;

namespace QSP.Utilities
{
    public static class Version
    {

        public static string AppProductVersion()
        {
            var ver = Assembly.GetEntryAssembly().GetName().Version;
            return $"{ver.Major}.{ver.Minor}.{ver.Build}";
        }
    }
}
