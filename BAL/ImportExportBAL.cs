// ============================================================
// ImportExportBAL.cs
// Uses ClosedXML for Excel (already in your project per memories)
// Uses CsvHelper for CSV
// Install via NuGet: ClosedXML, CsvHelper
// ============================================================

using ClosedXML.Excel;
using CsvHelper;
using CsvHelper.Configuration;
using fintech.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using YourApp.DAL;

namespace YourApp.BAL
{
    public class ImportExportBAL
    {
        private readonly ImportExportDAL _dal;

        public ImportExportBAL()
        {
            _dal = new ImportExportDAL();
        }

        // ── Column definitions per module ─────────────────────
        // Matches your SP output columns exactly
        private static Dictionary<string, string[]> _columns =
            new Dictionary<string, string[]>
        {
            {
                "Estimate", new[]
                {
                    "EstimateDate", "EstimateNumber", "Customer",
                    "ReferenceNumber", "ExpiryDate", "Amount", "Status",
                    "CreatedBy", "CreatedOn"
                }
            },
            {
                "Invoice", new[]
                {
                    "InvoiceDate", "DueDate", "CustomerName", "InvoiceNumber",
                    "OrderNumber", "InvoiceAmount", "AmountDue", "Status",
                    "CreatedBy", "CreatedOn"
                }
            },
            {
                "SalesOrder", new[]
                {
                    "Sale_OrderDate", "Sale_OrderNo", "CustomerName",
                    "Sale_EstimateNo", "ShipmentDate", "Amount", "Status",
                    "CreatedBy", "CreatedOn"
                }
            },
            {
                "CreditNote", new[]
                {
                    "CreditNoteDate", "CustomerName", "CreditNoteNumber",
                    "InvoiceNumber", "TotalAmount", "Status",
                    "CreatedBy", "CreatedOn"
                }
            }
        };

        // ── Import columns (subset — read-only cols excluded) ─
        // CreatedBy / CreatedOn are set server-side, not imported
        private static Dictionary<string, string[]> _importColumns =
            new Dictionary<string, string[]>
        {
            {
                "Estimate", new[]
                {
                    "EstimateDate", "EstimateNumber", "Customer",
                    "ReferenceNumber", "ExpiryDate", "Amount", "Status"
                }
            },
            {
                "Invoice", new[]
                {
                    "InvoiceDate", "DueDate", "CustomerName", "InvoiceNumber",
                    "OrderNumber", "InvoiceAmount", "AmountDue", "Status"
                }
            },
            {
                "SalesOrder", new[]
                {
                    "Sale_OrderDate", "Sale_OrderNo", "CustomerName",
                    "Sale_EstimateNo", "ShipmentDate", "Amount", "Status"
                }
            },
            {
                "CreditNote", new[]
                {
                    "CreditNoteDate", "CustomerName", "CreditNoteNumber",
                    "InvoiceNumber", "TotalAmount", "Status"
                }
            }
        };

        // ════════════════════════════════════════════════════
        // EXPORT — EXCEL
        // ════════════════════════════════════════════════════
        public byte[] ExportExcel(string moduleType, int entityId)
        {
            var data = _dal.GetExportData(moduleType, entityId);
            var cols = _columns[moduleType];

            using (var wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add(moduleType);

                // ── Header row ────────────────────────────────
                for (int c = 0; c < cols.Length; c++)
                {
                    var cell = ws.Cell(1, c + 1);
                    cell.Value = cols[c];
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.FontColor = XLColor.White;
                    cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#1a1a2e");
                    cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                }

                // ── Data rows ─────────────────────────────────
                int row = 2;
                foreach (var dataRow in data)
                {
                    for (int c = 0; c < cols.Length; c++)
                    {
                        string key = cols[c];
                        var cell = ws.Cell(row, c + 1);

                        if (dataRow.ContainsKey(key) && dataRow[key] != null)
                        {
                            string val = dataRow[key].ToString();
                            // Try parsing as decimal for amount columns
                            if ((key.Contains("Amount") || key.Contains("amount"))
                                && decimal.TryParse(val, out decimal dec))
                                cell.Value = dec;
                            // Try parsing as date
                            else if (DateTime.TryParse(val, out DateTime dt))
                                cell.Value = dt.ToString("dd-MM-yyyy");
                            else
                                cell.Value = val;
                        }

                        // Alternate row shading
                        if (row % 2 == 0)
                            cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#f5f8fa");
                    }
                    row++;
                }

                // ── Auto-fit columns ──────────────────────────
                ws.Columns().AdjustToContents();

                // ── Freeze header row ─────────────────────────
                ws.SheetView.FreezeRows(1);

                using (var ms = new MemoryStream())
                {
                    wb.SaveAs(ms);
                    return ms.ToArray();
                }
            }
        }

        // ════════════════════════════════════════════════════
        // EXPORT — CSV
        // ════════════════════════════════════════════════════
        public byte[] ExportCsv(string moduleType, int entityId)
        {
            var data = _dal.GetExportData(moduleType, entityId);
            var cols = _columns[moduleType];

            using (var ms = new MemoryStream())
            using (var sw = new StreamWriter(ms, System.Text.Encoding.UTF8))
            {
                // Header
                sw.WriteLine(string.Join(",", cols.Select(c => $"\"{c}\"")));

                // Rows
                foreach (var row in data)
                {
                    var values = cols.Select(col =>
                    {
                        string val = row.ContainsKey(col) && row[col] != null
                            ? row[col].ToString().Replace("\"", "\"\"")
                            : "";
                        return $"\"{val}\"";
                    });
                    sw.WriteLine(string.Join(",", values));
                }

                sw.Flush();
                return ms.ToArray();
            }
        }

