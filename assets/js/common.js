$(document).ready(function() {
    // Datatable
    if ($('#leaddatatable').length>0) {    
        $('#leaddatatable').DataTable();
    }
    // Date Range
    var start = moment().subtract(29, 'days');
    var end = moment();

    function cb(start, end) {
        $('#reportrange span').html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'));
    }

     if ($('#reportrange').length > 0) {    
         $('#reportrange').daterangepicker({
             startDate: start,
             endDate: end,
             ranges: {
                 'Today': [moment(), moment()],
                 'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
                 'Last 7 Days': [moment().subtract(6, 'days'), moment()],
                 'Last 30 Days': [moment().subtract(29, 'days'), moment()],
                 'This Month': [moment().startOf('month'), moment().endOf('month')],
                 'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
             }
         }, cb);

    cb(start, end);
    }
   
} );

if ($('input[name="daterange"]').length > 2) {    
 $(function() {
     $('input[name="daterange"]').daterangepicker(
         {
             ssingleDatePicker: 'true',
             showShortcuts: 'false',
             showTopbar: 'false'
         }
     );
 });
}

function swalValidationAlert(message, type) {
    return Swal.fire({
        text: message,
        icon: type,
        confirmButtonText: "OK",
        confirmButtonColor: type === "success" ? "#28a745"
            : type === "warning" ? "#ffc107"
                : "#dc3545"
    });
}
