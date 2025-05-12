using api_csvc.Helper;
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
    [RoutePrefix("api/v1")]
    public class AccountAPIController : ApiController
    {
        csvcapiEntities1 db = new csvcapiEntities1();

        [HttpPost]
        [Route("login-with-google")]
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
        [Route("get_full_account")]
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
        [Route("droplist_role")]
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
        [Route("update-account")]
        public IHttpActionResult Update_Account(UpdateAccount account)
        {

            var check_account = db.Accounts.FirstOrDefault(x => x.id_account == account.id_account);
            var check_role = db.dblRoles.FirstOrDefault(x => x.ten_role == account.ten_role);
            check_account.id_role = check_role.id_role;
            db.SaveChanges();
            return Ok(new { message = "Update thành công" });
        }

        [HttpPost]
        [Route("dang-nhap")]
        public async Task<IHttpActionResult> dang_nhap(Account ac)
        {
            var check_user = await db.Accounts.FirstOrDefaultAsync(x => x.username == ac.username && x.password == ac.password);
            var list_info = new List<dynamic>();
            if (check_user != null)
            {
                list_info.Add(new
                {
                    check_user.username,
                    check_user.password,
                    check_user.id_role
                });
                return Ok(new { data = list_info, message = "Đăng nhập thành công", success = true });
            }
            else
            {
                return Ok(new { message = "Sai thông tin tài khoản hoặc mật khẩu", success = false });
            }
        }
        [HttpPost]
        [Route("create-account")]
        public async Task<IHttpActionResult> create_account_user(Account ac)
        {
            if (db.Accounts.FirstOrDefault(x => x.username == ac.username) != null)
            {
                return Ok(new { message = "Tài khoản này đã tồn tại, vui lòng nhập tài khoản khác", success = false });
            }
            else
            {
                var new_items = new Account
                {
                    username = ac.username,
                    password = ac.password,
                    id_role = 1
                };
                return Ok(new { message = "Tạo mới tài khoản thành công", success = true });
            }
        }
    }
}
