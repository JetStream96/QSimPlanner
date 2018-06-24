using FolderSelect;
using System.IO;
using System.Windows.Forms;

namespace QSP.UI.Util
{
    public static class FileFolderBrowse
    {
        public static void LinkFolderBrowse(Button b, TextBox t)
        {
            b.Click += (sender, e) =>
            {
                using (var dialog = new FolderSelectDialog())
                {
                    var dir = t.Text;
                    if (Directory.Exists(dir)) dialog.InitialDirectory = dir;

                    if (dialog.ShowDialog())
                    {
                        t.Text = dialog.FileName;
                    }
                }
            };
        }

        public static void LinkFileBrowse(Button b, TextBox t,
            string fileTypeFilter = "text files (*.txt)|*.txt|All files (*.*)|*.*")
        {
            b.Click += (sender, e) =>
            {
                using (var dialog = new OpenFileDialog())
                {
                    dialog.Filter = fileTypeFilter;
                    var dir = t.Text;
                    if (Directory.Exists(dir))
                    {
                        dialog.InitialDirectory = dir;
                        dialog.RestoreDirectory = true;
                    }

                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        t.Text = dialog.FileName;
                    }
                }
            };
        }
    }
}
