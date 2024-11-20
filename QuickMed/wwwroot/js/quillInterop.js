


window.initializeQuill = (editorId) => {
    function tryInitializeQuill() {
        const editorElement = document.querySelector(editorId);
        if (editorElement) {
            new Quill(editorId, { theme: 'snow' });
        } else {
            // Retry after a short delay if editorId is not found
            setTimeout(tryInitializeQuill, 100); // retry every 100ms
        }
    }
    tryInitializeQuill();
};

