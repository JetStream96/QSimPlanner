const xml2js = require('xml2js')
const fs = require('fs')
const path = require('path')
const filePath = path.join(__dirname, 'log.txt')
const https = require('https')

const validXmlChars = /[\u0009\u000a\u000d\u0020-\uD7FF\uE000-\uFFFD]/

function isValidXmlChar(c) {
    if (c.length > 1) throw new Error()
    return validXmlChars.test(c)
}

/**
 * @param {string} str 
 * @returns {string} A string with all invalid xml chars removed.
 */
function withoutInvalidXmlChar(str) {
    let result = []

    for (let c of str) {
        if (isValidXmlChar(c)) result.push(c)
    }

    return result.join('')
}

/**
 * @param {object} obj 
 * @returns {object} The object with original keys, but has values with all 
 * invalid xml chars removed.
 */
function withoutInvalidXmlCharObj(obj) {
    let result = {}
    Object.keys(obj).forEach((key, index) => {
        result[key] = withoutInvalidXmlChar(obj[key])
    });
    return result
}

function toXml(obj) {
    let builder = new xml2js.Builder();
    let xml = builder.buildObject(obj);
    return xml
}

/**
 * Usage: range(2, 3): [2, 3, 4]
 */
function* range(start, count) {
    if (count < 0) {
        throw new Error('count cannot be negative.')
    }

    for (let n = start; n < start + count; n++) {
        yield n
    }
}

function sanitizeFilename(name) {
    return name.replace(/[^a-zA-Z0-9_\\-\\.]/g, '')
}

/**
 * @param {string} lastUpdated Should include format like '2017/06/07 15:38 GMT'
 * @returns {number} in ms 
 */
function parseDate(lastUpdated) {
    let match = lastUpdated.match(/(\d{4})\D+(\d{1,2})\D+(\d{1,2})\D+(\d{1,2})\D+(\d{1,2})/)
    return Date.UTC(match[1], match[2] - 1, match[3], match[4], match[5], 0, 0)
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

/**
 * @NoThrow
 * Log the message with current time stamp.
 * @param {string} msg 
 */
function log(msg, path = filePath) {
    let data = new Date().toISOString() + '   ' + msg + '\n'

    // appendFile automatically creates the given file.
    // TODO: multiple writes at the same time?
    fs.appendFile(path, data, err => {
        if (err) {
            unloggedErrors += data + '\n\n' + err.stack + '\n\n'
        }
    })
}

/**
 * Downloads the webpage.
 * @param {*} host E.g. www.google.com
 * @param {*} path E.g. /xyz/123
 * @param {(string, Error) => void} callback 
 */
function httpsDownload(host, path, callback) {
    let options = {
        host: host,
        path: path
    };

    let content = ''
    https.get(options, res => {
        res.setEncoding("utf8")
        res.on("data", chunk => { content += chunk; })
        res.on("end", () => callback(content, undefined))
    }).on('error', err => callback(undefined, err))
}

exports.toXml = toXml
exports.withoutInvalidXmlCharObj = withoutInvalidXmlCharObj
exports.sanitizeFilename = sanitizeFilename
exports.parseDate = parseDate
exports.repeat = repeat
exports.log = log
exports.httpsDownload = httpsDownload
