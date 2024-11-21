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