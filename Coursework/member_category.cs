//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Coursework
{
    using System;
    using System.Collections.Generic;
    
    public partial class member_category
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public member_category()
        {
            this.member = new HashSet<member>();
        }
    
        public byte id { get; set; }
        public string category_name { get; set; }
        public string description { get; set; }
        public int dvd_copy_limit { get; set; }
        public int fine_per_day { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<member> member { get; set; }
    }
}
