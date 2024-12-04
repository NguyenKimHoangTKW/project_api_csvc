using api_csvc.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
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
                return Ok(new { Message = "Email đăng nhập không nằm trong danh sách CBVC của trường, vui lòng kiểm tra lại" });
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
                return Ok(new { idRole = newAccount.id_role });
            }
            else
            {
                existingAccount.name = ac.name;
                await db.SaveChangesAsync();
                return Ok(new { idRole = existingAccount.id_role, Message = "Tài khoản đã được cập nhật thành công." });
            }
        }


        [HttpGet]
        [Route("api/get_full_account")]
        public async Task<IHttpActionResult> get_full_account()
        {
            var get_full = await db.Accounts
                .Select(x => new
                {
                    x.id_account,
                    x.name,
                    x.email,
                    x.dblRole.ten_role
                }).ToListAsync();
            return Ok(get_full);
        }

        [HttpGet]
        [Route("api/droplist_role")]
        public async Task<IHttpActionResult> droplist_role()
        {
            var drop = await db.dblRoles
                .Select(x => new
                {
                    x.id_role,
                    x.ten_role,
                }).ToListAsync();
            return Ok(drop);
        }

        [HttpPost]
        [Route("api/update-account")]
        public IHttpActionResult Update_Account(UpdateAccount account)
        {
           
            var check_account = db.Accounts.FirstOrDefault(x => x.id_account == account.id_account);
            var check_role = db.dblRoles.FirstOrDefault(x => x.ten_role == account.ten_role);
            check_account.id_role = check_role.id_role;
            db.SaveChanges();
            return Ok(new {message = "Update thành công"});
        }
    }
}