        // ════════════════════════════════════════════════════
        // TEMPLATE — EXCEL (blank with headers + sample row)
        // ════════════════════════════════════════════════════
        public byte[] GetTemplateExcel(string moduleType)
        {
            var cols = _importColumns[moduleType]; // import cols only

            using (var wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add($"{moduleType} Import Template");

                // Header
                for (int c = 0; c < cols.Length; c++)
                {
                    var cell = ws.Cell(1, c + 1);
                    cell.Value = cols[c];
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.FontColor = XLColor.White;
                    cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#1a1a2e");
                    cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                }

                // Sample row
                var sample = _GetSampleRow(moduleType);
                for (int c = 0; c < cols.Length; c++)
                {
                    ws.Cell(2, c + 1).Value = sample.ContainsKey(cols[c])
                        ? sample[cols[c]] : "";
                    ws.Cell(2, c + 1).Style.Font.FontColor = XLColor.Gray;
                }

                ws.Columns().AdjustToContents();
                ws.SheetView.FreezeRows(1);

                using (var ms = new MemoryStream())
                {
                    wb.SaveAs(ms);
                    return ms.ToArray();
                }
            }
        }

        // ════════════════════════════════════════════════════
        // TEMPLATE — CSV
        // ════════════════════════════════════════════════════
        public byte[] GetTemplateCsv(string moduleType)
        {
            var cols = _importColumns[moduleType];
            using (var ms = new MemoryStream())
            using (var sw = new StreamWriter(ms, System.Text.Encoding.UTF8))
            {
                sw.WriteLine(string.Join(",", cols.Select(c => $"\"{c}\"")));
                // Sample row
                var sample = _GetSampleRow(moduleType);
                var values = cols.Select(col =>
                {
                    string val = sample.ContainsKey(col) ? sample[col] : "";
                    return $"\"{val}\"";
                });
                sw.WriteLine(string.Join(",", values));
                sw.Flush();
                return ms.ToArray();
            }
        }

        // ════════════════════════════════════════════════════
        // IMPORT — EXCEL
        // ════════════════════════════════════════════════════
        public ImportResultsales ImportExcel(string moduleType, HttpPostedFileBase file, int entityId, int createdBy)
        {
            var result = new ImportResultsales();
            var rows = new List<Dictionary<string, string>>();

            using (var wb = new XLWorkbook(file.InputStream))
            {
                var ws = wb.Worksheet(1);
                var used = ws.RangeUsed();
                if (used == null) return result;

                int colCount = used.ColumnCount();
                int rowCount = used.RowCount();

                int headerRowIndex = 1;
                string firstCellR1 = ws.Cell(1, 1).GetString().Trim();
                if (firstCellR1.StartsWith("Import Template", StringComparison.OrdinalIgnoreCase)
                    || firstCellR1.StartsWith("Note:", StringComparison.OrdinalIgnoreCase)
                    || firstCellR1.StartsWith("*", StringComparison.OrdinalIgnoreCase))
                {
                    headerRowIndex = 2;
                }

                // Read headers — strip " *" required markers so keys match _Validate exactly
                var headers = new List<string>();
                for (int c = 1; c <= colCount; c++)
                {
                    string h = ws.Cell(headerRowIndex, c).GetString().Trim();
                    h = h.Replace(" *", "").Trim(); // "CustomerName *" → "CustomerName"
                    headers.Add(h);
                }

                // Read data rows
                for (int r = headerRowIndex + 1; r <= rowCount; r++)
                {
                    // Skip fully empty rows
                    bool isEmpty = true;
                    for (int c = 1; c <= colCount; c++)
                    {
                        if (!string.IsNullOrWhiteSpace(ws.Cell(r, c).GetString()))
                        { isEmpty = false; break; }
                    }
                    if (isEmpty) continue;

                    var dict = new Dictionary<string, string>();
                    for (int c = 1; c <= colCount; c++)
                    {
                        string header = c <= headers.Count ? headers[c - 1] : $"Col{c}";
                        dict[header] = ws.Cell(r, c).GetString().Trim();
                    }
                    rows.Add(dict);
                }
            }

            return _ProcessImport(moduleType, rows, entityId, createdBy);
        }

        // ════════════════════════════════════════════════════
        // IMPORT — CSV
        // ════════════════════════════════════════════════════
        //public ImportResultsales ImportCsv(string moduleType, HttpPostedFileBase file,
        //    int entityId, int createdBy)
        //{
        //    var rows = new List<Dictionary<string, string>>();

        //    using (var reader = new StreamReader(file.InputStream, System.Text.Encoding.UTF8))
        //    using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
        //    {
        //        HasHeaderRecord = true,
        //        TrimOptions = TrimOptions.Trim,
        //        MissingFieldFound = null   // skip missing fields silently
        //    }))
        //    {
        //        csv.Read();
        //        csv.ReadHeader();
        //        while (csv.Read())
        //        {
        //            var dict = new Dictionary<string, string>();
        //            foreach (var header in csv.HeaderRecord)
        //                dict[header] = csv.GetField(header) ?? "";
        //            rows.Add(dict);
        //        }
        //    }

        //    return _ProcessImport(moduleType, rows, entityId, createdBy);
        //}


