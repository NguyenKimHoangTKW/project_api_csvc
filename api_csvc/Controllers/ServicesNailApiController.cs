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
    public class ServicesNailApiController : ApiController
    {
        csvcapiEntities1 db = new csvcapiEntities1();
        private int unixTimestamp;
        public ServicesNailApiController()
        {
            DateTime now = DateTime.UtcNow;
            unixTimestamp = (int)(now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        [HttpGet]
        [Route("list-nail")]
        public async Task<IHttpActionResult> list_nail()
        {
            var get_list = await db.Services_Nail
                .Select(x => new
                {
                    id_services = x.id_services,
                    name_services = x.name_services,
                    price = x.price
                })
                .ToListAsync();

            if (get_list.Count == 0)
            {
                return Ok(new { message = "Chưa tồn tại sản phẩm", success = false });
            }

            return Ok(new { data = get_list, success = true });
        }

        [HttpPost]
        [Route("create-nail")]
        public async Task<IHttpActionResult> create_nail(Services_Nail items)
        {
            if (db.Services_Nail.FirstOrDefault(x => x.name_services == items.name_services) != null)
            {
                return Ok(new { message = "Tên sản phẩm này đã tồn tại", success = false });
            }

            var new_items = new Services_Nail
            {
                name_services = items.name_services,
                price = items.price,
                id_account = items.id_account,
                time_create = unixTimestamp,
                time_update = unixTimestamp
            };
            db.Services_Nail.Add(new_items);
            await db.SaveChangesAsync();
            return Ok(new { message = "Thêm mới sản phẩm thành công", success = true });
        }

        [HttpPost]
        [Route("info-nail")]
        public async Task<IHttpActionResult> info_nail(Services_Nail items)
        {
            var get_info = await db.Services_Nail
                .Where(x => x.id_services == items.id_services)
                .Select(x => new
                {
                    x.name_services,
                    x.price,
                    x.time_create,
                    x.time_update,
                    x.Account.username
                })
                .FirstOrDefaultAsync();
            return Ok(new { data = get_info });
        }
        [HttpPost]
        [Route("update-nail")]
        public async Task<IHttpActionResult> update_nail(Services_Nail items)
        {
            var get_items = await db.Services_Nail.FirstOrDefaultAsync(x => x.id_services == items.id_services);
            if (get_items != null)
            {
                get_items.name_services = items.name_services;
                get_items.price = items.price;
                get_items.time_update = unixTimestamp;
                await db.SaveChangesAsync();
            }
            else
            {
                return Ok(new { message = "Không tìm thấy thông tin sản phẩm", success = false });
            }
            return Ok(new { message = "Cập nhật sản phẩm thành công", success = true });
        }
        [HttpPost]
        [Route("delete-nail")]
        public async Task<IHttpActionResult> delete_nail(Services_Nail items)
        {
            var check_nail = await db.Services_Nail.FirstOrDefaultAsync(x => x.id_services == items.id_services);
            if (check_nail != null)
            {
                db.Services_Nail.Remove(check_nail);
            }
            else
            {
                return Ok(new { message = "Không tìm thấy thông tin sản phẩm", success = false });
            }
            await db.SaveChangesAsync();
            return Ok(new { message = "Xóa sản phẩm thành công", success = true });
        }
    }
}
