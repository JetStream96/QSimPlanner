/** Set the table with given id with the provided contents in csv.
 *  Entries with comma(,) is not supported. 
 */
function setTable(tableId: string, content: string, firstRowIsHeader = false): void {
    document.getElementById(tableId).innerHTML = getInnerHtml(content, firstRowIsHeader);
}

function getInnerHtml(str: string, firstRowIsHeader = false): string {
    let lines = str
        .split(/(\r\n)|\n/)
        .filter(s => s !== undefined && !_isEmptyLine(s))
        .map(line => line.split(','));

    return _arrayToHtml(lines, firstRowIsHeader);
}

function _isEmptyLine(s: string): boolean {
    return !/[^\s]/.test(s);
}

function _arrayToHtml(array: Array<Array<string>>, firstRowIsHeader: boolean): string {
    let tr = array.map(line => _includeInTags(line, 'td'));
    if (firstRowIsHeader) tr[0] = _includeInTags(array[0], 'th');
    return _includeInTags(tr, 'tr');
}

function _includeInTags(array: Array<string>, tagName: string): string {
    return array.map(s => `<${tagName}>${s}</${tagName}>`).join('');
}