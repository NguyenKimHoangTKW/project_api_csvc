using api_csvc.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web.Http;

namespace api_csvc.Controllers
{
    [RoutePrefix("api/v1")]
    public class ThietBiAPIController : ApiController
    {
        csvcapiEntities1 db = new csvcapiEntities1();
        [HttpGet]
        [Route("get_full_thiet_bi")]
        public async Task<IHttpActionResult> load_full_thiet_bi()
        {
            var check_thiet_bi = await db.dblThietBis.FirstOrDefaultAsync();
            if (check_thiet_bi == null)
            {
                return Ok(new { message = "Không có dữ liệu" ,});
            }
            else
            {
                var get_thietbi = await db.dblThietBis
                .Select(x => new
                {
                    x.id_thiet_bi,
                    x.ten_thiet_bi,
                    x.thong_so,
                    x.dblThuongHieu.ten_thuong_hieu,
                    x.so_luong,
                    x.mo_ta,
                    x.dblPhanLoai.ten_phan_loai,
                    x.dblTrangThai.ten_trang_thaii,
                    x.dblDonViTinh.ten_don_vi_tinh
                }).ToListAsync();
                return Ok(get_thietbi);
            }
        }
        [HttpPost]
        [Route("get-info-thiet-bi")]
        public async Task<IHttpActionResult> load_info(dblThietBi items)
        {
            var check_info_thiet_bi = await db.dblThietBis
                .Where(x => x.id_thiet_bi == items.id_thiet_bi)
                .Select(x => new
                {
                    x.ten_thiet_bi,
                    x.thong_so,
                    x.dblThuongHieu.ten_thuong_hieu,
                    x.so_luong,
                    x.mo_ta,
                    x.dblPhanLoai.ten_phan_loai,
                    x.dblTrangThai.ten_trang_thaii,
                    x.dblDonViTinh.ten_don_vi_tinh
                })
                .FirstOrDefaultAsync();
            return Ok(check_info_thiet_bi);
        }
        [HttpGet]
        [Route("get_full_thiet_bi_by_thuong_hieu")]
        public async Task<IHttpActionResult> load_thiet_bi_by_thuong_hieu()
        {
            var get_thuong_hieu = await db.dblThuongHieux.ToListAsync();
            var list_data = new List<dynamic>();
            foreach (var items in get_thuong_hieu)
            {
                var get_thiet_bi = await db.dblThietBis
                    .Where(x => x.id_thuong_hieu == items.id_thuong_hieu)
                    .Select(x => new
                    {
                        x.id_thiet_bi,
                        x.ten_thiet_bi,
                        x.thong_so,
                        x.dblThuongHieu.ten_thuong_hieu,
                        x.so_luong,
                        x.mo_ta,
                        x.dblPhanLoai.ten_phan_loai,
                        x.dblTrangThai.ten_trang_thaii
                    }).ToListAsync();
                list_data.Add(new
                {
                    id_thuong_hieu = items.id_thuong_hieu,
                    ten_thuong_hieu = items.ten_thuong_hieu,
                    thiet_bi = get_thiet_bi
                });
            }
            return Ok(list_data);
        }

        [HttpGet]
        [Route("droplist-don-vi-tinh")]
        public async Task<IHttpActionResult> get_full_don_vi_tinh()
        {
            var drop_list = await db.dblDonViTinhs
                .Select(x => new
                {
                    x.id_don_vi_tinh,
                    x.ten_don_vi_tinh,
                }).ToListAsync();
            return Ok(drop_list);
        }

