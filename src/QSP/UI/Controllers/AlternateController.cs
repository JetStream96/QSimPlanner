using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QSP.UI.Controls;
using System.Drawing;
using QSP.UI.Utilities;

namespace QSP.UI.Controllers
{
    public class AlternateController
    {
        private const int RowSeperation = 38;

        private IEnumerable<Control> controlsBelow;
        private GroupBox altnGroupBox;
        private List<AlternateRow> rows;

        public AlternateController(
            IEnumerable<Control> controlsBelow,
            GroupBox altnGroupBox)
        {
            this.controlsBelow = controlsBelow;
            this.altnGroupBox = altnGroupBox;
            rows = new List<AlternateRow>();
        }

        public void AddRows(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var row = new AlternateRow(i + 1);
                row.AddToGroupBox(altnGroupBox);
                rows.Add(row);
                ResizeAndMove();
            }
        }

        private void ResizeAndMove()
        {
            var s = altnGroupBox.Size;
            altnGroupBox.Size = new Size(s.Width, s.Height + RowSeperation);
            controlsBelow.MoveDown(RowSeperation);
        }

        public class AlternateRow
        {
            public Label AltnNumLbl;
            public TextBox IcaoTxtBox;
            public Button FindBtn;
            public Label RwyLbl;
            public ComboBoxWithBorder RwyComboBox;
            public Label RouteLbl;
            public TextBox RouteTxtBox;

            public Control[] AllControls
            {
                get
                {
                    return new Control[]
                    {
                        AltnNumLbl,
                        IcaoTxtBox,
                        FindBtn,
                        RwyLbl,
                        RwyComboBox,
                        RouteLbl,
                        RouteTxtBox
                    };
                }
            }

            // rowNum: Larger or equal to 1.
            public AlternateRow(int rowNum)
            {
                if (rowNum < 1)
                {
                    throw new ArgumentException();
                }

                CreateControls(rowNum);
                AllControls.MoveDown((rowNum - 1) * RowSeperation);
            }
            
            private void CreateControls(int num)
            {
                // AltnNumLbl
                AltnNumLbl = new Label();
                AltnNumLbl.Font = new Font("Segoe UI", 10.2F);
                AltnNumLbl.Location = new Point(12, 24);
                AltnNumLbl.Size = new Size(65, 23);
                AltnNumLbl.Text = $"ALTN {num}";

                // IcaoTxtBox
                IcaoTxtBox = new TextBox();
                IcaoTxtBox.CharacterCasing = CharacterCasing.Upper;
                IcaoTxtBox.Font = new Font("Segoe UI", 10.2F);
                IcaoTxtBox.Location = new Point(13, 45);
                IcaoTxtBox.Size = new Size(91, 30);
                IcaoTxtBox.Text = "";

                // FindBtn 
                FindBtn = new Button();
                FindBtn.BackColor = Color.DarkSlateGray;
                FindBtn.FlatStyle = FlatStyle.Flat;
                FindBtn.Font = new Font("Segoe UI", 10.2F);
                FindBtn.ForeColor = SystemColors.ButtonHighlight;
                FindBtn.Location = new Point(161, 19);
                FindBtn.Size = new Size(63, 33);
                FindBtn.Text = "Find";

                // RwyLbl
                RwyLbl = new Label();
                RwyLbl.Font = new Font("Segoe UI", 10.2F);
                RwyLbl.Location = new Point(229, 23);
                RwyLbl.Size = new Size(69, 23);
                RwyLbl.Text = "Runway";

                // RwyComboBox
                RwyComboBox = new ComboBoxWithBorder();
                RwyComboBox.BorderColor = Color.DimGray;
                RwyComboBox.BorderStyle = ButtonBorderStyle.Solid;
                RwyComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                RwyComboBox.FlatStyle = FlatStyle.Flat;
                RwyComboBox.Font = new Font("Segoe UI", 10.2F);
                RwyComboBox.Location = new Point(303, 19);
                RwyComboBox.Size = new Size(84, 31);

                // RouteLbl
                RouteLbl = new Label();
                RouteLbl.Font = new Font("Segoe UI", 10.2F);
                RouteLbl.Location = new Point(404, 22);
                RouteLbl.Size = new Size(55, 23);
                RouteLbl.Text = "Route";

                // RouteTxtBox
                RouteTxtBox = new TextBox();
                RouteTxtBox.CharacterCasing = CharacterCasing.Upper;
                RouteTxtBox.Font = new Font("Segoe UI", 10.2F);
                RouteTxtBox.Location = new Point(464, 19);
                RouteTxtBox.Size = new Size(525, 30);
                RouteTxtBox.Text = "";
            }

            public void AddToGroupBox(GroupBox alternateGroupBox)
            {
                var g = alternateGroupBox;
                g.Controls.AddRange(AllControls);
            }
        }
    }
}
