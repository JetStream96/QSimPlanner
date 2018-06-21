using QSP.RouteFinding.Airports;
using QSP.RouteFinding.Routes;
using System;
using System.Collections.Generic;
using static QSP.LibraryExtension.Types;

namespace QSP.RouteFinding.FileExport.Providers
{
    public enum ProviderType
    {
        Pmdg = 0,
        Fsx = 1,
        Fs9 = 2,
        AerosoftAirbus = 3,
        FlightFactor777 = 4,
        FlightFactorA320 = 5,
        Ifly737 = 6,
        Ifly747v2 = 7,
        JarDesignAirbus = 8,
        PmdgWind = 9,
        Xplane = 10
    }

    public static class Types
    {
        private static readonly
            IReadOnlyDictionary<ProviderType, (string, Func<ExportInput, string>)> lookup =
                new Dictionary<ProviderType, (string, Func<ExportInput, string>)>
                {
                    // [type] = (file_extension, export_method)
                    [ProviderType.Pmdg] = (".rte", PmdgProvider.GetExportText),
                    [ProviderType.Fsx] = (".PLN", FsxProvider.GetExportText),
                    [ProviderType.Fs9] = (".PLN", Fs9Provider.GetExportText),
                    [ProviderType.AerosoftAirbus] = ("", AerosoftAirbusProvider.GetExportText),
                    [ProviderType.FlightFactor777] = ("", AerosoftAirbusProvider.GetExportText),
                    [ProviderType.FlightFactorA320] = ("", FlightFactorA320Provider.GetExportText),
                    [ProviderType.Ifly737] = ("", Ifly737Provider.GetExportText),
                    [ProviderType.Ifly747v2] = ("", Ifly747v2Provider.GetExportText),
                    [ProviderType.JarDesignAirbus] = ("", JarDesignAirbusProvider.GetExportText),
                    [ProviderType.PmdgWind] = ("", PmdgWindUplinkProvider.GetExportText),
                    [ProviderType.Xplane] = ("", XplaneProvider.GetExportText)
                };

        public static string GetExtension(ProviderType type) => lookup[type].Item1;

        public static string GetExportText(ProviderType type, Route route,
            AirportManager airports)
        {
            var input = new ExportInput()
            {
                Route = route,
                Airports = airports
            };

            return lookup[type].Item2(input);
        }
    }
}
