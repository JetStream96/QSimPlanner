using QSP.Common.Options;
using QSP.LibraryExtension;
using QSP.RouteFinding.FileExport;
using QSP.RouteFinding.FileExport.Providers;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using FolderSelect;

namespace QSP.UI.Forms.Options
{
    public class FlightPlanExportController
    {
        private readonly IReadOnlyList<RouteExportMatching> exports;
        private readonly Locator<AppOptions> appOptionsLocator;

        public AppOptions AppOptions => appOptionsLocator.Instance;

        public FlightPlanExportController(
            IReadOnlyList<RouteExportMatching> exports,
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
        public Dictionary<string, ExportCommand> GetCommands()
        {
            return exports.ToDictionary(i => i.Key,
                i => new ExportCommand()
                {
                    ProviderType = i.Type,
                    CustomDirectory = i.TxtBox.Text,
                    Enabled = i.CheckBox.Checked
                    // TODO:
                });
        }

        /// <summary>
        /// Set the controls as in the AppOptions.
        /// </summary>
        public void SetExports()
        {
            foreach (var i in exports)
            {
                if (AppOptions.ExportCommands.TryGetValue(i.Key, out var cmd))
                {
                    i.TxtBox.Text = cmd.CustomDirectory;
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
                    using (var dialog = new FolderSelectDialog())
                    {
                        dialog.InitialDirectory = i.TxtBox.Text;

                        if (dialog.ShowDialog())
                        {
                            i.TxtBox.Text = dialog.FileName;
                        }
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
        public string Key { get; }
        public ProviderType Type { get; }
        public CheckBox CheckBox { get; }
        public TextBox TxtBox { get; }
        public Button BrowserBtn { get; }

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
