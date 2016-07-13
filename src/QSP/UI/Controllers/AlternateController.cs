using QSP.Common.Options;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Routes;
using QSP.RouteFinding.Routes.TrackInUse;
using QSP.RouteFinding.TerminalProcedures;
using QSP.UI.Controls;
using QSP.UI.UserControls;
using QSP.UI.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static QSP.UI.Utilities.RouteDistanceDisplay;

namespace QSP.UI.Controllers
{
    public class AlternateController
    {
        private const int RowSeperation = 38;

        private IEnumerable<Control> controlsBelow;
        private GroupBox altnGroupBox;
        private List<AltnRow> rows;
        private AppOptions appSettings;
        private AirportManager airportList;
        private WaypointList wptList;
        private TrackInUseCollection tracksInUse;
        private Form parentForm;
        private DestinationSidSelection destSidProvider;

        // Fired when the number of rows changes.
        public event EventHandler RowCountChanged;

        public int RowCount
        {
            get
            {
                return rows.Count;
            }
        }

        public AlternateController(
            IEnumerable<Control> controlsBelow,
            GroupBox altnGroupBox,
            AppOptions appSettings,
            AirportManager airportList,
            WaypointList wptList,
            TrackInUseCollection tracksInUse,
            Form parentForm,
            DestinationSidSelection destSidProvider)
        {
            this.controlsBelow = controlsBelow;
            this.altnGroupBox = altnGroupBox;
            this.appSettings = appSettings;
            this.airportList = airportList;
            this.wptList = wptList;
            this.tracksInUse = tracksInUse;
            this.parentForm = parentForm;
            this.destSidProvider = destSidProvider;

            rows = new List<AltnRow>();
        }

        public void AddRow()
        {
            int index = rows.Count;
            var row = new AlternateRowItems(index + 1);
            row.AddToGroupBox(altnGroupBox);

            var controller = new AltnRowControl(this, row);
            controller.Subsribe();

            rows.Add(new AltnRow() { Items = row, Control = controller });
            ResizeAndMove(SizeChange.Increase);            
            RowCountChanged?.Invoke(this, EventArgs.Empty);
        }
        
        /// <exception cref="InvalidOperationException"></exception>
        public void RemoveLastRow()
        {
            if (rows.Count == 0)
            {
                throw new InvalidOperationException();
            }

            var rowToRemove = rows[rows.Count - 1];

            foreach (var c in rowToRemove.Items.AllControls)
            {
                altnGroupBox.Controls.Remove(c);
                c.Dispose();
            }

            rowToRemove.Control.CleanUp();
            rows.RemoveAt(rows.Count - 1);
            ResizeAndMove(SizeChange.Decrease);
            RowCountChanged?.Invoke(this, EventArgs.Empty);
        }

        public Route[] Routes
        {
            get
            {
                return rows.Select(r => r.Control.OptionBtns.Route.Expanded)
                    .ToArray();
            }
        }

        private void ResizeAndMove(SizeChange type)
        {
            int sizeIncrease = RowSeperation;

            if (type == SizeChange.Decrease)
            {
                sizeIncrease = -sizeIncrease;
            }

            var s = altnGroupBox.Size;
            altnGroupBox.Size = new Size(s.Width, s.Height + sizeIncrease);
            controlsBelow.MoveDown(sizeIncrease);
        }

        private enum SizeChange
        {
            Increase,
            Decrease
        }

        public struct AltnRow
        {
            public AlternateRowItems Items; public AltnRowControl Control;
        }

        public class AltnRowControl
        {
            public AlternateController Parent;
            public AlternateRowItems Row;
            public RouteFinderSelection Controller;
            public RouteOptionBtns OptionBtns;

            public AltnRowControl(
                AlternateController Parent,
                AlternateRowItems row)
            {
                this.Parent = Parent;
                this.Row = row;

                Controller = new RouteFinderSelection(
                    Row.IcaoTxtBox,
                    false,
                    Row.RwyComboBox,
                    new ComboBox(),
                    new Button(),
                    Parent.appSettings.NavDataLocation,
                    Parent.airportList,
                    Parent.wptList,
                    new ProcedureFilter());

                SetOptionBtns();

                OptionBtns.Init(
                    Parent.appSettings,
                    Parent.wptList,
                    Parent.airportList,
                    Parent.tracksInUse,
                    Parent.destSidProvider,
                    Controller,
                    Row.DisLbl,
                    DistanceDisplayStyle.Short,
                    () => Row.RouteTxtBox.Text,
                    (s) => Row.RouteTxtBox.Text = s);
            }

            private void SetOptionBtns()
            {
                OptionBtns = new RouteOptionBtns();
                OptionBtns.Visible = false;
                OptionBtns.BorderStyle = BorderStyle.FixedSingle;
                                
                var locInForm = Row.ShowMoreBtn.LocationInForm();
                int x = locInForm.X + Row.ShowMoreBtn.Width - OptionBtns.Width;
                int y = locInForm.Y + Row.ShowMoreBtn.Height + 10;
                OptionBtns.Location = new Point(x, y);
                Parent.parentForm.Controls.Add(OptionBtns);                
            }

