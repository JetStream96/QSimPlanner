using QSP.RouteFinding.Airports;
using QSP.RouteFinding.Routes;
using QSP.Common;

namespace QSP.RouteFinding.FileExport.Providers
{
    public static class ProviderFactory
    {
        public static IExportProvider GetProvider(
            ProviderType type,
            ManagedRoute route, 
            AirportManager airports)
        {
            switch (type)
            {
                case ProviderType.Pmdg:
                    return new PmdgProvider(route, airports);

                default:
                    throw new EnumNotSupportedException();
            }
        }
    }
}
