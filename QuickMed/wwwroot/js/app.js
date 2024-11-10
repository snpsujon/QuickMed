function LoadPage(ff) {
    if (ff == '1') {
        fetch('login.html')
            .then(response => response.text())
            .then(html => {
                debugger;
                document.getElementById('bodyyy').innerHTML = "";
                document.getElementById('bodyyy').innerHTML = html;
            })
            .catch(error => {
                console.warn('Error loading HTML file:', error);
            });
    }
}