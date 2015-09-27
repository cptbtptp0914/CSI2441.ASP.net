//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace A2.University.Web.Models.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class Unit
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Unit()
        {
            this.UnitEnrolments = new HashSet<UnitEnrolment>();
        }

        [Key]
        [Display(Name = "Unit ID")]
        [Required(ErrorMessage = "The Unit ID field is required.")]
        public string unit_id { get; set; }

        [Display(Name = "Title")]
        [Required(ErrorMessage = "The Title field is required.")]
        public string title { get; set; }

        [Display(Name = "Coordinator")]
        [Required(ErrorMessage = "The Coordinator field is required.")]
        public long coodinator_id { get; set; }

        [Display(Name = "Credit Points")]
        [Required(ErrorMessage = "The Credit Points field is required.")]
        public int credit_points { get; set; }

        [Display(Name = "Unit Type")]
        [Required(ErrorMessage = "The Unit Type field is required.")]
        public long unit_type_id { get; set; }

        public IEnumerable<SelectListItem> UnitTypes { get; set; }

        public virtual Staff Staff { get; set; }
        public virtual UnitType UnitType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UnitEnrolment> UnitEnrolments { get; set; }
    }
}
