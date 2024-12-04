
const mainTblData = [];

function pushtoMixTemplate(formData) {
    formData['index'] = mainTblData.length + 1;
    mainTblData.push(formData);
    return mainTblData;
}
function ClearmainTableArray() {
    mainTblData.length = 0;
}

function SetMasterData(data) {
    // Assuming 'data' is an object with corresponding keys to input IDs
    $('#TempName').val(data.name);
    $('#mixTempID').val(data.id);
    $('#doseInsSelect').val(data.dose).trigger('change');
    $('#durationInsSelect').val(data.duration).trigger('change');
    $('#totalqty').val(data.totalQty);
    $('#instructionInsSelect').val(data.instruction).trigger('change');
    $('#notesIns').val(data.notes);
}





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

    FijModoEdit(but);

    customSelect2(true);
    getMixTableData(but);

}





function getMixTableData(but) {
    const row = $(but).closest('tr')[0];
    const secondTd = $(row).find('td').eq(1);
    if (row && row.hasAttribute("data-value")) {

        const treatmentIndex = parseInt(row.getAttribute("data-value"), 10);
        const treatmentIndexss = mainTblData.findIndex(t => t.index === treatmentIndex);
        var selectedData = mainTblData[treatmentIndexss];
        var datas = mainTblData[treatmentIndexss]['brand']['value'];

        if (instanceReference) {
            instanceReference.invokeMethodAsync("GetOusudData", datas)
                .then(data => {

                    setSelectOptions('ousud', data.ousud, datas);
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


function QrowAcepmixTemp(but) {

    var $row = $(but).parents('tr');  //accede a la fila
    var $cols = $row.find('td');  //lee campos
    if (!ModoEdicion($row)) return;  //Ya está en edición
    //Está en edición. Hay que finalizar la edición
    IterarCamposEdit($cols, function ($td) {  //itera por la columnas
        var cont = $td.find('input').val(); //lee contenido del input
        $td.html(cont);  //fija contenido y elimina controles
    });


    updateMixTempArray(but);
    FijModoNormal(but);
    params.onEdit($row);
}

function updateMixTempArray(but) {
    const row = $(but).closest('tr')[0]; // Get the parent row
    const col = $(row).find('td'); // Get all columns in the row
    const treatmentIndex = parseInt(row.getAttribute("data-value"), 10); // Read data-value for treatment index
    const treatmentIndexInArray = mainTblData.findIndex(t => t.index === treatmentIndex); // Find the treatment in the array

    if (treatmentIndexInArray === -1) {
        console.error("Treatment not found in the array.");
        return;
    }

    // Get the treatment object to update
    const treatmentData = mainTblData[treatmentIndexInArray];

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
                }
            }
        }
    });



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

function GetMixTempData() {

    const tempname = $("#TempName").val();
    const doseInsSelect = $("#doseInsSelect").val();
    const durationInsSelect = $("#durationInsSelect").val();
    const totalqty = $("#totalqty").val();
    const instructionInsSelect = $("#instructionInsSelect").val();
    const notesIns = $("#notesIns").val();



    var data = {
        templateName: tempname == '' ? 'MixTemp_' + getRandomInteger(1, 9999) : tempname,
        tempData: mainTblData,
        doseInsSelect: doseInsSelect,
        durationInsSelect: durationInsSelect,
        totalqty: totalqty,
        instructionInsSelect: instructionInsSelect,
        notesIns: notesIns,
        tempId: $('#mixTempID').val()

    };
    return data;
}


