using System;
using QSP.AviationTools;
using QSP.RouteFinding;
using static QSP.UI.Utilities;

namespace QSP
{

    public class FuelCalculationParameters
    {

        public double Zfw;
        public double ContPerc;
        public double MissedAppFuel;
        public double HoldingMin;
        public double ExtraFuel;
        public double APUTime;
        public double TaxiTime;
        public double FinalRsvMin;
        public double DisToDest;
        public double DisToAltn;
        public double AvgWindToDest;
        public double AvgWindToAltn;
        public Aircraft AC;
        public double Zfw_KG;
        public double MissedAppFuel_KG;

        public double ExtraFuel_KG;

        private WeightUnit _WtUnit;
        private MainForm frm = MainFormInstance();

        public WeightUnit WtUnit()
        {
            return _WtUnit;
        }

        private void readWtUnit()
        {
            if (frm.WtUnitSel_ComboBox.Text == "KG")
            {
                _WtUnit = WeightUnit.KG;
            }
            else
            {
                _WtUnit = WeightUnit.LB;
            }
        }

        private void setPropertyKG()
        {
            if (_WtUnit == WeightUnit.KG)
            {
                Zfw_KG = Zfw;
                MissedAppFuel_KG = MissedAppFuel;
                ExtraFuel_KG = ExtraFuel;
            }
            else
            {
                Zfw_KG = Zfw * AviationConstants.LB_KG;
                MissedAppFuel_KG = MissedAppFuel * AviationConstants.LB_KG;
                ExtraFuel_KG = ExtraFuel * AviationConstants.LB_KG;
            }
        }

        private void setAircraftType()
        {
            if (frm.ACList.Text == "737-600")
            {
                AC = Aircraft.B737600;
            }
            else if (frm.ACList.Text == "737-700")
            {
                AC = Aircraft.B737700;
            }
            else if (frm.ACList.Text == "737-800")
            {
                AC = Aircraft.B737800;
            }
            else if (frm.ACList.Text == "737-900")
            {
                AC = Aircraft.B737900;
            }
            else if (frm.ACList.Text == "777-200LR")
            {
                AC = Aircraft.B777200LR;
            }
            else if (frm.ACList.Text == "777F")
            {
                AC = Aircraft.B777F;
            }
        }

        public void ImportValues()
        {
            readWtUnit();
            setAircraftType();

            tryImportPattern1(ref Zfw, frm.ZFW.Text, "Invalid ZFW.");
            //need a better one
            tryImportPattern1(ref ContPerc, frm.ContPercentToDest.Text, "Invalid contingency fuel.");
            tryImportPattern1(ref MissedAppFuel, frm.MissedAppFuel.Text, "Invalid missed approach fuel.");
            tryImportPattern1(ref HoldingMin, frm.HoldTime_TxtBox.Text, "Invalid holding time.");
            tryImportPattern1(ref ExtraFuel, frm.ExtraFuel.Text, "Invalid extra fuel.");
            tryImportPattern1(ref APUTime, frm.APUTime.Text, "Invalid APU time.");
            tryImportPattern1(ref TaxiTime, frm.TaxiTime.Text, "Invalid taxi time.");
            tryImportPattern1(ref FinalRsvMin, frm.FinalRsv.Text, "Invalid time for final reserve.");
            DisToDest = RouteFindingCore.RouteToDest.TotalDistance;
            DisToAltn = RouteFindingCore.RouteToAltn.TotalDistance;

            AvgWindToDest = 0;
            AvgWindToAltn = 0;

            setPropertyKG();

        }


        private void tryImportPattern1(ref double var, string str, string exceptionMsg)
        {
            try
            {
                var = Convert.ToDouble(str);

                if (var < 0)
                {
                    throw new Exception(exceptionMsg);
                }
            }
            catch
            {
                throw new Exception(exceptionMsg);
            }
        }

        private void tryImportPattern2(ref double var, string str, string exceptionMsg)
        {
            try
            {
                var = Convert.ToDouble(str);

                if (var < -400 || var > 400)
                {
                    throw new Exception(exceptionMsg);
                }

            }
            catch
            {
                throw new Exception(exceptionMsg);
            }
        }

        public void FillInDefaultValueIfLeftBlank()
        {
            if (string.IsNullOrEmpty(frm.FinalRsv.Text))
            {
                frm.FinalRsv.Text = "30";
            }

            if (string.IsNullOrEmpty(frm.ContPercentToDest.Text))
            {
                frm.ContPercentToDest.Text = "5";
            }

            if (string.IsNullOrEmpty(frm.ExtraFuel.Text))
            {
                frm.ExtraFuel.Text = "0";
            }

            if (string.IsNullOrEmpty(frm.APUTime.Text))
            {
                frm.APUTime.Text = "30";
            }

            if (string.IsNullOrEmpty(frm.TaxiTime.Text))
            {
                frm.TaxiTime.Text = "20";
            }

            if (string.IsNullOrEmpty(frm.HoldTime_TxtBox.Text))
            {
                frm.HoldTime_TxtBox.Text = "0";
            }
        }
    }
}

