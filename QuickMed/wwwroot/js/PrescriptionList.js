function clearForm(formId) {
    const form = document.getElementById(formId);
    if (form) {
        const inputs = form.querySelectorAll("input");
        const selects = form.querySelectorAll("select");
        inputs.forEach(input => input.value = "");
        selects.forEach(select => select.selectedIndex = 0);
    }
}
window.setMaxDate = function () {
    var fromDate = document.getElementById("fromDate");

    // Get the current date
    var today = new Date();

    // Format the date to "YYYY-MM-DD"
    var formattedDate = today.getFullYear() + '-'
        + (today.getMonth() + 1).toString().padStart(2, '0') + '-'
        + today.getDate().toString().padStart(2, '0');

    // Set the 'max' attribute of the fromDate input
    fromDate.setAttribute('max', formattedDate);
}
function enableToDate() {
    var fromDate = document.getElementById("fromDate").value;
    var toDate = document.getElementById("toDate");
    //fromDate.setAttribute('max', new Date());

    if (fromDate) {
        toDate.disabled = false;
        toDate.setAttribute('min', fromDate);
        // Get the current date
        var today = new Date();

        // Format the date to "YYYY-MM-DD"
        var formattedDate = today.getFullYear() + '-'
            + (today.getMonth() + 1).toString().padStart(2, '0') + '-'
            + today.getDate().toString().padStart(2, '0');

        // Set the 'max' attribute of the fromDate input
        toDate.setAttribute('max', formattedDate);

    } else {
        toDate.disabled = true;
    }
}
function checkToDate() {
    var fromDate = new Date(document.getElementById("fromDate").value);
    var toDate = document.getElementById("toDate");
    if (new Date(toDate.value) < fromDate) {
        toDate.value = '';
        toDate.disabled = true;
    }
}
window.getSearchInput = function () {
    //debugger;
    // Get the values of the inputs and select elements
    var searchReg = document.getElementById('searchReg').value;
    var searchMobile = document.getElementById('searchMobile').value;
    var selectedDxId = document.getElementById('dxSelect').value;
    var selectedBrandId = document.getElementById('brandTempSelect').value;
    var fromDate = document.getElementById('fromDate').value;
    var toDate = document.getElementById('toDate').value;


    //           string? mobile = null,
    //string? prescriptionCode = null,
    //string? dxName = null,
    //DateTime? startDate = null,
    //DateTime? endDate = null

    var data = {
        mobile: searchMobile,
        prescriptionCode: searchReg,
        dxName: selectedDxId,
        startDate: fromDate,
        endDate: toDate,
        brand: selectedBrandId
    };
    return data;
}



