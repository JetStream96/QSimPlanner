using System.Linq;
using System.Windows.Forms;

namespace QSP.UI
{
    public static class Utilities
    {
        public static T FormInstance<T>() where T :Form 
        {
            return (T)Application.OpenForms.Cast<Form>().Where(x => x is T).FirstOrDefault();
        }

        public static MainForm MainFormInstance()
        {
            return (MainForm)Application.OpenForms.Cast<Form>().Where(x => x is MainForm).FirstOrDefault();
        }
    }
}
