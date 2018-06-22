using System;

namespace QSP.LibraryExtension
{
    public static class ControlFlow
    {
        public static (bool matched, R result) TryMatchCase<T, R>(this T matchTarget,
            (T input, R output) firstCase, params (T input, R output)[] cases)
        {
            if (matchTarget.Equals(firstCase.input)) return (true, firstCase.output);

            foreach (var (input, output) in cases)
            {
                if (matchTarget.Equals(input)) return (true, output);
            }

            return (false, default(R));
        }

        public static R MatchCase<T, R>(this T matchTarget, (T input, R output) firstCase,
            params (T input, R output)[] cases)
        {
            var (matched, result) = matchTarget.TryMatchCase(firstCase, cases);
            if (!matched) throw new ArgumentException();
            return result;
        }

        public static R MatchCaseDefault<T, R>(this T matchTarget, R defaultValue,
            (T input, R output) firstCase, params (T input, R output)[] cases)
        {
            var (matched, result) = matchTarget.TryMatchCase(firstCase, cases);
            return matched ? result : defaultValue;
        }
    }
}
