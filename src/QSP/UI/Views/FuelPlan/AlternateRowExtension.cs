using System.Windows.Forms;

namespace QSP.UI.Views.FuelPlan
{
    public static class AlternateRowExtension
    {
        public static void AddToLayoutPanel(this AlternateRowControl row, TableLayoutPanel p)
        {
            if (!(p.RowCount == 1 && p.Controls.Count == 0))
            {
                // Before initialization finished, p has one row but no AltenateRowControl
                // is added to p.

                p.RowCount += 1;
            }

            p.Controls.Add(row, 0, p.RowCount - 1);
            p.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        }        
    }
}
