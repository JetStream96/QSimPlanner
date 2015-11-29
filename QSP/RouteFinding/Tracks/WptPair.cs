using System;
namespace QSP.RouteFinding.Tracks
{

    public class WptPair : IEquatable<WptPair>
    {

        public int IndexFrom { get; set; }
        public int IndexTo { get; set; }


        public WptPair(int IndexFrom, int IndexTo)
        {
            this.IndexFrom = IndexFrom;
            this.IndexTo = IndexTo;

        }

        public bool Equals(WptPair other)
        {

            if (IndexFrom == other.IndexFrom && IndexTo == other.IndexTo)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

    }

}
