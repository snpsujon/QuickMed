function LoadPage(ff) {
    if (ff == '1') {
        fetch('login.html')
            .then(response => response.text())
            .then(html => {
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

function setupEditableTable(tableid, buttonid) {
    $(function () {
        $('#' + tableid).SetEditable({ $addButton: $('#' + buttonid) });

    });
}


function setupEditableTableWithoutButton(tableid) {
    $(function () {
        $('#' + tableid).editableTableWidget().find('td:first').focus();

    });
}

//make drugable table
function makeTableDragable(tableid) {
    $("#" + tableid + " tbody").sortable({
        helper: function (e, tr) {
            var $originals = tr.children();
            var $helper = tr.clone();
            $helper.children().each(function (index) {
                // Set helper cell sizes to match the original sizes
                $(this).width($originals.eq(index).width());
            });
            return $helper;
        },
        update: function (event, ui) {
            // Update the index column in all rows after reordering
            $("#" + tableid + " tbody tr").each(function (index) {
                $(this).children(":first").text(index + 1);  // Updating the first column to new index + 1
            });
        }

    }).disableSelection();
}

function makeSelect2(isTags) {
    $(document).ready(function () {
        $('.select2').select2({
            width: '100%',
            tags: isTags,
            allowClear: true  
        });
    });

}

function makeDataTable(tableid) {
    $(document).ready(function () {
        
        //Buttons examples
        var table = $('#' + tableid).DataTable({
            lengthChange: false,
            buttons: ['excel', 'pdf']
        });

        table.buttons().container()
            .appendTo('.dataTables_wrapper .col-md-6:eq(0)');


    
        });

}

function makeTextEditor(inputId) {
    $(document).ready(function () {

        tinymce.init({
            selector: '#' + inputId,
            height: 400,
            plugins: [
                'advlist autolink link image lists charmap print preview hr anchor pagebreak',
                'searchreplace wordcount visualblocks visualchars code fullscreen insertdatetime media nonbreaking',
                'table emoticons template paste help'
            ],
            toolbar: 'undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | ' +
                'bullist numlist outdent indent | link image | print preview media fullpage | ' +
                'forecolor backcolor emoticons | help',
            menu: {
                favs: { title: 'My Favorites', items: 'code visualaid | searchreplace | emoticons' }
            },
            menubar: 'favs file edit view insert format tools table help',
            content_style: 'body { font-family:Helvetica,Arial,sans-serif; font-size:14px; color: #7c8ea7; }'
        });


    });

}


function makeFileUpload() {
    $(function () {
        // Basic
        $('.dropify').dropify();

        // Translated
        $('.dropify-fr').dropify({
            messages: {
                default: 'Glissez-déposez un fichier ici ou cliquez',
                replace: 'Glissez-déposez un fichier ou cliquez pour remplacer',
                remove: 'Supprimer',
                error: 'Désolé, le fichier trop volumineux'
            }
        });

        // Used events
        var drEvent = $('#input-file-events').dropify();

        drEvent.on('dropify.beforeClear', function (event, element) {
            return confirm("Do you really want to delete \"" + element.file.name + "\" ?");
        });

        drEvent.on('dropify.afterClear', function (event, element) {
            alert('File deleted');
        });

        drEvent.on('dropify.errors', function (event, element) {
            console.log('Has Errors');
        });

        var drDestroy = $('#input-file-to-destroy').dropify();
        drDestroy = drDestroy.data('dropify')
        $('#toggleDropify').on('click', function (e) {
            e.preventDefault();
            if (drDestroy.isDropified()) {
                drDestroy.destroy();
            } else {
                drDestroy.init();
            }
        })
    });
}