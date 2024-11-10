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

function passDataToBlazor() {
    // Get the license key value from the input field
    var licenseKey = document.getElementById("license-key").value;

    // Call the Blazor method to pass the data
    DotNet.invokeMethodAsync('YourBlazorApp', 'ReceiveLicenseKey', licenseKey)
        .then(data => {
            console.log("Data passed to Blazor: " + data);
        })
        .catch(error => {
            console.error("Error passing data to Blazor: ", error);
        });
}

function clearLocalStorage() {
    // Clear all items in localStorage
    localStorage.clear();
    console.log('Local Storage Cleared');
}
function setLocalStorage() {
    localStorage.setItem("activeMenu","dashboard")
}