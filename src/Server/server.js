const http = require('http')
const fs = require('fs')
const nats = require('./tracks/nats')
const util = require('./util')
const path = require('path')

const filePath = './log.txt'
const savedDirectory = './saved/nats'

let westXml = ''
let eastXml = ''
let unloggedErrors = ''

const reqHandler = {
    '/nats/Eastbound.xml': () => eastXml,
    '/nats/Westbound.xml': () => westXml,
    '/err': () => unloggedErrors
}

/**
 * @param {*} request 
 * @param {http.ServerRes} response 
 */
function handleRequest(request, response) {
    let res = reqHandler[request.url]

    if (res === undefined) {
        response.end(res())
    } else {
        response.statusCode = 404;
        response.end('404 Not found')
    }
}

/**
 * @param {(err) => void} callback 
 */
function updateXmls(callback) {
    nats.downloadHtml((err, html) => {
        if (err) {
            callback(err)
        } else {
            updateEastXml(html, callback)
            updateWestXml(html, callback)
        }
    })
}

/**
 * @param {string} html
 * @param {Error => void}
 */
function updateEastXml(html, callback) {
    try {
        let [success, newXml] = nats.getEastboundTracks(html)

        if (success) {
            let xmlStr = util.toXml(util.withoutInvalidXmlCharObj(newXml))
            if (xmlStr != eastXml) {
                eastXml = xmlStr
                let p = path.join(savedDirectory, 'east', newXml.time)
                fs.writeFile(p, xmlStr, e => callback(e))
                log('Eastbound updated.')
            } else {
                log('No change in eastbound.')
            }
        } else {
            log('Cannot find eastbound part in html.')
        }

        callback(null)
    } catch (err) {
        callback(err)
    }
}

/**
 * @param {string} html
 * @param {Error => void}
 */
function updateWestXml(html, callback) {
    try {
        let [success, newXml] = nats.getWestboundTracks(html)

        if (success) {
            let xmlStr = util.toXml(util.withoutInvalidXmlCharObj(newXml))
            if (xmlStr != westXml) {
                westXml = xmlStr
                let p = path.join(savedDirectory, 'west', newXml.time)
                fs.writeFile(p, xmlStr, e => callback(e))
                log('Westbound updated.')
            } else {
                log('No change in westbound.')
            }
        } else {
            log('Cannot find westbound part in html.')
        }

        callback(null)
    } catch (err) {
        callback(err)
    }
}

/**
 * Log the message with current time stamp.
 * @param {string} msg 
 */
function log(msg) {
    let data = new Date().toString() + msg + '\n'
    fs.appendFile(filePath, data, err => {
        unloggedErrors += data + '\n\n' + err.stack + '\n\n'
    })
}

/**
 * Repeatedly calling the func with the specified interval. 
 * @param {() => void} func 
 * @param {number} interval in ms
 */
function repeat(func, interval) {
    func()
    setTimeout(() => repeat(func, interval), interval)
}

// Script starts here.

// Update xmls and schedule future tasks.
repeat(() => updateXmls(err => {
    if (err) {
        log(err.stack)
    }
}), 5 * 60 * 1000) // Update every 5 min.

let server = http.createServer(handleRequest)
server.listen(8081, '127.0.0.1', () => {
    console.log('server started')
})
