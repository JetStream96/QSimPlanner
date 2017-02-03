namespace QSP.LibraryExtension
{
    public static class Util
    {
        public static void Swap<T>(ref T item1, ref T item2)
        {
            var tmp = item2;
            item2 = item1;
            item1 = tmp;
        }
    }
}