function printModalContent() {
    try {
        debugger;


        var printContents = document.getElementById('printDiv').innerHTML;
        var originalContents = document.body.innerHTML;
        document.body.innerHTML = printContents;
        window.print();
        document.body.innerHTML = originalContents;
        window.location.reload();

    } catch (e) {
        console.log(e);
    }

}

function getPrintContent() {
    var printContents = document.getElementById('printDiv').innerHTML;
    originalContents = document.body.innerHTML;
    document.body.innerHTML = printContents;
    var nowContent = document.body.innerHTML;

    if (!printContents) {
        alert("Element with id 'printDiv' not found");
        return null;
    }
    return nowContent;

}