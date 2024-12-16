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
    public class CBVCApiController : ApiController
    {
        csvcapiEntities db = new csvcapiEntities();
        [HttpPost]
        [Route("api/get_cbvc_by_email")]
        public async Task<IHttpActionResult> get_cbvc_by_email(dblCBVC cbvc)
        {
            var get_cbvc = await db.dblCBVCs.FirstOrDefaultAsync(x => x.email == cbvc.email);
            return Ok(new {ten_cbvc = get_cbvc.name_CBVC, email = get_cbvc.email});
        } 
    }
}
