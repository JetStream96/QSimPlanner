using System;

namespace QSP.RouteFinding.Tracks
{
    public class WptPair : IEquatable<WptPair>
    {
        public int IndexFrom { get; }
        public int IndexTo { get; }

        public WptPair(int IndexFrom, int IndexTo)
        {
            this.IndexFrom = IndexFrom;
            this.IndexTo = IndexTo;
        }

        public bool Equals(WptPair other)
        {
            return other != null &&
                IndexFrom == other.IndexFrom &&
                IndexTo == other.IndexTo;
        }

        public override int GetHashCode()
        {
            return IndexFrom.GetHashCode() ^ IndexTo.GetHashCode();
        }
    }
}
