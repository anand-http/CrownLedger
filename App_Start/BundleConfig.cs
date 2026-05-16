using System.Web;
using System.Web.Optimization;

namespace fintech
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new Bundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
            #region JQdatatable
            bundles.Add(new ScriptBundle("~/bundles/scripts/JQdatatable").Include(
                "~/assets/js/plugins/tables/datatables/datatables.min.js",
                "~/assets/js/plugins/tables/datatables/extensions/fixed_columns.min.js"
                , "~/assets/js/plugins/tables/datatables/extensions/select.min.js"
                , "~/assets/js/plugins/tables/datatables/extensions/buttons.min.js"
                , "~/assets/js/plugins/tables/datatables/extensions/col_reorder.min.js"
                , "~/assets/js/plugins/forms/selects/select2.min.js"
            ));
            #endregion
        }
    }
}
