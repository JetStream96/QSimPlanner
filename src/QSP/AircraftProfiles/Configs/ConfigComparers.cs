using System.Collections.Generic;

namespace QSP.AircraftProfiles.Configs
{
    public class ConfigComparer : IComparer<AircraftConfig>
    {
        private ConfigItemComparer comp = new ConfigItemComparer();

        public int Compare(AircraftConfig x, AircraftConfig y)
        {
            return comp.Compare(x.Config, y.Config);
        }
    }

    public class ConfigItemComparer : IComparer<AircraftConfigItem>
    {
        public int Compare(AircraftConfigItem x, AircraftConfigItem y)
        {
            int ac = x.AC.CompareTo(y.AC);

            if (ac == 0)
            {
                return x.Registration.CompareTo(y.Registration);
            }
            else
            {
                return ac;
            }
        }
    }
}
