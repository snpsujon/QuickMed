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
    localStorage.setItem("activeMenu", "dashboard")
}

function unlock() {
    const data = "Some data to pass";
    //const url = `/dashboard?data=${encodeURIComponent(data)}`;
    const url = `/dashboard`;
    window.location.href = url;
}

function setActiveMenu(menuId) {
    // Remove active class from all menu items
    const allMenuItems = document.querySelectorAll('.mymenu-item');

    allMenuItems.forEach(item => {
        item.classList.remove('active');
    });

    // Add active class to the clicked menu item
    const menuItem = document.getElementById(menuId);
    if (menuItem) {
        menuItem.classList.add('active');
        // Save the active menu item to localStorage
        localStorage.setItem('activeMenu', menuId);
    }

    //// Redirect to respective pages
    //switch (menuId) {
    //    case 'dashboard':
    //        window.location.href = '/dashboard';
    //        break;
    //    case 'prescription':
    //        window.location.href = '/prescription';
    //        break;
    //    case 'prescription-list':
    //        window.location.href = '/prescription-list';
    //        break;
    //    case 'treatment':
    //        window.location.href = '/treatment';
    //        break;
    //    case 'favourite':
    //        window.location.href = '/favourite';
    //        break;
    //    case 'refer':
    //        window.location.href = '/refer';
    //        break;
    //    case 'advice':
    //        window.location.href = '/advice';
    //        break;
    //    case 'ix':
    //        window.location.href = '/ix';
    //        break;
    //    case 'dose':
    //        window.location.href = '/dose';
    //        break;
    //    case 'duration':
    //        window.location.href = '/duration';
    //        break;
    //    case 'dx':
    //        window.location.href = '/dx';
    //        break;
    //    case 'cc':
    //        window.location.href = '/cc';
    //        break;
    //    case 'mix':
    //        window.location.href = '/mix';
    //        break;
    //    case 'note':
    //        window.location.href = '/note';
    //        break;
    //    case 'appoinment':
    //        window.location.href = '/appoinment';
    //        break;
    //    case 'databaseTool':
    //        window.location.href = '/databaseTool';
    //        break;
    //    case 'checkUpgrade':
    //        window.location.href = '/checkUpgrade';
    //        break;
    //    case 'drug':
    //        window.location.href = '/drug';
    //        break;
    //}
}

// On page load, check localStorage for the active menu and apply the active class
window.onload = function () {
    const activeMenu = localStorage.getItem('activeMenu');
    if (activeMenu) {
        const menuItem = document.getElementById(activeMenu);
        if (menuItem) {
            menuItem.classList.add('active');
        }
    }
}

