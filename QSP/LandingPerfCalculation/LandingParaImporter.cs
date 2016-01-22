using System;
using static QSP.UI.Utilities;
using QSP.Core;

namespace QSP.LandingPerfCalculation
{

    public static class ParameterImporter
    {

        public static LandingParameters Import()
        {

            double WeightKG = 0;
            double RwyLengthMeter = 0;
            int ElevationFT = 0;
            int HeadwindKts = 0;
            double SlopePercent = 0;
            double TempCelsius = 0;
            int AppSpeedIncrease = 0;
            ReverserOption Reverser = default(ReverserOption);
            SurfaceCondition SurfaceCondition = default(SurfaceCondition);
            int FlapsIndex = 0;
            int AutoBrakeIndex = 0;
            MainForm frm = MainFormInstance();

            try
            {
                WeightKG = Convert.ToDouble(frm.Weight_LDG.Text);
                if (frm.WtUnit_LDG.Text == "LB")
                {
                    WeightKG *= AviationTools.Constants.LB_KG;
                }

                if (WeightKG < 0)
                {
                    throw new InvalidUserInputException("Landing weight is not valid.");
                }

            }
            catch
            {
                throw new InvalidUserInputException("Landing weight is not valid.");
            }
            
            try
            {
                RwyLengthMeter = Convert.ToDouble(frm.length_LDG.Text);

                if (frm.m_ft_LDG.Text == "FT")
                {
                    RwyLengthMeter *= AviationTools.Constants.FT_M_ratio;
                }

                if (RwyLengthMeter < 0)
                {
                    throw new InvalidUserInputException("Runway length is not valid.");
                }

            }
            catch
            {
                throw new InvalidUserInputException("Runway length is not valid.");
            }


            try
            {
                ElevationFT = Convert.ToInt32(frm.elevation_LDG.Text);

                if (ElevationFT < -2000 || ElevationFT > 20000)
                {
                    throw new InvalidUserInputException("Runway elevation is not valid.");
                }

            }
            catch
            {
                throw new InvalidUserInputException("Runway elevation is not valid.");
            }


            try
            {
                HeadwindKts = Convert.ToInt32(Math.Cos(MathTools.Utilities.ToRadian(Convert.ToInt32(frm.RwyHeading_LDG.Text) - Convert.ToInt32(frm.winddir_LDG.Text))) * Convert.ToInt32(frm.windspd_LDG.Text));

            }
            catch
            {
                throw new InvalidUserInputException("Wind entry is not valid.");
            }

            try
            {
                SlopePercent = Convert.ToDouble(frm.Slope_LDG.Text);
            }
            catch 
            {
                throw new InvalidUserInputException("Runway slope is not valid.");
            }

            try
            {
                TempCelsius = Convert.ToDouble(frm.oat_LDG.Text);

                if (frm.temp_c_f_LDG.SelectedIndex == 1)
                {
                    TempCelsius = AviationTools.CoversionTools.ToCelsius(TempCelsius);
                }

            }
            catch 
            {
                throw new InvalidUserInputException("OAT is not valid.");
            }

            try
            {
                AppSpeedIncrease = Convert.ToInt32(frm.AppSpdInc_LDG.Text);
            }
            catch 
            {
                throw new InvalidUserInputException("APP speed increase is not valid.");
            }

            try
            {
                Reverser = (ReverserOption)frm.REVSel_LDG.SelectedIndex;
            }
            catch 
            {
                throw new InvalidUserInputException("Reverser selection is not valid.");
            }

            try
            {
                SurfaceCondition = (SurfaceCondition)frm.SurfCond_LDG.SelectedIndex;
            }
            catch 
            {
                throw new InvalidUserInputException("Surface condition is not valid.");
            }


            FlapsIndex = frm.Flaps_LDG.SelectedIndex;
            AutoBrakeIndex = frm.BrakeSel_LDG.SelectedIndex;

            return new LandingParameters(Convert.ToInt32(WeightKG), Convert.ToInt32(RwyLengthMeter), ElevationFT, HeadwindKts, SlopePercent, Convert.ToInt32(TempCelsius), AppSpeedIncrease, Reverser, SurfaceCondition, FlapsIndex,
            AutoBrakeIndex);

        }

    }

}
