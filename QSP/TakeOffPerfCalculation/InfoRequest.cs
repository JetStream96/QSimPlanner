using System;

namespace QSP.TakeOffPerfCalculation
{

    static class InfoRequest
    {

        public static bool AltnThrustRatingAvail(Aircraft ac)
        {
            return LoadedData.GetPara(ac).AltnRatingAvail;
            //throw new ArgumentOutOfRangeException("Invalid aircraft.");
        }

        public static ThrustRatingOption[] AvailThrustRating(Aircraft ac)
        {
            switch (ac)
            {

                case Aircraft.B737600:
                    return new ThrustRatingOption[] { ThrustRatingOption.Normal };
                case Aircraft.B737700:
                    return new ThrustRatingOption[] { ThrustRatingOption.Normal };
                case Aircraft.B737800:
                    return new ThrustRatingOption[] { ThrustRatingOption.Normal };
                case Aircraft.B737900:
                    return new ThrustRatingOption[] { ThrustRatingOption.Normal };
                case Aircraft.B777200LR:
                    return new ThrustRatingOption[]{
                        ThrustRatingOption.Normal,
                        ThrustRatingOption.TO1,
                        ThrustRatingOption.TO2
                    };
                case Aircraft.B777F:

                    return new ThrustRatingOption[] { ThrustRatingOption.Normal };
            }

            throw new ArgumentOutOfRangeException("Invalid aircraft.");

        }

        public static string[] AllowedFlaps(Aircraft ac)
        {
            return LoadedData.GetPara(ac).AllowedFlaps;
           // throw new ArgumentOutOfRangeException("Invalid aircraft.");
        }

        public static string ThrustRatingToString(ThrustRatingOption item)
        {
            switch (item)
            {
                case ThrustRatingOption.Normal:

                    return "TO";
                case ThrustRatingOption.TO1:

                    return "TO1";
                case ThrustRatingOption.TO2:

                    return "TO2";
            }
            throw new ArgumentOutOfRangeException("Invalid thrust rating.");
        }

    }

}
