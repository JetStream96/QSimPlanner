using QSP.Common.Options;
using QSP.LibraryExtension;
using QSP.UI.Util;
using System;
using System.Windows.Forms;
using static QSP.RouteFinding.FileExport.Providers.Types;
using static System.Linq.Enumerable;

namespace QSP.UI.UserControls
{
    public partial class ExportMenu : UserControl
    {
        private Locator<AppOptions> appOption;

        public ExportMenu()
        {
            InitializeComponent();
        }

        private void ExportFiles(object sender, EventArgs e)
        {

        }

        private void ChangeSimPaths(object sender, EventArgs e)
        {

        }

        public void Init(Locator<AppOptions> appOption)
        {
            this.appOption = appOption;
            SetLayoutPanel();
        }

        private void SetLayoutPanel()
        {
            var commands = appOption.Instance.ExportCommands;
            var panel = formatTableLayoutPanel;

            while (panel.RowCount < commands.Count)
            {
                panel.RowCount++;
                panel.Controls.Add(new ExportMenuRow(), 0, panel.RowCount - 1);
                panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            }

            var commandList = commands.ToList();
            //TODO: commandList.Sort(c => c. SimDisplayName[);

            commandList.ForEach((command, i) =>
            {
                var c = (ExportMenuRow)panel.Controls[i];
                var m = Lookup[command.ProviderType];
                var sims = m.SupportedSims.Select(s => SimDisplayName[s.Type]).ToArray();

                c.CheckBox.Text = m.DisplayName;
                c.SimComboBox.Items.Clear();
                c.SimComboBox.Items.AddRange(sims);
               // c.PathTextBox.Text = m.SupportedSims.Select(x => x.Path.FullPath());
                FileFolderBrowse.LinkFolderBrowse(c.BrowseBtn, c.PathTextBox);
            });
        }
    }
}
