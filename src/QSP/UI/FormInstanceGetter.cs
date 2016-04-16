using System.Linq;
using System.Windows.Forms;

namespace QSP.UI
{
    public static class FormInstanceGetter
    {
        public static T GetInstance<T>() where T : Form
        {
            return (T)Application.OpenForms.Cast<Form>().Where(x => x is T).FirstOrDefault();
        }

        public static MainForm MainFormInstance()
        {
            return GetInstance<MainForm>();
        }
    }
}
