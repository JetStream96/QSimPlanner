using System.Windows.Forms;

namespace QSP.UI.Utilities
{
    public static class InputControl
    {
        public static void UpperCaseOnly(this Control c)
        {
            c.KeyPress += (s, e) => e.KeyChar = char.ToUpperInvariant(e.KeyChar);
        }

        public static void NumberOnly(this Control c)
        {
            c.KeyPress += (s, e) =>
            {

               // e.KeyChar = char.ToUpperInvariant(e.KeyChar);
            };
        }
    }
}