        public ImportResultsales ImportCsv(string moduleType, HttpPostedFileBase file,
      int entityId, int createdBy)
        {
            var result = new ImportResultsales();
            var rows = new List<Dictionary<string, string>>();

            using (var reader = new StreamReader(file.InputStream, System.Text.Encoding.UTF8))
            {
                var allLines = new List<string>();

                while (!reader.EndOfStream)
                    allLines.Add(reader.ReadLine());

                if (allLines.Count == 0)
                    return result;

                int headerRowIndex = 0;

                // Detect header row (same logic as Excel)
                string firstLine = allLines[0]?.Trim() ?? "";

                if (firstLine.StartsWith("Import Template", StringComparison.OrdinalIgnoreCase)
                    || firstLine.StartsWith("Note:", StringComparison.OrdinalIgnoreCase)
                    || firstLine.StartsWith("*", StringComparison.OrdinalIgnoreCase))
                {
                    headerRowIndex = 1;
                }

                // Parse CSV manually from detected header row
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = false, // we handle header manually
                    TrimOptions = TrimOptions.Trim,
                    MissingFieldFound = null
                };

                using (var csv = new CsvReader(new StringReader(string.Join(Environment.NewLine, allLines)), config))
                {
                    int currentRow = -1;
                    List<string> headers = new List<string>();

                    while (csv.Read())
                    {
                        currentRow++;

                        if (currentRow < headerRowIndex)
                            continue;

                        if (currentRow == headerRowIndex)
                        {
                            // Read headers and strip " *"
                            headers = csv.Parser.Record
                                .Select(h => (h ?? "").Trim().Replace(" *", "").Trim())
                                .ToList();
                            continue;
                        }

                        // Skip empty rows
                        bool isEmpty = csv.Parser.Record.All(f => string.IsNullOrWhiteSpace(f));
                        if (isEmpty) continue;

                        var dict = new Dictionary<string, string>();

                        for (int i = 0; i < headers.Count; i++)
                        {
                            string header = headers[i];
                            string value = i < csv.Parser.Record.Length
                                ? (csv.Parser.Record[i] ?? "").Trim()
                                : "";

                            if (!string.IsNullOrWhiteSpace(value))
                            {
                                if (DateTime.TryParse(value, out DateTime parsedDate))
                                {
                                    bool looksLikeDate =
                                        value.Contains("/") ||
                                        value.Contains("-") ||
                                        value.Contains(":");

                                    if (looksLikeDate)
                                    {
                                        value = parsedDate.ToString("dd-MM-yyyy HH:mm:ss");
                                    }
                                }
                            }


                            dict[header] = value;
                        }

                        rows.Add(dict);
                    }
                }
            }

            return _ProcessImport(moduleType, rows, entityId, createdBy);
        }


