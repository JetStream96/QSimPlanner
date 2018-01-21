using FakeItEasy;
using QSP.RouteFinding.Airports;

namespace IntegrationTest.QSP.RouteFinding.TestSetup
{
    public static class Mocks
    {
        public static IAirport GetAirport(string icao)
        {
            var a = A.Fake<IAirport>();
            A.CallTo(() => a.Icao).Returns(icao);
            return a;
        }
    }
}
