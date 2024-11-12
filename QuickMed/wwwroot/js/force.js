

function onInitTable(tableId) {
    const table = document.getElementById(tableId).getElementsByTagName("tbody")[0];
    for (var i = 0; i <= 5; i++) {
        const newRow = table.insertRow();
        const cell1 = newRow.insertCell(0);
        cell1.innerHTML = i + 1;
        const cell2 = newRow.insertCell(1);
        cell2.innerHTML = `<select class ='select2'>
        <option value="Option1">Option 1</option>
        <option value="Option2">Option 2</option>
        <option value="Option3">Option 3</option>
    </select>`;
    }
    $('#' + tableId).editableTableWidget().find('td:first').focus();
}