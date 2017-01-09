using System.Collections.Generic;

namespace QSP.AircraftProfiles.Configs
{
    public static class ConfigComparers
    {
        public static IComparer<AircraftConfigItem> ConfigItemComparer()
        {
            return Comparer<AircraftConfigItem>.Create((x, y) =>
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
            });
        }

        public static IComparer<AircraftConfig> ConfigComparer()
        {
            return Comparer<AircraftConfig>.Create(
                (x, y) => ConfigItemComparer().Compare(x.Config, y.Config));
        }
    }
}
