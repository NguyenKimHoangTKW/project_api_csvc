using api_csvc.Helper;
using api_csvc.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace api_csvc.Controllers
{
    [RoutePrefix("api/v1")]
    public class XuLyMonAnAPIController : ApiController
    {
        csvcapiEntities1 db = new csvcapiEntities1();
        private int unixTimestamp;
        private Account user;
        public XuLyMonAnAPIController()
        {
            DateTime now = DateTime.UtcNow;
            unixTimestamp = (int)(now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            user = SessionHelper.GetUser();
        }
        [HttpGet]
        [Route("danh_sach_group_loai_mon_an")]
        public async Task<IHttpActionResult> get_full_loai()
        {
            var get_list = await db.group_loai_mon_an
                .Select(x => new
                {
                    x.id_group_loai_mon_an,
                    x.ten_group_loai_mon_an,
                    x.img
                })
                .ToListAsync();
            if (get_list.Count > 0)
            {
                return Ok(new { data = get_list, success = true });
            }
            else
            {
                return Ok(new { message = "Không có dữ liệu", success = false });
            }
        }
        [HttpPost]
        [Route("mon-an-theo-loai-mon-an")]
        public async Task<IHttpActionResult> get_mon_an_theo_loai(mon_an items)
        {
            var list_data = new List<object>();
            var get_mon_an = await db.group_loai_mon_an
                .Where(x => x.id_group_loai_mon_an == items.id_group_loai_mon_an)
                .FirstOrDefaultAsync();
            var get_mon_an_ = await db.mon_an
                .Where(x => x.id_group_loai_mon_an == get_mon_an.id_group_loai_mon_an)
                .Select(x => new
                {
                    x.id_mon_an,
                    x.ten_mon_an,
                    x.price,
                    x.img
                })
                .ToListAsync();
            list_data.Add(new
            {
                name_group = get_mon_an.ten_group_loai_mon_an,
                img_group = get_mon_an.img,
                mon_an = get_mon_an_
            });
            if (get_mon_an_.Count > 0)
            {
                return Ok(new { data = list_data, success = true });
            }
            else
            {
                return Ok(new { message = "Không có món ăn", success = false });
            }
        }
        public class hoa_don
        {
            public int id_account { get; set; }
            public int id_mon_an { get; set; }
            public int id_chi_tiet_hoa_don { get; set; }
        }

        [HttpPost]
        [Route("them-gio-hang")]
        public async Task<IHttpActionResult> them_gio_hang(hoa_don hd)
        {
            var check_gio_hang = await db.chi_tiet_don_hang
                .Where(x => x.id_mon_an == hd.id_mon_an && x.id_account == user.id_account)
                .FirstOrDefaultAsync();
            if (check_gio_hang != null)
            {
                check_gio_hang.so_luong += 1;
            }
            else
            {
                check_gio_hang = new chi_tiet_don_hang
                {
                    id_account = user.id_account,
                    id_mon_an = hd.id_mon_an,
                    so_luong = 1
                };
                db.chi_tiet_don_hang.Add(check_gio_hang);
            }
            await db.SaveChangesAsync();
            return Ok(new { message = "Thêm vào giỏ hàng thành công", success = true });
        }
        [HttpGet]
        [Route("danh-sach-gio-hang")]
        public async Task<IHttpActionResult> danh_sach_gio_hang()
        {
            var get_list = await db.chi_tiet_don_hang
                .Where(x => x.id_account == user.id_account)
                .Select(x => new
                {
                    x.id_chi_tiet_hoa_don,
                    x.id_mon_an,
                    x.mon_an.ten_mon_an,
                       x.so_luong,
                    price = x.so_luong * x.mon_an.price
                })
                .ToListAsync();
            if (get_list.Count > 0)
            {
                return Ok(new { data = get_list, success = true });
            }
            else
            {
                return Ok(new { message = "Không có sản phẩm", success = false });
            }
        }
        [HttpPost]
        [Route("cap-nhat-so-luong-gio-hang-tang")]
        public async Task<IHttpActionResult> cap_nhat_so_luong_gio_hang_tang(hoa_don hd)
        {
            var check_hd = await db.chi_tiet_don_hang
                .Where(x => x.id_chi_tiet_hoa_don == hd.id_chi_tiet_hoa_don)
                .FirstOrDefaultAsync();
            if (check_hd != null)
            {
                check_hd.so_luong += 1;
            }
            else
            {
                return Ok(new { message = "Không tìm thấy thông tin hóa đơn", success = false });
            }
            await db.SaveChangesAsync(); ;
            return Ok(new { message = "Cập nhật số lượng thành công", success = true });
        }
        [HttpPost]
        [Route("cap-nhat-so-luong-gio-hang-giam")]
        public async Task<IHttpActionResult> cap_nhat_so_luong_gio_hang_giam(hoa_don hd)
        {
            var check_hd = await db.chi_tiet_don_hang
                .Where(x => x.id_chi_tiet_hoa_don == hd.id_chi_tiet_hoa_don)
                .FirstOrDefaultAsync();
            if (check_hd != null)
            {
                check_hd.so_luong -= 1;
            }
            else
            {
                return Ok(new { message = "Không tìm thấy thông tin hóa đơn", success = false });
            }
            await db.SaveChangesAsync(); ;
            return Ok(new { message = "Cập nhật số lượng thành công", success = true });
        }
        [HttpPost]
        [Route("save-hoa-don")]
        public async Task<IHttpActionResult> luu_hoa_don()
        {
            var luu_hoa_don = await db.chi_tiet_don_hang
                .Where(x => x.id_account == user.id_account)
                .ToListAsync();
            if (!luu_hoa_don.Any())
            {
                return Ok(new { message = "Không tồn tại hóa đơn", success = false });
            }
            else
            {
                foreach (var items in luu_hoa_don)
                {
                    var check_mon_an = await db.mon_an.Where(x => x.id_mon_an == items.id_mon_an).FirstOrDefaultAsync();
                    var new_record = new hoa_don_ban_hang
                    {
                        id_mon_an = check_mon_an.id_mon_an,
                        id_account = items.id_account,
                        so_luong = items.so_luong,
                        thanh_tien = items.so_luong * check_mon_an.price,
                        ngay_xuat_hoa_don = unixTimestamp
                    };
                    db.hoa_don_ban_hang.Add(new_record);
                }
                var check_chi_tiet_hoa_don = await db.chi_tiet_don_hang
                        .Where(x => x.id_account == user.id_account)
                        .ToArrayAsync();
                db.chi_tiet_don_hang.RemoveRange(check_chi_tiet_hoa_don);
                await db.SaveChangesAsync();
                return Ok(new { message = "Thanh toán thành công", success = true });
            }
        }
    }
}
