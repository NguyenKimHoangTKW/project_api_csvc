//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace api_csvc.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class dblThietBi
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public dblThietBi()
        {
            this.dblDanhSachMuons = new HashSet<dblDanhSachMuon>();
        }
    
        public int id_thiet_bi { get; set; }
        public string ten_thiet_bi { get; set; }
        public string thong_so { get; set; }
        public Nullable<int> id_thuong_hieu { get; set; }
        public int so_luong { get; set; }
        public string mo_ta { get; set; }
        public Nullable<int> id_don_vi_tinh { get; set; }
        public int id_phan_loai { get; set; }
        public int id_trang_thai { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<dblDanhSachMuon> dblDanhSachMuons { get; set; }
        public virtual dblDonViTinh dblDonViTinh { get; set; }
        public virtual dblPhanLoai dblPhanLoai { get; set; }
        public virtual dblThuongHieu dblThuongHieu { get; set; }
        public virtual dblTrangThai dblTrangThai { get; set; }
    }
}
