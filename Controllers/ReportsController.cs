using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.Mvc;

namespace fintech.Controllers
{
    public class ReportsController : Controller
    {
        // GET: Reports
        public ActionResult reports()
        {
            return View();
        }

        public ActionResult AccountPayablAgingReport()
        {
            return View("account-payable-aging-report");
        }

        public ActionResult AccountPayableDetails()
        {
            return View("account-payable-details");
        }

        public ActionResult AccountPayableDetails2()
        {
            return View("account-payable-details2");
        }

        public ActionResult AccountRecievableAgingReport()
        {
            return View("account-receivable-aging-report");
        }

        public ActionResult AccountRecievableDetails()
        {
            return View("account-receivable-details");
        }
        public ActionResult AccountRecievableDetails2()
        {
            return View("account-receivable-details2");
        }

        public ActionResult AccountSummary()
        {
            return View("account-summary");
        }
        public ActionResult AccountSummary2()
        {
            return View("account-summary2");
        }

        public ActionResult BalanceSheet()
        {
            return View("balance-sheet");
        }

        public ActionResult CashFlowStatement()
        {
            return View("cash-flow-statement");
        }

        public ActionResult CreditNotesDetails()
        {
            return View("credit-notes-details");
        }

        public ActionResult CustomerReceiptDetails()
        {
            return View("customer-receipt-details");
        }

        public ActionResult Expense()
        {
            return View("expense");
        }

        public ActionResult ProfitLoss()
        {
            return View("profit-loss");
        }

        public ActionResult PurchaseBillDetails()
        {
            return View("purchase-bill-details");
        }

        public ActionResult PurchaseOrderDetails()
        {
            return View("purchase-order-details");
        }

        public ActionResult PurchaseRegister()
        {
            return View("purchase-register");
        }

        public ActionResult PurchaseRegister2()
        {
            return View("purchase-register2");
        }

        public ActionResult SalesEstimateDetails()
        {
            return View("sales-estimate-details");
        }

        public ActionResult SalesInvoiceDetails()
        {
            return View("sales-invoice-details");
        }

        public ActionResult SalesOrderDetails()
        {
            return View("sales-order-details");
        }

        public ActionResult SalesRegister()
        {
            return View("sales-register");
        }

        public ActionResult StatementOfChangesInEquity()
        {
            return View("statement-of-changes-in-equity");
        }

        public ActionResult TrailBalance()
        {
            return View("trail-balance");
        }


    }
}