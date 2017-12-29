using System.Collections.Generic;
using System.IO;
using System;

namespace QSP.RouteFinding.Containers.CountryCode
{
    public static class FullNamesLoader
    {
        public static readonly string FilePath =
            @"navdata_additional/icao_nationality_code.txt";
               
        /// <exception cref="LoadCountryNamesException"></exception>
        public static Dictionary<string, string> Load()
        {
            var result = new Dictionary<string, string>();
            string[] lines;
            var sep = new char[] { '\t', ' ' };

            try
            {
                lines = File.ReadAllLines(FilePath);
            }
            catch (Exception ex)
            {
                throw new LoadCountryNamesException("", ex);
            }

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
