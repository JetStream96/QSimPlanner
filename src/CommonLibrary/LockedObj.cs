using System;

namespace CommonLibrary
{
    public class LockedObj<T>
    {
        private T val;
        private object _lock = new object();

        public LockedObj() : this(default(T)) { }

        public LockedObj(T val)
        {
            this.val = val;
        }

        public void Execute(Action<T> a)
        {
            lock (_lock)
            {
                a(val);
            }
        }

        public U Execute<U>(Func<T, U> f)
        {
            lock (_lock)
            {
                return f(val);
            }
        }

        public void Modify(Func<T, T> transformer)
        {
            lock (_lock)
            {
                var old = val;
                val = transformer(val);
            }
        }
        
        public T Value
        {
            // Beware that method calls on the return value of this may not be thread-safe.
            get
            {
                lock (_lock)
                {
                    return val;
                }
            }

            set
            {
                lock (_lock)
                {
                    this.val = value;
                }
            }
        }
    }
}
