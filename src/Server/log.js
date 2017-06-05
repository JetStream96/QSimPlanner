const fs = require('fs')
const filePath = './log.txt'

function log(msg, callback) {
    fs.appendFile(filePath, new Date().toString() + msg, callback)
}
