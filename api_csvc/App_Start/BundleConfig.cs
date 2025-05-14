using System.Web;
using System.Web.Optimization;

namespace api_csvc
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new Bundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));


            bundles.Add(new ScriptBundle("~/bundles/CTDT/js").Include(
                        "~/Style/assets/js/vendors.min.js",
                        "~/Style/assets/vendors/chartjs/Chart.min.js",
                        "~/Style/assets/js/app.min.js",
                        "~/Style/assets/vendors/select2/select2.min.js",
                        "~/Style/assets/js/sweetalert2@11.js",
                        "~/Style/assets/js/xlsx.full.min.js",
                        "~/Style/assets/js/exceljs.min.js",
                        "~/Style/assets/js/FileSaver.min.js",
                        "~/Style/assets/vendors/datatables/jquery.dataTables.min.js",
                        "~/Style/assets/vendors/datatables/dataTables.bootstrap.min.js",
                        "~/Style/assets/js/dataTables.buttons.min.js",
                        "~/Style/assets/js/jszip.min.js",
                        "~/Style/assets/js/pdfmake.min.js",
                        "~/Style/assets/js/vfs_fonts.js",
                        "~/Style/assets/js/buttons.html5.min.js",
                        "~/Style/assets/js/buttons.print.min.js"));
        }
    }
}
