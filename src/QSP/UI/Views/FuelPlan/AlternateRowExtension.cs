using System.Windows.Forms;

namespace QSP.UI.Views.FuelPlan
{
    public static class AlternateRowExtension
    {
        public static void AddToLayoutPanel(
            this AlternateRowControl row, TableLayoutPanel p)
        {
            if (!(p.RowCount == 1 && p.Controls.Count == 0))
            {
                // Before initialization finished, p has one row but no AltenateRowControl
                // is added to p.

                p.RowCount += 1;
            }

            p.Controls.Add(row, 0, p.RowCount - 1);
            SetRowSizes(p);
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
