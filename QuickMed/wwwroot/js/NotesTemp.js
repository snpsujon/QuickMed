

function populateNotesTable(dataArray, tblId) {

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


function GetNotesTempData() {
    const tableBody = $("#notesMainTbl tbody")[0];
    const secondColumnData = [];
    const tempname = $("#TempName").val();
    $(tableBody).find("tr").each(function () {
        const secondCell = $(this).children("td").eq(1);
        const cellData = secondCell.find("input").val().trim();
        secondColumnData.push(cellData);
    });

    var data = {
        templateName: tempname == '' ? 'NoteTemp_' + getRandomInteger(1, 9999) : tempname,
        
        tempData: secondColumnData,
    };
    return data;
}

