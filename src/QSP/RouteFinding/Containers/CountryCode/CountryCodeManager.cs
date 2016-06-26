using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSP.LibraryExtension;

namespace QSP.RouteFinding.Containers.CountryCode
{
    public class CountryCodeManager
    {
        private BiDictionary<int, string> letterCodeLookup;
        private Dictionary<int, string> fullNameLookup;

        public CountryCodeManager(
            BiDictionary<int, string> letterCodeLookup,
            Dictionary<int, string> fullNameLookup)
        {
            this.letterCodeLookup = letterCodeLookup;
            this.fullNameLookup = fullNameLookup;
        }

        /// <exception cref="ArgumentException"></exception>
        public string GetFullName(int code)
        {
            return fullNameLookup[code];
        }

        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public string GetFullName(string letterCode)
        {
            return GetFullName(GetCountryCode(letterCode));
        }

        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public int GetCountryCode(string letterCode)
        {
            return letterCodeLookup.GetBySecond(letterCode);
        }

        /// <exception cref="ArgumentException"></exception>
        public string GetLetter(int code)
        {
            return letterCodeLookup.GetByFirst(code);
        }
    }
}
