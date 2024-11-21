function MakeAvro(id) {
    $('#' + id).avro({
        bangla: true
    });
    $('#' + id).on('keydown', function (e) {
        if (e.ctrlKey && e.which === 77) { // Ctrl + M toggles between Bangla and English
            $(this).trigger('switch');
        }
    });

}