function pushtoPrescription(formData) {
    formData['index'] = treatments.length + 1;
    treatments.push(formData);
    return treatments;
}


function changeNxtDatebyVal(DaytoAdd) {
    const daysToAdd = parseInt(DaytoAdd, 10) || 0;
    const today = new Date();
    today.setDate(today.getDate() + daysToAdd);
    today.setHours(today.getHours() + 6);
    const formattedDate = today.toISOString().split("T")[0];
    document.getElementById("nxtMeetDate").value = formattedDate;
}


function updateBMI() {
    const weight = parseFloat(document.getElementById('bmiWeight').value) || 0;
    const heightFeet = parseFloat(document.getElementById('bmiHeightft').value) || 0;
    const heightInches = parseFloat(document.getElementById('bmiHeightin').value) || 0;

    // Convert height to total inches
    const totalHeightInches = (heightFeet * 12) + heightInches;

    const totalHeightMeter = totalHeightInches * 0.0254;

    if (weight > 0 && totalHeightInches > 0) {
        // Calculate BMI
        const bmi = (weight) / (totalHeightMeter * totalHeightMeter);
        const bmiCategory = getBMICategory(bmi);

        const idleWeight = updateIdealWeight(totalHeightInches);

        document.getElementById('bmiValue').value = bmi.toFixed(2);
        document.getElementById('bmiClass').value = bmiCategory;
        document.getElementById('bmiIdleWeight').value = idleWeight;



    } else {

    }
}

function getBMICategory(bmi) {
    if (bmi < 18.5) {
        return "Underweight";
    } else if (bmi >= 18.5 && bmi < 24.9) {
        return "Normal weight";
    } else if (bmi >= 25 && bmi < 29.9) {
        return "Overweight";
    } else {
        return "Obese";
    }
}


function updateIdealWeight(totalHeightInches) {
    const gender = 'male';

    if (totalHeightInches > 0) {
        let idealWeightInPounds;

        if (gender === 'male') {
            idealWeightInPounds = 50 + 2.3 * (totalHeightInches - 60);
        } else {
            idealWeightInPounds = 45.5 + 2.3 * (totalHeightInches - 60);
        }

        const idealWeightInKg = idealWeightInPounds * 0.453592;

        if (idealWeightInKg <= 0) {
            return 0;
        } else {
            return idealWeightInPounds.toFixed(2);
        }
    } else {
        return 0;
    }
}

function populateCCSelect() {

    if (instanceReference) {
        instanceReference.invokeMethodAsync("GetCCSelectData")
            .then(data => {

                setSelectOptions('ccLoads1', data.ccList);
                setSelectOptions('ccLoads2', data.duration);
                setSelectOptions('ccLoads3', data.dM);


            })
            .catch(error => {
                console.error("Error:", error);
            });
    } else {
        console.error("Instance reference is not set.");
    }

}
function populateDXSelect() {
    if (instanceReference) {
        instanceReference.invokeMethodAsync("GetDXSelectData")
            .then(data => {

                setSelectOptions('dxLoads1', data.dx);

            })
            .catch(error => {
                console.error("Error:", error);
            });
    } else {
        console.error("Instance reference is not set.");
    }

}

function populateDHSelect() {
    if (instanceReference) {
        instanceReference.invokeMethodAsync("GetDHSelectData")
            .then(data => {

                setSelectOptions('dhLoads1', data.brands);
                makeSelect2Custom("dhLoads1", "GetMedicines", 3);

            })
            .catch(error => {
                console.error("Error:", error);
            });
    } else {
        console.error("Instance reference is not set.");
    }
}


