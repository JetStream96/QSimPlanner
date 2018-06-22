using QSP.Common.Options;
using QSP.LibraryExtension;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.Routes;
using QSP.Utilities;
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
        Xplane = 10,
        FsxSteam = 11
    }

    public static class Types
    {
        private class Match
        {
            public string FileExtension { get; }
            public string DisplayName { get; }
            public Func<ExportInput, string> Export { get; }
            public IEnumerable<SimulatorType> SupportedSims { get; }

            public Match(string FileExtension, string DisplayName,
                Func<ExportInput, string> Export, IEnumerable<SimulatorType> SupportedSims)
            {
                this.FileExtension = FileExtension;
                this.DisplayName = DisplayName;
                this.Export = Export;
                this.SupportedSims = SupportedSims;
            }
        }

        private static readonly IEnumerable<SimulatorType> FSXP3D = Arr
        (
            SimulatorType.FSX,
            SimulatorType.FSX_Steam,
            SimulatorType.P3Dv1,
            SimulatorType.P3Dv2,
            SimulatorType.P3Dv3,
            SimulatorType.P3Dv4
        );

        private static readonly IEnumerable<SimulatorType> Xplane = Arr(SimulatorType.Xplane11);

        private static readonly IReadOnlyDictionary<ProviderType, Match> lookup = Dict
        (
            (ProviderType.Pmdg,
             new Match(".rte", "PMDG", PmdgProvider.GetExportText, FSXP3D)),

            (ProviderType.Fsx,
             new Match(".PLN", "FSX", FsxProvider.GetExportText, Arr(SimulatorType.FSX))),

            (ProviderType.Fs9,
             new Match(".PLN", "FS9", Fs9Provider.GetExportText, Arr(SimulatorType.FS9))),

            (ProviderType.AerosoftAirbus,
             new Match("Aerosoft Airbus", "", AerosoftAirbusProvider.GetExportText, FSXP3D)),

            (ProviderType.FlightFactor777,
             new Match("Flight Factor 777", "", AerosoftAirbusProvider.GetExportText, Xplane)),

            (ProviderType.FlightFactorA320,
             new Match("Flight Factor A320", "", FlightFactorA320Provider.GetExportText, Xplane)),

            (ProviderType.Ifly737,
             new Match("Ifly 737", "", Ifly737Provider.GetExportText, FSXP3D)),

            (ProviderType.Ifly747v2,
             new Match("Ifly 747 v2", "", Ifly747v2Provider.GetExportText, FSXP3D)),

            (ProviderType.JarDesignAirbus,
             new Match("JarDesign Airbus", "", JarDesignAirbusProvider.GetExportText, Xplane)),

            (ProviderType.PmdgWind,
             new Match("Pmdg wind uplink", "", PmdgWindUplinkProvider.GetExportText, FSXP3D)),

            (ProviderType.Xplane,
             new Match("X-plane", "", XplaneProvider.GetExportText, Xplane)),

            (ProviderType.FsxSteam,
             new Match("Fsx: Steam edition", ".PLN", FsxProvider.GetExportText,
                       Arr(SimulatorType.FSX_Steam)))
        );

        /// <summary>
        /// Returns the detected the simulator path. If it is not found or an
        /// error occurred, returns null.
        /// </summary>
        public static string GetSimulatorPath(SimulatorType t)
        {
            return t.MatchCaseDefault
            (
                null,

                (SimulatorType.FSX, SimulatorPath.FsxPath()),
                (SimulatorType.FSX_Steam, SimulatorPath.FsxSteamPath()),
                // Ignore FS9
                (SimulatorType.P3Dv1, SimulatorPath.P3Dv1Path()),
                (SimulatorType.P3Dv2, SimulatorPath.P3Dv2Path()),
                (SimulatorType.P3Dv3, SimulatorPath.P3Dv3Path()),
                (SimulatorType.P3Dv4, SimulatorPath.P3Dv4Path()),
                (SimulatorType.Xplane11, SimulatorPath.Xplane11Path())
            );
        }

        public static string GetExtension(ProviderType type) => lookup[type].FileExtension;

        public static string GetExportText(ProviderType type, Route route,
            AirportManager airports)
        {
            var input = new ExportInput()
            {
                Route = route,
                Airports = airports
            };

            return lookup[type].Export(input);
        }
    }
}
