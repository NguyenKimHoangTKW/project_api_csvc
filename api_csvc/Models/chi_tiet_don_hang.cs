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
    
    public partial class chi_tiet_don_hang
    {
        public int id_chi_tiet_hoa_don { get; set; }
        public Nullable<int> id_account { get; set; }
        public Nullable<int> id_mon_an { get; set; }
        public Nullable<int> so_luong { get; set; }
    
        public virtual Account Account { get; set; }
        public virtual mon_an mon_an { get; set; }
    }
}
