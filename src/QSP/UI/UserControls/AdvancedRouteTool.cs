using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.TerminalProcedures.Sid;

namespace QSP.UI.UserControls
{
    public partial class AdvancedRouteTool : UserControl
    {
        private ControlGroup fromGroup;
        private ControlGroup toGroup;

        private AirportManager airportList;

        public AdvancedRouteTool()
        {
            InitializeComponent();
        }

        public void Init(AirportManager airportList)
        {
            this.airportList = airportList;

            setControlGroups();
            attachEventHandlers();
            setDefaultState();
            
        }

        private void setControlGroups()
        {
            fromGroup = new ControlGroup(
                fromTypeComboBox,
                fromIdentLbl,
                fromIdentTxtBox,
                fromRwyLbl,
                fromRwyComboBox,
                sidLbl,
                sidComboBox,
                fromWptLbl,
                fromWptComboBox);

            toGroup = new ControlGroup(
               toTypeComboBox,
               toIdentLbl,
               toIdentTxtBox,
               toRwyLbl,
               toRwyComboBox,
               starLbl,
               starComboBox,
               toWptLbl,
               toWptComboBox);
        }

        private void attachEventHandlers()
        {
            attachHandlers(fromGroup);
            attachHandlers(toGroup);
        }

        private void attachHandlers(ControlGroup group)
        {
            group.TypeSelection.SelectedIndexChanged +=
                (sender, e) => typeChanged(group);

            group.Ident.TextChanged +=
                (sender, e) => identChanged(group);

            group.Rwy.SelectedIndexChanged +=
                (sender, e) => rwyChanged(group);
        }

        private void identChanged(ControlGroup group)
        {
            if (group.TypeSelection.SelectedIndex == 0)
            {
                // Airport
                var rwyCBox = group.Rwy;
                rwyCBox.Items.Clear();

                var icao = group.Ident.Text.Trim().ToUpper();
                var rwyList = airportList.RwyIdentList(icao);

                if (rwyList != null)
                {
                    rwyCBox.Items.AddRange(rwyList);
                    rwyCBox.SelectedIndex = 0;
                }
            }
            else
            {
               
            }
        }
        
        private void rwyChanged(ControlGroup group)
        {

        }

        private void typeChanged(ControlGroup group)
        {
            if (group.TypeSelection.SelectedIndex == 0)
            {
                // Airport
                group.IdentLbl.Text = "ICAO";
                group.RwyLbl.Enabled = true;
                group.Rwy.Enabled = true;
                group.TerminalProcedureLbl.Enabled = true;
                group.TerminalProcedure.Enabled = true;
                group.WptLbl.Enabled = false;
                group.Waypoint.Enabled = false;
            }
            else
            {
                group.IdentLbl.Text = "Ident";
                group.RwyLbl.Enabled = false;
                group.Rwy.Enabled = false;
                group.TerminalProcedureLbl.Enabled = false;
                group.TerminalProcedure.Enabled = false;
                group.WptLbl.Enabled = true;
                group.Waypoint.Enabled = true;
            }
        }

        private void setDefaultState()
        {
            initTypes(fromTypeComboBox);
            initTypes(toTypeComboBox);
            routeSummaryLbl.Text = "";
        }

        private void initTypes(ComboBox cbox)
        {
            var items = cbox.Items;
            items.Clear();
            items.AddRange(new string[] { "Airport", "Waypoint" });
            cbox.SelectedIndex = 0;
        }

        private class ControlGroup
        {
            public ComboBox TypeSelection;
            public Label IdentLbl;
            public TextBox Ident;
            public Label RwyLbl;
            public ComboBox Rwy;
            public Label TerminalProcedureLbl;
            public ComboBox TerminalProcedure;
            public Label WptLbl;
            public ComboBox Waypoint;

            public ControlGroup(
                ComboBox TypeSelection,
                Label IdentLbl,
                TextBox Ident,
                Label RwyLbl,
                ComboBox Rwy,
                Label TerminalProcedureLbl,
                ComboBox TerminalProcedure,
                Label WptLbl,
                ComboBox Waypoint)
            {
                this.TypeSelection = TypeSelection;
                this.IdentLbl = IdentLbl;
                this.Ident = Ident;
                this.RwyLbl = RwyLbl;
                this.Rwy = Rwy;
                this.TerminalProcedureLbl = TerminalProcedureLbl;
                this.TerminalProcedure = TerminalProcedure;
                this.WptLbl = WptLbl;
                this.Waypoint = Waypoint;
            }
        }
    }
}
