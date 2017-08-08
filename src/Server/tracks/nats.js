const https = require('https')

/**
 * @param {(err, html: string) => void} callback
 */
function downloadHtml(callback) {
    let options = {
        host: 'www.notams.faa.gov',
        port: 443,
        path: '/common/nat.html?'
    }

    let content = ''
    https.get(options, res => {
        res.setEncoding("utf8")
        res.on("data", chunk => content += chunk)
        res.on("end", () => callback(null, content))
    }).on('error', err => callback(err, null))
}

function removeHtmlTags(str) {
    return str.replace(/<[^>]*>/g, '')
}

/**
 * @returns {[boolean, object]} bool: Whether eastbound/westbound tracks part is found.
 * object: The content of track xml.
 */
function getTracks(html, isEastbound) {
    let re = isEastbound ?
        /\n([^\n]*?CZQXZQZX[\s\S]*?)<\/td>/ :
        /\n([^\n]*?EGGXZOZX[\s\S]*?)<\/td>/
    let match = html.match(re)
    if (!match) return [false, null];
    let message = removeHtmlTags(match[1])
    let [time, header] = _getGeneralInfo(html)

    return [true, {
        LastUpdated: time,
        Header: header,
        Direction: (isEastbound ? 'East' : 'West'),
        Message: message
    }]
}

/**
 * @param {string} html 
 * @returns {[string, string]} [timeUpdated, header]
 */
function _getGeneralInfo(html) {
    let matchTime = html.match(/(Last updated.*?)<\//)
    let matchHeader = html.match(/(The following are active North Atlantic Tracks.*?)<\//)
    return [matchTime[1], matchHeader[1]]
}

exports.downloadHtml = downloadHtml
exports.getTracks = getTracks
