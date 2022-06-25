using System.Web;
using System.Web.Optimization;

namespace Fargo_Application
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Content/plugins/jQuery/jQuery-2.1.4.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-ui").Include(
                        "~/Content/plugins/jQueryUI/jquery-ui.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/Bootstrap-Slider").Include(
                        "~/Content/plugins/bootstrap-slider/bootstrap-slider.js"));

            bundles.Add(new ScriptBundle("~/bundles/CommonJs").Include(
                        "~/Content/bootstrap/js/bootstrap.min.js",
                        "~/Content/plugins/datatables/jquery.dataTables.min.js",
                        "~/Content/plugins/datatables/dataTables.bootstrap.min.js",
                        "~/Content/plugins/fastclick/fastclick.min.js",
                        "~/Content/dist/js/app.min.js",
                        "~/Content/plugins/sparkline/jquery.sparkline.min.js",
                        "~/Content/plugins/jvectormap/jquery-jvectormap-1.2.2.min.js",
                        "~/Content/plugins/jvectormap/jquery-jvectormap-world-mill-en.js",
                        "~/Content/plugins/slimScroll/jquery.slimscroll.min.js",
                        "~/Content/plugins/chartjs/Chart.min.js",
                        //"~/Content/dist/js/pages/dashboard2.js",
                        "~/Content/dist/js/demo.js",
                        "~/Scripts/Application/App.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/bower_components/CommonCss").Include(
                      "~/Content/bootstrap/css/bootstrap.min.css",
                      "~/Content/plugins/datatables/dataTables.bootstrap.css",
                      "~/Content/plugins/jvectormap/jquery-jvectormap-1.2.2.css",
                      "~/Content/dist/css/AdminLTE.min.css",
                      "~/Content/dist/css/skins/_all-skins.min.css",
                      "~/Content/site.css"
                      ));

            bundles.Add(new StyleBundle("~/bower_components/BootstrapSlider").Include(
                      "~/Content/plugins/bootstrap-slider/slider.css"));
        }
    }
}
