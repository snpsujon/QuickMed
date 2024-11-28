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


function populateTreatmentTable(dataArray, tblId) {
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
        $("#treatmentId").val(data.tempId);

        const newRow = document.createElement("tr");
        newRow.setAttribute("data-value", data.index);


        //data.brand?.value == 0 ? "N/A" : data.brand?.text || "N/A"
        const cells = [
            { text: index + 1, value: index + 1 },
            { text: data.brand?.value == 0 ? "N/A" : data.brand?.text || "N/A", value: data.brand?.value || "" },
            { text: data.dose?.value == 0 ? "N/A" : data.dose?.text || "N/A", value: data.dose?.value || "" },
            { text: data.instruction?.value == 0 ? "N/A" : data.instruction?.text || "N/A", value: data.instruction?.value || "" },
            { text: data.duration?.value == 0 ? "N/A" : data.duration?.text || "N/A", value: data.duration?.value || "" }
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
        editButton.setAttribute("onclick", "QrowEdit(this);");
        editButton.innerHTML = '<i class="dripicons-pencil"></i>';
        buttonDiv.appendChild(editButton);

        // Delete button
        const deleteButton = document.createElement("button");
        deleteButton.id = "bElim";
        deleteButton.type = "button";
        deleteButton.className = "btn btn-sm btn-soft-danger btn-circle me-2";
        deleteButton.setAttribute("onclick", "tretmentDelete(this);");
        deleteButton.innerHTML = '<i class="dripicons-trash" aria-hidden="true"></i>';
        buttonDiv.appendChild(deleteButton);

        // Fav button
        const favButton = document.createElement("button");
        favButton.id = "bFav";
        favButton.type = "button";
        favButton.className = "btn btn-sm btn-soft-warning btn-circle";
        favButton.setAttribute("onclick", "rowFav(this);");
        favButton.innerHTML = '<i class="dripicons-heart" aria-hidden="true"></i>';
        buttonDiv.appendChild(favButton);

        // Accept button (hidden by default)
        const acceptButton = document.createElement("button");
        acceptButton.id = "bAcep";
        acceptButton.type = "button";
        acceptButton.className = "btn btn-sm btn-soft-purple me-2 btn-circle";
        acceptButton.style.display = "none";
        acceptButton.setAttribute("onclick", "QrowAcep(this);");
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

function QrowEdit(but) {
    var $td = $("tr[id='editing'] td");
    QrowAcep($td)
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

    //var cont = $td.html();
    var $secondTd = $row.find('td').eq(1);
    var cont = $secondTd.find('input').val();
    var div1 = '<div style="display: none;">' + cont + '</div>';  //guarda contenido
    var input1 = '<select class="form-control customselect2 ousud custom-select" value=""></select>';
    $secondTd.html(div1 + input1);

    //var cont = $td.html();
    var $thirdTd = $row.find('td').eq(2);
    cont = $thirdTd.find('input').val();
    div1 = '<div style="display: none;">' + cont + '</div>';  //guarda contenido
    input1 = '<select class="form-control customselect2 dose custom-select" value=""></select>';
    $thirdTd.html(div1 + input1);


    //var cont = $td.html();
    var $forthTd = $row.find('td').eq(3);
    cont = $forthTd.find('input').val();
    div1 = '<div style="display: none;">' + cont + '</div>';  //guarda contenido
    input1 = '<select class="form-control customselect2 instruc custom-select" value=""></select>';
    $forthTd.html(div1 + input1);


    //var cont = $td.html();
    var $fifthTd = $row.find('td').eq(4);
    cont = $fifthTd.find('input').val();
    div1 = '<div style="display: none;">' + cont + '</div>';  //guarda contenido
    input1 = '<select class="form-control customselect2 duration custom-select" value=""></select>';
    $fifthTd.html(div1 + input1);


    FijModoEdit(but);

    customSelect2(true);
    getTreatmentData(but);
}

function QrowAcep(but) {
    var $row = $(but).parents('tr');  //accede a la fila
    var $cols = $row.find('td');  //lee campos
    if (!ModoEdicion($row)) return;  //Ya está en edición
    //Está en edición. Hay que finalizar la edición
    IterarCamposEdit($cols, function ($td) {  //itera por la columnas
        var cont = $td.find('input').val(); //lee contenido del input
        $td.html(cont);  //fija contenido y elimina controles
    });


    updateTreatmrntArray(but);

    FijModoNormal(but);
    params.onEdit($row);


}

function rowFav(but) {
    var row = $(but).closest('tr')[0];  //accede a la fila
    if (row && row.hasAttribute("data-value")) {
        const treatmentIndex = parseInt(row.getAttribute("data-value"), 10);


        const treatmentIndexInArray = treatments.findIndex(t => t.index === treatmentIndex);

        if (treatmentIndexInArray === -1) {
            console.error("Treatment not found in the array.");
            return;
        }

        // Get the treatment object to update
        const treatmentData = treatments[treatmentIndexInArray];

        var data = {
            templateName: treatmentData['brand']['text'] + '_' + getRandomInteger(1, 9999),
            brandSelect: treatmentData['brand']['value'],
            doseSelect: treatmentData['dose']['value'],
            instructionSelect: treatmentData['instruction']['value'],
            durationSelectfav: treatmentData['duration']['value']
        };
        if (instanceReferenceforFavDrag != null) {
            instanceReferenceforFavDrag.invokeMethodAsync('SaveFavOusud', JSON.stringify(data))
                .then((data) => {
                    if (data) {
                        showAlert("Save Successful", "Record has been successfully Added to Fav List.", "success", "swal-success");
                    }
                })
                .catch(err => console.error('Error:', err));
        }
    } else {
        console.error("Could not find the row element or get its data-value attribute.");
    }


}




function updateTreatmrntArray(but) {
    const row = $(but).closest('tr')[0]; // Get the parent row
    const col = $(row).find('td'); // Get all columns in the row
    const treatmentIndex = parseInt(row.getAttribute("data-value"), 10); // Read data-value for treatment index
    const treatmentIndexInArray = treatments.findIndex(t => t.index === treatmentIndex); // Find the treatment in the array

    if (treatmentIndexInArray === -1) {
        console.error("Treatment not found in the array.");
        return;
    }

    // Get the treatment object to update
    const treatmentData = treatments[treatmentIndexInArray];

    // Iterate over each column to extract and update data
    col.each(function (index) {
        if (index !== 0 && index !== (col.length - 1)) { // Skip the first and last columns
            var td = $(this);
            var select = td.find('select'); // Get the select element
            if (select.length) {
                var value = select.val(); // Get the selected value
                var text = select.find("option:selected").text(); // Get the selected text
                td.text(text); // Update the table cell with the selected text
                td.attr("data-value", value); // Set the data-value attribute for reference

                // Update the treatment object based on the column index
                if (index === 1) {
                    treatmentData['brand']['value'] = value;
                    treatmentData['brand']['text'] = text;
                } else if (index === 2) {
                    treatmentData['dose']['value'] = value;
                    treatmentData['dose']['text'] = text;
                } else if (index === 3) {
                    treatmentData['instruction']['value'] = value;
                    treatmentData['instruction']['text'] = text;
                } else if (index === 4) {
                    treatmentData['duration']['value'] = value;
                    treatmentData['duration']['text'] = text;
                }
            }
        }
    });

    // Log the updated treatment data for debugging
    console.log("Updated Treatment:", treatmentData);
    console.log("Updated Treatments Array:", treatments);
}


function getTreatmentData(but) {
    const row = $(but).closest('tr')[0];
    const secondTd = $(row).find('td').eq(1);
    if (row && row.hasAttribute("data-value")) {

        const treatmentIndex = parseInt(row.getAttribute("data-value"), 10);
        const treatmentIndexss = treatments.findIndex(t => t.index === treatmentIndex);
        var selectedData = treatments[treatmentIndexss];
        var datas = treatments[treatmentIndexss]['brand']['value'];

        if (instanceReference) {
            instanceReference.invokeMethodAsync("GetOusudData", datas)
                .then(data => {

                    setSelectOptions('ousud', data.ousud, datas);
                    setSelectOptions('duration', data.duration, selectedData['duration']['value']);
                    setSelectOptions('instruc', data.instruction, selectedData['instruction']['value']);
                    setSelectOptions('dose', data.dose, selectedData['dose']['value']);

                    //console.log("Data received from Blazor:", data);
                })
                .catch(error => {
                    console.error("Error:", error);
                });
        } else {
            console.error("Instance reference is not set.");
        }


        //DotNet.invokeMethodAsync('QuickMed', 'GetOusudData', data)
        //    .then(data => {
        //        console.log("Data retrieved:", data); // Log the data or use it
        //        // Process the data as needed
        //    })
        //    .catch(error => {
        //        console.error("Error retrieving data:", error);
        //    });

    }


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

    $(table).find('tbody tr').each(function (index) {
        $(this).children(":first").text(index + 1); // Update the first column with the new index
    });
}

function getAdviceValue(e) {
    const adviceSelect = document.getElementById("adviceSelect");
    if (!adviceSelect) {
        console.error("Advice select element not found in the DOM.");
        return null;
    }
    return adviceSelect.value;
}

function populateAdviceTable(dataArray, tblId) {

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
            { text: data?.advice || "N/A", value: data?.id || "" }
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

function GetTretmentTempData() {
    const tempname = $("#TempName").val();
    const tempId = $("#treatmentId").val();
    var data = {
        templateName: tempname == '' ? 'TreatmentTemp_' + getRandomInteger(1, 9999) : tempname,
        treatment: treatments,
        advice: GetAdviceTblData(),
        tempId: tempId
    };
    return data;
}

function GetAdviceTblData() {
    // Get the data from the advice table
    const tableBody = $("#TretmentTmpAdviceTbl tbody")[0];
    const adviceData = [];
    $(tableBody).find("tr").each(function () {
        const secondCell = $(this).children("td").eq(1);
        const select = secondCell.find("select");
        if (select.length) {
            adviceData.push(select.find("option:selected").text());
        }
        else {
            const cellData = secondCell.text().trim();
            adviceData.push(cellData);
        }

    });
    return adviceData;
}

// To toggle visibility
function toggleButtonVisibility() {
    const saveBtn = document.getElementById("saveBtn");
    const updateBtn = document.getElementById("updateBtn");
    const cancelBtn = document.getElementById("cancelBtn");

    if (saveBtn.style.display === "none") {
        saveBtn.style.display = "block";
        updateBtn.style.display = "none";
        cancelBtn.style.display = "none";
    } else {
        saveBtn.style.display = "none";
        updateBtn.style.display = "block";
        cancelBtn.style.display = "block";
    }
}

function ClearTable(tableId) {
    const table = document.getElementById(tableId);
    const tbody = table.querySelector("tbody");

    if (tbody) {
        while (tbody.firstChild) {
            tbody.removeChild(tbody.firstChild);
        }
    } else {
        console.error("Table body not found in table with ID:", tableId);
    }
}
