using System.Windows.Forms;

namespace QSP.UI.Views.FuelPlan
{
    public static class AlternateRowExtension
    {
        public static void AddToLayoutPanel(
            this AlternateRowControl row, TableLayoutPanel altnLayoutPanel)
        {
            altnLayoutPanel.RowCount += 1;
            altnLayoutPanel.Controls.Add(row, 0, altnLayoutPanel.RowCount - 1);
            SetRowSizes(altnLayoutPanel);
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
