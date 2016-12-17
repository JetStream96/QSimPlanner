/** Set the table with given id with the provided contents in csv.
 *  Entries with comma(,) is not supported.
 */
function setTable(tableId, content, firstRowIsHeader) {
    if (firstRowIsHeader === void 0) { firstRowIsHeader = false; }
    document.getElementById(tableId).innerHTML = getInnerHtml(content, firstRowIsHeader);
}
function getInnerHtml(str, firstRowIsHeader) {
    if (firstRowIsHeader === void 0) { firstRowIsHeader = false; }
    var lines = str
        .split(/(\r\n)|\n/)
        .filter(function (s) { return s !== undefined && !isEmptyLine(s); })
        .map(function (line) { return line.split(','); });
    return arrayToHtml(lines, firstRowIsHeader);
}
function isEmptyLine(s) {
    return !/[^\s]/.test(s);
}
function arrayToHtml(array, firstRowIsHeader) {
    var tr = array.map(function (line) { return includeInTags(line, 'td'); });
    if (firstRowIsHeader)
        tr[0] = includeInTags(array[0], 'th');
    return includeInTags(tr, 'tr');
}
function includeInTags(array, tagName) {
    return array.map(function (s) { return "<" + tagName + ">" + s + "</" + tagName + ">"; }).join('');
}
