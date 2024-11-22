


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


function getQuillContent(selector) {
    const quill = Quill.find(document.querySelector(selector));
    var len = quill.getLength();
    var text = quill.getSemanticHTML(0, parseInt(len));
    return text;
};

function clearQuillContent(selector) {
    const quill = Quill.find(document.querySelector(selector));
    if (quill) {
        quill.setText(""); // Clears the content of the editor
    } else {
        console.error("Quill editor not found for selector:", selector);
    }
}
function setQuillContent(selector, content) {
    const quill = Quill.find(document.querySelector(selector));
    if (quill) {
        quill.root.innerHTML = content; // Set the content to the Quill editor
    } else {
        console.error("Quill editor not found for selector:", selector);
    }
}

