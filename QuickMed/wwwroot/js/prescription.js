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

                //console.log("Data received from Blazor:", data);
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

                //console.log("Data received from Blazor:", data);
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

                //console.log("Data received from Blazor:", data);
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

function getPresData() {
    var Pdata = GetPetaintData();
    var nxtMeetData = GetNextMeetData();
    var ccTableData = getTableDataById('ccTable');
    var hoTableData = getHOTableData();
    var mhTableData = getDataColumnAsKey('mhTable');
    var oeTableData = getDataColumnAsKey('oeTable', true);
    var dhTableData = getTableDataById('dhTable');
    var dxTableData = getTableDataById('Dxtable');
    var ixTableData = getTableDataById('TretmentTmpIXTbl');
    var noteTableData = getTableDataById('TretmentTmpNotesTbl');
    var reportTableData = getTableDataById('rptEntryTbl');
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
        height: heightInc,
        bmiweight: $("#bmiWeight").val()
    };

    console.log(data); // To check the retrieved data
    return data; // Return the object if needed elsewhere
}

function GetNextMeetData() {
    var data = {
        nextMeetingDuration: $("#nxtMeetDateSelect").val(), // Value of the dropdown
        nextMeetingDate: $("#nxtMeetDate").val(),          // Value of the date input
        payment: $("#Payment").val(),                      // Value of the payment input
        referredBy: $("#Reffer-Book").val()                // Value of the referred by input
    };

    console.log(data); // For debugging, logs the collected data
    return data;       // Returns the data object
}


function getTableDataById(tableId) {
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
    console.log(tableData);
    return tableData;
}


function getHOTableData() {
    let data = {};

    // Loop through all checkboxes with the unique class
    $('.health-checkbox').each(function () {
        let key = $(this).val(); // Use value as key
        let isChecked = $(this).is(':checked') ? 1 : 0; // Checked = 1, Unchecked = 0
        data[key] = isChecked;
    });
    data['FreeTextHO'] = $('#FreeTextHO').val();
    console.log(data);
    return data;
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
        tableData.push(rowData);
    });
    console.log(tableData);
    return tableData;
}