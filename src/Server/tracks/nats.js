const https = require('https')
const xml2js = require('xml2js')

/**
 * @param {(err, html) => void} callback html is a string
 */
function downloadHtml(callback) {
    let options = {
        host: 'www.notams.faa.gov',
        port: 443,
        path: '/common/nat.html'
    };

    let content = ''
    https.get(options, res => {
        res.setEncoding("utf8")
        res.on("data", chunk => content += chunk)
        res.on("end", () => callback(null, content))
    }).on('error', err => callback(err, null))
}

function removeHtmlTags(str)
{
    return str.replace(/<[^>]*>/g, '')
}

/**
 * @returns {[boolean, object]} bool: Whether westbound tracks part is found.
 * object: The content of westbound track xml.
 */
function getWestboundTracks(html)
{
    let match = html.match(/\n([^\n]*?EGGXZOZX[\s\S]*?)<\/td>/)
    if (!match) return [false, null];
    let message = removeHtmlTags(match[1])
    let [time, header] = _getGeneralInfo(html)
    
    return [true, { 
        lastUpdated: time, 
        header: header,
        direction: 'West',
        message: message
    }]
}

/**
 * @returns {[boolean, object]} bool: Whether eastbound tracks part is found.
 * object: The content of eastbound track xml.
 */
function getEastboundTracks(html)
{
    let match = html.match(/\n([^\n]*?CZQXZQZX[\s\S]*?)<\/td>/)
    if (!match) return [false, null];
    let message = removeHtmlTags(match[1])
    let [time, header] = _getGeneralInfo(html)
    
    return [true, { 
        lastUpdated: time, 
        header: header,
        direction: 'East',
        message: message
    }]
}

/**
 * @param {string} html 
 * @returns {[string, string]} [timeUpdated, header]
 */
function _getGeneralInfo(html)
{
    let matchTime = html.match(/(Last updated.*?)<\//)
    let matchHeader = html.match(/(The following are active North Atlantic Tracks.*?)<\//)
    return [matchTime[1], matchHeader[1]]
}

function toXml(obj)
{
    let builder = new xml2js.Builder();
    let xml = builder.buildObject(obj);
    console.log(xml)
    return xml
}

exports.downloadHtml = downloadHtml
exports.getWestboundTracks = getWestboundTracks
exports.getEastboundTracks = getEastboundTracks
exports.toXml = toXml
