using System;
using System.Collections.Generic;
using static QSP.Utilities.ConditionChecker;

namespace QSP.LibraryExtension
{
    // A one-to-one map.
    public class BiDictionary<TFirst, TSecond>
    {
        private Dictionary<TFirst, TSecond> firstToSecond;
        private Dictionary<TSecond, TFirst> secondToFirst;

        public BiDictionary()
        {
            firstToSecond = new Dictionary<TFirst, TSecond>();
            secondToFirst = new Dictionary<TSecond, TFirst>();
        }

        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public void Add(TFirst first, TSecond second)
        {
            Ensure<ArgumentNullException>(first != null && second != null);
            firstToSecond.Add(first, second);

            try
            {
                secondToFirst.Add(second, first);
            }
            catch (ArgumentException)
            {
                firstToSecond.Remove(first);
                throw;
            }
        }

        public TSecond GetByFirst(TFirst first)
        {
            return firstToSecond[first];
        }

        public TFirst GetBySecond(TSecond second)
        {
            return secondToFirst[second];
        }

        public bool TryGetByFirst(TFirst first, out TSecond second)
        {
            return firstToSecond.TryGetValue(first, out second);
        }

        public bool TryGetBySecond(TSecond second, out TFirst first)
        {
            return secondToFirst.TryGetValue(second, out first);
        }

        public bool ContainsFirst(TFirst first)
        {
            return firstToSecond.ContainsKey(first);
        }

        public bool ContainsSecond(TSecond second)
        {
            return secondToFirst.ContainsKey(second);
        }
    }
}
