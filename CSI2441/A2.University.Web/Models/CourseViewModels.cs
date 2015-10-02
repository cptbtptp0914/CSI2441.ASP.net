using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace A2.University.Web.Models
{
    public class CourseBaseViewModel
    {
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

        [Display(Name = "Coordinator")]
        public string coordinator_name { get; set; }

        [Display(Name = "Course Type")]
        public string course_type_title { get; set; }

        [Display(Name = "Credit Points")]
        public int credit_points { get; set; }

        [Display(Name = "Duration (months)")]
        public int duration { get; set; }
    }

    public class CourseDropDownListViewModel : CourseBaseViewModel
    {
        // to be populated by db
        public IEnumerable<SelectListItem> CoordinatorDropDownList { get; set; }
        public IEnumerable<SelectListItem> CourseTypeTitleDropDownList { get; set; }
    }

    public class CourseDetailsViewModel : CourseBaseViewModel
    {
        // No custom fields required
    }

    public class CourseCreateViewModel : CourseDropDownListViewModel
    {
        // No custom fields required
    }
}