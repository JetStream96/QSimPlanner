/** Set the table with given id with the provided contents in csv.
 *  Entries with comma(,) inside double quotes(") are not supported.
 */
function setTable(tableId, content, firstRowIsHeader=false) {
    document.getElementById(tableId).innerHTML = getInnerHtml(content, firstRowIsHeader)
}

function getInnerHtml(str, firstRowIsHeader=false) {
    var lines = str
        .split(/(\r\n)|\n/)
        .filter(s => s !== undefined && !isEmptyLine(s))
        .map(line => line.split(','))

    return arrayToHtml(lines, firstRowIsHeader)
}

function isEmptyLine(s) {
    return !/[^\s]/.test(s)
}

function arrayToHtml(array, firstRowIsHeader) {
    var tr = array.map(line => includeInTags(line, 'td'))

    if (firstRowIsHeader) {
        tr[0] = includeInTags(array[0], 'th')
    }
    return includeInTags(tr, 'tr')
}

function includeInTags(array, tagName) {
    return array.map(s => `<${tagName}>${s}</${tagName}>`).join('')
}
