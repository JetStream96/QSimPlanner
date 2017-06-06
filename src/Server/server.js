const http = require('http')
const fs = require('fs')
const nats = require('./tracks/nats')
const filePath = './log.txt'

let westXml = ''
let eastXml = ''

function handleRequest(request, response) {
    response.end(eastXml + '\n\n' + westXml)
}

/**
 * @param {(err) => void} callback 
 */
function updateXmls(callback) {
    nats.downloadHtml((err, html) => {
        if (err) {
            callback(err)
        } else {
            callback(updateEastXml(html) || updateWestXml(html))
        }
    }) 
}

/**
 * @returns {Error}
 */
function updateEastXml(html) {
    try {
        let [success, newXml] = nats.getEastboundTracks(html)

        if (success) {
            eastXml = nats.toXml(newXml)
        } 
    } catch (err) {
        return err
    }    
}

/**
 * @returns {Error}
 */
function updateWestXml(html, callback) {
    try {
        let [success, newXml] = nats.getWestboundTracks(html)
        if (success) {
            westXml = nats.toXml(newXml)
        }
    } catch (err) {
        return err
    }     
}

/**
 * Log the message with current time stamp.
 * @param {string} msg 
 * @param {err => void} callback 
 */
function log(msg, callback) {
    fs.appendFile(filePath, new Date().toString() + msg + '\n', callback)
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
        log(err.toString(), () => {})  // TODO: Failure on logging needs handling.
    } 
}), 5 * 60 * 1000)


let server = http.createServer(handleRequest)
server.listen(8080, '127.0.0.1', () => {
    console.log('server started')
})