        // ════════════════════════════════════════════════════
        // Core import processor — shared by Excel and CSV
        // ════════════════════════════════════════════════════
        private ImportResultsales _ProcessImport(string moduleType, List<Dictionary<string, string>> rows, int entityId, int createdBy)
        {
            var result = new ImportResultsales { TotalRows = rows.Count };

            // ── Step 1: Validate all rows first ───────────────────────────────────
            var validRows = new List<(int fileRow, Dictionary<string, string> row)>();
            for (int i = 0; i < rows.Count; i++)
            {
                int fileRow = i + 2;
                string validationError = _Validate(moduleType, rows[i]);
                if (!string.IsNullOrEmpty(validationError))
                {
                    result.FailCount++;
                    result.Errors.Add(new ImportRowError { Row = fileRow, Message = validationError });
                }
                else
                {
                    validRows.Add((fileRow, rows[i]));
                }
            }

            if (validRows.Count == 0) return result;

            // ── Step 2: Group rows into documents by DocumentNumber ───────────────
            string docNumberKey = moduleType == "Estimate" ? "EstimateNumber"
                                : moduleType == "SalesOrder" ? "OrderNumber"
                                : moduleType == "Invoice" ? "InvoiceNumber"
                                : moduleType == "CreditNote" ? "InvoiceNumber"
                                : moduleType == "RecInvoice" ? "InvoiceNumber"
                                : moduleType == "PurchaseOrder" ? "PurchaseOrderNumber"
                                : moduleType == "PurchaseBills" ? "Reference"
                                : moduleType == "PurchaseCreditNote" ? "BillNumber"
                                : moduleType == "PurchaseRecurringBills" ? "BillNumber"
                                : moduleType == "ExpenseHistory" ? "BillNumber"
                                : "DocumentNumber";

            // Build XML from grouped rows
            XElement xmlDoc = new XElement("Documents"); ;
            var expenseRecord = "";
            if (moduleType == "Estimate") { 
                xmlDoc = new XElement("Documents",
                   validRows.GroupBy(r => new
                   {
                       DocNumber = _Get(r.row, docNumberKey),
                       CustomerCode = _Get(r.row, "CustomerCode") 
                   })
                       .Select(g =>
                       {
                           var first = g.First().row;
                           return new XElement("Document",
                               // Header fields
                               new XElement("EntityId", entityId),
                               new XElement("CreatedBy", createdBy),
                               new XElement("CustomerCode", _Get(first, "CustomerCode")),
                               new XElement("DocumentDate", _Get(first, moduleType == "Estimate" ? "EstimateDate" : "DocumentDate")),
                               new XElement("ReferenceNumber", _Get(first, "ReferenceNumber")),
                               new XElement("ExpiryDate", _Get(first, "ExpiryDate")),
                               new XElement("Comments", _Get(first, "Comments")),
                               new XElement("TermsAndConditions", _Get(first, "TermsAndConditions")),
                               new XElement("SubTotal", _Get(first, "SubTotal")),
                               new XElement("Discount", _Get(first, "Discount")),
                               new XElement("OtherAdjustment", _Get(first, "OtherAdjustment")),
                               new XElement("Tax", _Get(first, "Tax")),
                               new XElement("Total", _Get(first, "Total")),
                               new XElement("Status", _Get(first, "Status")),

                               // All line items nested under <Items>
                               new XElement("Items",
                                   g.Select(r => new XElement("Item",
                                       new XElement("CustomerCode", _Get(first, "CustomerCode")),
                                       new XElement("ItemCode", _Get(r.row, "ItemCode")),
                                       new XElement("ItemType", _Get(r.row, "ItemType")),
                                       new XElement("Quantity", _Get(r.row, "Quantity")),
                                       new XElement("Rate", _Get(r.row, "Rate")),
                                       new XElement("TaxType", _Get(r.row, "TaxType")),
                                       new XElement("Amount", _Get(r.row, "Amount")),
                                       new XElement("DscountType", _Get(r.row, "DiscountType")),
                                       new XElement("Discount", _Get(r.row, "Discount"))
                                   ))
                               )
                           );
                       })
               );
        }
            else if (moduleType == "SalesOrder")
            {
                xmlDoc = new XElement("Documents",
                   validRows.GroupBy(r => new
                   {
                       DocNumber = _Get(r.row, docNumberKey),
                       CustomerCode = _Get(r.row, "CustomerCode")
                   })
                       .Select(g =>
                       {
                           var first = g.First().row;
                           return new XElement("Document",
                               // Header fields
                               new XElement("EntityId", entityId),
                               new XElement("CreatedBy", createdBy),
                               new XElement("CustomerCode", _Get(first, "CustomerCode")),
                               new XElement("Reference", _Get(first, "Reference")),
                               new XElement("SalesOrderDate", _Get(first, "SalesOrderDate")),
                               new XElement("PaymentTerms", _Get(first, "PaymentTerms")),
                               new XElement("Currency", _Get(first, "Currency")),
                               new XElement("DeliveryMethod", _Get(first, "DeliveryMethod")),
                               new XElement("ShipmentDate", _Get(first, "ShipmentDate")),
                               new XElement("SalesPerson", _Get(first, "SalesPerson")),
                               new XElement("TermsAndConditions", _Get(first, "TermsAndConditions")),
                               new XElement("Comments", _Get(first, "Comments")),
                               new XElement("SubTotal", _Get(first, "SubTotal")),
                               new XElement("Total", _Get(first, "Total")),
                               new XElement("Discount", _Get(first, "Discount")),
                               new XElement("OtherAdjustment", _Get(first, "OtherAdjustment")),
                               new XElement("Tax", _Get(first, "Tax")),
                               new XElement("Total", _Get(first, "Total")),
                               new XElement("Status", _Get(first, "Status")),

                               // All line items nested under <Items>
                               new XElement("Items",
                                   g.Select(r => new XElement("Item",
                                       new XElement("CustomerCode", _Get(first, "CustomerCode")),
                                       new XElement("ItemCode", _Get(r.row, "ItemCode")),
                                       new XElement("ItemType", _Get(r.row, "ItemType")),
                                       new XElement("Quantity", _Get(r.row, "Quantity")),
                                       new XElement("Rate", _Get(r.row, "Rate")),
                                       new XElement("TaxType", _Get(r.row, "TaxType")),
                                       new XElement("Amount", _Get(r.row, "Amount")),
                                       new XElement("DscountType", _Get(r.row, "DiscountType")),
                                       new XElement("Discount", _Get(r.row, "Discount"))
                                   ))
                               )
                           );
                       })
               );
            }
            else if (moduleType == "Invoice")
            {
                xmlDoc = new XElement("Documents",
                   validRows.GroupBy(r => new
                   {
                       DocNumber = _Get(r.row, docNumberKey),
                       CustomerCode = _Get(r.row, "CustomerCode")
                   })
                       .Select(g =>
                       {
                           var first = g.First().row;
                           return new XElement("Document",
                               // Header fields
                               new XElement("EntityId", entityId),
                               new XElement("CreatedBy", createdBy),
                               new XElement("CustomerCode", _Get(first, "CustomerCode")),
                               new XElement("OrderNumber", _Get(first, "OrderNumber")),
                               new XElement("InvoiceDate", _Get(first, "InvoiceDate")),
                               new XElement("AccountingDate", _Get(first, "AccountingDate")),
                               new XElement("Currency", _Get(first, "Currency")),
                               new XElement("PaymentTerms", _Get(first, "PaymentTerms")),
                               new XElement("DueDate", _Get(first, "DueDate")),
                               new XElement("SalesPerson", _Get(first, "SalesPerson")),
                               new XElement("Branch", _Get(first, "Branch")),
                               new XElement("TermsAndConditions", _Get(first, "TermsAndConditions")),
                               new XElement("Comments", _Get(first, "Comments")),
                               new XElement("SubTotal", _Get(first, "SubTotal")),
                               new XElement("Total", _Get(first, "Total")),
                               new XElement("Discount", _Get(first, "Discount")),
                               new XElement("OtherAdjustment", _Get(first, "OtherAdjustment")),
                               new XElement("Tax", _Get(first, "Tax")),
                               new XElement("Total", _Get(first, "Total")),
                               new XElement("Status", _Get(first, "Status")),

                               // All line items nested under <Items>
                               new XElement("Items",
                                   g.Select(r => new XElement("Item",
                                       new XElement("CustomerCode", _Get(first, "CustomerCode")),
                                       new XElement("ItemCode", _Get(r.row, "ItemCode")),
                                       new XElement("ItemType", _Get(r.row, "ItemType")),
                                       new XElement("Quantity", _Get(r.row, "Quantity")),
                                       new XElement("LedgerAccount", _Get(r.row, "LedgerAccount")),
                                       new XElement("Rate", _Get(r.row, "Rate")),
                                       new XElement("TaxType", _Get(r.row, "TaxType")),
                                       new XElement("Amount", _Get(r.row, "Amount")),
                                       new XElement("DscountType", _Get(r.row, "DiscountType")),
                                       new XElement("Discount", _Get(r.row, "Discount"))
                                   ))
                               )
                           );
                       })
               );
            }
            else if (moduleType == "RecInvoice")
            {
                xmlDoc = new XElement("Documents",
                   validRows.GroupBy(r => new
                   {
                       DocNumber = _Get(r.row, docNumberKey),
                       CustomerCode = _Get(r.row, "CustomerCode")
                   })
                       .Select(g =>
                       {
                           var first = g.First().row;
                           return new XElement("Document",
                               // Header fields
                               new XElement("EntityId", entityId),
                               new XElement("CreatedBy", createdBy),
                               new XElement("CustomerCode", _Get(first, "CustomerCode")),
                               new XElement("Frequency", _Get(first, "Frequency")),
                               new XElement("StartDate", _Get(first, "StartDate")),
                               new XElement("EndDate", _Get(first, "EndDate")),
                               new XElement("PaymentTerms", _Get(first, "PaymentTerms")),
                               new XElement("Currency", _Get(first, "Currency")),
                               new XElement("SalesPerson", _Get(first, "SalesPerson")),
                               new XElement("Branch", _Get(first, "Branch")),
                               new XElement("TermsAndConditions", _Get(first, "TermsAndConditions")),
                               new XElement("Comments", _Get(first, "Comments")),
                               new XElement("SubTotal", _Get(first, "SubTotal")),
                               new XElement("Total", _Get(first, "Total")),
                               new XElement("Discount", _Get(first, "Discount")),
                               new XElement("OtherAdjustment", _Get(first, "OtherAdjustment")),
                               new XElement("Tax", _Get(first, "Tax")),
                               new XElement("Total", _Get(first, "Total")),
                               new XElement("Status", _Get(first, "Status")),

                               // All line items nested under <Items>
                               new XElement("Items",
                                   g.Select(r => new XElement("Item",
                                       new XElement("CustomerCode", _Get(first, "CustomerCode")),
                                       new XElement("ItemCode", _Get(r.row, "ItemCode")),
                                       new XElement("ItemType", _Get(r.row, "ItemType")),
                                       new XElement("Quantity", _Get(r.row, "Quantity")),
                                       new XElement("LedgerAccount", _Get(r.row, "LedgerAccount")),
                                       new XElement("Rate", _Get(r.row, "Rate")),
                                       new XElement("TaxType", _Get(r.row, "TaxType")),
                                       new XElement("Amount", _Get(r.row, "Amount")),
                                       new XElement("DscountType", _Get(r.row, "DiscountType")),
                                       new XElement("Discount", _Get(r.row, "Discount"))
                                   ))
                               )
                           );
                       })
               );
            }
            else if (moduleType == "CreditNote")
            {
                xmlDoc = new XElement("Documents",
                   validRows.GroupBy(r => new
                   {
                       DocNumber = _Get(r.row, docNumberKey),
                       CustomerCode = _Get(r.row, "CustomerCode")
                   })
                       .Select(g =>
                       {
                           var first = g.First().row;
                           return new XElement("Document",
                               // Header fields
                               new XElement("EntityId", entityId),
                               new XElement("CreatedBy", createdBy),
                               new XElement("CustomerCode", _Get(first, "CustomerCode")),
                               new XElement("CreditNoteDate", _Get(first, "CreditNoteDate")),
                               new XElement("InvoiceNumber", _Get(first, "InvoiceNumber")),
                               new XElement("AccountingDate", _Get(first, "AccountingDate")),
                               new XElement("Currency", _Get(first, "Currency")),
                               new XElement("SalesPerson", _Get(first, "SalesPerson")),
                               new XElement("Reason", _Get(first, "Reason")),
                               new XElement("TermsAndConditions", _Get(first, "TermsAndConditions")),
                               new XElement("Comments", _Get(first, "Comments")),
                               new XElement("SubTotal", _Get(first, "SubTotal")),
                               new XElement("Total", _Get(first, "Total")),
                               new XElement("Discount", _Get(first, "Discount")),
                               new XElement("OtherAdjustment", _Get(first, "OtherAdjustment")),
                               new XElement("Tax", _Get(first, "Tax")),
                               new XElement("Total", _Get(first, "Total")),
                               new XElement("Status", _Get(first, "Status")),

                               // All line items nested under <Items>
                               new XElement("Items",
                                   g.Select(r => new XElement("Item",
                                       new XElement("CustomerCode", _Get(first, "CustomerCode")),
                                       new XElement("ItemCode", _Get(r.row, "ItemCode")),
                                       new XElement("ItemType", _Get(r.row, "ItemType")),
                                       new XElement("Quantity", _Get(r.row, "Quantity")),
                                       new XElement("LedgerAccount", _Get(r.row, "LedgerAccount")),
                                       new XElement("Rate", _Get(r.row, "Rate")),
                                       new XElement("TaxType", _Get(r.row, "TaxType")),
                                       new XElement("Amount", _Get(r.row, "Amount")),
                                       new XElement("DscountType", _Get(r.row, "DiscountType")),
                                       new XElement("Discount", _Get(r.row, "Discount"))
                                   ))
                               )
                           );
                       })
               );
            }
            else if (moduleType == "PurchaseOrder")
            {
                xmlDoc = new XElement("Documents",
                   validRows.GroupBy(r => new
                   {
                       DocNumber = _Get(r.row, docNumberKey),
                       VendorCode = _Get(r.row, "VendorCode")
                   })
                       .Select(g =>
                       {
                           var first = g.First().row;
                           return new XElement("Document",
                               // Header fields
                               new XElement("EntityId", entityId),
                               new XElement("CreatedBy", createdBy),
                               new XElement("VendorCode", _Get(first, "VendorCode")),
                               new XElement("ExpectedDeliveryDate", _Get(first, "ExpectedDeliveryDate")),
                               new XElement("PurchaseOrderDate", _Get(first, "PurchaseOrderDate")),
                               new XElement("Currency", _Get(first, "Currency")),
                               new XElement("PaymentTerms", _Get(first, "PaymentTerms")),
                               new XElement("DeliveryAddress", _Get(first, "DeliveryAddress")),
                               new XElement("DueDate", _Get(first, "DueDate")),
                               new XElement("Reference", _Get(first, "Reference")),
                               new XElement("Branch", _Get(first, "Branch")),
                               new XElement("TermsAndConditions", _Get(first, "TermsAndConditions")),
                               new XElement("Comments", _Get(first, "Comments")),
                               new XElement("SubTotal", _Get(first, "SubTotal")),
                               new XElement("Total", _Get(first, "Total")),
                               new XElement("Discount", _Get(first, "Discount")),
                               new XElement("OtherAdjustment", _Get(first, "OtherAdjustment")),
                               new XElement("Tax", _Get(first, "Tax")),
                               new XElement("Total", _Get(first, "Total")),
                               new XElement("Status", _Get(first, "Status")),

                               // All line items nested under <Items>
                               new XElement("Items",
                                   g.Select(r => new XElement("Item",
                                       new XElement("VendorCode", _Get(first, "VendorCode")),
                                       new XElement("ItemCode", _Get(r.row, "ItemCode")),
                                       new XElement("ItemType", _Get(r.row, "ItemType")),
                                       new XElement("Quantity", _Get(r.row, "Quantity")),
                                       new XElement("LedgerAccount", _Get(r.row, "LedgerAccount")),
                                       new XElement("Rate", _Get(r.row, "Rate")),
                                       new XElement("TaxType", _Get(r.row, "TaxType")),
                                       new XElement("Amount", _Get(r.row, "Amount")),
                                       new XElement("DscountType", _Get(r.row, "DiscountType")),
                                       new XElement("Discount", _Get(r.row, "Discount"))
                                   ))
                               )
                           );
                       })
               );
            }
            else if (moduleType == "PurchaseBills")
            {
                xmlDoc = new XElement("Documents",
                   validRows.GroupBy(r => new
                   {
                       DocNumber = _Get(r.row, docNumberKey),
                       VendorCode = _Get(r.row, "VendorCode")
                   })
                       .Select(g =>
                       {
                           var first = g.First().row;
                           return new XElement("Document",
                               // Header fields
                               new XElement("EntityId", entityId),
                               new XElement("CreatedBy", createdBy),
                               new XElement("VendorCode", _Get(first, "VendorCode")),
                               new XElement("PurchaseOrderNumber", _Get(first, "PurchaseOrderNumber")),
                               new XElement("BillDate", _Get(first, "BillDate")),
                               new XElement("AccountingDate", _Get(first, "AccountingDate")),
                               new XElement("Currency", _Get(first, "Currency")),
                               new XElement("PaymentTerms", _Get(first, "PaymentTerms")),
                               new XElement("DeliveryAddress", _Get(first, "DeliveryAddress")),
                               new XElement("DueDate", _Get(first, "DueDate")),
                               new XElement("Reference", _Get(first, "Reference")),
                               new XElement("Branch", _Get(first, "Branch")),
                               new XElement("TermsAndConditions", _Get(first, "TermsAndConditions")),
                               new XElement("Comments", _Get(first, "Comments")),
                               new XElement("SubTotal", _Get(first, "SubTotal")),
                               new XElement("Total", _Get(first, "Total")),
                               new XElement("Discount", _Get(first, "Discount")),
                               new XElement("OtherAdjustment", _Get(first, "OtherAdjustment")),
                               new XElement("Tax", _Get(first, "Tax")),
                               new XElement("Total", _Get(first, "Total")),
                               new XElement("Status", _Get(first, "Status")),

                               // All line items nested under <Items>
                               new XElement("Items",
                                   g.Select(r => new XElement("Item",
                                       new XElement("VendorCode", _Get(first, "VendorCode")),
                                       new XElement("ItemCode", _Get(r.row, "ItemCode")),
                                       new XElement("ItemType", _Get(r.row, "ItemType")),
                                       new XElement("Quantity", _Get(r.row, "Quantity")),
                                       new XElement("LedgerAccount", _Get(r.row, "LedgerAccount")),
                                       new XElement("Rate", _Get(r.row, "Rate")),
                                       new XElement("TaxType", _Get(r.row, "TaxType")),
                                       new XElement("Amount", _Get(r.row, "Amount")),
                                       new XElement("DscountType", _Get(r.row, "DiscountType")),
                                       new XElement("Discount", _Get(r.row, "Discount"))
                                   ))
                               )
                           );
                       })
               );
            }
            else if (moduleType == "PurchaseCreditNote")
            {
                xmlDoc = new XElement("Documents",
                   validRows.GroupBy(r => new
                   {
                       DocNumber = _Get(r.row, docNumberKey),
                       VendorCode = _Get(r.row, "VendorCode")
                   })
                       .Select(g =>
                       {
                           var first = g.First().row;
                           return new XElement("Document",
                               // Header fields
                               new XElement("EntityId", entityId),
                               new XElement("CreatedBy", createdBy),
                               new XElement("VendorCode", _Get(first, "VendorCode")),
                               new XElement("CreditNoteDate", _Get(first, "CreditNoteDate")),
                               new XElement("AccountingDate", _Get(first, "AccountingDate")),
                               new XElement("Currency", _Get(first, "Currency")),
                               new XElement("Reason", _Get(first, "Reason")),
                               new XElement("BillNumber", _Get(first, "BillNumber")),
                               new XElement("Branch", _Get(first, "Branch")),
                               new XElement("TermsAndConditions", _Get(first, "TermsAndConditions")),
                               new XElement("Comments", _Get(first, "Comments")),
                               new XElement("SubTotal", _Get(first, "SubTotal")),
                               new XElement("Total", _Get(first, "Total")),
                               new XElement("Discount", _Get(first, "Discount")),
                               new XElement("OtherAdjustment", _Get(first, "OtherAdjustment")),
                               new XElement("Tax", _Get(first, "Tax")),
                               new XElement("Total", _Get(first, "Total")),
                               new XElement("Status", _Get(first, "Status")),

                               // All line items nested under <Items>
                               new XElement("Items",
                                   g.Select(r => new XElement("Item",
                                       new XElement("VendorCode", _Get(first, "VendorCode")),
                                       new XElement("ItemCode", _Get(r.row, "ItemCode")),
                                       new XElement("ItemType", _Get(r.row, "ItemType")),
                                       new XElement("Quantity", _Get(r.row, "Quantity")),
                                       new XElement("Ledger", _Get(r.row, "Ledger")),
                                       new XElement("Rate", _Get(r.row, "Rate")),
                                       new XElement("TaxType", _Get(r.row, "TaxType")),
                                       new XElement("Amount", _Get(r.row, "Amount")),
                                       new XElement("DscountType", _Get(r.row, "DiscountType")),
                                       new XElement("Discount", _Get(r.row, "Discount"))
                                   ))
                               )
                           );
                       })
               );
            }
            else if (moduleType == "PurchaseRecurringBills")
            {
                xmlDoc = new XElement("Documents",
                   validRows.GroupBy(r => new
                   {
                       DocNumber = _Get(r.row, docNumberKey),
                       VendorCode = _Get(r.row, "VendorCode")
                   })
                       .Select(g =>
                       {
                           var first = g.First().row;
                           return new XElement("Document",
                               // Header fields
                               new XElement("EntityId", entityId),
                               new XElement("CreatedBy", createdBy),
                               new XElement("VendorCode", _Get(first, "VendorCode")),
                               new XElement("StartDate", _Get(first, "StartDate")),
                               new XElement("EndDate", _Get(first, "EndDate")),
                               new XElement("Currency", _Get(first, "Currency")),
                               new XElement("PaymentTerms", _Get(first, "PaymentTerms")),
                               new XElement("Frequency", _Get(first, "Frequency")),
                               new XElement("Branch", _Get(first, "Branch")),
                               new XElement("TermsAndConditions", _Get(first, "TermsAndConditions")),
                               new XElement("Comments", _Get(first, "Comments")),
                               new XElement("SubTotal", _Get(first, "SubTotal")),
                               new XElement("Total", _Get(first, "Total")),
                               new XElement("Discount", _Get(first, "Discount")),
                               new XElement("OtherAdjustment", _Get(first, "OtherAdjustment")),
                               new XElement("Tax", _Get(first, "Tax")),
                               new XElement("Total", _Get(first, "Total")),
                               new XElement("Status", _Get(first, "Status")),

                               // All line items nested under <Items>
                               new XElement("Items",
                                   g.Select(r => new XElement("Item",
                                       new XElement("VendorCode", _Get(first, "VendorCode")),
                                       new XElement("ItemCode", _Get(r.row, "ItemCode")),
                                       new XElement("ItemType", _Get(r.row, "ItemType")),
                                       new XElement("Quantity", _Get(r.row, "Quantity")),
                                       new XElement("LedgerAccount", _Get(r.row, "LedgerAccount")),
                                       new XElement("Rate", _Get(r.row, "Rate")),
                                       new XElement("TaxType", _Get(r.row, "TaxType")),
                                       new XElement("Amount", _Get(r.row, "Amount")),
                                       new XElement("DscountType", _Get(r.row, "DiscountType")),
                                       new XElement("Discount", _Get(r.row, "Discount"))
                                   ))
                               )
                           );
                       })
               );
            }
            else if (moduleType == "ExpenseHistory")
            {
                xmlDoc = new XElement("Documents",
                    validRows.GroupBy(r => new
                    {
                        DocNumber = _Get(r.row, docNumberKey),
                        VendorCode = _Get(r.row, "VendorCode")
                    })
                    .Select(g =>
                    {
                        var first = g.First().row;
                        var recordType = _Get(first, "RecordType");
                        expenseRecord = recordType;
                        if (recordType.Equals("ExpenseHistory", StringComparison.OrdinalIgnoreCase))
                        {
                            return new XElement("Document",
                                new XElement("EntityId", entityId),
                                new XElement("CreatedBy", createdBy),
                                new XElement("RecordType", _Get(first, "RecordType")),
                                new XElement("VendorCode", _Get(first, "VendorCode")),
                                new XElement("ExpenseType", _Get(first, "ExpenseType")),
                                new XElement("ExpenseAmount", _Get(first, "ExpenseAmount")),
                                new XElement("HSNCode", _Get(first, "HSNCode")),
                                new XElement("Date", _Get(first, "Date")),
                                new XElement("Currency", _Get(first, "Currency")),
                                new XElement("Department", _Get(first, "Department")),
                                new XElement("Description", _Get(first, "Description")),
                                new XElement("PaidFrom", _Get(first, "PaidFrom")),
                                new XElement("GstApplicable", _Get(first, "GstApplicable")),
                                new XElement("Branch", _Get(first, "Branch")),
                                new XElement("GSTTaxCode", _Get(first, "GSTTaxCode")),
                                new XElement("TaxAmount", _Get(first, "TaxAmount")),
                                new XElement("LedgerAccount", _Get(first, "LedgerAccount")),
                                new XElement("ITCAvailable", _Get(first, "ITCAvailable")),
                                new XElement("TdsChargeable", _Get(first, "TdsChargeable")),
                                new XElement("TdsCode", _Get(first, "TdsCode")),
                                new XElement("TdsAmount", _Get(first, "TdsAmount")),
                                new XElement("Status", _Get(first, "Status"))
                            );
                        }
                        else if(recordType.Equals("RecurringExpense", StringComparison.OrdinalIgnoreCase))
                        {
                            return new XElement("Document",
                               new XElement("EntityId", entityId),
                               new XElement("CreatedBy", createdBy),
                               new XElement("RecordType", _Get(first, "RecordType")),
                               new XElement("VendorCode", _Get(first, "VendorCode")),
                               new XElement("ExpenseType", _Get(first, "ExpenseType")),
                               new XElement("ExpenseAmount", _Get(first, "ExpenseAmount")),
                               new XElement("HSNCode", _Get(first, "HSNCode")),
                               new XElement("StartDate", _Get(first, "StartDate")),
                               new XElement("EndDate", _Get(first, "EndDate")),
                               new XElement("Currency", _Get(first, "Currency")),
                               new XElement("Department", _Get(first, "Department")),
                               new XElement("Description", _Get(first, "Description")),
                               new XElement("Frequency", _Get(first, "Frequency")),
                               new XElement("PaidFrom", _Get(first, "PaidFrom")),
                               new XElement("GstApplicable", _Get(first, "GstApplicable")),
                               new XElement("Branch", _Get(first, "Branch")),
                               new XElement("GSTTaxCode", _Get(first, "GSTTaxCode")),
                               new XElement("TaxAmount", _Get(first, "TaxAmount")),
                               new XElement("LedgerAccount", _Get(first, "LedgerAccount")),
                               new XElement("ITCAvailable", _Get(first, "ITCAvailable")),
                               new XElement("TdsChargeable", _Get(first, "TdsChargeable")),
                               new XElement("TdsCode", _Get(first, "TdsCode")),
                               new XElement("TdsAmount", _Get(first, "TdsAmount")),
                               new XElement("Status", _Get(first, "Status"))
                           );
                        }
                        else
                        {
                            return new XElement("Document",
                               new XElement("EntityId", entityId),
                               new XElement("CreatedBy", createdBy),
                               new XElement("RecordType", _Get(first, "RecordType")),
                               new XElement("VendorCode", _Get(first, "VendorCode")),
                               new XElement("ExpenseAmount", _Get(first, "ExpenseAmount")),
                               new XElement("Date", _Get(first, "Date")),
                               new XElement("Currency", _Get(first, "Currency")),
                               new XElement("Department", _Get(first, "Department")),
                               new XElement("Description", _Get(first, "Description")),
                               new XElement("PaidFrom", _Get(first, "PaidFrom")),
                               new XElement("Branch", _Get(first, "Branch")),
                               new XElement("TaxAmount", _Get(first, "TaxAmount")),
                               new XElement("LedgerAccount", _Get(first, "LedgerAccount")),
                               new XElement("Status", _Get(first, "Status"))
                           );
                        }
                    })
                );
            }

            string xmlString = xmlDoc.ToString(); // Pass this to BAL/DAL

            // ── Step 3: Send all to DAL in one call ───────────────────────────────
            try
            {
                var success = false;
                if (moduleType == "ExpenseHistory")
                {
                    success = _dal.ImportAll(expenseRecord, xmlString);
                }
                else
                {
                     success = _dal.ImportAll(moduleType, xmlString);
                }
                
                result.Success = success;

                if (result.Success) { 
                    result.SuccessCount = validRows.Count; 
                }
                else
                {
                    result.SuccessCount = 0;
                }
            }
            catch (Exception ex)
            {
                result.FailCount += validRows.Count;
                result.Errors.Add(new ImportRowError { Row = -1, Message = ex.Message });
            }

            return result;
        }

