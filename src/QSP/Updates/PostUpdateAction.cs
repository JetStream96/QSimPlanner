using QSP.AircraftProfiles.Configs;
using QSP.Common.Options;
using QSP.FuelCalculation.FuelData;
using QSP.LandingPerfCalculation;
using QSP.LibraryExtension;
using QSP.TOPerfCalculation;
using QSP.UI.Models;
using QSP.UI.Models.TakeoffLanding;
using QSP.UI.UserControls;
using QSP.UI.UserControls.TakeoffLanding.LandingPerf;
using QSP.UI.UserControls.TakeoffLanding.TOPerf;
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
        private Version backupVersion;

        public PostUpdateAction() { }

        /// <exception cref="Exception"></exception>
        public void DoAction()
        {
            var ver = GetVersions();

            // App is never updated. No action needed.
            if (ver.Backup == "") return;

            backupVersion = Version.Parse(ver.Backup);

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
            CopyCustomConfigs(ConfigLoader.CustomFolderPath);
            CopyCustomConfigs(FuelDataLoader.CustomFolderPath);
            CopyCustomConfigs(TOTableLoader.CustomFolderPath);
            CopyCustomConfigs(LdgTableLoader.CustomFolderPath);
            CopyFile(DeletedDefaultAc.DeletedAcFileName);
        }

        private void CopyCustomConfigs(string dir)
        {
            IOMethods.CopyFilesInDirectory(
                Path.Combine("..", backupVersion.ToString(), dir),
                dir,
                true);
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
                () => CopyFile(Path.Combine(ViewStateSaver.Directory, TOPerfControl.FileName)),
                () => CopyFile(Path.Combine(ViewStateSaver.Directory, LandingPerfControl.FileName)),
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
