using System.Collections.Generic;

namespace QSP.LibraryExtension
{
    public interface IReadOnlyBiDictionary<TFirst, TSecond>
    {
        IReadOnlyDictionary<TFirst, TSecond> FirstToSecond { get; }
        IReadOnlyDictionary<TSecond, TFirst> SecondToFirst { get; }
        TSecond GetByFirst(TFirst first);
        TFirst GetBySecond(TSecond second);
        bool TryGetByFirst(TFirst first, out TSecond second);
        bool TryGetBySecond(TSecond second, out TFirst first);
        bool ContainsFirst(TFirst first);
        bool ContainsSecond(TSecond second);
    }
}