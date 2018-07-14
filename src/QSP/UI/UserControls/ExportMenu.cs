using QSP.Common.Options;
using QSP.LibraryExtension;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.FileExport;
using QSP.RouteFinding.Routes;
using QSP.UI.Util;
using QSP.UI.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using static QSP.RouteFinding.FileExport.Providers.Types;
using static System.Linq.Enumerable;

namespace QSP.UI.UserControls
{
    public partial class ExportMenu : UserControl, IMessageDisplay
    {
        private Locator<AppOptions> appOption;

        // These two properties are required for exporting the route.
        public RouteGroup Route { get; set; }
        public AirportManager AirportList { get; set; }

        public ExportMenu()
        {
            InitializeComponent();
        }

        private void ExportFiles(object sender, EventArgs e)
        {
            var writer = new FileExporter(Route.Expanded, AirportList, commands);
            IEnumerable<FileExporter.Status> reports = null;

            try
            {
                reports = writer.Export();
            }
            catch (Exception ex)
            {
                this.ShowMessage(ex.Message, MessageLevel.Warning);
                return;
            }

            ShowReports(this, reports.ToList());
        }

        private static void ShowReports(IMessageDisplay view, List<FileExporter.Status> reports)
        {
            if (!reports.Any())
            {
                view.ShowMessage(
                    "No route file to export. Please set export settings in options page.",
                    MessageLevel.Info);
                return;
            }

            var msg = new StringBuilder();
            var success = reports.Where(r => r.Successful).ToList();

            if (success.Any())
            {
                msg.AppendLine($"{success.Count} company route(s) exported:");

                foreach (var i in success)
                {
                    msg.AppendLine(i.FilePath);
                }
            }

            var errors = reports.Where(r => !r.Successful).ToList();

            if (errors.Any())
            {
                msg.AppendLine($"\n\nFailed to export {errors.Count} file(s) into:");

                foreach (var j in errors)
                {
                    msg.AppendLine(j.FilePath);
                }
            }

            if (errors.Any(e => e.MayBePermissionIssue))
            {
                msg.AppendLine("\nYou can try to run this application " +
                    "as administrator.");
            }

            view.ShowMessage(
                msg.ToString(),
                errors.Any() ? MessageLevel.Warning : MessageLevel.Info);
        }

        private void ChangeSimPaths(object sender, EventArgs e)
        {
            ParentForm.Hide();

            // TODO: Option page
        }

        public void Init(Locator<AppOptions> appOption)
        {
            this.appOption = appOption;

            SetLayoutPanel(appOption);
        }

        public IEnumerable<ExportCommand> GetSelectedCommands()
        {

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

        public void ShowMessage(string s, MessageLevel lvl) => ShowMessage(s, lvl);
    }
}
