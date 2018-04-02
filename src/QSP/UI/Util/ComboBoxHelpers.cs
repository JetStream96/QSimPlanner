using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace QSP.UI.Util
{
    public static class ComboBoxHelpers
    {
        public static void SetItems(this ComboBox c, IEnumerable<object> items)
        {
            c.Items.Clear();
            c.Items.AddRange(items.ToArray());
        }

        /// <summary>
        /// Set the items of ComboBox and selects the option originally selected,
        /// if that option is in items. Otherwise, selects the first item.
        /// </summary>
        public static void SetItemsPreserveSelection(this ComboBox c, IEnumerable<object> items)
        {
            var original = c.Text;
            c.Items.Clear();
            c.Items.AddRange(items.ToArray());
            if (c.Items.Count > 0) c.SelectedIndex = 0;
            c.Text = original;
        }
    }
}
