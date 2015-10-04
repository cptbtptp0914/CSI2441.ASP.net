using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace A2.University.Web.Models.Entities
{
    public class CourseEnrolmentIndexViewModel
    {
        public List<CourseEnrolment> CourseEnrolments { get; set; } 
    }

    public class CourseEnrolmentBaseViewModel
    {
        // core fields

        // hidden from user
        public long course_enrolment_id { get; set; }

        [Display(Name = "Student")]
        public long student_id { get; set; }

        [Display(Name = "Course")]
        public long course_id { get; set; }

        [Display(Name = "Status")]
        public long course_status { get; set; }

        // derived fields
        [Display(Name = "First name")]
        public string firstname { get; set; }

        [Display(Name = "Surname")]
        public string lastname { get; set; }

        [Display(Name = "Course title")]
        public string title { get; set; }

        [Display(Name = "Student name")]
        public string fullname { get; set; }
    }

    public class CourseEnrolmentDropDownListViewModel : CourseEnrolmentBaseViewModel
    {
        // to be populated by db
        public IEnumerable<SelectListItem> StudentDropDownList { get; set; }
        public IEnumerable<SelectListItem> CourseDropDownList { get; set; }  
    }
}