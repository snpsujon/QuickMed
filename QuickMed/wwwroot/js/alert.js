window.showAlert = (title, text, icon, customStyle) => {
    Swal.fire({
        title: title,
        text: text,
        icon: icon, // Use a valid icon like "success", "error", etc.
        confirmButtonText: 'OK',
        customClass: {
            popup: customStyle  // Apply custom style (e.g., red color) to the popup
        }
    });
};


window.showDeleteConfirmation = (title, text) => {
    return Swal.fire({
        title: title,
        text: text,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Delete',
        cancelButtonText: 'Cancel'
    }).then((result) => {
        return result.isConfirmed; // Returns true if the user confirmed the action
    });
};