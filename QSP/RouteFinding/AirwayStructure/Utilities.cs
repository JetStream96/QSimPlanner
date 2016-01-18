using QSP.Core;
using QSP.RouteFinding.Tracks.Common;

namespace QSP.RouteFinding.AirwayStructure
{
    public static class Utilities
    {

        /// <exception cref="EnumNotSupportedException"></exception>
        public static ChangeCategory ToChangeCategory(TrackChangesOption value)
        {
            switch (value)
            {
                case TrackChangesOption.Yes:
                   return ChangeCategory.Normal;

                case TrackChangesOption.AddingNats:
                    return ChangeCategory.Nats;

                case TrackChangesOption.AddingPacots:
                    return ChangeCategory.Pacots;

                case TrackChangesOption.AddingAusots:
                    return ChangeCategory.Ausots;
            }
            throw new EnumNotSupportedException();
        }

        /// <exception cref="EnumNotSupportedException"></exception>
        public static ChangeCategory ToChangeCategory(TrackType type)
        {
            switch (type)
            {
                case TrackType.Nats:
                  return ChangeCategory.Nats;

                case TrackType.Pacots:
                    return ChangeCategory.Pacots;

                case TrackType.Ausots:
                    return ChangeCategory.Ausots;

                default:
                    throw new EnumNotSupportedException();
            }
        }

        /// <exception cref="EnumNotSupportedException"></exception>
        public static TrackChangesOption ToTrackChangesOption(TrackType type)
        {
            switch (type)
            {   
                case TrackType.Nats:
                    return TrackChangesOption.AddingNats;

                case TrackType.Pacots:
                    return TrackChangesOption.AddingPacots;

                case TrackType.Ausots:
                    return TrackChangesOption.AddingAusots;

                default:
                    throw new EnumNotSupportedException();
            }
        }
    }
}