        [HttpGet]
        [Route("droplist-phan-loai")]
        public async Task<IHttpActionResult> get_full_phan_loai()
        {
            var drop_list = await db.dblPhanLoais
                .Select(x => new
                {
                    x.id_phan_loai,
                    x.ten_phan_loai
                }).ToListAsync();

            return Ok(drop_list);
        }
        [HttpPost]
        [Route("update-thiet-bi")]
        public async Task<IHttpActionResult> Update_Thiet_bi(ThemMoiThietBi thietBi)
        {
            if (db.dblThietBis.FirstOrDefault(x => x.ten_thiet_bi == thietBi.ten_thiet_bi) != null)
            {
                return Ok(new { message = "Tên thiết bị đã tồn tại", success = false });
            }

            var check_thuong_hieu = await db.dblThuongHieux.FirstOrDefaultAsync(x => x.ten_thuong_hieu == thietBi.ten_thuong_hieu);
            var check_don_vi_tinh = await db.dblDonViTinhs.FirstOrDefaultAsync(x => x.ten_don_vi_tinh == thietBi.ten_don_vi_tinh);
            var check_phan_loai = await db.dblPhanLoais.FirstOrDefaultAsync(x => x.ten_phan_loai == thietBi.ten_phan_loai);

            var add = new dblThietBi
            {
                ten_thiet_bi = thietBi.ten_thiet_bi,
                thong_so = thietBi.thong_so,
                id_thuong_hieu = check_thuong_hieu.id_thuong_hieu,
                so_luong = thietBi.so_luong,
                mo_ta = thietBi.mo_ta,
                id_don_vi_tinh = check_don_vi_tinh.id_don_vi_tinh,
                id_phan_loai = check_phan_loai.id_phan_loai,
                id_trang_thai = 3
            };
            db.dblThietBis.Add(add);
            await db.SaveChangesAsync();
            return Ok(new { message = "Thêm mới thiết bị thành công", success = true });
        }

        [HttpPost]
        [Route("sua-thiet-bi")]
        public async Task<IHttpActionResult> Suathietbi(ThemMoiThietBi thietBi)
        {
            var check_thiet_bi = await db.dblThietBis.FirstOrDefaultAsync(x => x.id_thiet_bi == thietBi.id_thiet_bi);
            if (check_thiet_bi == null)
            {
                return Ok(new { message = "Không tìm thấy thông tin thiết bị", success = false });
            }
            var check_thuong_hieu = await db.dblThuongHieux.FirstOrDefaultAsync(x => x.ten_thuong_hieu == thietBi.ten_thuong_hieu);
            var check_don_vi_tinh = await db.dblDonViTinhs.FirstOrDefaultAsync(x => x.ten_don_vi_tinh == thietBi.ten_don_vi_tinh);
            var check_phan_loai = await db.dblPhanLoais.FirstOrDefaultAsync(x => x.ten_phan_loai == thietBi.ten_phan_loai);
            check_thiet_bi.ten_thiet_bi = thietBi.ten_thiet_bi;
            check_thiet_bi.thong_so = thietBi.thong_so;
            check_thiet_bi.id_thuong_hieu = check_thuong_hieu.id_thuong_hieu;
            check_thiet_bi.so_luong = thietBi.so_luong;
            check_thiet_bi.mo_ta = thietBi.mo_ta;
            check_thiet_bi.id_don_vi_tinh = check_don_vi_tinh.id_don_vi_tinh;
            check_thiet_bi.id_phan_loai = check_phan_loai.id_phan_loai;
            if (check_thiet_bi.so_luong == 0)
            {
                check_thiet_bi.id_trang_thai = 4;
            }
            else
            {
                check_thiet_bi.id_trang_thai = 3;
            }
            await db.SaveChangesAsync();
            return Ok(new { message = "Cập nhật dữ liệu thành công", success = true });
        }

        [HttpPost]
        [Route("delete-thiet-bi")]
        public async Task<IHttpActionResult> delete_thiet_bi(dblThietBi items)
        {
            var check_dsm = await db.dblDanhSachMuons.Where(x => x.id_thiet_bi == items.id_thiet_bi).ToListAsync();
            if (check_dsm.Any())
            {
                db.dblDanhSachMuons.RemoveRange(check_dsm);
                await db.SaveChangesAsync();
            }
            var check_thiet_bi = await db.dblThietBis.FirstOrDefaultAsync(x => x.id_thiet_bi == items.id_thiet_bi);
            if (check_thiet_bi == null)
            {
                return Ok(new { message = "Không tìm thấy thông tin thiết bị", success = false });
            }
            else
            {
                db.dblThietBis.Remove(check_thiet_bi);

            }
            await db.SaveChangesAsync();
            return Ok(new { message = "Xóa thiết bị thành công", success = true });
        }
    }
}