        // ── Per-module validation ─────────────────────────────
        private string _Validate(string moduleType, Dictionary<string, string> row)
        {
            switch (moduleType)
            {
                case "Estimate":
                    if (string.IsNullOrWhiteSpace(_Get(row, "CustomerCode")))
                        return "CustomerCode is required.";
                    if (!DateTime.TryParse(_Get(row, "EstimateDate"), out _))
                        return "EstimateDate is invalid or missing.";
                    break;

                case "Invoice":
                    if (string.IsNullOrWhiteSpace(_Get(row, "CustomerCode")))
                        return "CustomerCode is required.";
                    if (!DateTime.TryParse(_Get(row, "InvoiceDate"), out _))
                        return "InvoiceDate is invalid or missing.";
                    break;

                case "SalesOrder":
                    if (string.IsNullOrWhiteSpace(_Get(row, "CustomerCode")))
                        return "CustomerCode is required.";
                    if (!DateTime.TryParse(_Get(row, "SalesOrderDate"), out _))
                        return "SalesOrderDate is invalid or missing.";
                    break;

                case "CreditNote":
                    if (string.IsNullOrWhiteSpace(_Get(row, "CustomerCode")))
                        return "CustomerCode is required.";
                    if (!DateTime.TryParse(_Get(row, "CreditNoteDate"), out _))
                        return "CreditNoteDate is invalid or missing.";
                    break;
            }
            return null;
        }

