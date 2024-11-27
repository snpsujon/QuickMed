function clearForm(formId) {
    const form = document.getElementById(formId);
    if (form) {
        const inputs = form.querySelectorAll("input");
        const selects = form.querySelectorAll("select");
        inputs.forEach(input => input.value = "");
        selects.forEach(select => select.selectedIndex = 0);
    }
}

function enableToDate() {
    var fromDate = document.getElementById("fromDate").value;
    var toDate = document.getElementById("toDate");

    if (fromDate) {
        toDate.disabled = false; 
        toDate.setAttribute('min', fromDate);
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
}



