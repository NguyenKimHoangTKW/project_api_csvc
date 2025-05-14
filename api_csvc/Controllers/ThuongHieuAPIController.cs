using api_csvc.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.UI.WebControls;

namespace api_csvc.Controllers
{
    public class ThuongHieuAPIController : ApiController
    {
        csvcapiEntities1 db = new csvcapiEntities1();

        [HttpGet]
        [Route("get_full_thuong_hieu")]
        public async Task<IHttpActionResult> load_full_thuong_hieu()
        {
            var check_thuong_hieu = await db.dblThuongHieux.FirstOrDefaultAsync();
            if (check_thuong_hieu == null)
            {
                return BadRequest("Không tồn tại dữ liệu");
            }
            else
            {
                var get_thuong_hieu = await db.dblThuongHieux
               .Select(x => new
               {
                   x.id_thuong_hieu,
                   x.ten_thuong_hieu
               }).ToListAsync();
                return Ok(get_thuong_hieu);
            }
        }

        [HttpPost]
        [Route("create-thuong-hieu")]
        public async Task<IHttpActionResult> Create_thuong_hieu(dblThuongHieu thuongHieu)
        {
            if (string.IsNullOrEmpty(thuongHieu.ten_thuong_hieu))
            {
                return BadRequest("Bạn chưa nhập tên thương hiệu");
            }

            if (db.dblThuongHieux.Any(x => x.ten_thuong_hieu == thuongHieu.ten_thuong_hieu))
            {
                return BadRequest("Thương hiệu đã tồn tại, vui lòng nhập tên thương hiệu mới");
            }
            db.dblThuongHieux.Add(thuongHieu);
            await db.SaveChangesAsync();
            return Ok("Thêm mới dữ liệu thành công");
        }

        [HttpPut]
        [Route("update-thuong-hieu")]
        public async Task<IHttpActionResult> Update_thuong_hieu(dblThuongHieu thuongHieu)
        {
            var check_thuong_hieu = await db.dblThuongHieux.FirstOrDefaultAsync(x => x.id_thuong_hieu == thuongHieu.id_thuong_hieu);
            if (db.dblThuongHieux.FirstOrDefault(x => x.id_thuong_hieu == thuongHieu.id_thuong_hieu) == null)
            {
                return BadRequest("Không tìm thấy thương hiệu");
            }
            if (string.IsNullOrEmpty(thuongHieu.ten_thuong_hieu))
            {
                return BadRequest("Vui lòng nhập tên thương hiệu cần update");
            }

            check_thuong_hieu.ten_thuong_hieu = thuongHieu.ten_thuong_hieu;
            await db.SaveChangesAsync();
            return Ok("Update dữ liệu thành công");
        }

        [HttpDelete]
        [Route("delete-thiet-bi/{id}")]
        public async Task<IHttpActionResult> Delete_thiet_bi(int id)
        {
            var check_thiet_bi = await db.dblThietBis.FindAsync(id);
            if (check_thiet_bi == null)
            {
                return BadRequest("Không tìm thấy thiết bị");
            }
            db.dblThietBis.Remove(check_thiet_bi);
            await db.SaveChangesAsync();
            return Ok("Xóa dữ liệu thành công");
        }
    }
}
