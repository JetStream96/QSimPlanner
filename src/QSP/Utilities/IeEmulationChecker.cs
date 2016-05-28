using System;
using System.Reflection;
using Microsoft.Win32;

namespace QSP.Utilities
{
    // Does not need admin privilege. Since no admin right is required
    // to read registry key, or write to HKEY_CURRENT_USER.
    //
    public class IeEmulationChecker
    {
        public void Run()
        {
            if (RegKeyExists() == false)
            {
                AddRegistry();
            }
        }

        public void AddRegistry()
        {
            addExeWithIEEmulationMode(
                Assembly.GetEntryAssembly().GetName().Name + ".exe");
        }

        private void addExeWithIEEmulationMode(string exeName)
        {
            // e.g. exeName = "abc.exe"

            // emulationNum:
            // 7000:        IE7 Emulation
            // 8000:        IE8 Emulation
            // 8888:  Force IE8 Emulation
            // 9000:        IE9 Emulation
            // 9999:  Force IE9 Emulation
            // 10000:       IE10 Emulation
            // 10001: Force IE10 Emulation
            // 11000:       IE11 Emulation
            // 11001: Force IE11 Emulation

            int ver = getIEVersion();
            int emulationNum = 0;

            if (ver <= 6)
            {
                // Not supported, no emulation avail.
                throw new Exception(
                    "Versions of IE6 or earlier is not supported.");
            }
            else if (ver == 7)
            {
                emulationNum = 7000;
            }
            else if (ver == 8)
            {
                emulationNum = 8888;
            }
            else if (ver == 9)
            {
                emulationNum = 9999;
            }
            else if (ver == 10)
            {
                emulationNum = 10001;
            }
            else
            {
                //IE 11 or above
                emulationNum = 11001;
            }

            try
            {
                Registry.SetValue(
                    @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Internet Explorer" +
                    @"\MAIN\FeatureControl\FEATURE_BROWSER_EMULATION",
                    exeName,
                    emulationNum);
            }
            catch
            {
                throw new Exception("Failed to add registry.");
            }
        }

        private int getIEVersion()
        {
            try
            {
                string version =
                    Registry.GetValue(
                        @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Internet Explorer",
                        "Version", "").ToString();

                if (version == "")
                {
                    throw new Exception("IE is not installed on the system.");
                }

                string[] a = version.Split('.');
                int v = Convert.ToInt32(a[0]);

                if (v == 9)
                {
                    //IE 9,10,11, ...

                    //IE 9 
                    if (Convert.ToInt32(a[1]) < 10)
                    {
                        return 9;
                    }
                    else
                    {
                        version = Convert.ToString(
                            Registry.GetValue(
                                @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\" +
                                "Internet Explorer",
                                "svcVersion", ""));

                        a = version.Split('.');
                        return Convert.ToInt32(a[0]);
                    }
                }
                else
                {
                    return v;
                }
            }
            catch
            {
                throw new Exception("Failed to get the version of IE.");
            }
        }

        public bool RegKeyExists()
        {
            object defaultVal = new object();

            var keyValue = Registry.GetValue(
                @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Internet Explorer" +
                @"\MAIN\FeatureControl\FEATURE_BROWSER_EMULATION",
                Assembly.GetEntryAssembly().GetName().Name + 
                ".exe", defaultVal);

            return (keyValue != null && keyValue != defaultVal);
        }

#if DEBUG

        public void DebugRun()
        {
            if (DebugRegKeyExists() == false)
            {
                DebugAddRegistry();
            }
        }

        public void DebugAddRegistry()
        {
            addExeWithIEEmulationMode(
                Assembly.GetEntryAssembly().GetName().Name + ".vshost.exe");
        }

        public bool DebugRegKeyExists()
        {
            object defaultVal = new object();

            var keyValue = Registry.GetValue(
                @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Internet Explorer" +
                @"\MAIN\FeatureControl\FEATURE_BROWSER_EMULATION",
                Assembly.GetEntryAssembly().GetName().Name + 
                ".vshost.exe", defaultVal);

            return (keyValue != null && keyValue != defaultVal);
        }

#endif

    }
}
