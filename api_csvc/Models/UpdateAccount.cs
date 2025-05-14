using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api_csvc.Models
{
    public class UpdateAccount
    {
        public int id_account { get; set; }
        public string ten_role { get; set; }
        public string email { get; set; }
    }
}