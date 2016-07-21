namespace QSP.LibraryExtension
{
    public class Locator<T>
    {
        public T Instance { get; set; }

        public Locator() { }
        public Locator(T instance) { this.Instance = instance; }
    }
}
