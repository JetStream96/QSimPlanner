using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSP.RouteFinding.Containers;

namespace Tests.RouteFindingTest.TestDataGenerators
{
    public static class WptListInstance
    {
        public static WaypointList WptList = new WptListGenerator().Generate();
    }
}
