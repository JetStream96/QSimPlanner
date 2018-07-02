using QSP.Common.Options;
using QSP.RouteFinding.FileExport;
using QSP.UI.Util;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static QSP.RouteFinding.FileExport.Providers.Types;

namespace QSP.UI.UserControls
{
    public partial class ExportMenuRow : UserControl
    {
        public ExportMenuRow()
        {
            InitializeComponent();
        }

        public void Init(ExportCommand c, Func<AppOptions> option)
        {
            var m = Lookup[c.ProviderType];
            var sims = m.SupportedSims.Select(s => SimDisplayName[s.Type]).ToArray();

            CheckBox.Text = m.DisplayName;

            SimComboBox.SelectedIndexChanged += (s, e) =>
            {
                var selectedSim = m.SupportedSims.Where(x =>
                    SimDisplayName[x.Type] == SimComboBox.Text).ToList();

                // Should not happen unless something is missing.
                if (selectedSim.Count == 0) return;

                var simType = selectedSim[0].Type;
                var path = m.SupportedSims.First(x => x.Type == simType)
                                          .Path
                                          .FullPath(simType, option());

                PathTextBox.Text = Directory.Exists(path) ? path : "";
            };

            SimComboBox.SetItems(sims);
            if (sims.Length > 0) SimComboBox.SelectedIndex = 0;

            FileFolderBrowse.LinkFolderBrowse(BrowseBtn, PathTextBox);
        }
    }
}
