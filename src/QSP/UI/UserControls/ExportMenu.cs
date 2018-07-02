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
            SetLayoutPanel(appOption);
        }

        private void SetLayoutPanel(Locator<AppOptions> appOption)
        {
            var commands = appOption.Instance.ExportCommands;
            var panel = formatTableLayoutPanel;

            while (panel.RowCount < commands.Count)
            {
                panel.RowCount++;
                panel.Controls.Add(new ExportMenuRow(), 0, panel.RowCount - 1);
                panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            }

            var commandList = commands.OrderBy(c => Lookup[c.ProviderType].DisplayName);

            commandList.ForEach((command, i) =>
            {
                var c = (ExportMenuRow)panel.Controls[i];
                c.Init(command, () => appOption.Instance);
            });
        }
    }
}
