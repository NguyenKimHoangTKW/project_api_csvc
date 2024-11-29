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
    public class AccountAPIController : ApiController
    {
        db_api_csvcEntities db = new db_api_csvcEntities();

        [HttpPost]
        [Route("api/login-with-google")]
        public async Task<IHttpActionResult> LoginWithGoogle(Account ac)
        {

            var isCbvcEmail = await db.dblCBVCs.AnyAsync(x => x.email == ac.email);
            if (!isCbvcEmail)
            {
                return BadRequest("Email đăng nhập không nằm trong danh sách CBVC của trường, vui lòng kiểm tra lại");
            }

            var existingAccount = await db.Accounts.FirstOrDefaultAsync(x => x.email == ac.email);

            if (existingAccount == null)
            {
                var newAccount = new Account
                {
                    name = ac.name,
                    email = ac.email,
                    id_role = 1
                };
                db.Accounts.Add(newAccount);
                await db.SaveChangesAsync();
                return Ok("Tài khoản đã được tạo mới thành công.");
            }
            else
            {
                existingAccount.name = ac.name;
                await db.SaveChangesAsync();
                return Ok("Tài khoản đã được cập nhật thành công.");
            }
        }

    }
}
