using System;

namespace QSP.RouteFinding.FileExport.Providers
{
    public enum ProviderType
    {
        Pmdg = 0,
        Fsx = 1,
        Fs9 = 2
    }

    public static class Types
    {
        public static string GetExtension(ProviderType type)
        {
            switch (type)
            {
                case ProviderType.Pmdg:
                    return ".rte";

                case ProviderType.Fsx:
                case ProviderType.Fs9:
                    return ".PLN";
                
                default:
                    throw new ArgumentException();
            }
        }
    }
}
