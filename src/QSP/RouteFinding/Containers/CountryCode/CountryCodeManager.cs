using QSP.LibraryExtension;
using System.Collections.Generic;
using System.Linq;

namespace QSP.RouteFinding.Containers.CountryCode
{
    // A class that enables conversion between letter code (e.g. EK), full name (e.g. 
    // United Kindom), and an interger code.
    public class CountryCodeManager
    {
        // CountryCode <=> LetterCode
        private IReadOnlyBiDictionary<int, string> codeLookup;

        // LetterCode <=> FullName
        private Dictionary<string, string> _fullNameLookup;

        public IReadOnlyDictionary<string, string> FullNameLookup => _fullNameLookup;

        public IReadOnlyDictionary<int, string> CodeToLetterLookup => codeLookup.FirstToSecond;

        public IReadOnlyDictionary<string, int> LetterToCodeLookup => codeLookup.SecondToFirst;

        public CountryCodeManager()
            : this(new BiDictionary<int, string>(),
                 new Dictionary<string, string>())
        { }

        /// <exception cref="ArgumentException"></exception>
        public CountryCodeManager(
            IReadOnlyBiDictionary<int, string> codeLookup,
            Dictionary<string, string> fullNameLookup)
        {
            this.codeLookup = codeLookup;
            this._fullNameLookup = fullNameLookup;
            Validate();
        }

        private void Validate()
        {
            // Make the BiDictionary and the other Dictionary compatible.
            // i.e. The sets of letter codes must be equal.

            // ToList forces the copying, which prevents changes made 
            // later to affect the value of the Dictionary.KeyCollection.
            var letterBiDict = LetterToCodeLookup.Keys.ToList();
            var letterDict = _fullNameLookup.Keys.ToList();

            var withoutFullName = letterBiDict.Except(letterDict);

            foreach (var i in withoutFullName)
            {
                _fullNameLookup.Add(i, "Unknown");
            }

            var withoutIntCode = letterDict.Except(letterBiDict);

            foreach (var j in withoutIntCode)
            {
                _fullNameLookup.Remove(j);
            }
        }

        /// <exception cref="ArgumentException"></exception>
        public string GetFullName(int code)
        {
            return GetFullName(GetLetter(code));
        }

        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public string GetFullName(string letterCode)
        {
            return GetFullName(letterCode);
        }

        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public int GetCountryCode(string letterCode)
        {
            return codeLookup.GetBySecond(letterCode);
        }

        /// <exception cref="ArgumentException"></exception>
        public string GetLetter(int code)
        {
            return codeLookup.GetByFirst(code);
        }
    }
}
