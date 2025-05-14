using api_csvc.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml.Linq;

namespace api_csvc.Controllers
{
    [RoutePrefix("api/v1")]
    public class QuanLyMuonTraAPIController : ApiController
    {
        csvcapiEntities1 db = new csvcapiEntities1();

        [HttpGet]
        [Route("drop-list-thiet-bi")]
        public async Task<IHttpActionResult> drop_list_thiet_bi()
        {
            var list_thiet_bi = await db.dblThietBis
                .Select(x => new
                {
                    x.id_thiet_bi,
                    x.ten_thiet_bi
                }).ToListAsync();
            return Ok(list_thiet_bi);
        }

        [HttpGet]
        [Route("drop-list-don-vi-tinh")]
        public async Task<IHttpActionResult> drop_list_don_vi_tinh()
        {
            var list_don_vi_tinh = await db.dblDonViTinhs
                .Select(x => new
                {
                    x.id_don_vi_tinh,
                    x.ten_don_vi_tinh
                }).ToListAsync();
            return Ok(list_don_vi_tinh);
        }
        [HttpPost]
        [Route("user_muon_thiet_bi")]
        public async Task<IHttpActionResult> create_muon_thiet_bi(UserMuonThietBi muonThietBi)
        {

            DateTime now = DateTime.UtcNow;
            int unixTimestamp = (int)(now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            var get_cbvc = await db.dblCBVCs.FirstOrDefaultAsync(x => x.email == muonThietBi.email);
            var get_phong_su_dung = await db.dblPhongHocs.FirstOrDefaultAsync(x => x.ten_phong_hoc == muonThietBi.ten_phong_hoc);
            var get_thiet_bi = await db.dblThietBis.FirstOrDefaultAsync(x => x.ten_thiet_bi == muonThietBi.ten_thiet_bi);
            if (muonThietBi.so_luong_muon > get_thiet_bi.so_luong)
            {
                return Ok(new { message = "Số lượng mượn không hợp lệ" });
            }
            var muon = new dblDanhSachMuon
            {
                id_cbvc = get_cbvc.id_CBVC,
                id_phong_su_dung = get_phong_su_dung.id_phong_hoc,
                id_thiet_bi = get_thiet_bi.id_thiet_bi,
                so_luong_muon = muonThietBi.so_luong_muon,
                yeu_cau = muonThietBi.yeu_cau,
                id_trang_thai = 5,
                ly_do_huy = null,
                ngay_dang_ky_muon = unixTimestamp,
                ngay_muon = null,
                ngay_tra = null,
            };
            db.dblDanhSachMuons.Add(muon);
            await db.SaveChangesAsync();
            return Ok(new { message = "Mượn thành công, chờ duyệt" });
        }

        [HttpGet]
        [Route("get-full-thiet-bi-muon")]
        public async Task<IHttpActionResult> update_cho_muon_thiet_bi()
        {
            var check_danh_sach_muon = await db.dblDanhSachMuons.ToListAsync();
            if (!check_danh_sach_muon.Any())
            {
                return Ok(new { message = "Không có thiết bị nào đang mượn" });
            }
            else
            {
                var get_all = check_danh_sach_muon
                    .Select(x => new
                    {
                        x.id_danh_sach_muon,
                        x.dblCBVC.name_CBVC,
                        x.dblPhongHoc.ten_phong_hoc,
                        x.dblThietBi.ten_thiet_bi,
                        x.so_luong_muon,
                        x.yeu_cau,
                        x.ly_do_huy,
                        x.dblTrangThai.ten_trang_thaii,
                        x.ngay_dang_ky_muon,
                        x.ngay_huy,
                        x.ngay_muon,
                        x.ngay_tra
                    })
                    .OrderByDescending(x => x.id_danh_sach_muon)
                    .ToList();
                return Ok(get_all);
            }
        }
        [HttpPost]
        [Route("get-full-thiet-bi-muon-by-cbvc")]
        public async Task<IHttpActionResult> get_thiet_bi_user(dblCBVC cBVC)
        {
            var check_cbvc = await db.dblCBVCs.FirstOrDefaultAsync(x => x.email == cBVC.email);
            var check_danh_sach_muon = await db.dblDanhSachMuons.Where(x => x.id_cbvc == check_cbvc.id_CBVC).ToListAsync();
            if (!check_danh_sach_muon.Any())
            {
                return Ok(new { message = "Không có thiết bị nào đang mượn" });
            }
            else
            {
                var get_all = check_danh_sach_muon
                    .Select(x => new
                    {
                        x.id_danh_sach_muon,
                        x.dblCBVC.name_CBVC,
                        x.dblPhongHoc.ten_phong_hoc,
                        x.dblThietBi.ten_thiet_bi,
                        x.so_luong_muon,
                        x.yeu_cau,
                        x.dblTrangThai.ten_trang_thaii,
                        x.ly_do_huy,
                        x.ngay_dang_ky_muon,
                        x.ngay_huy,
                        x.ngay_muon,
                        x.ngay_tra
                    }).ToList();
                return Ok(get_all);
            }
        }

        [HttpPost]
        [Route("user-huy-muon-thiet-bi")]
        public async Task<IHttpActionResult> Huy_Muon_user(dblDanhSachMuon danhSachMuon)
        {
            DateTime now = DateTime.UtcNow;
            int unixTimestamp = (int)(now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            var check_danh_sach_muon = await db.dblDanhSachMuons.FirstOrDefaultAsync(x => x.id_danh_sach_muon == danhSachMuon.id_danh_sach_muon);
            if (check_danh_sach_muon.id_trang_thai == 6)
            {
                return Ok(new { message = "Bạn đã hủy mượn thiết bị này" });
            }
            else
            {
                check_danh_sach_muon.ly_do_huy = danhSachMuon.ly_do_huy;
                check_danh_sach_muon.id_trang_thai = 6;
                check_danh_sach_muon.ngay_huy = unixTimestamp;
                await db.SaveChangesAsync();
                return Ok(new { message = "Hủy mượn thành công" });
            }
        }

        [HttpPost]
        [Route("duyet-muon-user")]
        public async Task<IHttpActionResult> Duyet_muon_thiet_bi(UserMuonThietBi userMuonThietBi)
        {
            var check_trang_thai = db.dblTrangThais.FirstOrDefault(x => x.ten_trang_thaii == userMuonThietBi.ten_trang_thai);
            var check_danh_sach_muon = db.dblDanhSachMuons.FirstOrDefault(x => x.id_danh_sach_muon == userMuonThietBi.id_danh_sach_muon);
            var check_thiet_bi = db.dblThietBis.FirstOrDefault(x => x.ten_thiet_bi == check_danh_sach_muon.dblThietBi.ten_thiet_bi);
            DateTime now = DateTime.UtcNow;
            int unixTimestamp = (int)(now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            if (check_trang_thai.id_trang_thai == 7)
            {
                check_danh_sach_muon.id_trang_thai = 7;
                check_danh_sach_muon.ngay_muon = unixTimestamp;
                check_danh_sach_muon.ly_do_huy = "Đã duyệt mượn, vui lòng đến Ban Kế hoạch và cơ sở vật chất, kỹ thuật để nhận";
                check_thiet_bi.so_luong = (check_thiet_bi.so_luong - check_danh_sach_muon.so_luong_muon);
            }
            else if (check_trang_thai.id_trang_thai == 8)
            {
                check_danh_sach_muon.id_trang_thai = 8;
                check_danh_sach_muon.ngay_tra = unixTimestamp;
                check_danh_sach_muon.ly_do_huy = "Đã duyệt mượn, vui lòng đến Ban Kế hoạch và cơ sở vật chất, kỹ thuật để nhận";
                check_thiet_bi.so_luong = (check_danh_sach_muon.so_luong_muon + check_thiet_bi.so_luong);
            }
            else if (check_trang_thai.id_trang_thai == 6)
            {
                check_danh_sach_muon.id_trang_thai = 6;
                check_danh_sach_muon.ngay_huy = unixTimestamp;
                check_danh_sach_muon.ly_do_huy = userMuonThietBi.ly_do_huy;
            }
            await db.SaveChangesAsync();
            var check_thiet_bi_khac_0 = db.dblThietBis.FirstOrDefault(x => x.so_luong == 0);

            if (check_thiet_bi_khac_0 != null)
            {
                check_thiet_bi_khac_0.id_trang_thai = 4;
            }
            else
            {
                var any_thiet_bi = db.dblThietBis.FirstOrDefault();
                if (any_thiet_bi != null)
                {
                    any_thiet_bi.id_trang_thai = 3;
                }
            }
            await db.SaveChangesAsync();
            return Ok(new { message = "Cập nhật thành công" });
        }
        [HttpPost]
        [Route("info-danh-sach-muon")]
        public async Task<IHttpActionResult> info(dblDanhSachMuon items)
        {
            var check_danhsach = await db.dblDanhSachMuons
                .Where(x => x.id_danh_sach_muon == items.id_danh_sach_muon)
                .Select(x => new
                {
                    x.dblThietBi.ten_thiet_bi,
                    x.dblTrangThai.ten_trang_thaii,
                    x.ly_do_huy
                })
                .FirstOrDefaultAsync();
            return Ok(check_danhsach);
        }
        [HttpGet]
        [Route("droplist_trang_thai_duyet_muon")]
        public async Task<IHttpActionResult> drop_list_trangthai()
        {
            var desiredStatusIds = new List<int> { 6, 7, 8 };
            var statuses = await db.dblTrangThais
                .Where(status => desiredStatusIds.Contains(status.id_trang_thai))
                .Select(x => new
                {
                    x.id_trang_thai,
                    x.ten_trang_thaii
                })
                .ToListAsync();
            return Ok(statuses);
        }
    }
}
