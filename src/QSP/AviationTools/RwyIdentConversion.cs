using System;
using static QSP.Utilities.ConditionChecker;

namespace QSP.AviationTools
{
    public static class RwyIdentConversion
    {
        /// <summary>
        /// Input examples: "05", "26", "14R", "25C", "36L"
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public static string RwyIdentOppositeDir(string rwy)
        {
            Ensure<ArgumentException>(rwy.Length == 2 || rwy.Length == 3);

            int numPart = 0;
            char charPart = '\0';
            bool hasCharPart = false;

            if (rwy.Length == 2)
            {
                numPart = int.Parse(rwy);
            }
            else
            {
                numPart = int.Parse(rwy.Substring(0, 2));
                hasCharPart = true;
                charPart = rwy[2];
            }

            Ensure<ArgumentException>(0 < numPart && numPart <= 36);

            if (numPart >= 19)
            {
                numPart -= 18;
            }
            else
            {
                numPart += 18;
            }

            string numPartStr = numPart.ToString().PadLeft(2, '0');

            if (hasCharPart)
            {
                return numPartStr + GetOpposite(charPart);
            }
            else
            {
                return numPartStr;
            }
        }

        private static char GetOpposite(char c)
        {
            switch (c)
            {
                case 'R':
                    return 'L';

                case 'L':
                    return 'R';

                case 'C':
                    return 'C';

                default:
                    throw new ArgumentException();
            }
        }
    }
}
