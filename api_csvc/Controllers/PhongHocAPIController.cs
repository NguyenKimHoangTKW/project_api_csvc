using api_csvc.Models;
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
    public class PhongHocAPIController : ApiController
    {
        csvcapiEntities1 db = new csvcapiEntities1();

        [HttpGet]
        [Route("get_full_phong_hoc")]
        public async Task<IHttpActionResult> load_full_phong_hoc()
        {
            bool check_phong_hoc = await db.dblPhongHocs.AnyAsync();
            if (check_phong_hoc)
            {
                var get_phong_hoc = await db.dblPhongHocs
                    .Select(x => new
                    {
                        id_phong_hoc = x.id_phong_hoc,
                        ten_phong_hoc = x.ten_phong_hoc
                    }).ToListAsync();
                return Ok(get_phong_hoc);
            }
            else
            {
                return BadRequest("No Data Avaliable");
            }
        }
        [HttpPost]
        [Route("create_phong_hoc")]
        public async Task<IHttpActionResult> Create_phong_hoc(dblPhongHoc phongHoc)
        {
            if (string.IsNullOrEmpty(phongHoc.ten_phong_hoc))
            {
                return Ok(new { message = "Bạn chưa nhập tên phòng" });
            }

            if (db.dblPhongHocs.Any(x => x.ten_phong_hoc == phongHoc.ten_phong_hoc))
            {
                return Ok(new { message = "Phòng học đã tồn tại, vui lòng nhập phòng học mới" });
            }
            db.dblPhongHocs.Add(phongHoc);
            await db.SaveChangesAsync();
            return Ok(new { message = "Thêm mới dữ liệu thành công" });
        }

        [HttpPut]
        [Route("update_phong_hoc")]
        public async Task<IHttpActionResult> Update_phong_hoc(dblPhongHoc phongHoc)
        {
            var check_phong_hoc = await db.dblPhongHocs.FirstOrDefaultAsync(x => x.id_phong_hoc == phongHoc.id_phong_hoc);
            if (db.dblPhongHocs.FirstOrDefault(x => x.id_phong_hoc == phongHoc.id_phong_hoc) == null)
            {
                return Ok(new { message = "Không tìm thấy phòng học" });
            }
            if (string.IsNullOrEmpty(phongHoc.ten_phong_hoc))
            {
                return Ok(new { message = "Vui lòng nhập tên phòng học cần update" });
            }

            check_phong_hoc.ten_phong_hoc = phongHoc.ten_phong_hoc;
            await db.SaveChangesAsync();
            return Ok(new { message = "Update dữ liệu thành công" });
        }

        [HttpPost]
        [Route("delete_phong_hoc")]
        public async Task<IHttpActionResult> Delete_phong_hoc(dblPhongHoc phongHoc)
        {
            var check_phong_hoc = await db.dblPhongHocs.FindAsync(phongHoc.id_phong_hoc);
            if (check_phong_hoc == null)
            {
                return Ok(new { message = "Không tìm thấy phòng" });
            }
            db.dblPhongHocs.Remove(check_phong_hoc);
            await db.SaveChangesAsync();
            return Ok(new { message = "Xóa dữ liệu thành công" });
        }

    }
}
