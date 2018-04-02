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

            // Setting the Text property to null or an empty string ("") sets 
            // the SelectedIndex to -1. Setting the Text property to a value 
            // that is in the Items collection sets the SelectedIndex to the index 
            // of that item. Setting the Text property to a value that is not in the 
            // collection leaves the SelectedIndex unchanged.
            if (original != "") c.Text = original;
        }
    }
}
