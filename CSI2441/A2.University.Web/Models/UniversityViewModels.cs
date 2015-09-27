using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using A2.University.Web.Models.Entities;
using GridMvc.DataAnnotations;

/**
 * TODO: Implement ViewModels instead of relying on entity models if there's time.
 * For now, content serves as backup for entity models if overwritten by VS.
 */
namespace A2.University.Web.Models
{
    public class StudentViewModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public StudentViewModel()
        {
            this.CourseEnrolments = new HashSet<CourseEnrolment>();
            this.UnitEnrolments = new HashSet<UnitEnrolment>();
        }

        [Key]
        [Display(Name = "Student ID")]
        public long StudentID { get; set; }

        [Display(Name = "First name")]
        [Required(ErrorMessage = "The First name field is required.")]
        [RegularExpression("(^[a-zA-Z]+$)", ErrorMessage = "Must be a name.")]
        public string Firstname { get; set; }

        [Display(Name = "Surname")]
        [Required(ErrorMessage = "The Surname field is required.")]
        [RegularExpression("(^[a-zA-Z]+$)", ErrorMessage = "Must be a name.")]
        public string Surname { get; set; }

        [Display(Name = "DOB")]
        [DataType(DataType.Date, ErrorMessage = "Must be a date.")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime Dob { get; set; }

        [Display(Name = "Email")]
        [GridColumn(Title = "Email", SortEnabled = true, FilterEnabled = true)]
        public string Email { get; set; }

        [Display(Name = "Landline")]
        [Required(ErrorMessage = "The Landline field is required.")]
        [RegularExpression("(\\+?\\(?[0-9 ]{2}?\\)?[0-9 ]{6,})", ErrorMessage = "Must be a phone number.")]
        public string Landline { get; set; }

        [Display(Name = "Mobile")]
        [Required(ErrorMessage = "The Mobile field is required.")]
        [RegularExpression("(\\+?\\(?[0-9 ]{2}?\\)?[0-9 ]{8,})", ErrorMessage = "Must be a phone number.")]
        public string Mobile { get; set; }

        [Display(Name = "Address")]
        [Required(ErrorMessage = "The Address field is required.")]
        public string Adrs { get; set; }

        [Display(Name = "City")]
        [Required(ErrorMessage = "The City field is required.")]
        [RegularExpression("^([a-zA-Z]+\\s)*[a-zA-Z]+$", ErrorMessage = "Must be a name.")]
        public string AdrsCity { get; set; }

        [Display(Name = "State")]
        [Required(ErrorMessage = "The State field is required.")]
        public string AdrsState { get; set; }

        [Display(Name = "Postcode")]
        [Required(ErrorMessage = "The Postcode field is required.")]
        [RegularExpression("(^[0-9]{4}$)", ErrorMessage = "Must be a valid postcode.")]
        public int AdrsPostcode { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CourseEnrolment> CourseEnrolments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UnitEnrolment> UnitEnrolments { get; set; }
    }

    public class StaffViewModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public StaffViewModel()
        {
            this.Courses = new HashSet<Course>();
            this.Units = new HashSet<Unit>();
        }

        public long StaffID { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }

        // return full name for dropdownlist
        [Display(Name = "Coordinator")]
        public string fullname
        {
            get { return Firstname + " " + Lastname; }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Course> Courses { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Unit> Units { get; set; }
    }

    public class UnitViewModel
    {
        [Key]
        [Display(Name = "Unit ID")]
        [Required(ErrorMessage = "The Unit ID field is required.")]
        [RegularExpression("([A-Z]{3}[0-9]{4})", ErrorMessage = "Must be a valid Unit ID.")]
        public string UnitID { get; set; }

        [Display(Name = "Title")]
        [Required(ErrorMessage = "The Title field is required.")]
        [RegularExpression("(^[A-Z^ ][a-zA-Z0-9 ]{5,}[^ ]$)", ErrorMessage = "Must be a valid Title.")]
        public string Title { get; set; }

        [Display(Name = "Coordinator")]
        [Required(ErrorMessage = "The Coordinator field is required.")]
        public long CoordinatorID { get; set; }

        [Display(Name = "Credit Points")]
        [Required(ErrorMessage = "The Credit Points field is required.")]
        public int CreditPoints { get; set; }

        [Display(Name = "Unit Type")]
        [Required(ErrorMessage = "The Unit Type field is required.")]
        public long UnitTypeID { get; set; }

        public IEnumerable<SelectListItem> UnitTypes { get; set; }

        public virtual Staff Staff { get; set; }
        public virtual UnitType UnitType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UnitEnrolment> UnitEnrolments { get; set; }
    }

    public class UnitTypeViewModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UnitTypeViewModel()
        {
            this.Units = new HashSet<Unit>();
        }

        public long UnitTypeID { get; set; }

        [Display(Name = "Unit Type")]
        public string Title { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Unit> Units { get; set; }
    }

    public class CourseViewModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CourseViewModel()
        {
            this.CourseEnrolments = new HashSet<CourseEnrolment>();
        }

        public string CourseID { get; set; }
        public string Title { get; set; }
        public long CoordinatorID { get; set; }
        public long CourseTypeID { get; set; }

        public virtual CourseType CourseType { get; set; }
        public virtual Staff Staff { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CourseEnrolment> CourseEnrolments { get; set; }
    }

    public class CourseTypeViewModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CourseTypeViewModel()
        {
            this.Courses = new HashSet<Course>();
        }

        public long CourseTypeID { get; set; }
        public string Title { get; set; }
        public int CreditPoints { get; set; }
        public int Duration { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Course> Courses { get; set; }
    }

    public class UnitEnrolmentViewModel
    {
        public long UnitEnrolmentID { get; set; }
        public long StudentID { get; set; }
        public string UnitID { get; set; }
        public int YearSem { get; set; }
        public int Mark { get; set; }

        public virtual Student Student { get; set; }
        public virtual Unit Unit { get; set; }
    }

    public class CourseEnrolmentViewModel
    {
        public long CourseEnrolmentID { get; set; }
        public long StudentID { get; set; }
        public string CourseID { get; set; }
        public string CourseStatus { get; set; }

        public virtual Course Course { get; set; }
        public virtual Student Student { get; set; }
    }
}