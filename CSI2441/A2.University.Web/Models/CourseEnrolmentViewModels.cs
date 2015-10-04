using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using A2.University.Web.Models.Entities;
using FluentValidation.Attributes;

namespace A2.University.Web.Models
{
    public class CourseEnrolmentIndexViewModel
    {
        public List<CourseEnrolment> CourseEnrolments { get; set; } 
    }

    [Validator(typeof(CourseEnrolmentBaseViewModelValidator))]
    public class CourseEnrolmentBaseViewModel
    {
        // core fields

        // hidden from user
        public long course_enrolment_id { get; set; }

        [Display(Name = "Student")]
        public long student_id { get; set; }

        [Display(Name = "Course")]
        public string course_id { get; set; }

        [Display(Name = "Status")]
        public string course_status { get; set; }

        // derived fields

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

        // course status dropdownlist
        public IEnumerable<SelectListItem> CourseStatusDropDownList = new List<SelectListItem>
        {
            new SelectListItem { Value = "COMPLETED", Text = "COMPLETED" },
            new SelectListItem { Value = "DISCONTIN", Text = "DISCONTIN" }, // make DISCONTIN when student enrols into new course but current course is ENROLLED
            new SelectListItem { Value = "ENROLLED", Text = "ENROLLED" }, // default value in db
            new SelectListItem { Value = "INACTIVE", Text = "INACTIVE" },
            new SelectListItem { Value = "INTERMIT", Text = "INTERMIT" },
            new SelectListItem { Value = "LAPSED", Text = "LAPSED" },
            new SelectListItem { Value = "UNCONFIRMED", Text = "UNCONFIRMED" }
        }; 
    }

    public class CourseEnrolmentDetailsViewModel : CourseEnrolmentBaseViewModel
    {
        // No custom fields required
    }

    public class CourseEnrolmentCreateViewModel : CourseEnrolmentDropDownListViewModel
    {
        // Inherits CourseEnrolmentDropDownListViewModel, no custom fields required
    }

    [Validator(typeof(CourseEnrolmentEditViewModelValidator))]
    public class CourseEnrolmentEditViewModel : CourseEnrolmentDropDownListViewModel
    {
        // Inherits CourseEnrolmentDropDownListViewModel, no custom fields required
    }
}