using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP.Core;
using QSP.RouteFinding.Data;
using System;

namespace UnitTest.Common
{
    public static class Utilities
    {
        public static bool WithinPrecisionPercent(double expected, double actual, double tolerancePercent)
        {
            return (Math.Abs(actual - expected) <= actual * tolerancePercent / 100.0);
        }

        public static bool WithinPrecision(double actual, double expected, double tolerance)
        {
            return (Math.Abs(actual - expected) <= tolerance);
        }

        public static bool IsBetween(int num, int upper, int lower)
        {
            return (num >= upper && num <= lower);
        }

        //
        public const string navDBLoc = "F:\\FSX\\aerosoft\\Airbus_Fallback\\Navigraph";

        public static bool navDBready = false;
        [TestMethod()]

        public static void PrepareTest()
        {
            QspCore.AppSettings.NavDataLocation = navDBLoc;


            if (navDBready == false)
            {
                new NavDataLoader(navDBLoc).LoadAllData();
                navDBready = true;

            }

        }

    }
}
