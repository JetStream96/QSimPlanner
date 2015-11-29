using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSP.Core;

namespace QSP.LandingPerfCalculation
{
    public static class LoadedData
    {
        private struct acPara
        {
            public Aircraft ac;
            public PerfData para;

            public acPara(Aircraft ac, PerfData para)
            {
                this.ac = ac;
                this.para = para;
            }
        }

        private static List<acPara> para;

        public static void Load()
        {
            para = new List<acPara>();
            para.Add(new acPara(Aircraft.B737600, PerfImporter.ReadFromXml(@"PerformanceData\LDG\737600_LDG.xml")));
            para.Add(new acPara(Aircraft.B737700, PerfImporter.ReadFromXml(@"PerformanceData\LDG\737700_LDG.xml")));
            para.Add(new acPara(Aircraft.B737800, PerfImporter.ReadFromXml(@"PerformanceData\LDG\737800_LDG.xml")));
            para.Add(new acPara(Aircraft.B737900, PerfImporter.ReadFromXml(@"PerformanceData\LDG\737900_LDG.xml")));
            para.Add(new acPara(Aircraft.B777200LR, PerfImporter.ReadFromXml(@"PerformanceData\LDG\777200LR_LDG.xml")));
            para.Add(new acPara(Aircraft.B777F, PerfImporter.ReadFromXml(@"PerformanceData\LDG\777F_LDG.xml")));
        }

        public static PerfData GetPara(Aircraft ac)
        {
            foreach (var i in para)
            {
                if (i.ac == ac)
                {
                    return i.para;
                }
            }
            throw new EnumNotSupportedException();
        }
    }
}
