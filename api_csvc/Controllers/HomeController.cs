using api_csvc.Helper;
using api_csvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace api_csvc.Controllers
{
    public class HomeController : Controller
    {
        csvcapiEntities1 db = new csvcapiEntities1();
        [UserAuthorize(2)]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult login()
        {
            return View();
        }
        [UserAuthorize(2)]
        public ActionResult thiet_bi()
        {
            ViewBag.ThuongHieu = new SelectList(db.dblThuongHieux.OrderBy(l => l.id_thuong_hieu), "id_thuong_hieu", "ten_thuong_hieu");
            ViewBag.DonViTinh = new SelectList(db.dblDonViTinhs.OrderBy(l => l.id_don_vi_tinh), "id_don_vi_tinh", "ten_don_vi_tinh");
            ViewBag.PhanLoai = new SelectList(db.dblPhanLoais.OrderBy(l => l.id_phan_loai), "id_phan_loai", "ten_phan_loai");
            return View();
        }
        [UserAuthorize(2)]
        public ActionResult nguoi_dung()
        {
            ViewBag.TypeUser = new SelectList(db.dblRoles.OrderBy(l => l.id_role), "id_role", "ten_role");
            return View();
        }
        [UserAuthorize(2)]
        public ActionResult danh_sach_cbvc()
        {
            return View();
        }
        [UserAuthorize(2)]
        public ActionResult danh_sach_thiet_bi_muon()
        {
            var desiredStatusIds = new List<int> { 6, 7, 8 };
            ViewBag.ThuongHieu = new SelectList(db.dblThuongHieux.OrderBy(l => l.id_thuong_hieu), "id_thuong_hieu", "ten_thuong_hieu");
            ViewBag.DonViTinh = new SelectList(db.dblDonViTinhs.OrderBy(l => l.id_don_vi_tinh), "id_don_vi_tinh", "ten_don_vi_tinh");
            ViewBag.PhanLoai = new SelectList(db.dblPhanLoais.OrderBy(l => l.id_phan_loai), "id_phan_loai", "ten_phan_loai");
            ViewBag.TrangThai = new SelectList(db.dblTrangThais.OrderBy(l => l.id_trang_thai).Where(x => desiredStatusIds.Contains(x.id_trang_thai)), "id_trang_thai", "ten_trang_thaii");
            return View();
        }
    }
}
