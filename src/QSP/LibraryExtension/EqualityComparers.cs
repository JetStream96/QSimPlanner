using System;
using System.Collections.Generic;

namespace QSP.LibraryExtension
{
    public static class EqualityComparers
    {
        public static EqualityComparer<T> Create<T>(Func<T, T, bool> selector)
        {
            return new CustomEqualityComparer<T>(selector);
        }

        private class CustomEqualityComparer<T> : EqualityComparer<T>
        {
            private Func<T, T, bool> selector;

            public CustomEqualityComparer(Func<T, T, bool> selector)
            {
                this.selector = selector;
            }

            public override bool Equals(T x, T y)
            {
                return selector(x, y);
            }

            public override int GetHashCode(T obj)
            {
                return base.GetHashCode();
            }
        }
    }
}