        private string _Get(Dictionary<string, string> row, string key)
            => row.ContainsKey(key) ? row[key] : "";

        // ── Sample row for templates ──────────────────────────
        private Dictionary<string, string> _GetSampleRow(string moduleType)
        {
            string today = DateTime.Now.ToString("dd-MM-yyyy");
            string future = DateTime.Now.AddDays(30).ToString("dd-MM-yyyy");

            switch (moduleType)
            {
                case "Estimate":
                    return new Dictionary<string, string>
                    {
                        { "EstimateDate",    today   },
                        { "EstimateNumber",  "EST001" },
                        { "Customer",        "ABC Company" },
                        { "ReferenceNumber", "REF-001" },
                        { "ExpiryDate",      future  },
                        { "Amount",          "5000.00" },
                        { "Status",          "Pending" }
                    };
                case "Invoice":
                    return new Dictionary<string, string>
                    {
                        { "InvoiceDate",   today   },
                        { "DueDate",       future  },
                        { "CustomerName",  "ABC Company" },
                        { "InvoiceNumber", "INV001" },
                        { "OrderNumber",   "ORD001" },
                        { "InvoiceAmount", "5000.00" },
                        { "AmountDue",     "5000.00" },
                        { "Status",        "Pending" }
                    };
                case "SalesOrder":
                    return new Dictionary<string, string>
                    {
                        { "Sale_OrderDate",  today   },
                        { "Sale_OrderNo",    "SO001"  },
                        { "CustomerName",    "ABC Company" },
                        { "Sale_EstimateNo", "EST001" },
                        { "ShipmentDate",    future  },
                        { "Amount",          "5000.00" },
                        { "Status",          "Pending" }
                    };
                case "CreditNote":
                    return new Dictionary<string, string>
                    {
                        { "CreditNoteDate",   today   },
                        { "CustomerName",     "ABC Company" },
                        { "CreditNoteNumber", "CN001"  },
                        { "InvoiceNumber",    "INV001" },
                        { "TotalAmount",      "1000.00" },
                        { "Status",           "Pending" }
                    };
                default:
                    return new Dictionary<string, string>();
            }
        }
    }
}
