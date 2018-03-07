using QSP.RouteFinding.Airports;
using QSP.RouteFinding.Routes;
using System;

namespace QSP.RouteFinding.FileExport.Providers
{
    public enum ProviderType
    {
        Pmdg = 0,
        Fsx = 1,
        Fs9 = 2
    }

    public static class Types
    {
        public static string GetExtension(ProviderType type)
        {
            switch (type)
            {
                case ProviderType.Pmdg:
                    return ".rte";

                case ProviderType.Fsx:
                case ProviderType.Fs9:
                    return ".PLN";

                default:
                    throw new ArgumentException();
            }
        }

        public static string GetExportText(ProviderType type, Route route,
            AirportManager airports)
        {
            switch (type)
            {
                case ProviderType.Pmdg:
                    return PmdgProvider.GetExportText(route, airports);

                case ProviderType.Fsx:
                    return FsxProvider.GetExportText(route, airports);

                case ProviderType.Fs9:
                    return Fs9Provider.GetExportText(route, airports);

                default:
                    throw new ArgumentException();
            }
        }
    }
}
