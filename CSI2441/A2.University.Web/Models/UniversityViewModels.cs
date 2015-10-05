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
    public class CourseViewModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CourseViewModel()
        {
            this.CourseEnrolments = new HashSet<CourseEnrolment>();
        }

        [Display(Name = "Course ID")]
        [Required(ErrorMessage = "The Course ID field is required.")]
        [RegularExpression("([A-Z]{1}[0-9]{2})", ErrorMessage = "Must be a valid Unit ID.")]
        public string course_id { get; set; }

        [Display(Name = "Title")]
        [Required(ErrorMessage = "The Title field is required.")]
        [RegularExpression("(^[a-zA-Z0-9\\.\\,\\#\\/\\(\\) ]{5,}$)", ErrorMessage = "Must be a valid Title.")]
        public string title { get; set; }

        [Display(Name = "Coordinator")]
        [Required(ErrorMessage = "The Coordinator field is required.")]
        public long coordinator_id { get; set; }

        [Display(Name = "Course Type")]
        [Required(ErrorMessage = "The Course Type field is required.")]
        public long course_type_id { get; set; }

        // SelectListItems used for ViewData, see Controller
        public IEnumerable<SelectListItem> Coordinators { get; set; }
        public IEnumerable<SelectListItem> CourseTypes { get; set; }

        public virtual CourseType CourseType { get; set; }
        public virtual Staff Staff { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CourseEnrolment> CourseEnrolments { get; set; }
    }

    public class CourseEnrolmentViewModel
    {
        public long course_enrolment_id { get; set; }
        public long student_id { get; set; }
        public string course_id { get; set; }
        public string course_status { get; set; }

        public virtual Course Course { get; set; }
        public virtual Student Student { get; set; }
    }

    public class CourseTypeViewModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CourseTypeViewModel()
        {
            this.Courses = new HashSet<Course>();
        }

        public long course_type_id { get; set; }

        [Display(Name = "Course Type")]
        public string title { get; set; }

        [Display(Name = "Credit Points")]
        public int credit_points { get; set; }

        [Display(Name = "Duration (months)")]
        public int duration { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Course> Courses { get; set; }
    }

    public class StaffViewModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public StaffViewModel()
        {
            this.Courses = new HashSet<Course>();
            this.Units = new HashSet<Unit>();
        }

        [Display(Name = "Staff ID")]
        public long staff_id { get; set; }

        [Display(Name = "First name")]
        [Required(ErrorMessage = "The First name field is required.")]
        [RegularExpression("(^[a-zA-Z]+$)", ErrorMessage = "Must be a name.")]
        public string firstname { get; set; }

        [Display(Name = "Surname")]
        [Required(ErrorMessage = "The Surname field is required.")]
        [RegularExpression("(^[a-zA-Z]+$)", ErrorMessage = "Must be a name.")]
        public string surname { get; set; }

        [Display(Name = "Email")]
        public string email { get; set; }

        // return full name for dropdownlist
        [Display(Name = "Coordinator")]
        public string fullname
        {
            get { return firstname + " " + surname; }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Course> Courses { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Unit> Units { get; set; }
    }

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
        public long student_id { get; set; }

        [Display(Name = "First name")]
        [Required(ErrorMessage = "The First name field is required.")]
        [RegularExpression("(^[a-zA-Z]+$)", ErrorMessage = "Must be a name.")]
        public string firstname { get; set; }

        [Display(Name = "Surname")]
        [Required(ErrorMessage = "The Surname field is required.")]
        [RegularExpression("(^[a-zA-Z]+$)", ErrorMessage = "Must be a name.")]
        public string lastname { get; set; }

        [Display(Name = "DOB")]
        [DataType(DataType.Date, ErrorMessage = "Must be a date.")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime dob { get; set; }

        [Display(Name = "Gender")]
        [Required(ErrorMessage = "The Gender field is required.")]
        [RegularExpression("(M|F)", ErrorMessage = "Must be a valid Gender.")]
        public string gender { get; set; }

        [Display(Name = "Email")]
        public string email { get; set; }

        [Display(Name = "Landline")]
        [Required(ErrorMessage = "The Landline field is required.")]
        public string ph_landline { get; set; }

        [Display(Name = "Mobile")]
        [Required(ErrorMessage = "The Mobile field is required.")]
        public string ph_mobile { get; set; }

        [Display(Name = "Address")]
        [Required(ErrorMessage = "The Address field is required.")]
        public string adrs { get; set; }

        [Display(Name = "City")]
        [Required(ErrorMessage = "The City field is required.")]
        public string adrs_city { get; set; }

        [Display(Name = "State")]
        [Required(ErrorMessage = "The State field is required.")]
        public string adrs_state { get; set; }

        [Display(Name = "Postcode")]
        [Required(ErrorMessage = "The Postcode field is required.")]
        [RegularExpression("(^[0-9]{4}$)", ErrorMessage = "Must be a valid postcode.")]
        public int adrs_postcode { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CourseEnrolment> CourseEnrolments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UnitEnrolment> UnitEnrolments { get; set; }
    }

    public class UnitViewModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UnitViewModel()
        {
            this.UnitEnrolments = new HashSet<UnitEnrolment>();
        }

        [Key]
        [Display(Name = "Unit ID")]
        [Required(ErrorMessage = "The Unit ID field is required.")]
        [RegularExpression("([A-Z]{3}[0-9]{4})", ErrorMessage = "Must be a valid Unit ID.")]
        public string unit_id { get; set; }

        [Display(Name = "Title")]
        [Required(ErrorMessage = "The Title field is required.")]
        [RegularExpression("(^[a-zA-Z0-9\\.\\,\\# ]{5,}$)", ErrorMessage = "Must be a valid Title.")]
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

    public class UnitEnrolmentViewModel
    {
        public long unit_enrolment_id { get; set; }
        public long student_id { get; set; }
        public string unit_id { get; set; }
        public int year_sem { get; set; }
        public int mark { get; set; }

        public virtual Student Student { get; set; }
        public virtual Unit Unit { get; set; }
    }

    public class UnitTypeViewModel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UnitTypeViewModel()
        {
            this.Units = new HashSet<Unit>();
        }

        public long unit_type_id { get; set; }

        [Display(Name = "Unit Type")]
        public string title { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Unit> Units { get; set; }
    }
}