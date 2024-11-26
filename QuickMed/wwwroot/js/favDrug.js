
function GetDrugTempData() {

    const tempname = $("#TempName").val();
    const brandSelect = $("#brandSelect").val();
    const doseSelect = $("#doseSelect").val();
    const instructionSelect = $("#instructionSelect").val();
    const durationSelectfav = $("#durationSelectfav").val();

    var data = {
        templateName: tempname == '' ? 'FavTemp_' + getRandomInteger(1, 9999) : tempname,
        brandSelect: brandSelect,
        doseSelect: doseSelect,
        instructionSelect: instructionSelect,
        durationSelectfav: durationSelectfav
    };
    return data;
}