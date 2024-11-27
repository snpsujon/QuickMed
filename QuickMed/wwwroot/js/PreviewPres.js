function printModalContent() {
    try {
        debugger;

        // Create a container for the iframe
        const iframeContainer = document.createElement('div');

        // Create a new iframe
        const iframe = document.createElement('iframe');
        iframe.style.border = 'none';
        iframe.style.width = '0';
        iframe.style.height = '0'; // Ensure iframe is invisible

        // Append iframe to the container
        iframeContainer.appendChild(iframe);
        document.body.appendChild(iframeContainer);

        // Write HTML content to the iframe
        const doc = iframe.contentWindow.document;
        doc.open();
        doc.write(`
        <html>
        <head>
            <title>Print Content</title>
        </head>
        <body>
            <h1>Hello World</h1>
            <p>This content will be printed.</p>
        </body>
        </html>
    `);
        doc.close();

        // Wait for the iframe to load before printing
        iframe.onload = function () {
            debugger;
            // Ensure the content is fully loaded before printing
            console.log('Iframe loaded, triggering print...');
            iframe.contentWindow.print();
            // Clean up after printing
            document.body.removeChild(iframeContainer);
        };

    } catch (e) {
        console.log(e);
    }

}
