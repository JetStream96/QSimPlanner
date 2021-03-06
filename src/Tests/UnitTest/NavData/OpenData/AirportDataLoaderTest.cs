﻿using NUnit.Framework;
using QSP.NavData.OpenData;
using QSP.RouteFinding.Airports;
using System;
using System.Linq;

namespace UnitTest.NavData.OpenData
{
    [TestFixture]
    public class AirportDataLoaderTest
    {
        private const double delta = 0.005;

        [Test]
        public void LoadFromFileTest()
        {
            var directory = AppDomain.CurrentDomain.BaseDirectory;
            var loader = new AirportDataLoader(directory + @"\NavData\OpenData");

            var airports = loader.LoadFromFile();

            // Airport parameter incomplete - should not load.
            Assert.IsNull(airports["37NJ"]);

            var vhhh = airports["VHHH"];

            AssertVhhh(vhhh);
            AssertVhhhRwys(vhhh);
        }

        private static void AssertVhhh(IAirport vhhh)
        {
            Assert.IsTrue("VHHH" == vhhh.Icao);
            Assert.IsTrue("Chek Lap Kok International Airport" == vhhh.Name);
            Assert.AreEqual(22.3089008331, vhhh.Lat, delta);
            Assert.AreEqual(113.915000916, vhhh.Lon, delta);
            Assert.AreEqual(28, vhhh.Elevation);
            Assert.AreEqual(false, vhhh.TransAvail);
            Assert.AreEqual(12467, vhhh.LongestRwyLengthFt);
        }
        private static void AssertVhhhRwys(IAirport vhhh)
        {
            var rwys = vhhh.Rwys;

            var _7L = rwys.Where(r => r.RwyIdent == "07L").First();
            Assert.IsTrue(_7L.Heading == "74");
            Assert.AreEqual(12467, _7L.LengthFt);
            Assert.AreEqual(197, _7L.WidthFt);
            Assert.AreEqual(false, _7L.HasIlsInfo);
            Assert.AreEqual(22.3104, _7L.Lat, delta);
            Assert.AreEqual(113.896, _7L.Lon, delta);
            Assert.AreEqual(22, _7L.ElevationFt);
            Assert.IsTrue("ASP" == _7L.SurfaceType);

            var _7R = rwys.Where(r => r.RwyIdent == "07R").First();
            Assert.IsTrue(_7R.Heading == "74");
            Assert.AreEqual(12467, _7R.LengthFt);
            Assert.AreEqual(197, _7R.WidthFt);
            Assert.AreEqual(false, _7R.HasIlsInfo);
            Assert.AreEqual(22.2962, _7R.Lat, delta);
            Assert.AreEqual(113.898, _7R.Lon, delta);
            Assert.AreEqual(28, _7R.ElevationFt);
            Assert.IsTrue("ASP" == _7R.SurfaceType);

            var _25R = rwys.Where(r => r.RwyIdent == "25R").First();
            Assert.IsTrue(_25R.Heading == "254");
            Assert.AreEqual(12467, _25R.LengthFt);
            Assert.AreEqual(197, _25R.WidthFt);
            Assert.AreEqual(false, _25R.HasIlsInfo);
            Assert.AreEqual(22.3216, _25R.Lat, delta);
            Assert.AreEqual(113.931, _25R.Lon, delta);
            Assert.AreEqual(23, _25R.ElevationFt);
            Assert.IsTrue("ASP" == _25R.SurfaceType);

            var _25L = rwys.Where(r => r.RwyIdent == "25L").First();
            Assert.IsTrue(_25L.Heading == "254");
            Assert.AreEqual(12467, _25L.LengthFt);
            Assert.AreEqual(197, _25L.WidthFt);
            Assert.AreEqual(false, _25L.HasIlsInfo);
            Assert.AreEqual(22.3074, _25L.Lat, delta);
            Assert.AreEqual(113.933, _25L.Lon, delta);
            Assert.AreEqual(27, _25L.ElevationFt);
            Assert.IsTrue("ASP" == _25L.SurfaceType);
        }

    }
}
