using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace QSP.RouteFinding.Routes
{
    public class RouteString : IReadOnlyList<string>
    {
        public IReadOnlyList<string> Value { get; private set; }

        public int Count { get { return Value.Count; } }
        public string this[int index] { get { return Value[index]; } }

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
