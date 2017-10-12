using CommonLibrary.LibraryExtension;
using System.Linq;
using System.Windows.Forms;

namespace QSP.UI.UserControls
{
    public static class AlternateRowExtension
    {
        public static void AddToLayoutPanel(
            this AlternateRowItems row, TableLayoutPanel altnLayoutPanel)
        {
            altnLayoutPanel.RowCount += 1;
            MoveRowsDown(altnLayoutPanel);
            altnLayoutPanel.Controls.Add(row, 0, altnLayoutPanel.RowCount - 2);
            SetRowSizes(altnLayoutPanel);
        }

        private static void MoveRowsDown(TableLayoutPanel panel)
        {
            var addRemoveBtns = panel.Controls.Cast<Control>()
                .Where(c => panel.GetRow(c) == panel.RowCount - 2);

            addRemoveBtns.ForEach(i => panel.SetRow(i, panel.RowCount - 1));
        }

        private static void SetRowSizes(TableLayoutPanel panel)
        {
            var styles = panel.RowStyles;

            for (int i = 0; i < styles.Count - 1; i++)
            {
                styles[i].SizeType = SizeType.AutoSize;
            }

            styles[styles.Count - 1].SizeType = SizeType.Percent;
            styles[styles.Count - 1].Height = 1.0F;
        }
    }
}
