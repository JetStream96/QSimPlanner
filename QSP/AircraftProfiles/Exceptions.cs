using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSP.AircraftProfiles
{
    [Serializable]
    public class PerfFileNotFoundException : Exception
    {
        public PerfFileNotFoundException() { }

        public PerfFileNotFoundException(string message) 
            : base(message) { }

        public PerfFileNotFoundException(string message, Exception inner) 
            : base(message, inner) { }

        protected PerfFileNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) 
            : base(info, context) { }
    }
}
