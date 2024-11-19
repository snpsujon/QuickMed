var deletebun = '<button id="bElim" type="button" class="btn btn-sm btn-soft-danger btn-circle" onclick="Qdeleterow(this);"><i class="dripicons-trash" aria-hidden="true"></i></button>';

let instanceReference;

function setInstanceReferenceForAll(dotNetObject) {
    instanceReference = dotNetObject;
}

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



function myrowAddNew(tabId, isSelect2 = true) {  // Adds a new row to the specified table.
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
                    input = '<select class="form-control customselect2 custom-select" value=""></select>';

                } else {
                    var input = '<input class="form-control" value="" />';
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
        allowClear: true
    });

    $('.select2W').select2({
        width: '100%',
        tags: false,
        allowClear: true
    });

}


function makeDataTable(tableid, newData = []) {
    $(document).ready(function () {
        // Check if the DataTable is already initialized
        if (!$.fn.dataTable.isDataTable('#' + tableid)) {
            // Initialize the DataTable
            var table = $('#' + tableid).DataTable({
                lengthChange: false,
                buttons: ['excel', 'pdf']
            });

            // Append the DataTable buttons to the container
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


function setSelectOptions(selectClass, options, defaultValue) {
    var $select = $('.' + selectClass);

    if ($select.length) {
        $select.find('option:not(:first)').remove();

        options.forEach(option => {
            var $opt = $('<option></option>').val(option.value).text(option.text);

            // Set default selected option
            if (option.value === defaultValue) {
                $opt.attr('selected', 'selected');
            }

            $select.append($opt);
        });
    }
}
