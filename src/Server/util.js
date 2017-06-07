const xml2js = require('xml2js')

const validXmlChars = /[\u0009\u000a\u000d\u0020-\uD7FF\uE000-\uFFFD]/

function isValidXmlChar(c) {
    if (c.length > 1) {
        throw new Error()
    }

    return validXmlChars.test(c)
}

/**
 * @param {string} str 
 * @returns {string} A string with all invalid xml chars removed.
 */
function withoutInvalidXmlChar(str) {
    let result = []
    
    for (let c of str) {
        if (isValidXmlChar(c)) {
            result.push(c)
        }
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
    Object.keys(obj).forEach((key,index) => {
        result[key] = withoutInvalidXmlChar(obj[key])
    });
    return result
}

function toXml(obj)
{
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

exports.toXml = toXml
exports.withoutInvalidXmlCharObj = withoutInvalidXmlCharObj
