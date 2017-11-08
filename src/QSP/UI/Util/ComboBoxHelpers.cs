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
    }
}
