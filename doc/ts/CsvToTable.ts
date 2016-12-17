/** Set the table with given id with the provided contents in csv.
 *  Entries with comma(,) is not supported. 
 */
function setTable(tableId: string, content: string, firstRowIsHeader = false): void {
    document.getElementById(tableId).innerHTML = getInnerHtml(content, firstRowIsHeader);
}

function getInnerHtml(str: string, firstRowIsHeader = false): string {
    let lines = str
        .split(/(\r\n)|\n/)
        .filter(s => s !== undefined && !isEmptyLine(s))
        .map(line => line.split(','));

    return arrayToHtml(lines, firstRowIsHeader);
}

function isEmptyLine(s: string): boolean {
    return !/[^\s]/.test(s);
}

function arrayToHtml(array: Array<Array<string>>, firstRowIsHeader: boolean): string {
    let tr = array.map(line => includeInTags(line, 'td'));
    if (firstRowIsHeader) tr[0] = includeInTags(array[0], 'th');
    return includeInTags(tr, 'tr');
}

function includeInTags(array: Array<string>, tagName: string): string {
    return array.map(s => `<${tagName}>${s}</${tagName}>`).join('');
}