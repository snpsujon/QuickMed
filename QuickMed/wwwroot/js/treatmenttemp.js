const treatments = [];

function OsudAddbtn() {
    // Get the selected elements
    const brandSelect = document.getElementById("brandSelect");
    const doseSelect = document.getElementById("doseSelect");
    const durationSelect = document.getElementById("durationSelect");
    const instructionSelect = document.getElementById("instructionSelect");

    if (!brandSelect || !doseSelect || !durationSelect || !instructionSelect) {
        console.error("One or more select elements were not found in the DOM.");
        return null;
    }

    // Create an object to store both the selected text and value
    const formData = {
        index: treatments.length + 1,
        brand: {
            value: brandSelect.value,
            text: brandSelect.selectedOptions[0].text
        },
        dose: {
            value: doseSelect.value,
            text: doseSelect.selectedOptions[0].text
        },
        duration: {
            value: durationSelect.value,
            text: durationSelect.selectedOptions[0].text
        },
        instruction: {
            value: instructionSelect.value,
            text: instructionSelect.selectedOptions[0].text
        }
    };

    // Add the object to the selections array
    treatments.push(formData);

    // Return the array of selections
    return treatments;

}


function populateTreatmentTable(dataArray) {
    console.log("Data received:", dataArray);

    // Convert to array if it is an object with array properties
    if (!Array.isArray(dataArray) && typeof dataArray === 'object') {
        dataArray = Object.values(dataArray);
    }

    if (!Array.isArray(dataArray)) {
        console.error("Expected an array but received:", dataArray);
        return;
    }



    const tableBody = document.getElementById("TretmentTmpTbl").getElementsByTagName("tbody")[0];

    tableBody.innerHTML = "";

    dataArray.forEach((data, index) => {
        const newRow = document.createElement("tr");
        newRow.setAttribute("data-value", data.index);

        const cells = [
            { text: index + 1, value: index + 1 },
            { text: data.brand.text, value: data.brand.value },
            { text: data.dose.text, value: data.dose.value },
            { text: data.instruction.text, value: data.instruction.value },
            { text: data.duration.text, value: data.duration.value }
        ];

        cells.forEach(cellData => {
            const cell = document.createElement("td");
            cell.textContent = cellData.text;
            cell.setAttribute("data-value", cellData.value);
            newRow.appendChild(cell);
        });

        // Create the 'buttons' cell with edit, delete, accept, and cancel buttons
        const buttonCell = document.createElement("td");
        buttonCell.setAttribute("name", "buttons");

        const buttonDiv = document.createElement("div");
        buttonDiv.className = "float-end";

        // Edit button
        const editButton = document.createElement("button");
        editButton.id = "bEdit";
        editButton.type = "button";
        editButton.className = "btn btn-sm btn-soft-success btn-circle me-2";
        editButton.setAttribute("onclick", "rowEdit(this);");
        editButton.innerHTML = '<i class="dripicons-pencil"></i>';
        buttonDiv.appendChild(editButton);

        // Delete button
        const deleteButton = document.createElement("button");
        deleteButton.id = "bElim";
        deleteButton.type = "button";
        deleteButton.className = "btn btn-sm btn-soft-danger btn-circle";
        deleteButton.setAttribute("onclick", "tretmentDelete(this);");
        deleteButton.innerHTML = '<i class="dripicons-trash" aria-hidden="true"></i>';
        buttonDiv.appendChild(deleteButton);

        // Accept button (hidden by default)
        const acceptButton = document.createElement("button");
        acceptButton.id = "bAcep";
        acceptButton.type = "button";
        acceptButton.className = "btn btn-sm btn-soft-purple me-2 btn-circle";
        acceptButton.style.display = "none";
        acceptButton.setAttribute("onclick", "rowAcep(this);");
        acceptButton.innerHTML = '<i class="dripicons-checkmark"></i>';
        buttonDiv.appendChild(acceptButton);

        // Cancel button (hidden by default)
        const cancelButton = document.createElement("button");
        cancelButton.id = "bCanc";
        cancelButton.type = "button";
        cancelButton.className = "btn btn-sm btn-soft-info btn-circle datarowdelete";
        cancelButton.style.display = "none";
        cancelButton.setAttribute("onclick", "rowCancel(this);");
        cancelButton.innerHTML = '<i class="dripicons-cross" aria-hidden="true"></i>';
        buttonDiv.appendChild(cancelButton);

        // Append the button div to the button cell, and the cell to the row
        buttonCell.appendChild(buttonDiv);
        newRow.appendChild(buttonCell);

        // Append the row to the table body
        tableBody.appendChild(newRow);
    });
}

function tretmentDelete(but, tableid) {
    const row = $(but).closest('tr')[0];
    const table = $(but).closest('table')[0];

    console.log("Row found:", row);

    if (row && row.hasAttribute("data-value")) {
        const treatmentIndex = parseInt(row.getAttribute("data-value"), 10);

        console.log("Treatment index to remove:", treatmentIndex);

        const treatmentToRemoveIndex = treatments.findIndex(t => t.index === treatmentIndex);

        if (treatmentToRemoveIndex !== -1) {
            treatments.splice(treatmentToRemoveIndex, 1);
            console.log("Updated treatments array:", treatments);
        } else {
            console.warn("No matching treatment found in the array.");
        }

    } else {
        console.error("Could not find the row element or get its data-value attribute.");
    }


    var $row = $(but).parents('tr');  //accede a la fila

    params.onBeforeDelete($row);
    $row.remove();
    params.onDelete();

    $("#TretmentTmpTbl tbody tr").each(function (index) {
        $(this).children(":first").text(index + 1);  // Updating the first column to new index + 1
    });

    //var rows = $(but).closest('tr');


}