            public void Subsribe()
            {
                Controller.Subscribe();
                Row.ShowMoreBtn.Click += ShowBtns;
                OptionBtns.Subscribe();
            }

            // Make sure the parent form no longer holds a reference 
            // to any of the private members.
            public void CleanUp()
            {
                Parent.parentForm.Controls.Remove(OptionBtns);
                OptionBtns.Dispose();
            }

            private void ShowBtns(object sender, EventArgs e)
            {
                OptionBtns.Visible ^= true;
                OptionBtns.BringToFront();
            }

            private void HideBtns(object sender, EventArgs e)
            {
                OptionBtns.Visible = false;
            }
        }

        public class AlternateRowItems
        {
            public TextBox IcaoTxtBox;
            public Button FindBtn;
            public Label RwyLbl;
            public ComboBoxWithBorder RwyComboBox;
            public Label RouteLbl;
            public TextBox RouteTxtBox;
            public Label DisLbl;
            public Button ShowMoreBtn;

            public Control[] AllControls
            {
                get
                {
                    return new Control[]
                    {
                        IcaoTxtBox,
                        FindBtn,
                        RwyLbl,
                        RwyComboBox,
                        RouteLbl,
                        RouteTxtBox,
                        DisLbl,
                        ShowMoreBtn
                    };
                }
            }

            // rowNum: Larger or equal to 1.
            public AlternateRowItems(int rowNum)
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
                // IcaoTxtBox
                IcaoTxtBox = new TextBox();
                IcaoTxtBox.TextAlign = HorizontalAlignment.Center;
                IcaoTxtBox.CharacterCasing = CharacterCasing.Upper;
                IcaoTxtBox.Font = new Font("Segoe UI", 10.2F);
                IcaoTxtBox.Location = new Point(12, 20);
                IcaoTxtBox.Size = new Size(60, 30);
                IcaoTxtBox.Text = "";

                // FindBtn 
                FindBtn = new Button();
                FindBtn.BackColor = Color.DarkSlateGray;
                FindBtn.FlatStyle = FlatStyle.Flat;
                FindBtn.Font = new Font("Segoe UI", 10.2F);
                FindBtn.ForeColor = SystemColors.ButtonHighlight;
                FindBtn.Location = new Point(76, 19);
                FindBtn.Size = new Size(55, 33);
                FindBtn.Text = "Find";

                // RwyLbl
                RwyLbl = new Label();
                RwyLbl.Font = new Font("Segoe UI", 10.2F);
                RwyLbl.Location = new Point(136, 23);
                RwyLbl.Size = new Size(69, 23);
                RwyLbl.Text = "Runway";

                // RwyComboBox
                RwyComboBox = new ComboBoxWithBorder();
                RwyComboBox.BorderColor = Color.DimGray;
                RwyComboBox.BorderStyle = ButtonBorderStyle.Solid;
                RwyComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                RwyComboBox.FlatStyle = FlatStyle.Flat;
                RwyComboBox.Font = new Font("Segoe UI", 10.2F);
                RwyComboBox.Location = new Point(215, 19);
                RwyComboBox.Size = new Size(60, 31);

                // RouteLbl
                RouteLbl = new Label();
                RouteLbl.Font = new Font("Segoe UI", 10.2F);
                RouteLbl.Location = new Point(285, 22);
                RouteLbl.Size = new Size(55, 23);
                RouteLbl.Text = "Route";

                // RouteTxtBox
                RouteTxtBox = new TextBox();
                RouteTxtBox.CharacterCasing = CharacterCasing.Upper;
                RouteTxtBox.Font = new Font("Segoe UI", 10.2F);
                RouteTxtBox.Location = new Point(345, 19);
                RouteTxtBox.Size = new Size(494, 30);
                RouteTxtBox.Text = "";

                // DisLbl
                DisLbl = new Label();
                DisLbl.Font = new Font("Segoe UI", 10.2F);
                DisLbl.Location = new Point(855, 22);
                DisLbl.AutoSize = true;
                DisLbl.Text = "";

                // OptionBtn
                ShowMoreBtn = new Button();
                ShowMoreBtn.BackColor = SystemColors.ButtonHighlight;
                ShowMoreBtn.FlatStyle = FlatStyle.Flat;
                ShowMoreBtn.BackgroundImage = Properties.Resources.add_icon;
                ShowMoreBtn.BackgroundImageLayout = ImageLayout.Zoom;
                ShowMoreBtn.ForeColor = Color.FromArgb(20, 20, 20);
                ShowMoreBtn.Location = new Point(1050, 19);
                ShowMoreBtn.Size = new Size(33, 33);
                ShowMoreBtn.Text = "";
            }

            public void AddToGroupBox(GroupBox alternateGroupBox)
            {
                var g = alternateGroupBox;
                g.Controls.AddRange(AllControls);
            }
        }
    }
}
