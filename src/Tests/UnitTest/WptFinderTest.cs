using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using static QSP.MathTools.Utilities;
using static QSP.RouteFinding.RouteFindingCore;
using static UnitTest.Common.Utilities;
using QSP.AviationTools.Coordinates;

namespace UnitTest
{

    [TestClass()]
    public class WptFinderTest
    {

        [TestMethod()]
        public void WptFinderTest_ResultSameAsSlowMethod()
        {
            PrepareTest();

            var v = new LatLon(-80.0, 55.6685);
            double dis = 1000.0;

            var x = slowMethod(v, dis);
            var y = fastMethod(v, dis);

            Assert.AreEqual(true, listsAreEqual(x, y));
        }

        public static bool listsAreEqual(List<string> item1, List<string> item2)
        {
            bool found = false;

            foreach (var i in item1)
            {
                found = false;

                foreach (var j in item2)
                {
                    if (i == j)
                    {
                        found = true;
                        break;
                    }
                }

                if (found == false)
                {
                    return false;
                }
            }
            return true;
        }

        public static List<string> fastMethod(LatLon latLon, double dis)
        {
            var result = WptList.Find(latLon.Lat, latLon.Lon, dis);
            List<string> stringResult = new List<string>();

            foreach (var item in result)
            {
                stringResult.Add(WptList[item.Index].ID);
            }

            return stringResult;
        }


        public static List<string> slowMethod(LatLon latLon, double dis)
        {
            List<string> result = new List<string>();

            for (int i = 0; i < WptList.Count; i++)
            {
                if (GreatCircleDistance(latLon, WptList.LatLonAt(i)) <= dis)
                {
                    result.Add(WptList[i].ID);
                }
            }

            return result;
        }

    }
}
