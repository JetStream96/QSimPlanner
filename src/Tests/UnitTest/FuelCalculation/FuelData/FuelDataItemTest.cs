using System.Xml.Linq;
using NUnit.Framework;
using QSP.FuelCalculation.FuelDataNew;

namespace UnitTest.FuelCalculation.FuelData
{
    [TestFixture]
    public class FuelDataItemTest
    {
        [Test]
        public void DeserializeTest()
        {
            var serializer = new FuelDataItem.Serializer();
            var doc = XDocument.Parse(FileText);
            var item = serializer.Deserialize(doc.Root);
        }

        public static readonly string FileText =
@"<?xml version=""1.0"" encoding=""utf-8""?>
<Data>
  <!-- 
       Unless specified otherwise,
       all weights in kg.
       all speeds in knots.
       all altitudes in feet.
       all fuel flows(FF) in kg/min.
  -->

  <ProfileName>Boeing 737-800</ProfileName>
  <HoldingFuelPerMinute>37.7</HoldingFuelPerMinute>
  <HoldingFuelRefWt>60000</HoldingFuelRefWt>
  <TaxiFuelPerMin>12.2</TaxiFuelPerMin>
  <ApuFuelPerMin>1.8</ApuFuelPerMin>
  <MissedAppFuel>130</MissedAppFuel>
  <ClimbIAS>280</ClimbIAS>
  <DescentIAS>280</DescentIAS>

  <DataPoint>
    <Weight>40000</Weight>
    <CruiseFF>27.1</CruiseFF>
    <CruiseIAS>215.9772</CruiseIAS>
    <ClimbGrad>0.08997</ClimbGrad>
    <ClimbFF>85.416666666666671</ClimbFF>
    <DescentGrad>0.06681</DescentGrad>
    <DescentFF>13.083333333333334</DescentFF>
    <OptCruiseAlt>43828.6</OptCruiseAlt>
    <EtopsCruiseTAS>0</EtopsCruiseTAS>
    <EtopsCruiseFF>0</EtopsCruiseFF>
  </DataPoint>

  <DataPoint>
    <Weight>85000</Weight>
    <CruiseFF>53.06333333333334</CruiseFF>
    <CruiseIAS>297.1032</CruiseIAS>
    <ClimbGrad>0.04299</ClimbGrad>
    <ClimbFF>94.13333333333334</ClimbFF>
    <DescentGrad>0.04661</DescentGrad>
    <DescentFF>14.416666666666666</DescentFF>
    <OptCruiseAlt>30071.65</OptCruiseAlt>
    <EtopsCruiseTAS>0</EtopsCruiseTAS>
    <EtopsCruiseFF>0</EtopsCruiseFF>
  </DataPoint>

</Data>";
    }
}