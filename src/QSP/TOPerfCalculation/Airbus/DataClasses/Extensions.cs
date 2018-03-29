using System.Collections.Generic;
using System.Linq;

namespace QSP.TOPerfCalculation.Airbus.DataClasses
{
    public static class Extensions
    {
        /// <summary>
        /// Returns the available flaps in the order of first appearance of 'Tables'
        /// property. Since 'Tables' is in the same order as they appear in the xml
        /// file, this returning value preserves the order in xml file.
        /// </summary>
        public static IEnumerable<string> AvailableFlaps(this AirbusPerfTable t) =>
            t.Tables.Select(x => x.Flaps).Distinct().OrderBy(s => s);
    }
}
