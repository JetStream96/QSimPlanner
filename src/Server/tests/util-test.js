const util = require('../util')
const miniTest = require('./mini-test')

const [test, assertEquals] = [miniTest.test, miniTest.assertEquals]

test(() => {
    let date = util.parseDate("2017/06/07 15:38 GMT");
    assertEquals(Date.UTC(2017, 6 - 1, 7, 15, 38, 0, 0), date)
}, 'parseDate test')

test(() => {
    let [host, path] = util.splitHostQuery('www.google.com/xyz/abc')
    assertEquals('www.google.com', host)
    assertEquals('/xyz/abc', path)
}, 'splitHostQuery test')
