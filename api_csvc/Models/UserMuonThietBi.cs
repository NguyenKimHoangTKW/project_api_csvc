using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api_csvc.Models
{
    public class UserMuonThietBi
    {
        public int id_danh_sach_muon {  get; set; }
        public string email { get; set; }
        public string ten_phong_hoc { get; set; }
        public string ten_thiet_bi {  get; set; }
        public int so_luong_muon {  get; set; }
        public string yeu_cau { get; set; }
        public string ten_trang_thai {  get; set; }
        public string ly_do_huy {  get; set; }
    }
}