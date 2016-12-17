using Microsoft.Win32;

namespace QSP.Utilities
{
    public static class RegistryUitl
    {
        /// <summary>
        /// If the operation failed, this either returns null or throws an exception.
        /// </summary>
        /// <param name="hKey">E.g. RegistryHive.LocalMachine </param>
        /// <param name="subKeyName">E.g. @"SOFTWARE\Microsoft\Windows NT\CurrentVersion"</param>
        /// <param name="valueName">E.g. "RegisteredOrganization"</param>
        /// <param name="view">E.g. RegistryView.Registry64 </param>
        /// <returns></returns>
        public static string GetRegistry(
            RegistryHive hKey, string subKeyName, string valueName, RegistryView view)
        {
            var localKey = RegistryKey.OpenBaseKey(hKey, view)
                                      .OpenSubKey(subKeyName);

            return (string)localKey?.GetValue(valueName);
        }

        /// <summary>
        /// Same as GetRegistry but always returns null if fails.
        /// </summary>
        public static string TryGetRegistry(
            RegistryHive hKey, string subKeyName, string valueName, RegistryView view)
        {
            try
            {
                return GetRegistry(hKey, subKeyName, valueName, view);
            }
            catch
            {
                return null;
            }
        }
    }
}