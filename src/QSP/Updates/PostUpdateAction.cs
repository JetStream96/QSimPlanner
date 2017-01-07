using QSP.AircraftProfiles.Configs;
using QSP.Common.Options;
using QSP.FuelCalculation.FuelData;
using QSP.LandingPerfCalculation;
using QSP.LibraryExtension;
using QSP.TOPerfCalculation;
using QSP.UI.ControlStates;
using QSP.UI.Forms;
using QSP.UI.ToLdgModule.LandingPerf;
using QSP.UI.ToLdgModule.TOPerf;
using QSP.UI.UserControls;
using System;
using System.IO;
using System.Xml.Linq;
using static QSP.Updates.Utilities;
using static QSP.Utilities.LoggerInstance;

namespace QSP.Updates
{
    // Copy preference and custom aircraft profiles from old version.
    //
    public class PostUpdateAction
    {
        private Version backupVersion, newVersion;

        public PostUpdateAction() { }

        // May throw exception.
        public void DoAction()
        {
            var ver = GetVersions();

            // App is never updated. No action needed.
            if (ver.Backup == "") return;

            backupVersion = Version.Parse(ver.Backup);
            newVersion = Version.Parse(ver.Current);

            if (RequireAction())
            {
                SetOption();
                SetStates();
                SetConfigFiles();
                SetUpdateStatus();
            }
        }

        private void SetConfigFiles()
        {
            CopyConfigDirectory(ConfigLoader.CustomFolderPath);
            CopyConfigDirectory(FuelDataLoader.CustomFolderPath);
            CopyConfigDirectory(TOTableLoader.CustomFolderPath);
            CopyConfigDirectory(LdgTableLoader.CustomFolderPath);
        }

        private void CopyConfigDirectory(string dir)
        {
            IOMethods.CopyDirectory(Path.Combine("..", backupVersion.ToString(), dir), dir, true);
        }
        
        private static bool RequireAction()
        {
            var doc = XDocument.Load("updater.xml");
            return doc.Root.Element("PostUpdateActionCompleted").Value == "0";
        }

        private static void SetUpdateStatus()
        {
            var doc = XDocument.Load("updater.xml");
            doc.Root.Element("PostUpdateActionCompleted").Value = "1";
            File.WriteAllText("updater.xml", doc.ToString());
        }

        private void SetOption()
        {
            try
            {
                CopyFile(OptionManager.DefaultPath);
            }
            catch (Exception ex)
            {
                Log(ex);
            }
        }

        // Copies the stats for: fuel page, takeoff page, landing page and the window.
        private void SetStates()
        {
            Action[] actions =
            {
                () => CopyFile(FuelPageState.FileLocation),
                () => CopyFile(Path.Combine(StateManager.Directory, TOPerfControl.FileName)),
                () => CopyFile(Path.Combine(StateManager.Directory, LandingPerfControl.FileName)),
                () => CopyFile(WindowSize.FileLocation)
            };

            foreach (var i in actions)
            {
                try
                {
                    i();
                }
                catch (Exception ex)
                {
                    Log(ex);
                }
            }
        }

        // relativePath: path relative to its version folder
        private void CopyFile(string relativePath)
        {
            var oldPath = Path.Combine(GetFolder(backupVersion), relativePath);
            var newPath = relativePath;

            if (File.Exists(oldPath))
            {
                var newDir = Path.GetDirectoryName(newPath);
                Directory.CreateDirectory(newDir);
                File.Copy(oldPath, newPath, true);
            }
        }
    }
}
