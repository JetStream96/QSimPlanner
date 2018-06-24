using QSP.Common.Options;
using QSP.LibraryExtension;
using QSP.UI.Util;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using static QSP.LibraryExtension.Types;

namespace QSP.UI.Forms.Options
{
    public partial class SimulatorPathsMenu : UserControl
    {
        private (SimulatorType, TextBox, Button)[] Matching =>
            Arr
            (
                (SimulatorType.FSX, textBox1, button1),
                (SimulatorType.FSX_Steam, textBox2, button2),
                (SimulatorType.FS9, textBox3, button3),
                (SimulatorType.P3Dv1, textBox4, button4),
                (SimulatorType.P3Dv2, textBox5, button5),
                (SimulatorType.P3Dv3, textBox6, button6),
                (SimulatorType.P3Dv4, textBox7, button7),
                (SimulatorType.Xplane11, textBox8, button8)
            );

        public SimulatorPathsMenu()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Set the event handler of buttons and checkboxes.
        /// Set controls to their default state.
        /// </summary>
        public void Init()
        {
            AddBrowseBtnHandler();
            SetDefaultState();
        }

        private void AddBrowseBtnHandler()
        {
            foreach (var i in Matching)
            {
                var (_, textbox, button) = i;
                FileFolderBrowse.LinkFolderBrowse(button, textbox);
            }
        }

        private void SetDefaultState()
        {
            foreach (var i in Matching)
            {
                var (_, textbox, _) = i;
                textbox.Enabled = true;
            }
        }

        /// <summary>
        /// Returns all simulator paths seleted by the user.
        /// </summary>
        public Dictionary<SimulatorType, string> GetSimulatorPaths()
        {
            return Matching.Select(x =>
            {
                var (type, textBox, _) = x;
                return (type, textBox.Text);
            }).ToDictionary(y => y.Item1, y => y.Item2);
        }

        public void SetControlsAsInOptions(AppOptions o)
        {
            var p = o.SimulatorPaths;
            Matching.ForEach(m =>
            {
                var (type, textbox, _) = m;
                if (p.TryGetValue(type, out var path)) textbox.Text = path;
            });
        }
    }
}
