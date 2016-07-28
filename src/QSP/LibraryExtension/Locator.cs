namespace QSP.LibraryExtension
{
    public class Locator<T>
    {
        public T Instance { get; set; }

        public Locator() { }
        public Locator(T instance) { this.Instance = instance; }
    }

    public static class LocatorFactory
    {
        public static Locator<T> ToLocator<T>(this T instance)
        {
            return new Locator<T>(instance);
        }
    }
}
