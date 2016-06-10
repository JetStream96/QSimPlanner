using QSP.Common;

namespace QSP.RouteFinding.FileExport.Providers
{
    public enum ProviderType
    {
        Pmdg
    }

    public static class Types
    {
        public static string GetExtension(ProviderType type)
        {
            switch (type)
            {
                case ProviderType.Pmdg:
                    return ".rte";

                default:
                    throw new EnumNotSupportedException();
            }
        }
    }
}
