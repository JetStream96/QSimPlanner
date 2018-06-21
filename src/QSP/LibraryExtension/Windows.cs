using System.Security.Principal;

namespace QSP.LibraryExtension
{
    public static class Windows
    {
        /// <summary>
        /// Returns whether the current instance is running as admin.
        /// </summary>
        /// <exception cref="Exception"></exception>
        public static bool IsAdministrator()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}
