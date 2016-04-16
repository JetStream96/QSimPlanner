using System;

namespace QSP.RouteFinding.Tracks
{
    public class WptPair : IEquatable<WptPair>
    {
        public int IndexFrom { get; private set; }
        public int IndexTo { get; private set; }

        public WptPair(int IndexFrom, int IndexTo)
        {
            this.IndexFrom = IndexFrom;
            this.IndexTo = IndexTo;
        }

        public bool Equals(WptPair other)
        {
            return (IndexFrom == other.IndexFrom && IndexTo == other.IndexTo);
        }
    }
}
