using QSP.Common.Options;
using QSP.Utilities;
using System.IO;

namespace QSP.RouteFinding.FileExport
{
    public interface IExportPath
    {
        /// <summary>
        /// The returned path may be null, if the root directory of
        /// the given simulator is not set in AppOptions.
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

        /// <summary>
        /// Return value may be null, or the path may not exist.
        /// </summary>
        public string FullPath(SimulatorType Type, AppOptions Option)
        {
            var simPath = Option.SimulatorPaths[Type];
            return ExceptionHelpers.DefaultIfThrows(
                 () => Path.GetFullPath(Path.Combine(simPath, relativePath)),
                 null);
        }
    }
}
