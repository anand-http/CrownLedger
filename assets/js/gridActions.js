



// ============================================================
// gridActions.js — Common Grid Action Handler
// Drop this file in: /Scripts/Common/gridActions.js
// Include it on every page that uses these 5 actions.
//
// USAGE on each page — call initGridActions() with your config:
//   initGridActions({
//       moduleType: 'Estimate',          // Estimate | Invoice | SalesOrder | CreditNote
//       gridId:     'tblEstimates',      // your <table> id
//       checkboxSelector: '.row-check',  // checkbox class inside each row
//       emailModalId: '#emailModal'      // optional — if you have an email modal
//   });
// ============================================================

var GridActions = (function () {

    var _config = {};

    // ── Initialise ────────────────────────────────────────────
    function init(config) {
        _config = $.extend({
            moduleType: '',
            gridId: '',
            checkboxSelector: '.row-check',
            selectAllSelector: '#chkSelectAll',
            emailModalId: '#emailModal',
            baseUrl: '/GridAction'
        }, config);

        _bindSelectAll();
        _bindActionButtons();
    }

    // ── Get selected IDs from checked rows ───────────────────
    function _getSelectedIds() {
        var ids = [];
        $(_config.checkboxSelector + ':checked').each(function () {
            var id = $(this).closest('tr').data('id'); // each <tr data-id="123">
            if (id) ids.push(id);
        });
        return ids;
    }

    // ── Validate at least one row is selected ────────────────
    function _validate() {
        var ids = _getSelectedIds();
        if (ids.length == 0) {
            // CHANGED: replaced alert with Swal
            Swal.fire({
                icon: 'warning',
                title: 'No Record Selected',
                text: 'Please select at least one record.',
                confirmButtonText: 'OK'
            });
            return null;
        }
        return ids;
    }

    // ── Select All checkbox ──────────────────────────────────
    function _bindSelectAll() {
        $(_config.selectAllSelector).on('change', function () {
            $(_config.checkboxSelector).prop('checked', $(this).is(':checked'));
        });
    }

    // ── Bind the 5 buttons ───────────────────────────────────
    function _bindActionButtons() {
        // button print
        $('#btnPrint').click(function () {
            if (_config.moduleType == "PurchaseOrderHistory") {
                if (PurchasesOrderHistory.rows({ selected: true }).count() === 0) {
                    _validate()
                    return;
                }
                PurchasesOrderHistory.button('.buttons-print').trigger();
            } else if (_config.moduleType == "PurchaseBills") {
                if (BillsHistory.rows({ selected: true }).count() === 0) {
                    _validate()
                    return;
                }
                BillsHistory.button('.buttons-print').trigger();
            }
            else if (_config.moduleType == "PurchaseRecurringBills") {
                if (RecurringBillsHistory.rows({ selected: true }).count() === 0) {
                    _validate()
                    return;
                }
                RecurringBillsHistory.button('.buttons-print').trigger();
            } else if (_config.moduleType == "PurchaseCreditNote") {
                if (PurchasesCreditNotesHistory.rows({ selected: true }).count() === 0) {
                    _validate()
                    return;
                }
                PurchasesCreditNotesHistory.button('.buttons-print').trigger();
            }
            else if (_config.moduleType == "ExpenseHistory") {
                if (ExpenseHistory.rows({ selected: true }).count() === 0) {
                    _validate()
                    return;
                }
                ExpenseHistory.button('.buttons-print').trigger();
            }

            //sales
            if (_config.moduleType == "Estimate") {
                if (SalesEstimateHistoryTable.rows({ selected: true }).count() === 0) {
                    _validate()
                    return;
                }
                SalesEstimateHistoryTable.button('.buttons-print').trigger();
            } else if (_config.moduleType == "Invoice") {
                if (SalesInvoiceHistoryTable.rows({ selected: true }).count() === 0) {
                    _validate()
                    return;
                }
                SalesInvoiceHistoryTable.button('.buttons-print').trigger();
            }
            else if (_config.moduleType == "SalesOrder") {
                if (SalesOrderHistoryTable.rows({ selected: true }).count() === 0) {
                    _validate()
                    return;
                }
                SalesOrderHistoryTable.button('.buttons-print').trigger();
            } else if (_config.moduleType == "CreditNote") {
                if (SalesCreditNoteHistoryTable.rows({ selected: true }).count() === 0) {
                    _validate()
                    return;
                }
                SalesCreditNoteHistoryTable.button('.buttons-print').trigger();
            }
            else if (_config.moduleType == "RecInvoice") {
                if (SalesRecInvHistoryTable.rows({ selected: true }).count() === 0) {
                    _validate()
                    return;
                }
                SalesRecInvHistoryTable.button('.buttons-print').trigger();
            }
        });


       //Copy 
       $('#btnCopy').click(function () {

                let table = null;

                if (_config.moduleType == "PurchaseOrderHistory") {
                    table = PurchasesOrderHistory;
                }
                else if (_config.moduleType == "PurchaseBills") {
                    table = BillsHistory;
                }
                else if (_config.moduleType == "PurchaseRecurringBills") {
                    table = RecurringBillsHistory;
                }
                else if (_config.moduleType == "PurchaseCreditNote") {
                    table = PurchasesCreditNotesHistory;
                }
                else if (_config.moduleType == "ExpenseHistory") {
                    table = ExpenseHistory;
                }
                else if (_config.moduleType == "Estimate") {
                    table = SalesEstimateHistoryTable;
                }
                else if (_config.moduleType == "Invoice") {
                    table = SalesInvoiceHistoryTable;
                }
                else if (_config.moduleType == "SalesOrder") {
                    table = SalesOrderHistoryTable;
                }
                else if (_config.moduleType == "CreditNote") {
                    table = SalesCreditNoteHistoryTable;
                }
                else if (_config.moduleType == "RecInvoice") {
                    table = SalesRecInvHistoryTable;
                }

                if (!table) return;

                let rowCount = table.rows({ selected: true }).count();

                if (rowCount === 0) {
                    _validate();
                    return;
                }

                table.button('.buttons-copy').trigger();

                Swal.fire({
                    icon: 'success',
                    title: 'Copied!',
                    text: rowCount + ' row' + (rowCount > 1 ? 's' : '') + ' copied to clipboard',
                    timer: 2000,
                    showConfirmButton: false
                });
            });

        // btn-download
        $('#btnDownload').click(function () {
            if (_config.moduleType == "PurchaseOrderHistory") {
                if (PurchasesOrderHistory.rows({ selected: true }).count() === 0) {
                    _validate()
                    return;
                }
                PurchasesOrderHistory.button('.buttons-excel').trigger();
            } else if (_config.moduleType == "PurchaseBills") {
                if (BillsHistory.rows({ selected: true }).count() === 0) {
                    _validate()
                    return;
                }
                BillsHistory.button('.buttons-excel').trigger();
            }
            else if (_config.moduleType == "PurchaseRecurringBills") {
                if (RecurringBillsHistory.rows({ selected: true }).count() === 0) {
                    _validate()
                    return;
                }
                RecurringBillsHistory.button('.buttons-excel').trigger();
            } else if (_config.moduleType == "PurchaseCreditNote") {
                if (PurchasesCreditNotesHistory.rows({ selected: true }).count() === 0) {
                    _validate()
                    return;
                }
                PurchasesCreditNotesHistory.button('.buttons-excel').trigger();
            }
            else if (_config.moduleType == "ExpenseHistory") {
                if (ExpenseHistory.rows({ selected: true }).count() === 0) {
                    _validate()
                    return;
                }
                ExpenseHistory.button('.buttons-excel').trigger();
            }

            //sales
            if (_config.moduleType == "Estimate") {
                if (SalesEstimateHistoryTable.rows({ selected: true }).count() === 0) {
                    _validate()
                    return;
                }
                SalesEstimateHistoryTable.button('.buttons-excel').trigger();
            } else if (_config.moduleType == "Invoice") {
                if (SalesInvoiceHistoryTable.rows({ selected: true }).count() === 0) {
                    _validate()
                    return;
                }
                SalesInvoiceHistoryTable.button('.buttons-excel').trigger();
            }
            else if (_config.moduleType == "SalesOrder") {
                if (SalesOrderHistoryTable.rows({ selected: true }).count() === 0) {
                    _validate()
                    return;
                }
                SalesOrderHistoryTable.button('.buttons-excel').trigger();
            } else if (_config.moduleType == "CreditNote") {
                if (SalesCreditNoteHistoryTable.rows({ selected: true }).count() === 0) {
                    _validate()
                    return;
                }
                SalesCreditNoteHistoryTable.button('.buttons-excel').trigger();
            }
            else if (_config.moduleType == "RecInvoice") {
                if (SalesRecInvHistoryTable.rows({ selected: true }).count() === 0) {
                    _validate()
                    return;
                }
                SalesRecInvHistoryTable.button('.buttons-excel').trigger();
            }
        });
     

        // 2. EMAIL
        $(document).on('click', '#btnEmail', function () {
            var ids = _validate(); if (!ids) return;
            // Store ids on modal and open it
            $(_config.emailModalId).data('ids', ids).modal('show');
        });

        // Wire send button inside modal (call this from your modal's Send click)
        $(document).on('click', '#btnSendEmail', function () {
            var ids = $(_config.emailModalId).data('ids');
            var toEmail = $('#txtEmailTo').val().trim();
            var EmailSub = $('#txtSubject').val().trim();
            var EmailBody = $('#txtMessage').val().trim();
            if (!toEmail) {
                Swal.fire({ icon: 'warning', title: 'Missing Field', text: 'Please enter recipient email.' });
                return;
            }
            if (!EmailSub) {
                Swal.fire({ icon: 'warning', title: 'Missing Field', text: 'Please enter subject of the email.' });
                return;
            }
            $(_config.emailModalId).modal('hide');
            _showToast('Hang tight, we’re sending your email...');
            _postAction('/Email', { moduleType: _config.moduleType, ids: ids, toEmail: toEmail, EmailSub: EmailSub, EmailBody: EmailBody }, function () {
                _showToast('Email sent successfully.');

                $('#txtEmailTo').val('');
                $('#txtSubject').val('');
                $('#txtMessage').val('');
                $(_config.emailModalId).data('ids', null);  
            });
        });

        // 5. DELETE
        $('#btnDelete').on('click', function () {
            var ids = _validate(); if (!ids) return;

            // CHANGED: replaced confirm() with Swal
            Swal.fire({
                icon: 'warning',
                title: 'Are you sure?',
                text: 'Delete ' + ids.length + ' record(s)? This cannot be undone.',
                showCancelButton: true,
                confirmButtonText: 'Yes, Delete',
                cancelButtonText: 'Cancel',
                confirmButtonColor: '#d33',
                cancelButtonColor: '#6c757d'
            }).then(function (result) {
                if (!result.isConfirmed) return;

                _postAction('/Delete', { moduleType: _config.moduleType, ids: ids }, function (response) {
                    _showToast(response.message || 'Record(s) deleted.');
                    $(_config.checkboxSelector + ':checked').each(function () {
                        $(this).closest('tr').fadeOut(300, function () { $(this).remove(); });
                    });
                    $(_config.selectAllSelector).prop('checked', false);
                });
            });
        });
    }

    // ── Generic AJAX POST ─────────────────────────────────────
    function _postAction(endpoint, payload, onSuccess) {
        $.ajax({
            url: _config.baseUrl + endpoint,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(payload),
            success: function (response) {
                if (response.success) {
                    onSuccess(response);
                } else {
                    // CHANGED: replaced alert with Swal
                    Swal.fire({
                        icon: 'error',
                        title: 'Action Failed',
                        text: response.message || 'Something went wrong.'
                    });
                }
            },
            error: function (xhr) {
                // CHANGED: replaced alert with Swal
                Swal.fire({
                    icon: 'error',
                    title: 'Server Error',
                    text: xhr.responseJSON && xhr.responseJSON.message
                        ? xhr.responseJSON.message
                        : xhr.statusText
                });
            }
        });
    }

    // ── Toast helper (uses Bootstrap toast if available) ─────
    function _showToast(msg) {
        if ($('#gridActionToast').length === 0) {
            $('body').append(
                '<div id="gridActionToast" style="position:fixed;bottom:24px;right:24px;' +
                'background:#323232;color:#fff;padding:12px 20px;border-radius:6px;' +
                'z-index:9999;display:none;font-size:14px;">' + msg + '</div>'
            );
        } else {
            $('#gridActionToast').text(msg);
        }
        $('#gridActionToast').fadeIn(200).delay(2500).fadeOut(400);
    }

    // ── Public API ────────────────────────────────────────────
    return { init: init };

})();
