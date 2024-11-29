using api_csvc.Models;
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
    public class ThietBiAPIController : ApiController
    {
        db_api_csvcEntities db = new db_api_csvcEntities();
        [HttpGet]
        [Route("api/get_full_thiet_bi")]
        public async Task<IHttpActionResult> load_full_thiet_bi()
        {
            var check_thiet_bi = await db.dblThietBis.FirstOrDefaultAsync();
            if(check_thiet_bi == null)
            {
                return BadRequest("Không có dữ liệu");
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
                    x.dblTrangThai.ten_trang_thaii
                }).ToListAsync();
                return Ok(get_thietbi);
            }
        }

        [HttpGet]
        [Route("api/get_full_thiet_bi_by_thuong_hieu")]
        public async Task<IHttpActionResult> load_thiet_bi_by_thuong_hieu()
        {
            var get_thuong_hieu = await db.dblThuongHieux.ToListAsync();
            var list_data = new List<dynamic>();
            foreach(var items in get_thuong_hieu)
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
    }
}
