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
    
    public partial class Services_Nail
    {
        public int id_services { get; set; }
        public string name_services { get; set; }
        public Nullable<double> price { get; set; }
        public Nullable<int> time_create { get; set; }
        public Nullable<int> time_update { get; set; }
        public Nullable<int> id_account { get; set; }
    
        public virtual Account Account { get; set; }
    }
}
