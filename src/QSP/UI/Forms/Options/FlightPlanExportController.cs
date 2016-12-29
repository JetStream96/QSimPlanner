using QSP.Common.Options;
using QSP.LibraryExtension;
using QSP.RouteFinding.FileExport;
using QSP.RouteFinding.FileExport.Providers;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace QSP.UI.Forms.Options
{
    public class FlightPlanExportController
    {
        private IEnumerable<RouteExportMatching> exports;
        private Locator<AppOptions> appOptionsLocator;

        public AppOptions AppOptions => appOptionsLocator.Instance;

        public FlightPlanExportController(
            IEnumerable<RouteExportMatching> exports,
            Locator<AppOptions> appOptionsLocator)
        {
            this.exports = exports;
            this.appOptionsLocator = appOptionsLocator;
        }

        /// <summary>
        /// Set the event handler of buttons and checkboxes.
        /// Set controls to their default state.
        /// </summary>
        public void Init()
        {
            AddBrowseBtnHandler();
            AddCheckBoxEventHandler();
            SetDefaultState();
        }

        /// <summary>
        /// Returns the commands as set by the user.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, ExportCommand> GetCommands()
        {
            var cmds = new Dictionary<string, ExportCommand>();

            foreach (var i in exports)
            {
                var command = new ExportCommand(i.Type, i.TxtBox.Text, i.CheckBox.Checked);
                cmds.Add(i.Key, command);
            }

            return cmds;
        }

        /// <summary>
        /// Set the controls as in the AppOptions.
        /// </summary>
        public void SetExports()
        {
            foreach (var i in exports)
            {
                ExportCommand cmd;

                if (AppOptions.ExportCommands.TryGetValue(i.Key, out cmd))
                {
                    i.TxtBox.Text = cmd.Directory;
                    i.CheckBox.Checked = cmd.Enabled;
                }
            }
        }

        private void AddBrowseBtnHandler()
        {
            foreach (var i in exports)
            {
                i.BrowserBtn.Click += (sender, e) =>
                {

                    var dialog = new CommonOpenFileDialog();
                    dialog.InitialDirectory = i.TxtBox.Text;
                    dialog.IsFolderPicker = true;
                    var result = dialog.ShowDialog();

                    if (result == CommonFileDialogResult.Ok)
                    {
                        i.TxtBox.Text = dialog.FileName;
                    }
                };
            }
        }

        private void AddCheckBoxEventHandler()
        {
            foreach (var i in exports)
            {
                i.CheckBox.CheckedChanged += (sender, e) =>
                {
                    i.TxtBox.Enabled = i.CheckBox.Checked;
                };
            }
        }

        private void SetDefaultState()
        {
            foreach (var i in exports)
            {
                i.CheckBox.Checked = false;
                i.TxtBox.Enabled = false;
            }
        }
    }

    public class RouteExportMatching
    {
        public string Key { get; private set; }
        public ProviderType Type { get; private set; }
        public CheckBox CheckBox { get; private set; }
        public TextBox TxtBox { get; private set; }
        public Button BrowserBtn { get; private set; }

        public RouteExportMatching(
            string Key,
            ProviderType Type,
            CheckBox CheckBox,
            TextBox TxtBox,
            Button BrowserBtn)
        {
            this.Key = Key;
            this.Type = Type;
            this.CheckBox = CheckBox;
            this.TxtBox = TxtBox;
            this.BrowserBtn = BrowserBtn;
        }
    }
}
