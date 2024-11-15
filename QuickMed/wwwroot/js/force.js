function onInitTable(tableId, data) {
    $(document).ready(function () { // Ensure DOM is fully loaded
        const table = document.getElementById(tableId).getElementsByTagName("tbody")[0];

        // Clear the table first
        clearTable(tableId);

        // Now, insert the new rows
        for (var i = 0; i < 5; i++) {  // Note: Change to `i < 6` to add exactly 6 rows
            const newRow = table.insertRow();

            const cell1 = newRow.insertCell(0);
            cell1.innerHTML = i + 1;

            const cell2 = newRow.insertCell(1);

            // Create the select element
            let select = document.createElement("select");
            select.classList.add("select2");

            // Add an empty option as the placeholder for select2
            let placeholderOption = document.createElement("option");
            placeholderOption.value = "";
            placeholderOption.text = "Select One";
            select.appendChild(placeholderOption);

            // Loop through data array to create options
            if (Array.isArray(data) && data.length > 0) { // Check if data is valid
                data.forEach(optionText => {
                    const option = document.createElement("option");
                    option.value = optionText.adviceName;
                    option.text = optionText.adviceName;
                    select.appendChild(option);
                });
            }

            // Append the select element to the cell
            cell2.appendChild(select);

            // Apply select2 after appending to the DOM
            $(select).select2({
                width: "100%",
                placeholder: "Select One", // Ensures placeholder text is shown
                allowClear: true,
                tags: true// Enables clear button
            });
            const cell3 = newRow.insertCell(2);

            // Create a button dynamically
            let deleteButton = document.createElement("button");
            deleteButton.className = "btn btn-soft-danger btn-sm";
            deleteButton.innerHTML = '<i class="dripicons-trash"></i>';
            deleteButton.onclick = function () {
                debugger;
                const row = this.closest('tr'); // Find the closest row
                const index = row.rowIndex - 1;
                table.deleteRow(index);
            };

            // Append the button to cell3
            cell3.appendChild(deleteButton);
        }
    });
}



function initializeButtonClick(data) {
    document.getElementById("but_add").addEventListener("click", function () {
        addNewRowOnTable(data);
    });
}

function addNewRowOnTable(data) {
    const table = document.getElementById("mainTable-advice").getElementsByTagName("tbody")[0];
    var rowsLength = table.rows.length;
    rowsLength = rowsLength + 1;
    const newRow = table.insertRow();

    // Add cells with the specified content
    const cell1 = newRow.insertCell(0);
    cell1.innerHTML = rowsLength;

    const cell2 = newRow.insertCell(1);

    let select = document.createElement("select");
    select.classList.add("select2");

    // Add a blank option as the default selected option
    var op = document.createElement("option");
    op.value = "";
    op.text = "Select One";
    op.selected = true;
    op.disabled = true;
    select.appendChild(op);

    // Loop through data array to create options






    data.forEach(optionText => {
        const option = document.createElement("option");
        option.value = optionText.adviceName;
        option.text = optionText.adviceName;
        select.appendChild(option);
    });

    // Append the select element to the cell
    cell2.appendChild(select);

    // Apply select2 to the newly added select element
    $(select).select2({
        width: "100%",
        placeholder: "Select One",
        allowClear: true,
        tags:true
    });

    const cell3 = newRow.insertCell(2);

    // Create a button dynamically
    let deleteButton = document.createElement("button");
    deleteButton.className = "btn btn-soft-danger btn-sm";
    deleteButton.innerHTML = '<i class="dripicons-trash"></i>';
    deleteButton.onclick = function () {
        debugger;
        const row = this.closest('tr'); // Find the closest row
        const index = row.rowIndex - 1;
        table.deleteRow(index);
    };

    // Append the button to cell3
    cell3.appendChild(deleteButton);


}
function GetTableData(tableId) {

    const selectedValues = [];
    const table = document.getElementById(tableId).getElementsByTagName("tbody")[0];

    // Loop through each row in the table
    for (let i = 0; i < table.rows.length; i++) {
        const row = table.rows[i];
        const select = row.cells[1].querySelector(".select2"); // Get the select element in the second cell

        if (select) {
            selectedValues.push(select.value); // Add the selected value to the array
        }
    }

    console.log(selectedValues); // Output selected values for debugging
    return selectedValues;
}

function clearTable(tableId) {
    debugger;
    const table = document.getElementById(tableId).getElementsByTagName("tbody")[0];

    // Deleting rows from the last one to the first
    for (let i = table.rows.length - 1; i >= 0; i--) {
        table.deleteRow(i);
    }
}
function GeneTable(tableId,masterDataList,selectedDataList) {
    const table = document.getElementById(tableId).getElementsByTagName("tbody")[0];
    clearTable(tableId)
}