using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using QSP.Common.Options;
using static QSP.Utilities.LoggerInstance;
using QSP.UI.UserControls;
using QSP.UI.ToLdgModule.TOPerf;
using QSP.UI.ToLdgModule.LandingPerf;
using QSP.UI.ControlStates;

namespace QSP.Updates
{
    // Copy preference and aircraft profiles from old version, 
    // and modify if necessary to make them compatible with new profiles 
    // and/or preference.
    //
    public class PostUpdateAction
    {
        private string backupFolder, newFolder;
        private List<string> errors = new List<string>();

        public PostUpdateAction(string backupFolder, string newFolder)
        {
            this.backupFolder = backupFolder;
            this.newFolder = newFolder;
        }

        public void DoAction()
        {
            SetOption();
            SetStates();
        }

        private void SetOption()
        {
            try
            {
                CopyFile(OptionManager.DefaultPath);
            }
            catch (Exception ex)
            {
                WriteToLog(ex);
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
                WriteToLog(ex);
            }

            // Takeoff page state
            try
            {
                CopyFile(Path.Combine(StateManager.Directory, 
                    TOPerfControl.FileName));
            }
            catch (Exception ex)
            {
                WriteToLog(ex);
            }

            // Landing page state
            try
            {
                CopyFile(Path.Combine(StateManager.Directory,
                    LandingPerfControl.FileName));
            }
            catch (Exception ex)
            {
                WriteToLog(ex);
            }
        }
        
        // relativePath: path relative to its version folder
        private void CopyFile(string relativePath)
        {
            var oldPath = Path.Combine(backupFolder, relativePath);
            var newPath = Path.Combine(newFolder, relativePath);

            if (File.Exists(oldPath))
            {
                File.Copy(oldPath, newPath, true);
            }
        }

        // Only copy the newly added configs.
        private static void SetAircraftConfigs()
        {

        }


    }
}
