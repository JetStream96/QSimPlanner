using Gecko;
using QSP.GoogleMap;
using QSP.MathTools;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.Data.Interfaces;
using QSP.UI.Presenters.MiscInfo;
using QSP.UI.Util;
using QSP.UI.Util.ScrollBar;
using QSP.UI.Views.Factories;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace QSP.UI.Views.MiscInfo
{
    public partial class AirportMapControl : UserControl, IAirportMapView
    {
        private AirportMapPresenter presenter;
        private PictureBox picBox;
        private GeckoWebBrowser browser;

        public bool BrowserEnabled
        {
            get => browser != null;

            set
            {
                if (value && BrowserEnabled == false)
                {
                    EnableBrowser();
                }
                else if (value == false && BrowserEnabled)
                {
                    DisableBrowser();
                }
            }
        }

        public bool StaticMapEnabled
        {
            get => picBox != null;

            set
            {
                if (value && StaticMapEnabled == false)
                {
                    EnableStaticMap();
                }
                else if (value == false && StaticMapEnabled)
                {
                    DisableStaticMap();
                }
            }
        }

        public AirportMapControl()
        {
            InitializeComponent();
        }

        public void Init(AirportMapPresenter presenter)
        {
            this.presenter = presenter;

            ResetAirport();
            SetEmptyDataGrid();

            icaoComboBox.Text = "";

            AddToolTip();
            SetUpdateBtnImage();
            icaoComboBox.UpperCaseOnly();
        }

        private void SetUpdateBtnImage()
        {
            var oldSize = updateBtn.Size;
            var size = new Size(oldSize.Width - 4, oldSize.Height - 4);
            updateBtn.BackgroundImage = ImageUtil.Resize(Properties.Resources.processing, size);
        }

        private void AddToolTip()
        {
            var tp = ToolTipFactory.GetToolTip();
            tp.SetToolTip(metarLbl, "View METAR");
            tp.SetToolTip(updateBtn, "Refresh METAR");
        }

        private void SetEmptyDataGrid()
        {
            airportDataGrid.Columns.Clear();
            airportDataGrid.Rows.Clear();
            airportDataGrid.ColumnCount = 10;
            airportDataGrid.RowCount = 0;

            SetColumnsLables();
        }

        private void ResetAirport()
        {
            airportNameLbl.Text = "";
            latLonLbl.Text = "";
            elevationLbl.Text = "";
            transAltLbl.Text = "";
            metarLbl.Text = "";
            metarLbl.Visible = false;
            updateBtn.Visible = false;
        }

        private void SetMetar()
        {
            presenter.SetMetarText();
        }

        private string TransitionAlts((int, int) transAltLevel)
        {
            var (alt, level) = transAltLevel;

            // If TL is 0, that means it's not a fixed value.
            // Show "-" instead.
            if (level == 0)
            {
                return alt.ToString() + " / -";
            }
            else
            {
                return alt.ToString() + " / FL" + Numbers.RoundToInt(level / 100.0).ToString();
            }
        }

        public IReadOnlyList<IRwyData> Runways
        {
            set
            {
                var runways = value;
                airportDataGrid.Columns.Clear();
                airportDataGrid.Rows.Clear();
                airportDataGrid.ColumnCount = 10;
                airportDataGrid.RowCount = runways.Count;
                SetColumnsLables();

                for (int i = 0; i < runways.Count; i++)
                {
                    var rwy = runways[i];

                    airportDataGrid[0, i].Value = rwy.RwyIdent;
                    airportDataGrid[1, i].Value = rwy.LengthFt;
                    airportDataGrid[2, i].Value = rwy.Heading;
                    airportDataGrid[3, i].Value = rwy.Lat;
                    airportDataGrid[4, i].Value = rwy.Lon;

                    if (rwy.HasIlsInfo && rwy.IlsAvail)
                    {
                        airportDataGrid[5, i].Value = rwy.IlsFreq;
                        airportDataGrid[6, i].Value = rwy.IlsHeading;
                        airportDataGrid[7, i].Value = rwy.GlideslopeAngle.ToString("0.0");
                    }
                    else
                    {
                        airportDataGrid[5, i].Value = "";
                        airportDataGrid[6, i].Value = "";
                        airportDataGrid[7, i].Value = "";
                    }

                    airportDataGrid[8, i].Value = rwy.ElevationFt;
                    airportDataGrid[9, i].Value = rwy.SurfaceType;
                }

                SetGridViewHeight();
            }
        }

        private void SetGridViewHeight()
        {
            var view = airportDataGrid;
            var height = 10 + view.ColumnHeadersHeight;
            foreach (DataGridViewRow dr in view.Rows) height += dr.Height;
            view.Height = height;

            // Limit the number of loops to prevent infinite loop.
            for (int i = 0; i < 10; i++)
            {
                if (airportDataGrid.VScrollBarVisible()) airportDataGrid.Height += 10;
            }
        }

        private void SetColumnsLables()
        {
            airportDataGrid.Columns[0].Name = "RWY";
            airportDataGrid.Columns[1].Name = "Length(FT)";
            airportDataGrid.Columns[2].Name = "Heading";
            airportDataGrid.Columns[3].Name = "LAT";
            airportDataGrid.Columns[4].Name = "LON";
            airportDataGrid.Columns[5].Name = "ILS freq";
            airportDataGrid.Columns[6].Name = "ILS course";
            airportDataGrid.Columns[7].Name = "Glideslope angle";
            airportDataGrid.Columns[8].Name = "Threshold altitude(FT)";
            airportDataGrid.Columns[9].Name = "Surface Type";
        }

        private void updateBtn_Click(object sender, EventArgs e)
        {
            SetMetar();
        }

        private void icaoComboBox_TextChanged(object sender, EventArgs e)
        {
            ResetAirport();
            presenter.UpdateAirport();
        }

        private void metarLbl_Click(object sender, EventArgs e)
        {
            var frm = new MetarForm();
            frm.icaoTxtBox.Text = IcaoText;
            frm.icaoTxtBox.Enabled = false;
            frm.resultRichTxtBox.Text = metarLbl.Text ?? "";
            frm.ShowDialog();
        }

        private void EnableBrowser()
        {
            if (!Xpcom.IsInitialized)
            {
                Xpcom.Initialize("Firefox");
            }

            var wb = new GeckoWebBrowser();

            wb.Location = Point.Empty;
            wb.Size = MapSize;
            wb.Navigate("http://rendering/");
            
            tableLayoutPanel2.Controls.Add(wb, 0, 1);
            browser = wb;
        }

        private Size MapSize => new Size(tableLayoutPanel2.Width, 700);

        public string IcaoText => icaoComboBox.Text.Trim().ToUpper();

        public IEnumerable<string> IcaoList
        {
            set
            {
                icaoComboBox.Items.Clear();
                icaoComboBox.Items.AddRange(value.ToArray());
            }
        }

        public string MetarText
        {
            set
            {
                metarLbl.Text = value;
                metarLbl.Visible = true;
                updateBtn.Visible = true;
            }
        }

        public string AirportName { set => airportNameLbl.Text = value; }
        public int ElevationFt { set => elevationLbl.Text = value + " FT"; }
        public bool TransitionAltExist { set => transExistLbl.Visible = value; }
        public (int, int) TransitionAltLevel { set => transAltLbl.Text = TransitionAlts(value); }

        public ICoordinate LatLon
        {
            set => latLonLbl.Text = value.Lat.ToString("F6") + " / " + value.Lon.ToString("F6");
        }

        private void DisableBrowser()
        {
            tableLayoutPanel2.Controls.Remove(browser);
            browser = null;
        }

        public void ShowMap(ICoordinate c)
        {
            if (BrowserEnabled)
            {
                ShowMapBrowser(c.Lat, c.Lon);
            }
            else if (StaticMapEnabled)
            {
                ShowStaticMap(c.Lat, c.Lon);
            }
        }

        private void ShowMapBrowser(double lat, double lon)
        {
            browser.Navigate($"https://qsimplanner.azurewebsites.net/map/airport?lat={lat}&lon={lon}");
        }

        private void EnableStaticMap()
        {
            var pb = new PictureBox();

            pb.Location = Point.Empty;
            pb.Size = MapSize;
            pb.BackgroundImageLayout = ImageLayout.None;

            // Added so that it doesn't display the default image, 
            // when the first time LoadAsync is called. 
            pb.Image = new Bitmap(1, 1);

            tableLayoutPanel2.Controls.Add(pb);
            picBox = pb;
        }

        private void DisableStaticMap()
        {
            tableLayoutPanel2.Controls.Remove(picBox);
            picBox = null;
        }

        private void ShowStaticMap(double lat, double lon)
        {
            picBox.LoadAsync(StaticMap.GetMapUrl(lat, lon, picBox.Width, picBox.Height));
        }

        private void AirportMapControl_Load(object sender, EventArgs e)
        {
            if (icaoComboBox.Items.Count > 0) icaoComboBox.SelectedIndex = 0;
        }
    }
}
