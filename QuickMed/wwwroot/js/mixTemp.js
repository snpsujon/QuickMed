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