using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP.RouteFinding.Tracks.Nats.Utilities;

namespace Tests.RouteFinding.Tracks.Nats.Utilities
{
    [TestClass]
    public class MessageSplitterTest
    {
        [TestMethod]
        public void BothDirectionExists()
        {
            string html =
@"<i>- Last updated at 2016/02/02 15:05 GMT</i>
<tr valign=>The following are active North Atlantic Tracks issued by Shanwick Center (EGGX) ...</th></tr>
<font color=""#000099"">
012111 EGGXZOZX
[Message W1]

<font color=""#000099"">
012112 EGGXZOZX
[Message W2]

</td></tr></table>
<font color=""#000099"">
021356 CZQXZQZX
[Message E1]

<font color=""#000099"">
021357 CZQXZQZX
[Message E2]

<font color=""#000099"">
021357 CZQXZQZX
[Message E3]

</td></tr></table>";

            var msg = new MessageSplitter(html).Split();
            
        }
    }
}
