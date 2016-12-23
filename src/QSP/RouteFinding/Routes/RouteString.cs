using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace QSP.RouteFinding.Routes
{
    public class RouteString : IReadOnlyList<string>
    {
        public IReadOnlyList<string> Value { get; }
        public int Count => Value.Count;
        public string this[int index] => Value[index];

        public RouteString(IReadOnlyList<string> Value)
        {
            this.Value = Value;
        }

        public IEnumerator<string> GetEnumerator() => Value.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public static RouteString From(params string[] values) => new RouteString(values);
        public static RouteString Empty => new RouteString(new string[0]);
    }

    public static class RouteStringExtension
    {
        public static RouteString ToRouteString(this IEnumerable<string> item)
        {
            return new RouteString(item.ToList());
        }
    }
}
