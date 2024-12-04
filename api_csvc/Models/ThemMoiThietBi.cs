using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;

namespace api_csvc.Models
{
    public class ThemMoiThietBi
    {
        public int id_thiet_bi {  get; set; }
        public string ten_thiet_bi {  get; set; }
        public string thong_so {  get; set; }
        public string ten_thuong_hieu { get; set; }
        public int so_luong {  get; set; }
        public string mo_ta { get; set; }
        public string ten_don_vi_tinh {  get; set; }
        public string ten_phan_loai {  get; set; }
        
    }
}