using QSP.Common.Options;
using QSP.LibraryExtension;
using System;
using System.Windows.Forms;

namespace QSP.UI.UserControls
{
    public partial class ExportMenu : UserControl
    {
        private Locator<AppOptions> appOption;

        public ExportMenu()
        {
            InitializeComponent();
        }

        private void browseBtn_Click(object sender, EventArgs e)
        {

        }

        private void changePathBtn_Click(object sender, EventArgs e)
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
            }
        }
    }
}
