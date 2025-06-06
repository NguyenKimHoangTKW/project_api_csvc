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
    
    public partial class Book
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Book()
        {
            this.books_by_categories = new HashSet<books_by_categories>();
            this.Details_book_genre = new HashSet<Details_book_genre>();
        }
    
        public int id_books { get; set; }
        public string bookName { get; set; }
        public string bookCover { get; set; }
        public string rating { get; set; }
        public Nullable<int> id_languages { get; set; }
        public Nullable<int> pageNo { get; set; }
        public Nullable<int> id_author { get; set; }
        public string readed { get; set; }
        public string description { get; set; }
        public string completion { get; set; }
        public string lastRead { get; set; }
        public string backgroundColor { get; set; }
        public string navTintColor { get; set; }
    
        public virtual Author Author { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<books_by_categories> books_by_categories { get; set; }
        public virtual Language Language { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Details_book_genre> Details_book_genre { get; set; }
    }
}
