﻿var deletebun = '<button id="bElim" type="button" class="btn btn-sm btn-soft-danger btn-circle" onclick="Qdeleterow(this);"><i class="dripicons-trash" aria-hidden="true"></i></button>';

let instanceReference;
let instanceReferenceforFavDrag;

function setInstanceReferenceForAll(dotNetObject) {
    instanceReference = dotNetObject;
}


function setInstanceReferenceForFavDrag(dotNetObject) {
    instanceReferenceforFavDrag = dotNetObject;
}

function LoadPage() {
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



function passDataToBlazor() {

    // Get the license key value from the input field
    var licenseKey = document.getElementById("license-key").value;

    // Call the Blazor method to pass the data
    DotNet.invokeMethodAsync('YourBlazorApp', 'ReceiveLicenseKey', licenseKey)
        .then(data => {
            console.log("Data passed to Blazor: " + data);
            alert(data);
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
    const button = document.getElementById('buttonsubmit');
    const errorMessage = document.getElementById('error-message');
    const loader = document.getElementById('loader');
    const buttonText = document.getElementById('button-text');

    // Show the loader and disable the button
    loader.style.display = 'inline-block';
    buttonText.style.display = 'none';
    button.disabled = true;
    errorMessage.style.display = 'none';
    errorMessage.textContent = '';
    var license = $("#license").val();
    if (license.trim() === '') {
        // Show error message
        errorMessage.textContent = 'Please enter a valid license.';
        errorMessage.style.display = 'block';
    }

    //const url = `/dashboard`;
    //window.location.href = url;
    // Simulate a process (e.g., API call)
    setTimeout(() => {

        instanceReference.invokeMethodAsync("ValidateLicense", license).then(result => {
            loader.style.display = 'none';
            buttonText.style.display = 'inline';
            button.disabled = false;
            errorMessage.textContent = result;
            errorMessage.style.display = 'block';
            if (result == "Congrats! Your License is Approved.") {
                const url = `/dashboard`;
                window.location.href = url;
            }

        })
            .catch(error => {
                loader.style.display = 'none';
                buttonText.style.display = 'inline';
                button.disabled = false;
                errorMessage.textContent = "Something Went Wrong";
                errorMessage.style.display = 'block';
            });
    }, 3000);






    //const data = "Some data to pass";
    //const url = `/license?data=${encodeURIComponent(data)}`;
    ////const url = `/dashboard`;
    //window.location.href = url;
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

function setupEditableTable(tableid, buttonid = null, isSelect2 = true) {
    if (buttonid == null) {
        $('#' + tableid).SetEditable();
    } else {
        $('#' + tableid).MySetEditable({ $addButton: $('#' + buttonid) }, isSelect2);
    }
}

$.fn.MySetEditable = function (options, isSelect2) {
    var defaults = {
        columnsEd: null,         //Index to editable columns. If null all td editables. Ex.: "1,2,3,4,5"
        $addButton: null,        //Jquery object of "Add" button
        onEdit: function () { },   //Called after edition
        onBeforeDelete: function () { }, //Called before deletion
        onDelete: function () { }, //Called after deletion
        onAdd: function () { }     //Called when added a new row
    };
    params = $.extend(defaults, options);
    //this.find('thead tr').append('<th name="buttons"></th>');  //encabezado vacío
    //this.find('tbody tr').append(colEdicHtml);
    var $tabedi = this;   //Read reference to the current table, to resolve "this" here.
    //Process "addButton" parameter
    if (params.$addButton != null) {
        //Se proporcionó parámetro
        params.$addButton.click(function () {
            myrowAddNew($tabedi.attr("id"), isSelect2);
        });
    }
    //Process "columnsEd" parameter
    if (params.columnsEd != null) {
        //Extract felds
        colsEdi = params.columnsEd.split(',');
    }
};

window.saveAsFile = function (fileName, byteBase64) {
    const link = document.createElement('a');
    link.download = fileName;
    link.href = 'data:application/octet-stream;base64,' + byteBase64;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
};


function toggleLoadingIcon(id, isLoading) {
    const icon = document.getElementById(id);

    if (isLoading) {
        // Show loading icon with rotation effect
        icon.className = 'dripicons-loading display-3 refreshing'; // Replace with loading icon class
        icon.style.color = ''; // Reset color
    } else {
        // Revert to the original icon
        icon.className = 'dripicons-time-reverse display-3'; // Replace with original icon class
        icon.style.color = 'red'; // Set color
    }
}


window.uploadFile = async (element) => {
    const file = element.files[0];
    if (file) {
        return new Promise((resolve, reject) => {
            const reader = new FileReader();
            reader.onload = () => {
                const base64String = btoa(String.fromCharCode(...new Uint8Array(reader.result)));
                resolve(base64String);  // Resolving with base64 encoded string
            };
            reader.onerror = reject;
            reader.readAsArrayBuffer(file);
        });
    } else {
        throw new Error("No file selected");
    }
};

function clearFileInput(inputId) {
    const inputElement = document.getElementById(inputId);
    if (inputElement) {
        inputElement.value = ""; // Clear the file input
    }
}


function ClearTable(tableid) {
    $('#' + tableid + ' tbody').empty();
}


function myrowAddNew(tabId, isSelect2 = true, nameCalledFunction = '', className = 'customClass', selectedValue = '') {  // Adds a new row to the specified table.
    var $tab_en_edic = $("#" + tabId);  // Table to edit
    var $filas = $tab_en_edic.find('tbody tr');

    if ($filas.length == 0) {
        // No data rows; create a complete new row
        var $row = $tab_en_edic.find('thead tr');  // Header row
        var $cols = $row.find('th');  // Read columns
        var htmlDat = '';

        $cols.each(function () {
            if ($(this).attr('name') === 'buttons') {
                // Buttons column
                htmlDat += '<td name="buttons"></td>';
            } else {
                htmlDat += '<td></td>';
            }
        });

        $tab_en_edic.find('tbody').append('<tr>' + htmlDat + '</tr>');
    } else {
        // Clone the last row, but create an empty new row to avoid issues
        var $newRow = $('<tr></tr>');  // Create new row
        var $cols = $tab_en_edic.find('thead th');  // Read columns from the header

        $cols.each(function (index) {
            if (index === 0) {
                // First column is for the SL number
                $newRow.append('<td></td>');
            } else if ($(this).attr('name') === 'buttons') {
                // Append a buttons cell if specified
                $newRow.append('<td name="buttons"></td>');
            } else {
                // Append input cell for other columns
                var div = '<div style="display: none;"></div>';  // Store content
                var input = '';
                if (isSelect2) {
                    input = '<select class="form-control customselect2 custom-select ' + className + index + '" value=""></select>';

                } else {
                    input = '<input class="form-control" value="" />';
                }

                if (tabId == 'rptEntryTbl' && index === 1) {
                    input = '<input class="form-control" value="" type="date"/>';
                }


                $newRow.append('<td class="Qcutomselect">' + div + input + '</td>');
            }
        });

        // Append the newly created row to the table
        $tab_en_edic.find('tbody').append($newRow);
    }
    // Remove any row with the name attribute "bogichogi"
    $tab_en_edic.find('tr[name="bogichogi"]').remove();
    // Update SL numbers for all rows to keep them in ascending order
    $tab_en_edic.find('tbody tr').each(function (index) {
        $(this).find('td').first().text(index + 1);  // Set SL in the first column
    });

    $tab_en_edic.find('tr:last').find('td:last').html(deletebun);
    if (nameCalledFunction !== '' && nameCalledFunction !== "" && nameCalledFunction !== null) {
        if (typeof window[nameCalledFunction] === 'function') {
            window[nameCalledFunction](selectedValue); // Pass parameters using spread syntax
        } else {
            console.log(`Function ${nameCalledFunction} does not exist.`);
        }
    }

    customSelect2(true);

    // Trigger the onAdd callback function if defined
    if (typeof params !== 'undefined' && typeof params.onAdd === 'function') {
        params.onAdd();
    }

}



function customSelect2(isTags) {
    $('.customselect2').select2({
        width: '100%',
        tags: isTags
    });

    $('.Qcutomselect').find('.select2-selection__arrow').hide();
}


function Qdeleterow(but) {
    var $row = $(but).parents('tr');  //accede a la fila
    var $table = $row.closest('table');

    params.onBeforeDelete($row);
    $row.remove();
    params.onDelete();

    $table.find('tbody tr').each(function (index) {
        $(this).children(":first").text(index + 1);  // Update the first column with new index + 1
    });
}

function OnChangeEvent(elementId, myfunction, dotNetHelper) {
    $("#" + elementId).on('change', function () {
        var selectedValue = $(this).val();
        dotNetHelper.invokeMethodAsync(myfunction, selectedValue);
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
    $('.select2').select2({
        width: '100%',
        tags: isTags,
        allowClear: true,
        placeholder: "Select"
    });

    $('.select2W').select2({
        width: '100%',
        tags: false,
        allowClear: true
    });

}

function makeSelect2Custom(classs, invokeMethod, minInput) {
    $('.' + classs).select2({
        ajax: {
            transport: function (params, success, failure) {
                instanceReference.invokeMethodAsync(invokeMethod, params.data.term || "")
                    .then(success)
                    .catch(failure);
            },
            processResults: function (data) {
                return {
                    results: data.map(item => ({ id: item.id, text: item.name })),
                };
            },
            delay: 250,
            cache: true
        },
        minimumInputLength: minInput,
        placeholder: 'Search',
    });
}


// Add event listener to the document or a static parent element
document.addEventListener('click', function (event) {
    // Check if the clicked element has the 'dTeditRow' class
    if (event.target && event.target.classList.contains('dTRowActionBtn')) {
        const id = event.target.getAttribute('data-id');
        const method = event.target.getAttribute('data-method');


        // Call Blazor method if needed
        if (instanceReference) {
            instanceReference.invokeMethodAsync(method, id);
        } else {
            console.error("Instance reference is not set");
        }
    }
});


function makeDataTable(tableid, newData = []) {

    // Check if the DataTable is already initialized
    if (!$.fn.dataTable.isDataTable('#' + tableid)) {
        // Initialize the DataTable
        var table = $('#' + tableid).DataTable({
            lengthChange: false,
            buttons: ['excel', 'pdf']
        });

        // Append the DataTable  buttons to the container
        table.buttons().container()
            .appendTo('.dataTables_wrapper .col-md-6:eq(0)');
    } else {
        // If DataTable is already initialized, just get the existing table instance
        var table = $('#' + tableid).DataTable();
    }

    // Check if new data is provided and add it to the table
    if (newData.length > 0) {
        // Clear the existing table
        table.clear();

        // Add the new data to the table
        table.rows.add(newData);

        // Redraw the table to refresh the state and remove "No data available" message
        table.draw();
    }
}
function makeDataTableQ(tableid, data = []) {

    if (!$.fn.dataTable.isDataTable('#' + tableid)) {
        var table = $('#' + tableid).DataTable({
            data: data,
            lengthChange: false,
            buttons: ['excel', 'pdf'],
            pageLength: 10,
        });

        table.buttons().container()
            .appendTo('.dataTables_wrapper .col-md-6:eq(0)');

    }
    else {
        var table = $('#' + tableid).DataTable();
    }
    if (data != null) {

        if (data.length > 0) {
            table.clear();
            table.rows.add(data);
            table.draw();
        }
        else {
            table.clear();
            table.draw();
        }
    }


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

function downloadFileFromBytes(fileName, base64Content) {
    const link = document.createElement('a');
    link.href = `data:application/pdf;base64,${base64Content}`;
    link.download = fileName;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}
function getRandomInteger(min, max) {
    return Math.floor(Math.random() * (max - min + 1)) + min;
}


function setSelectOptions(selectClass, options, defaultValue = '') {
    var $select = $('.' + selectClass);
    if ($select.length) {
        var $lastSelect = $select.last();
        $lastSelect.find('option').remove();
        var firstOption = '<option value="0">Select</option>';
        $lastSelect.append(firstOption);
        options.forEach(option => {
            var $opt = $('<option></option>').val(option.value).text(option.text);
            if (defaultValue != '' && option.value === defaultValue) {
                $opt.attr('selected', 'selected');
            }
            $lastSelect.append($opt);
        });
    }
}

function showQModal() {
    var modal = new bootstrap.Modal(document.getElementById('bd-example-modal-xl'));
    modal.show();
}

//function ClearAllFields() {
//    // Clear all fields
//    $('#app').find('input, select, textarea').val('').trigger('change');

//}


function ClearFormData() {
    debugger;
    // Clear text inputs
    document.querySelectorAll("input[type='text'], input[type='number']").forEach(input => {
        input.value = "";
    });



    // Clear textareas
    document.querySelectorAll("textarea").forEach(textarea => {
        textarea.value = "";
    });

    document.querySelectorAll("select").forEach(select => {
        select.selectedIndex = 0; // Set to the first option (default)
        // If using select2, trigger reset
        if ($(select).hasClass("select2")) {
            $(select).val("Select").trigger("change");
        }
        if ($(select).hasClass("select2W")) {
            $(select).val("Select").trigger("change");
        }
        //if ($(select).hasClass("select2C")) {
        //    $(select).val("Select").trigger("change");
        //}


    });

}
