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
    using System.ComponentModel.DataAnnotations;

    public partial class studio
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public studio()
        {
            this.dvd = new HashSet<dvd>();
        }
    
        public byte id { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string name { get; set; }

        [Required]
        [Display(Name = "Owner")]
        public string owner { get; set; }

        [Required]
        [Display(Name = "Founder")]
        public string founder { get; set; }

        [Required]
        [Display(Name = "Founded Date")]
        public System.DateTime founded_date { get; set; }

        [Required]
        [Display(Name = "Headquarter")]
        public string headquarter { get; set; }

        [Required]
        [Display(Name = "Movies Produced")]
        public int movies_produced { get; set; }

        [Required]
        [Display(Name = "Website")]
        public string website { get; set; }

        [Required]
        [Display(Name = "Logo")]
        public string logo { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<dvd> dvd { get; set; }
    }
}
