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
    public class PhongHocAPIController : ApiController
    {
        db_api_csvcEntities db = new db_api_csvcEntities();

        [HttpGet]
        [Route("api/get_full_phong_hoc")]
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
        [Route("api/create_phong_hoc")]
        public async Task<IHttpActionResult> Create_phong_hoc(dblPhongHoc phongHoc)
        {
            if (string.IsNullOrEmpty(phongHoc.ten_phong_hoc))
            {
                return BadRequest("Bạn chưa nhập tên phòng");
            }

            if (db.dblPhongHocs.Any(x => x.ten_phong_hoc == phongHoc.ten_phong_hoc))
            {
                return BadRequest("Phòng học đã tồn tại, vui lòng nhập phòng học mới");
            }
            db.dblPhongHocs.Add(phongHoc);
            await db.SaveChangesAsync();
            return Ok("Thêm mới dữ liệu thành công");
        }

        [HttpPut]
        [Route("api/update_phong_hoc")]
        public async Task<IHttpActionResult> Update_phong_hoc(dblPhongHoc phongHoc)
        {
            var check_phong_hoc = await db.dblPhongHocs.FirstOrDefaultAsync(x => x.id_phong_hoc == phongHoc.id_phong_hoc);
            if (db.dblPhongHocs.FirstOrDefault(x => x.id_phong_hoc == phongHoc.id_phong_hoc) == null)
            {
                return BadRequest("Không tìm thấy phòng học");
            }
            if (string.IsNullOrEmpty(phongHoc.ten_phong_hoc))
            {
                return BadRequest("Vui lòng nhập tên phòng học cần update");
            }

            check_phong_hoc.ten_phong_hoc = phongHoc.ten_phong_hoc;
            await db.SaveChangesAsync();
            return Ok("Update dữ liệu thành công");
        }

        [HttpDelete]
        [Route("api/delete_phong_hoc/{id}")]
        public async Task<IHttpActionResult> Delete_phong_hoc(int id)
        {
            var check_phong_hoc = await db.dblPhongHocs.FindAsync(id);
            if (check_phong_hoc == null)
            {
                return BadRequest("Không tìm thấy phòng");
            }
            db.dblPhongHocs.Remove(check_phong_hoc);
            await db.SaveChangesAsync();
            return Ok("Xóa dữ liệu thành công");
        }

    }
}