function populateIXTablePres(dataArray, tblId) {

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
function populateNoteTablePres(dataArray, tblId) {

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

function getPresData(isPreview = false) {
    var Pdata = GetPetaintData();
    var nxtMeetData = GetNextMeetData();
    var ccTableData = getTableDataById('ccTable', isPreview);
    var hoTableData = getHOTableData(isPreview);
    var mhTableData = getDataColumnAsKey('mhTable');
    var oeTableData = getDataColumnAsKey('oeTable', true);
    var dhTableData = getTableDataById('dhTable', isPreview);
    var dxTableData = getTableDataById('Dxtable', isPreview);
    var ixTableData = getTableDataById('TretmentTmpIXTbl', isPreview);
    var noteTableData = getTableDataById('TretmentTmpNotesTbl', isPreview);
    var reportTableData = getTableDataById('rptEntryTbl', isPreview);
    var adviceTableData = GetAdviceTblData();
    var refferData = getQuillContent('#editors_pres');

    var data = {
        pdata: Pdata,
        nxtMeetData: nxtMeetData,
        ccTableData: ccTableData,
        hoTableData: hoTableData,
        mhTableData: mhTableData,
        oeTableData: oeTableData,
        dhTableData: dhTableData,
        dxTableData: dxTableData,
        ixTableData: ixTableData,
        noteTableData: noteTableData,
        reportTableData: reportTableData,
        treatments: treatments,
        advice: adviceTableData,
        reffer: refferData
    };
    console.log(data); // To check the retrieved data

    return data;

}


function GetPetaintData() {
    var heightInc = (parseFloat($("#bmiHeightft").val()) * 12) + parseFloat($("#bmiHeightin").val());
    var data = {
        name: $("#Patient-Name").val(),
        age: $("#Patient-Age").val(),
        sex: $("#Patient-Sex").val(),
        address: $("#Patient-Address").val(),
        mobile: $("#Patient-Mobile").val(),
        regNo: $("#Patient-Reg").val(),
        weight: $("#Patient-Wt").val(),
        date: $("#Practicum-Date").val(),
        height: heightInc.toString(),
        bmiweight: $("#bmiWeight").val()
    };

    return data; // Return the object if needed elsewhere
}

function GetNextMeetData() {
    var data = {
        nextMeetingDuration: $("#nxtMeetDateSelect").val(), // Value of the dropdown
        nextMeetingDate: $("#nxtMeetDate").val(),          // Value of the date input
        payment: $("#Payment").val(),                      // Value of the payment input
        referredBy: $("#Reffer-Book").val()                // Value of the referred by input
    };

    return data;       // Returns the data object
}


function getTableDataById(tableId, isPreview) {
    const table = document.getElementById(tableId); // Select the table by ID
    if (!table) {
        console.error(`Table with ID "${tableId}" not found.`);
        return [];
    }

    const tableData = [];
    const rows = table.querySelectorAll('tbody tr'); // Get all rows in the table's tbody

    rows.forEach((row) => {
        const rowData = {};

        row.querySelectorAll('td').forEach((cell, cellIndex) => {
            let value = '';

            // Check for input fields
            const input = cell.querySelector('input');
            if (input) {
                value = input.value.trim(); // Get value from input
            }

            // Check for select fields
            const select = cell.querySelector('select');
            if (select) {
                value = select.value.trim(); // Get the selected option's value
                if (isPreview) {
                    value = select.selectedOptions[0].text.trim(); // Get the selected option's text
                }

            }

            // Default to cell text if no input/select
            if (!input && !select) {
                value = cell.innerText.trim();
            }

            // Store data using generic column names
            rowData[`column_${cellIndex + 1}`] = value;
        });

        tableData.push(rowData); // Add row data to table data
    });
    return tableData;
}




function getHOTableData(isPreview) {
    let data = {};

    // Loop through all checkboxes with the unique class
    $('.health-checkbox').each(function () {
        let key = $(this).val(); // Use value as key
        let isChecked = $(this).is(':checked') ? true : false; // Checked = 1, Unchecked = 0
        data[key] = isChecked;
    });

    if (isPreview) {
        let dataho = {
            HealthCheckData: data, // This maps to the C# HealthCheckData property
            FreeTextHO: $('#FreeTextHO').val() // This maps to the FreeTextHO property
        };

        return dataho; // Return the structured object

    } else {
        data['FreeTextHO'] = $('#FreeTextHO').val();
        return data;
    }

}


function getDataColumnAsKey(tableId, isUnit = false) {
    const table = document.getElementById(tableId); // Select the table by ID
    if (!table) {
        console.error(`Table with ID "${tableId}" not found.`);
        return [];
    }

    const tableData = [];
    const rows = table.querySelectorAll('tbody tr');
    rows.forEach((row) => {
        const rowData = {};
        var key = '';
        var value = '';
        var unit = '';


        row.querySelectorAll('td').forEach((cell, cellIndex) => {

            if (cellIndex == 1) {
                const input = cell.querySelector('input');
                if (input) {
                    key = input.value.trim();
                }
            }
            if (cellIndex == 2) {
                const input = cell.querySelector('input');
                if (input) {
                    value = input.value.trim();
                }
            }
            if (cellIndex == 3) {
                const input = cell.querySelector('input');
                if (input) {
                    unit = input.value.trim();
                }
            }

            if ((key != '' && value != '')) {
                if (isUnit) {
                    rowData[key] = { value: value, unit: unit };
                } else {
                    rowData[key] = value;
                }
            }

        });
        if (Object.keys(rowData).length > 0) {
            tableData.push(rowData);
        }

    });
    return tableData;
}

function SearchPorP(id) {
    var value = document.getElementById(id).value;
    if (id == "Patient-Mobile") {
        if (value.length > 9 && value.length < 12) {
            if (instanceReference) {
                instanceReference.invokeMethodAsync("GetPorPResult", value, true)
                    .then(data => {

                        if (data != null) {
                            setPatientData(data);
                        }
                        else {
                            showAlert("Oh No!!", "No Data Found.", "info", "swal-info");
                        }
                    })
                    .catch(error => {
                        console.error("Error:", error);
                    });
            } else {
                console.error("Instance reference is not set.");
            }
        }
        else {
            showAlert("Oh No!!", "Mobile number will be 11 Digits.", "error", "swal-error");
        }
    }
    else {
        if (value.length == 6) {
            if (instanceReference) {
                instanceReference.invokeMethodAsync("GetPorPResult", value, false)
                    .then(data => {

                        if (data != null) {
                            //setPatientData(data);
                            console.log(data);

                        }
                        else {
                            showAlert("Oh No!!", "No Data Found.", "info", "swal-info");
                        }

                    })
                    .catch(error => {
                        console.error("Error:", error);
                    });
            } else {
                console.error("Instance reference is not set.");
            }
        }
        else {
            showAlert("Oh No!!", "Reg No will be 6 Digits.", "error", "swal-error");
        }
    }


}

function setPatientData(data) {
    document.getElementById("Patient-Name").value = data.name || '';
    document.getElementById("Patient-Address").value = data.address || '';
    document.getElementById("Patient-Mobile").value = data.mobile || '';
    document.getElementById("Patient-Wt").value = data.weight || '';
    document.getElementById("Patient-Age").value = data.age || '';
    document.getElementById("Patient-Sex").value = data.gender || '';
}


function setPrescriptionData(data) {

    document.getElementById("Payment").value = data.payment || '';
    document.getElementById("Reffer-Book").value = data.refferedBy || '';

    var height = convertToFeetAndInches(data.height);
    document.getElementById("bmiWeight").value = data.weight || '';
    document.getElementById("bmiHeightft").value = height.feet || '';
    document.getElementById("bmiHeightin").value = height.remainingInches || '';
    updateBMI();

    changeNxtDatebyVal(data.nextMeetingValue);
    setQuillContent('#editors_pres', data.refferedTo || '');

}

function populateTable(dataArray, tblId) {

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

        cells.forEach((cellData, index) => {
            const cell = document.createElement("td");

            if (index != 0) {
                const input = document.createElement("input");
                input.type = "text"; // Specify input type
                input.value = cellData.text; // Set the value of the input
                input.classList.add("form-control"); // Add the 'form-control' class

                cell.appendChild(input); // Append the input to the cell

            } else {
                cell.textContent = cellData.text;
            }


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


function convertToFeetAndInches(inches) {
    var feet = Math.floor(inches / 12);
    var remainingInches = inches % 12;
    var data = {
        feet: feet,
        remainingInches: remainingInches
    };
    return data;
}

function populateReportTable(dataArray, tblId) {
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
            { text: data?.reportDate || new Date(), value: data?.id || "" },
            { text: data?.reportName || "N/A", value: data?.id || "" },
            { text: data?.result || "N/A", value: data?.id || "" },
            { text: data?.unit || "N/A", value: data?.id || "" },
        ];

        cells.forEach((cellData, index) => {
            const cell = document.createElement("td");
            if (index == 1) {
                const input = document.createElement("input");
                input.type = "date"; // Specify input type
                const rawDate = cellData.text; // Original date value

                let formattedDate = ""; // Initialize an empty formattedDate
                if (rawDate) {
                    const date = new Date(rawDate); // Parse the date
                    if (!isNaN(date.getTime())) { // Check if the date is valid
                        formattedDate = date.toISOString().split("T")[0]; // Format the date as YYYY-MM-DD
                    }
                }

                input.value = formattedDate; // Set the formatted value
                input.classList.add("form-control"); // Add the 'form-control' class

                cell.appendChild(input); // Append the input to the cell
            }
            if (index > 1) {
                const input = document.createElement("input");
                input.type = "text"; // Specify input type
                input.value = cellData.text; // Set the value of the input
                input.classList.add("form-control"); // Add the 'form-control' class

                cell.appendChild(input); // Append the input to the cell

            }
            if (index == 0) {
                cell.textContent = cellData.text;
            }


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

function setHoTableData(data) {
    $('.health-checkbox').each(function () {
        let key = $(this).attr('id'); // Use the id as the key
        if (data.hasOwnProperty(key)) {
            $(this).prop("checked", data[key]); 
        }
    });
    $('#FreeTextHO').val(data.freeTextHO);

}
