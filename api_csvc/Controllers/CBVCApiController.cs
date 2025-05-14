using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using api_csvc.Models;

namespace api_csvc.Controllers
{
    [RoutePrefix("api/v1")]
    public class CBVCApiController : ApiController
    {
        csvcapiEntities1 db = new csvcapiEntities1();
        [HttpPost]
        [Route("get_cbvc_by_email")]
        public async Task<IHttpActionResult> get_cbvc_by_email(dblCBVC cbvc)
        {
            var get_cbvc = await db.dblCBVCs.FirstOrDefaultAsync(x => x.email == cbvc.email);
            return Ok(new { ten_cbvc = get_cbvc.name_CBVC, email = get_cbvc.email });
        }
        [HttpGet]
        [Route("get-full-cbvc")]
        public async Task<IHttpActionResult> full_cbvc()
        {
            var full_data = await db.dblCBVCs
                .Select(x => new
                {
                    x.id_CBVC,
                    x.name_CBVC,
                    x.email
                }).ToListAsync();

            if (full_data.Count > 0)
            {
                return Ok(new { data = full_data, success = true });
            }
            else
            {
                return Ok(new { message = "Không tìm thấy danh sách cán bộ viên chức", success = false });
            }
        }

        [HttpPost]
        [Route("them-moi-cbvc")]
        public async Task<IHttpActionResult> add_new(dblCBVC items)
        {
            if (db.dblCBVCs.FirstOrDefault(x => x.email == items.email) != null)
            {
                return Ok(new { message = "Cán bộ viên chức này đã tồn tại, vui lòng kiểm tra lại", success = false });
            }
            db.dblCBVCs.Add(items);
            await db.SaveChangesAsync();
            return Ok(new { message = "Thêm mới cán bộ viên chức thành công", success = true });
        }

        [HttpPost]
        [Route("get-info-cbvc")]
        public async Task<IHttpActionResult> get_info(dblCBVC items)
        {
            var check_cbvc = await db.dblCBVCs
                .Where(x => x.id_CBVC == items.id_CBVC)
                .Select(x => new
                {
                    x.name_CBVC,
                    x.email
                })
                .FirstOrDefaultAsync();
            return Ok(check_cbvc);
        }

        [HttpPost]
        [Route("update-cbvc")]
        public async Task<IHttpActionResult> update_cbvc(dblCBVC items)
        {
            var check_cbvc = await db.dblCBVCs.FirstOrDefaultAsync(x => x.id_CBVC == items.id_CBVC);
            if (check_cbvc == null)
            {
                return Ok(new { message = "Không tìm thấy thông tin cán bộ viên chức", success = false });
            }
            else
            {
                check_cbvc.name_CBVC = items.name_CBVC;
                check_cbvc.email = items.email;
            }
            await db.SaveChangesAsync();
            return Ok(new { message = "Cập nhật cán bộ viên chức thành công", success = true });
        }

        [HttpPost]
        [Route("delete-cbvc")]
        public async Task<IHttpActionResult> delete(dblCBVC items)
        {
            var check_danh_sach_muon = await db.dblDanhSachMuons.Where(x => x.id_cbvc == items.id_CBVC).ToListAsync();
            if (check_danh_sach_muon.Any())
            {
                db.dblDanhSachMuons.RemoveRange(check_danh_sach_muon);
                await db.SaveChangesAsync();
            }
            var check_cbcv = await db.dblCBVCs.FirstOrDefaultAsync(x => x.id_CBVC == items.id_CBVC);
            if (check_cbcv == null)
            {
                return Ok(new { message = "Không tìm thấy thông tin cán bộ viên chức", success = false });
            }
            else
            {
                db.dblCBVCs.Remove(check_cbcv);
                await db.SaveChangesAsync();
            }
            return Ok(new { message = "Xóa cán bộ viên chức thành công", success = true });
        }
    }
}
