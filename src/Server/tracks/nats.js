const http = require('http')
const xml2js = require('xml2js')

/**
 * @param {(err, html) => void} callback html is a string
 */
function downloadHtml(callback) {
    let options = {
        host: 'https://www.notams.faa.gov',
        port: 80,
        path: '/common/nat.html?'
    };

    let content = ''
    http.get(options, res => {
        res.setEncoding("utf8");
        res.on("data", chunk => content += chunk)
        res.on("end", () => callback(null, content))
    }).on('error', err => callback(err, null))
}

function removeHtmlTags(str)
{
    str.replace(/<[^>]*>/, '')
}

/**
 * @returns {[bool, object]} bool: Whether westbound tracks part is found.
 * object: The content of westbound track xml.
 */
function getWestboundTracks(html)
{
    let match = html.match(/\\n([^\\n]*?EGGXZOZX[\\s\\S]]*?)<\/td>/)
    if (!match) return [false, null];
    let message = removeHtmlTags(match[0])
    let [time, header] = _getGeneralInfo(html)
    
    return { 
        LastUpdated: time, 
        Header: header,
        Direction: 'West',
        Message: message
    }
}

/**
 * @returns {[bool, object]} bool: Whether eastbound tracks part is found.
 * object: The content of eastbound track xml.
 */
function getEastboundTracks(html)
{
    let match = html.match(/\\n([^\\n]*?CZQXZQZX.*?)<\/td>/)
    if (!match) return [false, null];
    let message = removeHtmlTags(match[0])
    let [time, header] = _getGeneralInfo(html)
    
    return { 
        LastUpdated: time, 
        Header: header,
        Direction: 'East',
        Message: message
    }
}

/**
 * @param {string} html 
 * @returns {[string, string]} [timeUpdated, header]
 */
function _getGeneralInfo(html)
{
    let matchTime = html.match(/(Last updated.*?)<\//)
    let matchHeader = html.match(/(The following are active North Atlantic Tracks.*?)<\//)
    return [matchTime[0], matchHeader[0]]
}

function toXml(obj)
{
    let builder = new xml2js.Builder();
    let xml = builder.buildObject(obj);
    return xml
}
