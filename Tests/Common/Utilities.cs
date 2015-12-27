using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP.Core;
using QSP.RouteFinding.Data;
using System;

namespace Tests.Common
{
    public static class Utilities
    {
        public static bool WithinPrecisionPercent(double expected, double actual, double tolerancePercent)
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

        public static bool IsBetween(int num,int upper,int lower)
        {
            if(num>=upper && num <=lower)
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
