using System;

namespace QSP.AircraftProfiles.Configs
{
    [Serializable]
    public class PerfFileNotFoundException : Exception
    {
        public PerfFileNotFoundException() { }

        public PerfFileNotFoundException(string message)
            : base(message)
        { }

        public PerfFileNotFoundException(string message, Exception inner)
            : base(message, inner)
        { }

        protected PerfFileNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        { }
    }
}
