using QSP.Common.Options;
using QSP.UI.ControlStates;
using QSP.UI.ToLdgModule.LandingPerf;
using QSP.UI.ToLdgModule.TOPerf;
using QSP.UI.UserControls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Windows.Forms;
using QSP.AircraftProfiles.Configs;
using QSP.FuelCalculation.FuelData;
using QSP.LandingPerfCalculation;
using QSP.LibraryExtension;
using QSP.TOPerfCalculation;
using static QSP.Updates.Utilities;
using static QSP.Utilities.LoggerInstance;

namespace QSP.Updates
{
    // Copy preference and custom aircraft profiles from old version.
    //
    public class PostUpdateAction
    {
        private List<string> errors = new List<string>();
        private Version backupVersion, newVersion;

        public PostUpdateAction() { }

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

        private static DialogResult ShowError(string message)
        {
            return MessageBox.Show(message, "Update error",
                MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
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
                errors.Add("Cannot copy old option file for the new version." +
                    " Please configure the options manually.");
            }
        }

        private void SetStates()
        {
            // Fuel page state
            try
            {
                CopyFile(FuelPageState.FileLocation);
            }
            catch (Exception ex)
            {
                Log(ex);
            }

            // Takeoff page state
            try
            {
                CopyFile(Path.Combine(StateManager.Directory, TOPerfControl.FileName));
            }
            catch (Exception ex)
            {
                Log(ex);
            }

            // Landing page state
            try
            {
                CopyFile(Path.Combine(StateManager.Directory, LandingPerfControl.FileName));
            }
            catch (Exception ex)
            {
                Log(ex);
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
