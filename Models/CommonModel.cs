using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fintech.Models
{
    public abstract class CommonDataTableModel
    {
        public Int32 LoginID { get; set; }
        public Boolean Active { get; set; }



        public string ActiveText { get { return Active ? "YES" : "NO"; } }
        public Int32 Entity_ID { get; set; }
        public string timestamp { get; set; }
        public string searchText { get; set; }
        public int start { get; set; }
        public int length { get; set; }

        public string orderby { get; set; }
        public string search { get; set; }

        public long Records { get; set; }

        public long TotalRecord { get; set; }
    }
    public class lDataTable : CommonDataTableModel
    {
        public int draw { get; set; }
        public long recordsTotal { get; set; }
        public long recordsFiltered { get; set; }

        public dynamic data { get; set; }
    }
    public class CommonModel
    {
        public class AutoCompleteModel
        {
            public string ID { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public string Product_Desc { get; set; }
            public string Product_HSNCode { get; set; }
            public decimal Selling_Price { get; set; }
            public decimal Cost_Price { get; set; }

            //public string DisplayText { get { return Code + "-" + Name; } }
        }
        public class AutoMulitSelectModel
        {
            public string id { get; set; }
            public string text { get; set; }
            public int TotalRecord { get; set; }
            //public string DisplayText { get { return Code + "-" + Name; } }
        }
        public class AutoMulitSelectModelResult
        {
            public int Total { get; set; }
            public List<AutoMulitSelectModel> Results { get; set; }
        }
        public class AutoCompleteFlexModel
        {
            public string ID { get; set; }
            public string Code { get; set; }
            public string Text { get; set; }
            //public string DisplayText { get { return Code + "-" + Text; } }
        }

        public class AutoCompleteRequestModel
        {
            public string Module { get; set; }

            public string Table { get; set; }
            public string Prefix { get; set; }
            public string Text { get; set; }
            public int Entity_ID { get; set; }
            public string WhereCond { get; set; }
            public int PageSize { get; set; }
            public int PageNum { get; set; }
        }

        public class AutoCompleteWithChildModel
        {
            public string ID { get; set; }
            public string Name { get; set; }
            public string ChildID { get; set; }
            public string ChildName { get; set; }
            public string Code { get; set; } // Change on 20092024 for Added ToCity for rail service By Prajapati Revtaram

            //public string DisplayText { get { return Code + "-" + Name; } }
        }

        public class ImportResult
        {
            public bool Success { get; set; }  // did at least 1 record insert?
            public int InsertedCount { get; set; }  // how many customers were inserted
            public string Message { get; set; }  // human-readable result message
        }

        public class ItemDetailModel
        {
            public string ItemType { get; set; }
            public int ItemId { get; set; }
            public decimal Quantity { get; set; }
            public decimal Rate { get; set; }
            public int TaxGroupId { get; set; }
            public decimal BaseAmount { get; set; }
            public int GL_ID { get; set; }
            public decimal DiscountValue { get; set; }
            public string DiscountType { get; set; }
        }
    }

    public class File
    {
        public bool GenerateFileName { get; set; }

        public string UploadText { get; set; }

        public string UploadType { get; set; }

        public string FilePath { get; set; }

        public string FileName { get; set; }

        public string FileId { get; set; }

        public string FileContent { get; set; }

        public string Domain { get; set; }
        public string ImageType { get; set; }
        public string ImageSubType { get; set; }
        public string ImageId { get; set; }

        public string ErrorMessage { get; set; }
        public string CloudAccessURL { get; set; }
        public string CloudAccessURLValidity { get; set; }

    }

    public class MultiFileUpload
    {
        public string Domain { get; set; }
        public List<File> Files { get; set; }
    }

    public class UploadFileResponseCDN
    {
        public string CloudAccessURL { get; set; }
        public string Authenticated { get; set; }
        public string SessionId { get; set; }
        public string LogReferenceId { get; set; }
        public string ClientCode { get; set; }
        public int EntityId { get; set; }
        public string ClientId { get; set; }
        public string StatusType { get; set; }
        public string StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public List<File> Files { get; set; }


    }
}