using QSP.RouteFinding.Airports;
using QSP.RouteFinding.Routes;
using System;
using System.Collections.Generic;
using static QSP.LibraryExtension.Types;

namespace QSP.RouteFinding.FileExport.Providers
{
    public enum ProviderType
    {
        // Do not change the int associated with each item. 
        // They are stored in app options in users' files.

        Pmdg = 0,
        Fsx = 1,
        Fs9 = 2
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
            [ProviderType.Fs9] = (".PLN", Fs9Provider.GetExportText)
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
