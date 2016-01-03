using System.Collections.Generic;
using QSP.Core;

namespace QSP.TakeOffPerfCalculation
{
    public static class LoadedData
    {

        private struct acPara
        {
            public Aircraft ac;
            public AircraftParameters para;

            public acPara(Aircraft ac, AircraftParameters para)
            {
                this.ac = ac;
                this.para = para;
            }
        }

        private static List<acPara> para;

        public static void Load()
        {
            para = new List<acPara>();
            para.Add(new acPara(Aircraft.B737600, new AircraftParameters(Aircraft.B737600)));
            para.Add(new acPara(Aircraft.B737700, new AircraftParameters(Aircraft.B737700)));
            para.Add(new acPara(Aircraft.B737800, new AircraftParameters(Aircraft.B737800)));
            para.Add(new acPara(Aircraft.B737900, new AircraftParameters(Aircraft.B737900)));
            para.Add(new acPara(Aircraft.B777200LR, new AircraftParameters(Aircraft.B777200LR)));
            para.Add(new acPara(Aircraft.B777F, new AircraftParameters(Aircraft.B777F)));
        }                      

        public static AircraftParameters GetPara(Aircraft ac)
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
