using System.Web.Optimization;

namespace PunasMarketing
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();
            BundleTable.EnableOptimizations = true;


            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));


            bundles.Add(new ScriptBundle("~/bundles/Admin").Include(
                         "~/Content/AdminHtml/js/demo.min.js",
                         "~/ Content/AdminHtml/js/application.min.js",
                         "~/Content/AdminHtml/js/elephant.min.js",
                         "~/Content/AdminHtml/js/vendor.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/dropzonescripts").Include(
            "~/Scripts/dropzone/dropzone.min.js"));

            bundles.Add(new StyleBundle("~/Content/dropzonescss").Include(
                     "~/Scripts/dropzone/css/basic.css",
                     "~/Scripts/dropzone/css/dropzone.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                "~/Content/AdminHtml/css/elephant-rtl.min.css",
                "~/Content/themes/base/jquery-ui.css",
                "~/Content/themes/base/jquery.ui.base.css",
                "~/Content/themes/base/jquery.ui.core.css",
                "~/Scripts/Jquery-toast/jquery.toast.css",
                "~/Content/AdminHtml/css/vendor-rtl.min.css",
                "~/Content/AdminHtml/css/application-rtl.min.css",
                "~/Content/AdminHtml/css/demo-rtl.min.css",
                "~/Content/Font/fonts/FontByekan.css",
                "~/Content/AdminHtml/css/Addon.css",
                "~/Content/CustomStyles.css",
                "~/Content/themes/fontStyles/style.css",
                "~/Content/themes/fontStyles/fontiran.css"
            ));

            bundles.Add(new ScriptBundle("~/bundles/dropzonescripts").Include(
                      "~/Scripts/dropzone/dropzone.min.js"));

            bundles.Add(new StyleBundle("~/Content/dropzonescss").Include(
                     "~/Scripts/dropzone/css/basic.css",
                     "~/Scripts/dropzone/css/dropzone.css"));

            bundles.Add(new ScriptBundle("~/bundles/AdminJs").Include(
                "~/Scripts/jquery-3.2.1.min.js",
                "~/Scripts/jquery-ui-1.8.24.js",
                "~/Content/AdminHtml/js/vendor.min.js",
                "~/Content/AdminHtml/js/elephant.min.js",
                "~/Content/AdminHtml/js/application.min.js",
                "~/Content/AdminHtml/js/demo.min.js",
                "~/Scripts/jquery.unobtrusive-ajax.min.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/CAdminJs").Include(
               "~/Scripts/sweetalert.min.js",
               "~/Scripts/PunasMarketing.js",
               "~/Scripts/toastr.js",
               "~/Scripts/AjaxOption.js"
               ));
            bundles.Add(new ScriptBundle("~/bundles/datatable").Include(
                "~/Scripts/CommonScripts/data-table.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/cheques").Include(
                "~/Scripts/CommonScripts/cheques.js"
            ));


            bundles.Add(new ScriptBundle("~/bundles/fiscalsjs").Include(
                "~/Scripts/CommonScripts/fiscals.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/DatePickerjs").Include(
                "~/Scripts/PersianDateTimePicker/jquery-ui-datetimepicker.min.js"));

            bundles.Add(new StyleBundle("~/Content/DatePickercss").Include(
                "~/Scripts/PersianDateTimePicker/jquery-ui-datetimepicker.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/CloseFiscaljs").Include(
                "~/Scripts/CommonScripts/CloseFiscal.js"));

            bundles.Add(new ScriptBundle("~/bundles/Sanadjs").Include(
                "~/Scripts/CommonScripts/Sanad.js"));

            bundles.Add(new ScriptBundle("~/bundles/Transactionsjs").Include(
                "~/Scripts/CommonScripts/Transactions.js"));

            bundles.Add(new ScriptBundle("~/bundles/customersjs").Include(
                "~/Scripts/CommonScripts/customers.js"));

            bundles.Add(new ScriptBundle("~/bundles/Factorjs").Include(
               "~/Scripts/CommonScripts/Factors.js"));

            bundles.Add(new ScriptBundle("~/bundles/inventoryCheckingJs").Include(
                "~/Scripts/CommonScripts/inventoryChecking.js"));

            bundles.Add(new ScriptBundle("~/bundles/invoiceJs").Include(
                "~/Scripts/CommonScripts/invoices.js"));

            bundles.Add(new ScriptBundle("~/bundles/marketerJs").Include(
                "~/Scripts/CommonScripts/marketers.js"));

            bundles.Add(new ScriptBundle("~/bundles/offerJs").Include(
                "~/Scripts/CommonScripts/offers.js"));

            bundles.Add(new ScriptBundle("~/bundles/orderJs").Include(
                "~/Scripts/CommonScripts/orders.js"));

            bundles.Add(new ScriptBundle("~/bundles/otherTafsiliJs").Include(
                "~/Scripts/CommonScripts/otherTafsilis.js"));

            bundles.Add(new ScriptBundle("~/bundles/personnelsJs").Include(
                "~/Scripts/CommonScripts/personnels.js"));

            bundles.Add(new ScriptBundle("~/bundles/productsJs").Include(
                "~/Scripts/CommonScripts/products.js"));

            bundles.Add(new ScriptBundle("~/bundles/regionsJs").Include(
                "~/Scripts/CommonScripts/regions.js"));

            bundles.Add(new ScriptBundle("~/bundles/reportsJs").Include(
                "~/Scripts/CommonScripts/reports.js"));

            bundles.Add(new ScriptBundle("~/bundles/sarfaslsJs").Include(
                "~/Scripts/CommonScripts/sarfasls.js"));

            bundles.Add(new ScriptBundle("~/bundles/suppliersJs").Include(
                "~/Scripts/CommonScripts/suppliers.js"));

            bundles.Add(new ScriptBundle("~/bundles/warehousesJs").Include(
                "~/Scripts/CommonScripts/warehouses.js"));
        }
    }
}
