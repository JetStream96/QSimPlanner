using QSP.Common.Options;
using QSP.LibraryExtension;
using QSP.LibraryExtension.Sets;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.Routes;
using QSP.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        FsxSteam = 11,
        P3Dv1 = 12,
        P3Dv2 = 13,
        P3Dv3 = 14,
        P3Dv4 = 15
    }

    public static class Types
    {
        public class SimTypePath
        {
            public SimulatorType Type { get; }
            public IExportPath Path { get; }

            public SimTypePath(SimulatorType Type, IExportPath Path)
            {
                this.Type = Type;
                this.Path = Path;
            }
        }

        public class Match
        {
            public string FileExtension { get; }
            public string DisplayName { get; }
            public Func<ExportInput, string> Export { get; }
            public IEnumerable<SimTypePath> SupportedSims { get; }

            public Match(string FileExtension,
                string DisplayName,
                Func<ExportInput, string> Export,
                IEnumerable<SimTypePath> SupportedSims)
            {
                this.FileExtension = FileExtension;
                this.DisplayName = DisplayName;
                this.Export = Export;
                this.SupportedSims = SupportedSims;
            }
        }

        private static IEnumerable<SimulatorType> FSXP3D = Arr
        (
            SimulatorType.FSX,
            SimulatorType.FSX_Steam,
            SimulatorType.P3Dv1,
            SimulatorType.P3Dv2,
            SimulatorType.P3Dv3,
            SimulatorType.P3Dv4
        );

        private static IEnumerable<SimTypePath> FSXP3DRelative(string relativePath) =>
            FSXP3D.Select(t => new SimTypePath(t, new RelativePath(relativePath)));

        private static IEnumerable<SimTypePath> Xplane(string relativePath) =>
            Arr(new SimTypePath(SimulatorType.Xplane11, new RelativePath(relativePath)));

        private static readonly string FSXDocumentFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "Flight Simulator X Files");
        
        // Some directories may be missing because they cannot be found. 
        public static readonly IReadOnlyDictionary<ProviderType, Match> Lookup = Dict
        (
            (ProviderType.Pmdg,
             new Match(".rte", "PMDG", PmdgProvider.GetExportText,
                FSXP3DRelative("PMDG/FLIGHTPLANS"))),

            (ProviderType.Fsx,
             new Match(".PLN", "FSX", FsxProvider.GetExportText,
                 Arr(new SimTypePath(SimulatorType.FSX, new AbsolutePath(FSXDocumentFolder))))),

            (ProviderType.Fs9,
             new Match(".PLN", "FS9", Fs9Provider.GetExportText,
                 Arr(new SimTypePath(SimulatorType.FS9, new AbsolutePath(Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    "Flight Simulator File")))))),

            (ProviderType.AerosoftAirbus,
             new Match(".flp", "Aerosoft Airbus", AerosoftAirbusProvider.GetExportText,
                 FSXP3D.Select(t => new SimTypePath(t, new AbsolutePath(Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    "Aerosoft/Airbus/Flightplans")))))),

            (ProviderType.FlightFactor777,
             new Match(".flp", "Flight Factor 777", AerosoftAirbusProvider.GetExportText,
                 Xplane(""))),

            (ProviderType.FlightFactorA320,
             new Match(".in", "Flight Factor A320", FlightFactorA320Provider.GetExportText,
                 Xplane("Aircraft/A320/data"))),

            (ProviderType.Ifly737,
             new Match(".FLTPLAN", "Ifly 737", Ifly737Provider.GetExportText,
                 FSXP3DRelative("iFly/737NG/navdata/FLTPLAN"))),

            (ProviderType.Ifly747v2,
             new Match(".route", "Ifly 747 v2", Ifly747v2Provider.GetExportText,
                 FSXP3DRelative("iFly/744/navdata/FLTPLAN"))),

            (ProviderType.JarDesignAirbus,
             new Match(".txt", "JarDesign Airbus", JarDesignAirbusProvider.GetExportText,
                 Xplane("Aircraft/Heavy Metal/320JARDesign/FlightPlans"))),

            (ProviderType.PmdgWind,
             new Match(".wx", "Pmdg wind uplink", PmdgWindUplinkProvider.GetExportText,
                 FSXP3DRelative("PMDG/WX"))),

            (ProviderType.Xplane,
             new Match(".fms", "X-plane", XplaneProvider.GetExportText,
                 Xplane("Output/FMS plans"))),

            (ProviderType.FsxSteam,
             new Match(".PLN", "Fsx: Steam edition", FsxProvider.GetExportText,
                Arr(new SimTypePath(SimulatorType.FSX_Steam, new AbsolutePath(Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    "Flight Simulator X - Steam Edition Files﻿")))))),

            (ProviderType.P3Dv1,
             new Match(".PLN", "P3D v1", FsxProvider.GetExportText,
                Arr(new SimTypePath(SimulatorType.P3Dv1, new AbsolutePath(Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    "Prepar3D v1 Files")))))),

            (ProviderType.P3Dv2,
             new Match(".PLN", "P3D v2", FsxProvider.GetExportText,
                Arr(new SimTypePath(SimulatorType.P3Dv2, new AbsolutePath(Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    "Prepar3D v2 Files")))))),

            (ProviderType.P3Dv3,
             new Match(".PLN", "P3D v3", FsxProvider.GetExportText,
                Arr(new SimTypePath(SimulatorType.P3Dv3, new AbsolutePath(Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    "Prepar3D v3 Files")))))),

            (ProviderType.P3Dv4,
             new Match(".PLN", "P3D v4", FsxProvider.GetExportText,
                Arr(new SimTypePath(SimulatorType.P3Dv4, new AbsolutePath(Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    "Prepar3D v4 Files"))))))
        );

        public static IReadOnlySet<ExportCommand> DefaultExportCommands()
        {
            return Lookup.Select(kv =>
            {
                var sim = kv.Value.SupportedSims.ToList();
                var defaultSim = (sim.Count > 0) ? (SimulatorType?)sim[0].Type : null;
                return new ExportCommand(kv.Key, "", false, defaultSim);
            }).ToReadOnlySet();
        }

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

        public static string GetExtension(ProviderType type) => Lookup[type].FileExtension;

        public static string GetExportText(ProviderType type, Route route,
            AirportManager airports)
        {
            var input = new ExportInput()
            {
                Route = route,
                Airports = airports
            };

            return Lookup[type].Export(input);
        }

        public static readonly IReadOnlyDictionary<SimulatorType, string> SimDisplayName = 
            Dict
        (
            (SimulatorType.FSX, "FSX"),
            (SimulatorType.FSX_Steam, "FSX: Steam edition"),
            (SimulatorType.FS9, "FS9"),
            (SimulatorType.P3Dv1, "P3D v1"),
            (SimulatorType.P3Dv2, "P3D v2"),
            (SimulatorType.P3Dv3, "P3D v3"),
            (SimulatorType.P3Dv4, "P3D v4"),
            (SimulatorType.Xplane11, "X-plane")
        );

        /// <summary>
        /// Gets the export directory for the specified simulator.
        /// Returned path may be null or not exist.
        /// </summary>
        /// <param name="sim">null if using the custom export directory</param>
        /// <exception cref="Exception">The ExportCommand does not support
        /// the given SimulatorType</exception>
        public static string ExportDirectory(SimulatorType? sim, ExportCommand c,
            AppOptions option)
        {
            if (sim == null) return c.CustomDirectory;
            return Lookup[c.ProviderType].SupportedSims
                .First(x => x.Type == sim.Value)
                .Path
                .FullPath(sim.Value, option);
        }
    }
}
