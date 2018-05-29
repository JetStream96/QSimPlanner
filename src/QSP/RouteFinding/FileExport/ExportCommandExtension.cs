using QSP.RouteFinding.FileExport.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSP.RouteFinding.FileExport
{
    public static class ExportCommandExtension
    {
        public static string Extension(this ExportCommand c)
        {
            return Types.GetExtension(c.ProviderType);
        }
    }
}
