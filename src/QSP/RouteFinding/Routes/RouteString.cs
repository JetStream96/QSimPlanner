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

        public IEnumerator<string> GetEnumerator()
        {
            return Value.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public static class RouteStringExtension
    {
        public static RouteString ToRouteString(this IEnumerable<string> item)
        {
            return new RouteString(item.ToList());
        }
    }
}
