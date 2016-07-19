using System;

namespace QSP.RouteFinding.Containers.CountryCode
{
    [Serializable]
    public class LoadCountryNamesException : Exception
    {
        public LoadCountryNamesException() { }
        public LoadCountryNamesException(string message) : base(message) { }
        public LoadCountryNamesException(string message, Exception inner) : base(message, inner) { }
        protected LoadCountryNamesException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
