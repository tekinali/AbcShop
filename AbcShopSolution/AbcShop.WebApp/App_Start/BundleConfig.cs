using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace AbcShop.WebApp.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Css -StyleBunde

            // Admin
            bundles.Add(new StyleBundle("~/admin/css/base").Include(
                "~/Themes/Admin/css/sb-admin-2.min.css",
               "~/Themes/Admin/vendor/datatables/dataTables.bootstrap4.min.css",
               "~/Themes/Admin/css/custom-checkbox.css",
               "~/Content/custom-checkbox.css"
                ).Include("~/Themes/Admin/vendor/fontawesome-free/css/all.min.css", new CssRewriteUrlTransform()));



            // Home
            bundles.Add(new StyleBundle("~/home/css/base").Include(
            "~/Content/bootstrap.min.css",
            "~/Content/Site.css",
            "~/Content/PagedList.css",        
            "~/Content/custom-checkbox.css"
            ));




            // JS- ScriptBundle

            // Admin
            bundles.Add(new ScriptBundle("~/admin/js/base").Include(
                "~/Themes/Admin/vendor/jquery/jquery.min.js",
                "~/Themes/Admin/vendor/bootstrap/js/bootstrap.bundle.min.js",
                "~/Themes/Admin/vendor/jquery-easing/jquery.easing.min.js",
                "~/Themes/Admin/js/sb-admin-2.min.js"
                ));

            bundles.Add(new ScriptBundle("~/admin/js/dataTable").Include(
                "~/Themes/Admin/vendor/datatables/jquery.dataTables.min.js",
                "~/Themes/Admin/vendor/datatables/dataTables.bootstrap4.min.js"
                ));

            bundles.Add(new ScriptBundle("~/admin/js/jqvalidate").Include(
               "~/Scripts/jquery.validate.min.js",
               "~/Scripts/jquery.validate.unobtrusive.min.js"
               ));

            // home

            bundles.Add(new ScriptBundle("~/home/js/base").Include(
           "~/Scripts/jquery-3.3.1.min.js",
           "~/Scripts/bootstrap.min.js",
           "~/Scripts/jquery.validate.min.js",
           "~/Scripts/jquery.validate.unobtrusive.min.js"
           ));

            BundleTable.EnableOptimizations = true;


        }



    }
}