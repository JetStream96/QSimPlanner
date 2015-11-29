using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP.RouteFinding.Data;
using QSP;
using QSP.Core;

namespace UnitTest.Common
{
    public static class Utilities
    {
        public static bool WithinPrecisionPercent(double actual, double expected, double tolerancePercent)
        {
            if (Math.Abs(actual - expected) <= actual * tolerancePercent / 100)
            {
                return true;
            }
            return false;
        }

        public static bool WithinPrecision(double actual, double expected, double tolerance)
        {
            if (Math.Abs(actual - expected) <=  tolerance)
            {
                return true;
            }
            return false;
        }

        //
        public const string navDBLoc = "F:\\FSX\\aerosoft\\Airbus_Fallback\\Navigraph";

        public static bool navDBready = false;
        [TestMethod()]

        public static void PrepareTest()
        {
            QspCore.AppSettings.NavDBLocation = navDBLoc;


            if (navDBready == false)
            {
                DatabaseLoader.LoadAllDB(navDBLoc);
                navDBready = true;

            }

        }
        
    }
}
