using System;

namespace QSP.RouteFinding.FileExport.Providers
{
    public enum ProviderType
    {
        Pmdg,
        Fsx,
        Fs9
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
                    return ".FLN";
                
                default:
                    throw new ArgumentException();
            }
        }
    }
}
