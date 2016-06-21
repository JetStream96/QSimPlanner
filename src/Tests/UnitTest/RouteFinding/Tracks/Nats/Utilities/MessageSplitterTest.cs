using NUnit.Framework;
using QSP.RouteFinding.Tracks.Nats.Utilities;
using QSP.RouteFinding.Tracks.Nats;
using System.Collections.Generic;

namespace UnitTest.RouteFinding.Tracks.Nats.Utilities
{
    [TestFixture]
    public class MessageSplitterTest
    {
        // Beware that there are strange ASCII characters in the string,
        // which aren't visible unless in a text editor like Sublime.

        [Test]
        public void BothDirectionExists()
        {
            string html =
@"<i>- Last updated at 2016/02/02 15:05 GMT</i>
<tr valign=>The following are active North Atlantic Tracks issued by Shanwick Center (EGGX) ...</th></tr>
<font color=""#000099"">
012111 EGGXZOZX
[Message W1]

<font color=""#000099"">
012112 EGGXZOZX
[Message W2]

</td></tr></table>
<font color=""#000099"">
021356 CZQXZQZX
[Message E1]

<font color=""#000099"">
021357 CZQXZQZX
[Message E2]

<font color=""#000099"">
021357 CZQXZQZX
[Message E3]

</td></tr></table>";

            var msg = new MessageSplitter(html).Split();

            Assert.AreEqual(2, msg.Count);
            var west = GetMsg(msg, NatsDirection.West);
            var east = GetMsg(msg, NatsDirection.East);

            Assert.IsTrue(west.Header ==
                "The following are active North Atlantic Tracks issued by Shanwick Center (EGGX) ...");
            Assert.IsTrue(west.LastUpdated == "Last updated at 2016/02/02 15:05 GMT");            
            Assert.IsTrue(west.Message == @"012111 EGGXZOZX
[Message W1]


012112 EGGXZOZX
[Message W2]

");

            Assert.IsTrue(east.Header ==
                "The following are active North Atlantic Tracks issued by Shanwick Center (EGGX) ...");
            Assert.IsTrue(east.LastUpdated == "Last updated at 2016/02/02 15:05 GMT");
            Assert.IsTrue(east.Message == @"021356 CZQXZQZX
[Message E1]


021357 CZQXZQZX
[Message E2]


021357 CZQXZQZX
[Message E3]

");

        }

        [Test]
        public void OnlyEastboundExists()
        {
            string html =
@"<i>- Last updated at 2016/02/02 15:05 GMT</i>
<tr valign=>The following are active North Atlantic Tracks issued by Shanwick Center (EGGX) ...</th></tr>

<font color=""#000099"">
021356 CZQXZQZX
[Message E1]

<font color=""#000099"">
021357 CZQXZQZX
[Message E2]

<font color=""#000099"">
021357 CZQXZQZX
[Message E3]

</td></tr></table>";

            var msg = new MessageSplitter(html).Split();

            Assert.AreEqual(1, msg.Count);
            var west = GetMsg(msg, NatsDirection.West);
            var east = GetMsg(msg, NatsDirection.East);

            Assert.IsTrue(west == null);

            Assert.IsTrue(east.Header ==
                "The following are active North Atlantic Tracks issued by Shanwick Center (EGGX) ...");
            Assert.IsTrue(east.LastUpdated == "Last updated at 2016/02/02 15:05 GMT");
            Assert.IsTrue(east.Message == @"021356 CZQXZQZX
[Message E1]


021357 CZQXZQZX
[Message E2]


021357 CZQXZQZX
[Message E3]

");

        }

        private static IndividualNatsMessage GetMsg(
                        IEnumerable<IndividualNatsMessage> msgs, NatsDirection dir)
        {
            foreach (var i in msgs)
            {
                if (i.Direction == dir)
                {
                    return i;
                }
            }
            return null;
        }
    }
}
