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
    
    public partial class books_by_categories
    {
        public int id_books_by_categories { get; set; }
        public Nullable<int> id_books { get; set; }
        public Nullable<int> id_categories { get; set; }
    
        public virtual Book Book { get; set; }
        public virtual Category Category { get; set; }
    }
}
