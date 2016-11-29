using System;
using System.Collections.Generic;
using static QSP.Utilities.ExceptionHelpers;

namespace QSP.LibraryExtension
{
    // A one-to-one map.
    public class BiDictionary<TFirst, TSecond>
    {
        private Dictionary<TFirst, TSecond> _firstToSecond;
        private Dictionary<TSecond, TFirst> _secondToFirst;

        public IReadOnlyDictionary<TFirst, TSecond> FirstToSecond => _firstToSecond;
        public IReadOnlyDictionary<TSecond, TFirst> SecondToFirst => _secondToFirst;

        public BiDictionary()
        {
            _firstToSecond = new Dictionary<TFirst, TSecond>();
            _secondToFirst = new Dictionary<TSecond, TFirst>();
        }

        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public void Add(TFirst first, TSecond second)
        {
            Ensure<ArgumentNullException>(first != null && second != null);
            _firstToSecond.Add(first, second);

            try
            {
                _secondToFirst.Add(second, first);
            }
            catch (ArgumentException)
            {
                _firstToSecond.Remove(first);
                throw;
            }
        }

        /// <exception cref="ArgumentNullException"></exception>
        public void RemoveByFirst(TFirst first)
        {
            TSecond second;
            if (TryGetByFirst(first, out second) == false) return;

            _firstToSecond.Remove(first);
            _secondToFirst.Remove(second);
        }

        /// <exception cref="ArgumentNullException"></exception>
        public void RemoveBySecond(TSecond second)
        {
            TFirst first;
            if (TryGetBySecond(second, out first) == false) return;

            _secondToFirst.Remove(second);
            _firstToSecond.Remove(first);
        }

        public TSecond GetByFirst(TFirst first)
        {
            return _firstToSecond[first];
        }

        public TFirst GetBySecond(TSecond second)
        {
            return _secondToFirst[second];
        }

        public bool TryGetByFirst(TFirst first, out TSecond second)
        {
            return _firstToSecond.TryGetValue(first, out second);
        }

        public bool TryGetBySecond(TSecond second, out TFirst first)
        {
            return _secondToFirst.TryGetValue(second, out first);
        }

        public bool ContainsFirst(TFirst first)
        {
            return _firstToSecond.ContainsKey(first);
        }

        public bool ContainsSecond(TSecond second)
        {
            return _secondToFirst.ContainsKey(second);
        }
    }
}
