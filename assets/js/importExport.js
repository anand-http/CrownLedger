var ImportExport = (function () {

    var _config = {};
    var _baseUrl = '/ImportExport';

    function init(config) {
        _config = $.extend({
            moduleType: '',
            entityId: 0,
            createdBy: 0,
            dropdownTarget: '#btnCreateNewDropdown' // ← id of your dropdown <ul>
        }, config);

        _injectDropdownItems();   // CHANGED: build dropdown items from JS
        _bindExport();
        _bindImport();
        _bindTemplateDownload();
    }

    // ── Inject Import/Export items into existing dropdown ─
    function _injectDropdownItems() {
        var label = _config.moduleType
            .replace('SalesOrder', 'Sales Order')
            .replace('CreditNote', 'Credit Note')
            .replace('PurchaseOrder', 'Purchase Order')
            .replace('PurchaseBills', 'Bills History')
            .replace('PurchaseCreditNote', 'Purchase Credit Note')
            .replace('PurchaseRecurringBills', 'Recurring Bills')
            .replace('ExpenseHistory', 'Expense History');

        if (_config.moduleType == "ExpenseHistory") {
            var html =
                '<li><hr class="dropdown-divider"></li>' +
                '<li><a class="dropdown-item" id="btnImport"        href="javascript:void(0)">Import ' + label + '</a></li>' +
                '<li><a class="dropdown-item" id="btnExportExcel"   href="javascript:void(0)" data-moduletype="ExpenseHistory">Export ' + label + ' (.xlsx)</a></li>' +
                '<li><a class="dropdown-item" id="btnExportExcel"   href="javascript:void(0)" data-moduletype="RecurringExpense">Export ' + "Recurring Expense" + ' (.xlsx)</a></li>' +
                '<li><a class="dropdown-item" id="btnExportCsv"     href="javascript:void(0)" data-moduletype="ExpenseHistory">Export ' + label + ' (.csv)</a></li>' +
                '<li><a class="dropdown-item" id="btnExportCsv"     href="javascript:void(0)" data-moduletype="RecurringExpense">Export ' + "Recurring Expense" + ' (.csv)</a></li>' +
                '<li><hr class="dropdown-divider"></li>' +
                '<li><h6 class="dropdown-header">Download Template</h6></li>' +
                '<li><a class="dropdown-item" id="btnTemplateExcel" href="javascript:void(0)" data-moduletype="ExpenseHistory">Template Expense History (.xlsx)</a></li>' +
                '<li><a class="dropdown-item" id="btnTemplateExcel" href="javascript:void(0)" data-moduletype="RecurringExpense">Template Recurring Expense (.xlsx)</a></li>' +
                '<li><a class="dropdown-item" id="btnTemplateExcel" href="javascript:void(0)" data-moduletype="ExpenseBulkBooking">Template Bulk Booking (.xlsx)</a></li>' +
                '<li><a class="dropdown-item" id="btnTemplateCsv"   href="javascript:void(0)" data-moduletype="ExpenseHistory">Template Expense History (.csv)</a></li>'+
                '<li><a class="dropdown-item" id="btnTemplateCsv"   href="javascript:void(0)" data-moduletype="RecurringExpense">Template Recurring Expense (.csv)</a></li>'+
                '<li><a class="dropdown-item" id="btnTemplateCsv"   href="javascript:void(0)" data-moduletype="ExpenseBulkBooking">Template Bulk Booking (.csv)</a></li>';
        } else {
        var html =
            '<li><hr class="dropdown-divider"></li>' +
            '<li><a class="dropdown-item" id="btnImport"        href="javascript:void(0)">Import ' + label + '</a></li>' +
            '<li><a class="dropdown-item" id="btnExportExcel"   href="javascript:void(0)">Export ' + label + ' (.xlsx)</a></li>' +
            '<li><a class="dropdown-item" id="btnExportCsv"     href="javascript:void(0)">Export ' + label + ' (.csv)</a></li>' +
            '<li><hr class="dropdown-divider"></li>' +
            '<li><h6 class="dropdown-header">Download Template</h6></li>' +
            '<li><a class="dropdown-item" id="btnTemplateExcel" href="javascript:void(0)">Template (.xlsx)</a></li>' +
            '<li><a class="dropdown-item" id="btnTemplateCsv"   href="javascript:void(0)">Template (.csv)</a></li>';
        }

        // Append to the existing Create New dropdown <ul>
        $(_config.dropdownTarget).append(html);

        // Also inject hidden file input once if not present
        if ($('#importFileInput').length === 0) {
            $('body').append(
                '<input type="file" id="importFileInput" ' +
                'accept=".xlsx,.csv" style="display:none;" />'
            );
        }
    }

    // ── Export ────────────────────────────────────────────
    function _bindExport() {
        $(document).on('click', '#btnExportExcel', function () {
            let module = $(this).data('moduletype');
            console.log(module);
            _triggerDownload('xlsx',module);
        });
        $(document).on('click', '#btnExportCsv', function () {
            let module = $(this).data('moduletype');
            _triggerDownload('csv', module);
        });
    }

    //function _triggerDownload(format) {
    //    var url = _baseUrl + '/Export'
    //        + '?moduleType=' + _config.moduleType
    //        + '&format=' + format
    //        + '&entityId=' + 10;
    //        //+ '&entityId=' + _config.entityId;

    //    var link = document.createElement('a');
    //    link.href = url;
    //    link.download = '';
    //    document.body.appendChild(link);
    //    link.click();
    //    document.body.removeChild(link);
    //    _showToast('Export started.');
    //}

    function _triggerDownload(format, module) {
        console.log(module);
        if (_config.moduleType == "ExpenseHistory") {
            var url = '/ImportExport/Export'
                + '?moduleType=' + encodeURIComponent(module)
                + '&format=' + encodeURIComponent(format)
                + '&entityId=' + 10;

            window.location.href = url;
            _showToast('Export started.');
        }
        else {
            var url = '/ImportExport/Export'
                + '?moduleType=' + encodeURIComponent(_config.moduleType)
                + '&format=' + encodeURIComponent(format)
                + '&entityId=' + 10;

            window.location.href = url;
            _showToast('Export started.');
        }
    }

    // ── Template download ─────────────────────────────────
    function _bindTemplateDownload() {
        $(document).on('click', '#btnTemplateExcel', function () {
            if (_config.moduleType == "ExpenseHistory") {
                window.location.href = _baseUrl + '/Template'
                    + '?moduleType=' + $(this).data('moduletype') + '&format=xlsx';
            } else {
                window.location.href = _baseUrl + '/Template'
                    + '?moduleType=' + _config.moduleType + '&format=xlsx';
            }
        });
        $(document).on('click', '#btnTemplateCsv', function () {
            if (_config.moduleType == "ExpenseHistory") {
                window.location.href = _baseUrl + '/Template'
                    + '?moduleType=' + $(this).data('moduletype') + '&format=csv';
            } else {
                window.location.href = _baseUrl + '/Template'
                    + '?moduleType=' + _config.moduleType + '&format=csv';
            }
        });
    }

    // ── Import ────────────────────────────────────────────
    function _bindImport() {
        $(document).on('click', '#btnImport', function () {
            $('#importFileInput').val('').trigger('click');
        });

        $(document).on('change', '#importFileInput', function () {
            var file = this.files[0];
            if (!file) return;
            var ext = file.name.split('.').pop().toLowerCase();
            if (ext !== 'xlsx' && ext !== 'csv') {
                Swal.fire({
                    icon: 'warning',
                    title: 'Invalid File',
                    text: 'Please select an Excel (.xlsx) or CSV (.csv) file.'
                });
                return;
            }
            _uploadFile(file, ext);
        });
    }

    function _uploadFile(file, format) {
        var formData = new FormData();
        formData.append('file', file);
        formData.append('moduleType', _config.moduleType);
        formData.append('format', format);
        formData.append('entityId', _config.entityId || 0);
        formData.append('createdBy', _config.createdBy || 0);
       

        Swal.fire({
            title: 'Importing...',
            text: 'Please wait while your file is being processed.',
            allowOutsideClick: false,
            didOpen: function () { Swal.showLoading(); }
        });

        $.ajax({
            url: _baseUrl + '/Import',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                Swal.close();
                if (response.success) {
                    _showImportSummary(response);
                } else {
                    Swal.fire({
                        icon: 'error', title: 'Import Failed',
                        text: response.message || 'Something went wrong.'
                    });
                }
            },
            error: function (xhr) {
                Swal.close();
                Swal.fire({
                    icon: 'error', title: 'Server Error',
                    text: xhr.responseJSON && xhr.responseJSON.message
                        ? xhr.responseJSON.message : xhr.statusText
                });
            }
        });
    }

    function _showImportSummary(response) {
        Swal.fire({
            icon: response.failCount === 0 ? 'success' : 'warning',
            title: 'Import Complete',
            text: 'Your data saved successfully!',
            //html:
            //    '<div style="text-align:left">'
            //    + '<p><strong>Total Rows:</strong> ' + response.totalRows + '</p>'
            //    + '<p style="color:#27ae60"><strong>✔ Imported:</strong> ' + response.successCount + '</p>'
            //    + (response.failCount > 0
            //        ? '<p style="color:#c0392b"><strong>✘ Failed:</strong> ' + response.failCount + '</p>'
            //        : '')
            //    + errorHtml
            //    + '</div>',
            confirmButtonText: 'OK'
        }).then(function () {
            if (response.successCount > 0) location.reload();
        });
    }

    function _showToast(msg) {
        if ($('#ieToast').length === 0) {
            $('body').append(
                '<div id="ieToast" style="position:fixed;bottom:24px;right:24px;'
                + 'background:#323232;color:#fff;padding:12px 20px;border-radius:6px;'
                + 'z-index:9999;display:none;font-size:14px;"></div>'
            );
        }
        $('#ieToast').text(msg).fadeIn(200).delay(2500).fadeOut(400);
    }

    return { init: init };

})();