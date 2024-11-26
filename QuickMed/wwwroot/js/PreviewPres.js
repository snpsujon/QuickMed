function printModalContent() {
    debugger;

    const content = document.querySelector('#master_table').innerHTML;
    debugger;
    const iframeContainer = document.createElement('div');

    // Create a new iframe
    const iframe = document.createElement('iframe');
    iframe.style.border = 'none';

    // Append iframe to the container
    iframeContainer.appendChild(iframe);
    document.body.appendChild(iframeContainer);

    // Write the HTML content to the iframe
    const doc = iframe.contentDocument || iframe.contentWindow.document;
    doc.open();
    doc.write(`
    <h1> Hellow World </h1>
    `);
    doc.close();
    // Print the content of the iframe
    iframe.onload = function () {
        iframe.contentWindow.print();
        document.body.removeChild(iframeContainer); // Clean up after printing
    };
}