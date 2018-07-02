using QSP.Common.Options;
using System;
using System.IO;
using static QSP.RouteFinding.FileExport.Providers.Types;

namespace QSP.RouteFinding.FileExport
{
    public interface IExportPath
    {
        /// <summary>
        /// The returned path may not exist.
        /// </summary>
        string FullPath(SimulatorType Type, AppOptions Option);
    }

    public class AbsolutePath : IExportPath
    {
        private readonly string absolutePath;
        public AbsolutePath(string absolutePath) { this.absolutePath = absolutePath; }

        /// <summary>
        /// Option is not needed and can be null.
        /// </summary>
        public string FullPath(SimulatorType Type, AppOptions Option = null) => absolutePath;
    }

    /// <summary>
    /// A path relative to the root of simulator path.
    /// </summary>
    public class RelativePath : IExportPath
    {
        private readonly string relativePath;
        public RelativePath(string relativePath) { this.relativePath = relativePath; }
        public string FullPath(SimulatorType Type, AppOptions Option) =>
            Path.Combine(Option.SimulatorPaths[Type], relativePath);
    }
}
