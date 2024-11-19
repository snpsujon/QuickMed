

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
        tags: true
    });

    const cell3 = newRow.insertCell(2);

    // Create a button dynamically
    let deleteButton = document.createElement("button");
    deleteButton.className = "btn btn-soft-danger btn-sm";
    deleteButton.innerHTML = '<i class="dripicons-trash"></i>';
    deleteButton.onclick = function () {
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
            if (select.value != "") {
                selectedValues.push(select.value); // Add the selected value to the array
            }

        }
    }

    return selectedValues;
}

function clearTable(tableId) {
    const table = document.getElementById(tableId).getElementsByTagName("tbody")[0];

    // Deleting rows from the last one to the first
    for (let i = table.rows.length - 1; i >= 0; i--) {
        table.deleteRow(i);
    }
}
function GeneTable(tableId, masterDataList, selectedDataList) {
    debugger;
    const table = document.getElementById(tableId).getElementsByTagName("tbody")[0];
    clearTable(tableId);
    // Now, insert the new rows
    for (var i = 0; i < selectedDataList.length; i++) {  // Note: Change to `i < 6` to add exactly 6 rows
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
        if (Array.isArray(masterDataList) && masterDataList.length > 0) { // Check if data is valid
            masterDataList.forEach(optionText => {
                debugger;
                const option = document.createElement("option");
                option.value = optionText.adviceName;
                option.text = optionText.adviceName;
                if (optionText.adviceName === selectedDataList[i].advice) {
                    option.selected = true;
                }

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

}



function GenerateAdviceTemplateName() {

    return 'AdviceTemp_' + getRandomInteger(1, 9999)
}

function onAdviceChange(selectElement) {
    const selectedValue = selectElement.value; 
    debugger;
    const selectedValue = selectElement.value;
    if (instanceReference) {
        instanceReference.invokeMethodAsync("ChangeAdviceData", selectedValue)
            .then(data => {
            })
            .catch(error => {
                console.error("Error:", error);
            });
    } else {
        console.error("Instance reference is not set.");
    }


}
function GetIXTempData() {
    const tableBody = $("#makeEditable_IxTemp tbody")[0];
    const secondColumnData = [];
    const tempname = $("#TempName").val();
    $(tableBody).find("tr").each(function () {
        const secondCell = $(this).children("td").eq(1);
        const cellData = secondCell.find("input").val().trim();
        secondColumnData.push(cellData);
    });

    var data = {
        templateName: tempname == '' ? 'IXTemp_' + getRandomInteger(1, 9999) : tempname,

        tempData: secondColumnData,
    };
    return data;
}






function populateIXTable(dataArray, tblId) {

    if (!Array.isArray(dataArray) && typeof dataArray === 'object') {
        dataArray = Object.values(dataArray);
    }

    if (!Array.isArray(dataArray)) {
        console.error("Expected an array but received:", dataArray);
        return;
    }

    const tableBody = $("#" + tblId + " tbody")[0];

    tableBody.innerHTML = "";
    dataArray.forEach((data, index) => {
        const newRow = document.createElement("tr");
        newRow.setAttribute("data-value", index + 1);

        const cells = [
            { text: index + 1, value: index + 1 },
            { text: data?.name || "N/A", value: data?.id || "" }
        ];

        cells.forEach(cellData => {
            const cell = document.createElement("td");
            cell.textContent = cellData.text;
            cell.setAttribute("data-value", cellData.value);
            newRow.appendChild(cell);
        });

        var buttonCell = document.createElement("td");
        buttonCell.setAttribute("name", "buttons");

        buttonCell.innerHTML = deletebun;
        newRow.appendChild(buttonCell);
        tableBody.appendChild(newRow);
    });
}
