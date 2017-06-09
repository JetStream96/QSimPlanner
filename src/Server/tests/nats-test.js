const nats = require('../tracks/nats')
const miniTest = require('./mini-test')
const [test, assertEquals] = [miniTest.test, miniTest.assertEquals]

const html =
`<i>- Last updated at 2016/02/02 15:05 GMT</i>
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

</td></tr></table>`

test(() => {
    let [found, obj] = nats.getWestboundTracks(html)
    assertEquals(true, found)
    assertEquals('Last updated at 2016/02/02 15:05 GMT', obj.LastUpdated)
    assertEquals('The following are active North Atlantic Tracks issued by Shanwick Center (EGGX) ...', obj.Header)
    assertEquals('West', obj.Direction)
    assertEquals(`012111 EGGXZOZX
[Message W1]


012112 EGGXZOZX
[Message W2]

`, obj.Message)

}, 'getWestboundTracks test')

test(() => {
    let [found, obj] = nats.getEastboundTracks(html)
    assertEquals(true, found)
    assertEquals('Last updated at 2016/02/02 15:05 GMT', obj.LastUpdated)
    assertEquals('The following are active North Atlantic Tracks issued by Shanwick Center (EGGX) ...', obj.Header)
    assertEquals('East', obj.Direction)
    assertEquals(`021356 CZQXZQZX
[Message E1]


021357 CZQXZQZX
[Message E2]


021357 CZQXZQZX
[Message E3]

`, obj.Message)

}, 'getEastboundTracks test')


