using System;
using System.Collections.Generic;
using static QSP.Utilities.ExceptionHelpers;

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
            try
            {
                var numPart = int.Parse(rwy.Substring(0, 2));
                var charPart = rwy.Substring(2);
                return OppositeNum(numPart) + oppositeDirection[charPart];
            }
            catch
            {
                throw new ArgumentException("Incorrect runway ident format.");
            }
        }

        private static string OppositeNum(int numPart)
        {
            Ensure<ArgumentException>(0 < numPart && numPart <= 36);
            var opposite = numPart >= 19 ? (numPart - 18) : (numPart + 18);
            return opposite.ToString().PadLeft(2, '0');
        }

        private static IReadOnlyDictionary<string, string> oppositeDirection =
            new Dictionary<string, string>()
            {
                [""] = "",
                ["R"] = "L",
                ["L"] = "R",
                ["C"] = "C"
            };
    }
}
