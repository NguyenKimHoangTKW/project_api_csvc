using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace api_csvc
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "TrangChu",
                url: "trang-chu",
                defaults: new { controller = "Home", action = "login" },
                namespaces: new string[] { "api_csvc.Controllers" }
            );
            routes.MapRoute(
               name: "trangchuadmin",
               url: "trang-chu-admin",
               defaults: new { controller = "Home", action = "Index" }
            );
            routes.MapRoute(
               name: "ThietBiAdmin",
               url: "admin/danh-sach-thiet-bi",
               defaults: new { controller = "Home", action = "thiet_bi" }
            );
            routes.MapRoute(
               name: "NguoiDungAdmin",
               url: "admin/nguoi-dung",
               defaults: new { controller = "Home", action = "nguoi_dung" }
            );
            routes.MapRoute(
               name: "CBVCAdmin",
               url: "admin/danh-sach-cbvc",
               defaults: new { controller = "Home", action = "danh_sach_cbvc" }
            ); routes.MapRoute(
               name: "ThietBiMuonAdmin",
               url: "admin/danh-sach-thiet-bi-muon",
               defaults: new { controller = "Home", action = "danh_sach_thiet_bi_muon" }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "login", id = UrlParameter.Optional }
            );
        }
    }
}
