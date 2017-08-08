const http = require('http')
const fs = require('fs')
const nats = require('./tracks/nats')
const util = require('./util')
const path = require('path')
const mkdirp = require('mkdirp')
const express = require('express')
const bodyParser = require('body-parser')
const SyncFileWriter = require('./sync-file-writer').SyncFileWriter
const savedDirectory = path.join(__dirname, 'saved/nats')
const AntiSpamList = require('./anti-spam-list').AntiSpamList
const configFilePath = path.join(__dirname, 'config.json')
const lastUpdatedTime = require('./last-updated-time')

let westXml = ''
let eastXml = ''
let lastWestObj = { Message: '' }
let lastEastObj = { Message: '' }
let lastWestDate = 0
let lastEastDate = 0
let unloggedErrors = ''

let errorReportWriter = new SyncFileWriter(
    path.join(__dirname, 'error-report'), 'error-report.txt')

let logFileWriter = new SyncFileWriter(__dirname, 'log.txt')

let userList = new AntiSpamList()
let maxRequestBodySize = 100 * 1000

const reqHandler = {
    'nats': {
        'Eastbound.xml': () => eastXml,
        'Westbound.xml': () => westXml,
    },
    'err': () => (unloggedErrors === '' ? 'No unlogged error.' : unloggedErrors)
}

function setReqHandler(app, parentPath, obj) {
    Object.keys(obj).forEach(k => {
        let val = obj[k]
        if (typeof val === 'function') {
            app.get(parentPath + '/:id', (req, res) => {
                handleRequest(obj, req, res)
            })
        } else {
            // Is an object
            setReqHandler(app, parentPath + '/' + k, val)
        }
    })
}

function handleRequest(obj, request, response) {
    let resGetter = obj[request.params.id]

    if (typeof resGetter === 'function') {
        response.end(resGetter())
    } else {
        response.statusCode = 404
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
            return
        }

        // No error
        updateEastXml(html, (e, updated) => {
            if (updated) lastUpdatedTime.saveFileEast(Date.now(), callback)
        });
        updateWestXml(html, (e, updated) => {
            if (updated) lastUpdatedTime.saveFileWest(Date.now(), callback)
        });
    })
}

function xmlFileName(lastUpdated) {
    return util.sanitizeFilename(lastUpdated.match(/\d.+/)[0])
}

function saveXml(subDirectory, lastUpdated, xmlStr, callback) {
    let dir = path.join(savedDirectory, subDirectory)
    let p = path.join(dir, xmlFileName(lastUpdated) + '.txt')
    let p1 = path.join(dir, 'latest.xml')
    mkdirp(dir, e => {
        if (!e) {
            fs.writeFile(p, xmlStr, err => fs.writeFile(p1, xmlStr, callback))
        } else {
            callback(e)
        }
    })
}

function loadLatestXmls() {
    try {
        westXml = fs.readFileSync(path.join(savedDirectory, 'west/latest.xml'), 'utf-8')
        eastXml = fs.readFileSync(path.join(savedDirectory, 'east/latest.xml'), 'utf-8')
    } catch (e) { }
}

/**
 * @param {string} html
 * @param {(Error, updated: boolean) => void}
 */
function updateEastXml(html, callback) {
    try {
        let [success, newXml] = nats.getEastboundTracks(html)

        if (success) {
            let date = util.parseDate(newXml.LastUpdated)
            if (date > lastEastDate && lastEastObj.Message !== newXml.Message) {
                let xmlStr = util.toXml(util.withoutInvalidXmlCharObj(newXml))
                eastXml = xmlStr
                lastEastObj = newXml
                lastEastDate = date
                saveXml('east', newXml.LastUpdated, xmlStr, e => callback(e, true))
                logFileWriter.add('Eastbound updated.')
            } else {
                logFileWriter.add('No change in eastbound.')
            }
        } else {
            logFileWriter.add('Cannot find eastbound part in html.')
        }

        callback(null, false)
    } catch (err) {
        callback(err, false)
    }
}

/**
 * @param {string} html
 * @param {(Error, updated: boolean) => void}
 */
function updateWestXml(html, callback) {
    try {
        let [success, newXml] = nats.getWestboundTracks(html)

        if (success) {
            let date = util.parseDate(newXml.LastUpdated)
            if (date > lastWestDate && lastWestObj.Message !== newXml.Message) {
                let xmlStr = util.toXml(util.withoutInvalidXmlCharObj(newXml))
                westXml = xmlStr
                lastWestObj = newXml
                lastWestDate = date
                saveXml('west', newXml.LastUpdated, xmlStr, e => callback(e, true))
                logFileWriter.add('Westbound updated.')
            } else {
                logFileWriter.add('No change in westbound.')
            }
        } else {
            logFileWriter.add('Cannot find westbound part in html.')
        }

        callback(null, false)
    } catch (err) {
        callback(err, false)
    }
}

function readConfigFile() {
    return JSON.parse(fs.readFileSync(configFilePath, 'utf-8'))
}

// Script starts here.

loadLatestXmls(); // Do not remove this semicolon.
[lastWestDate, lastEastDate] = lastUpdatedTime.tryLoadFromFile()

// Update xmls and schedule future tasks.
util.repeat(() => updateXmls(err => {
    if (err) logFileWriter.add(err.stack)
}), 5 * 60 * 1000) // Update every 5 min.

let app = express()

app.use(bodyParser.json())
app.use(bodyParser.urlencoded({ extended: true }))

setReqHandler(app, '', reqHandler)

app.post('/error-report', (req, res) => {
    let ip = req.ip

    if (!userList.decrementToken(ip) &&
        req.body.toString().length <= maxRequestBodySize) {
        let obj = {
            ip: req.ip,
            time: new Date(Date.now()),
            text: req.body
        }
        errorReportWriter.add(JSON.stringify(obj)) + '\n'
    }

    res.send("OK")
})

let config = readConfigFile()
let port = parseInt(config.port)
let server = app.listen(port, () => {
    console.log('server started')
    logFileWriter.add('server started')
})
