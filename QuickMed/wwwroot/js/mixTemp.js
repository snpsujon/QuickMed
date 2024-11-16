
const mainTblData = [];
function AddNewPlusBtn() {
    // Get the selected elements
    const brandSelect = document.getElementById("brandTempSelect");
    const doseSelect = document.getElementById("doseTempSelect");

    if (!brandSelect || !doseSelect ) {
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