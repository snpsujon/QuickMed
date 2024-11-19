
const mainTblData = [];
function AddNewPlusBtn() {
    // Get the selected elements
    const brandSelect = document.getElementById("brandTempSelect");
    const doseSelect = document.getElementById("doseTempSelect");

    if (!brandSelect || !doseSelect) {
        console.error("One or more select elements were not found in the DOM.");
        return null;
    }

    // Create an object to store both the selected text and value
    const formData = {
        index: mainTblData.length + 1,
        brand: {
            value: brandTempSelect.value,
            text: brandTempSelect.selectedOptions[0].text
        },
        dose: {
            value: doseTempSelect.value,
            text: doseTempSelect.selectedOptions[0].text
        }
    };

    // Add the object to the selections array
    mainTblData.push(formData);

    // Return the array of selections
    return mainTblData;

}



function populateMixTempTable(dataArray, tblId) {
    //getOusudData();
    console.log("Data received:", dataArray);

    // Convert to array if it is an object with array properties
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
        newRow.setAttribute("data-value", data.index);


        //data.brand?.value == 0 ? "N/A" : data.brand?.text || "N/A"
        const cells = [
            { text: index + 1, value: index + 1 },
            { text: data.brand?.value == 0 ? "N/A" : data.brand?.text || "N/A", value: data.brand?.value || "" },
            { text: data.dose?.value == 0 ? "N/A" : data.dose?.text || "N/A", value: data.dose?.value || "" }

        ];


        cells.forEach((cellData, index) => {
            const cell = document.createElement("td");
            cell.textContent = cellData.text;
            cell.setAttribute("data-value", cellData.value);
            //if (index === 1) {
            //    cell.classList.add("special-class");
            //}
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
        editButton.setAttribute("onclick", "QrowEditmixTemp(this);");
        editButton.innerHTML = '<i class="dripicons-pencil"></i>';
        buttonDiv.appendChild(editButton);

        // Delete button
        const deleteButton = document.createElement("button");
        deleteButton.id = "bElim";
        deleteButton.type = "button";
        deleteButton.className = "btn btn-sm btn-soft-danger btn-circle";
        deleteButton.setAttribute("onclick", "mixTempDelete(this);");
        deleteButton.innerHTML = '<i class="dripicons-trash" aria-hidden="true"></i>';
        buttonDiv.appendChild(deleteButton);

        // Accept button (hidden by default)
        const acceptButton = document.createElement("button");
        acceptButton.id = "bAcep";
        acceptButton.type = "button";
        acceptButton.className = "btn btn-sm btn-soft-purple me-2 btn-circle";
        acceptButton.style.display = "none";
        acceptButton.setAttribute("onclick", "QrowAcepmixTemp(this);");
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


function QrowEditmixTemp(but) {
    var $td = $("tr[id='editing'] td");
    QrowAcepmixTemp($td)
    var $row = $(but).parents('tr');
    var $cols = $row.find('td');
    if (ModoEdicion($row)) return;  //Ya está en edición
    //Pone en modo de edición
    IterarCamposEdit($cols, function ($td) {  //itera por la columnas
        var cont = $td.html(); //lee contenido
        var div = '<div style="display: none;">' + cont + '</div>';  //guarda contenido
        var input = '<input class="form-control input-sm"  value="' + cont + '" disabled>';
        $td.html(div + input);  //fija contenido
    });
    FijModoEdit(but);
    var cont = $td.html();
    var $secondTd = $row.find('td').eq(1);
    var div1 = '<div style="display: none;">' + cont + '</div>';  //guarda contenido
    var input1 = '<select class="form-control customselect2 ousud custom-select" value=""></select>';
    $secondTd.html(div1 + input1);
    customSelect2(true);
    getOusudData(but);

}

function QrowAcepmixTemp(but) {
    var $row = $(but).parents('tr');  //accede a la fila
    var $cols = $row.find('td');  //lee campos
    if (!ModoEdicion($row)) return;  //Ya está en edición
    //Está en edición. Hay que finalizar la edición
    IterarCamposEdit($cols, function ($td) {  //itera por la columnas
        var cont = $td.find('input').val(); //lee contenido del input
        $td.html(cont);  //fija contenido y elimina controles
    });

    const row = $(but).closest('tr')[0];
    const secondTd = $(row).find('td').eq(1);
    var select = secondTd.find('select'); // Get the select element
    var value = select.val(); // Get the value of the selected option
    var text = select.find("option:selected").text(); // Get the text of the selected option
    secondTd.text(text);
    secondTd.attr("data-value", value);
    const treatmentIndex = parseInt(row.getAttribute("data-value"), 10);
    updatemixTempArray(treatmentIndex);
    FijModoNormal(but);
    params.onEdit($row);


}

function updatemixTempArray(index) {
    const treatmentIndexss = mainTblData.findIndex(t => t.index === treatmentIndex);


}


function mixTempDelete(but, tableid) {
    const row = $(but).closest('tr')[0];
    const table = $(but).closest('table')[0];

    console.log("Row found:", row);

    if (row && row.hasAttribute("data-value")) {
        const treatmentIndex = parseInt(row.getAttribute("data-value"), 10);

        console.log("Treatment index to remove:", treatmentIndex);

        const treatmentToRemoveIndex = mainTblData.findIndex(t => t.index === treatmentIndex);

        if (treatmentToRemoveIndex !== -1) {
            mainTblData.splice(treatmentToRemoveIndex, 1);
            console.log("Updated treatments array:", mainTblData);
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

    $(table).find('tbody tr').each(function (index) {
        $(this).children(":first").text(index + 1); // Update the first column with the new index
    });
}

