using System;
using QSP.LibraryExtension;
using QSP.RouteFinding.Containers;

namespace QSP.NavData.AAX
{
    public class CountryCodeGenerator
    {
        private readonly int DefaultCode;
        private readonly BiDictionary<int, string> _countryCodeLookup =
             new BiDictionary<int, string>();
        private int countryCode = 0;

        public IReadOnlyBiDictionary<int, string> CountryCodeLookup => _countryCodeLookup;

        // Throws ArgumentException if DefaultCode > 0.
        public CountryCodeGenerator(int DefaultCode = Waypoint.DefaultCountryCode)
        {
            if (DefaultCode > 0) throw new ArgumentException();
            this.DefaultCode = DefaultCode;
        }

        // Assign country code based on letterCode. Same letterCode returns the same countryCode.
        // The country codes will be 1, 2, 3 ..., or DefaultCode if letterCode is empty string.
        //
        public int GetCountryCode(string letterCode)
        {
            if (letterCode == "") return DefaultCode;            
            if (_countryCodeLookup.TryGetBySecond(letterCode, out var code)) return code;

            do
            {
                countryCode++;
            } while (countryCode == DefaultCode);

            _countryCodeLookup.Add(countryCode, letterCode);
            return countryCode;
        }
    }
}