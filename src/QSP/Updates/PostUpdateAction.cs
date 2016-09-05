using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using QSP.Common.Options;
using static QSP.Utilities.LoggerInstance;

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

        }

        private void SetOption()
        {
            try
            {
                var optionPath = OptionManager.DefaultPath;
                var oldPath = Path.Combine(backupFolder, optionPath);
                var newPath = Path.Combine(newFolder, optionPath);

                if (File.Exists(oldPath))
                {
                    File.Copy(oldPath, newPath, true);
                }
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

        }

        // Only copy the newly added configs.
        private static void SetAircraftConfigs()
        {

        }


    }
}
