using Moq;
using QSP.RouteFinding.Airports;

namespace IntegrationTest.QSP.RouteFinding.TestSetup
{
    public static class Mocks
    {
        public static IAirport GetAirport(string icao)
        {
            var a = new Mock<IAirport>();
            a.Setup(i => i.Icao).Returns(icao);
            return a.Object;
        }
    }
}
