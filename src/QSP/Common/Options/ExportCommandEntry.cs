using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSP.RouteFinding.FileExport.Providers;
using QSP.RouteFinding.FileExport;

namespace QSP.Common.Options
{
    public class ExportCommandEntry
    {
        public ExportCommand Command { get; private set; }
        public string Key { get; private set; }

        public ExportCommandEntry(ExportCommand Command, string Key)
        {
            this.Command = Command;
            this.Key = Key;
        }
    }
}
