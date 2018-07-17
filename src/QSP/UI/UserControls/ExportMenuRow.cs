using QSP.Common.Options;
using QSP.RouteFinding.FileExport;
using QSP.UI.Util;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static QSP.LibraryExtension.Types;
using static QSP.RouteFinding.FileExport.Providers.Types;

namespace QSP.UI.UserControls
{
    public partial class ExportMenuRow : UserControl
    {
        private const string Custom = "Custom";

        private ExportCommand command;
        private Func<AppOptions> option;

        public ExportMenuRow()
        {
            InitializeComponent();
        }

        // Null if it's a custom path.
        private SimulatorType? SelectedSimType
        {
            get
            {
                var text = SimComboBox.Text;
                if (text == Custom) return null;

                var m = Lookup[command.ProviderType];
                var selectedSim = m.SupportedSims.Where(x =>
                    SimDisplayName[x.Type] == SimComboBox.Text).ToList();

                // Should not happen unless something is missing.
                if (selectedSim.Count == 0) return null;

                return selectedSim[0].Type;
            }
        }

        // Returned path may be null or not exist.
        private string GetDirectoryPath()
        {
            var simType = SelectedSimType;
            if (simType == null) return command.CustomDirectory;
            var sim = simType.Value;
            return Lookup[command.ProviderType].SupportedSims
                .First(x => x.Type == sim)
                .Path
                .FullPath(sim, option());
        }

        public void Init(ExportCommand c, Func<AppOptions> option)
        {
            this.command = c;
            this.option = option;

            var m = Lookup[c.ProviderType];
            var sims = m.SupportedSims.Select(s => SimDisplayName[s.Type])
                        .Concat(List(Custom)).ToArray();

            CheckBox.Text = m.DisplayName;
            CheckBox.Checked = c.Enabled;

            SimComboBox.SelectedIndexChanged += (s, e) =>
            {
                // Only allow selecting custom path.
                BrowseBtnEnabled = SimComboBox.SelectedIndex == SimComboBox.Items.Count - 1;

                var path = GetDirectoryPath();
                var pathValid = path != null;
                PathTextBox.Text = pathValid ? path : "";
            };

            SimComboBox.SetItems(sims);
            if (sims.Length > 0) SimComboBox.SelectedIndex = 0;
            SimComboBox.Text = c.DefaultSimulator == null ? Custom :
                SimDisplayName[c.DefaultSimulator.Value];

            FileFolderBrowse.LinkFolderBrowse(BrowseBtn, PathTextBox);
        }

        private bool BrowseBtnEnabled
        {
            set
            {
                BrowseBtn.Enabled = value;
                BrowseBtn.ForeColor = value ? Color.Black : Color.LightGray;
                BrowseBtn.BackColor = value ? Color.White : Color.LightGray;
            }
        }

        /// <summary>
        /// Gets the user-selected command.
        /// </summary>
        public ExportCommand SelectedCommand
        {
            get
            {
                var sim = SelectedSimType;
                return new ExportCommand(
                    command.ProviderType,
                    sim == null ? PathTextBox.Text : command.CustomDirectory,
                    CheckBox.Checked,
                    sim);
            }
        }
    }
}
