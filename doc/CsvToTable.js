function setTable(tableId, content, firstRowIsHeader) {
    if (firstRowIsHeader === void 0) { firstRowIsHeader = false; }
    document.getElementById(tableId).innerHTML = getInnerHtml(content, firstRowIsHeader);
}
function getInnerHtml(str, firstRowIsHeader) {
    if (firstRowIsHeader === void 0) { firstRowIsHeader = false; }
    var lines = str
        .split(/(\r\n)|\n/)
        .filter(function (s) { return s !== undefined && !_isEmptyLine(s); })
        .map(function (line) { return line.split(','); });
    return _arrayToHtml(lines, firstRowIsHeader);
}
function _isEmptyLine(s) {
    return !/[^\s]/.test(s);
}
function _arrayToHtml(array, firstRowIsHeader) {
    var tr = array.map(function (line) { return _includeInTags(line, 'td'); });
    if (firstRowIsHeader)
        tr[0] = _includeInTags(array[0], 'th');
    return _includeInTags(tr, 'tr');
}
function _includeInTags(array, tagName) {
    return array.map(function (s) { return ("<" + tagName + ">" + s + "</" + tagName + ">"); }).join('');
}
