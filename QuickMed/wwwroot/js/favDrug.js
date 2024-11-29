
function GetDrugTempData() {

    const tempname = $("#TempName").val();
    const brandSelect = $("#brandSelect").val();
    const doseSelect = $("#doseSelect").val();
    const instructionSelect = $("#instructionSelect").val();
    const durationSelectfav = $("#durationSelectfav").val();
    const tempId = $("#favTempId").val();

    var data = {
        templateName: tempname == '' ? 'FavTemp_' + getRandomInteger(1, 9999) : tempname,
        brandSelect: brandSelect,
        doseSelect: doseSelect,
        instructionSelect: instructionSelect,
        durationSelectfav: durationSelectfav,
        tempId: tempId
    };
    return data;
}

function setFavMasterData(data) {
    $('#TempName').val(data.name);
    $('#favTempId').val(data.id);
    $('#brandSelect').val(data.brandId).trigger('change');
    $('#doseSelect').val(data.doseId).trigger('change');
    $('#instructionSelect').val(data.instructionId).trigger('change');
    $('#durationSelectfav').val(data.durationId).trigger('change');

}