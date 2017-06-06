const http = require('http')
const fs = require('fs')
const nats = require('./tracks/nats')
const filePath = './log.txt'

let westXml = ''
let eastXml = ''

function handleRequest(request, response) {
    response.end("Hello world !!!!!!")
}

/**
 * @param {(err) => void} callback 
 */
function updateXmls(callback) {
    nats.downloadHtml((err, html) => {
        if (err) {
            callback(err)
        } else {
            callback(updateEastXml(html) || updateEastXml(html))
        }
    }) 
}

/**
 * @returns {Error}
 */
function updateEastXml(html) {
    try {
        eastXml = nats.getEastboundTracks(html)
        return null
    } catch (err) {
        return err
    }    
}

/**
 * @returns {Error}
 */
function updateWestXml(html, callback) {
    try {
        westXml = nats.getWestboundTracks(html)
        return null
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
repeat(() => updateXmls(err => log(err.toString(), () => {})), 5 * 60 * 1000)
// TODO: Failure on logging needs handling.



let server = http.createServer(handleRequest)
server.listen(8080, '127.0.0.1', () => {
    console.log('server is running!!!!!')
})


