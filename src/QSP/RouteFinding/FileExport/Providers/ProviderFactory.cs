using QSP.RouteFinding.Airports;
using QSP.RouteFinding.Routes;
using System;

namespace QSP.RouteFinding.FileExport.Providers
{
    public static class ProviderFactory
    {
        public static IExportProvider GetProvider(
            ProviderType type,
            Route route,
            AirportManager airports)
        {
            switch (type)
            {
                case ProviderType.Pmdg:
                    return new PmdgProvider(route, airports);

                case ProviderType.Fsx:
                    return new FsxProvider(route, airports);

                case ProviderType.Fs9:
                    return new Fs9Provider(route, airports);

                default:
                    throw new ArgumentException();
            }
        }
    }
}
