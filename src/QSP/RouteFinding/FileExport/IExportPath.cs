using QSP.Common.Options;
using System.IO;
using static QSP.RouteFinding.FileExport.Providers.Types;

namespace QSP.RouteFinding.FileExport
{
    public interface IExportPath
    {
        string FullPath(SimulatorType Type);
    }

    public class AbsolutePath : IExportPath
    {
        private readonly string absolutePath;
        public AbsolutePath(string absolutePath) { this.absolutePath = absolutePath; }
        public string FullPath(SimulatorType Type) => absolutePath;
    }

    /// <summary>
    /// A path relative to the root of simulator path.
    /// </summary>
    public class RelativePath : IExportPath
    {
        private readonly string relativePath;
        public RelativePath(string relativePath) { this.relativePath = relativePath; }
        public string FullPath(SimulatorType Type) =>
            Path.Combine(GetSimulatorPath(Type), relativePath);
    }
}
