using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace QSP.RouteFinding.Containers.CountryCode
{
    public static class FullNamesLoader
    {
        public static readonly string FilePath =
            @"NavData/icao_nationality_code.txt";

        public static Dictionary<string, string> Load()
        {
            // TODO: Exceptions?
            var result = new Dictionary<string, string>();
            var lines = File.ReadAllLines(FilePath);
            var sep = new char[] { '\t', ' ' };

            foreach (var line in lines)
            {
                var words = line.Split(sep, 2);

                if (words.Length == 2)
                {
                    var letterCode = words[0].Trim().ToUpper();
                    var country = words[1].Trim();

                    result.Add(letterCode, country);
                }
            }

            return result;
        }
    }
}
