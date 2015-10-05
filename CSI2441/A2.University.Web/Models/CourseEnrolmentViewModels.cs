using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using A2.University.Web.Models.Entities;
using FluentValidation.Attributes;

namespace A2.University.Web.Models
{
    [Validator(typeof(CourseEnrolmentBaseViewModelValidator))]
    public class CourseEnrolmentBaseViewModel
    {
        // core fields

        // hidden from user
        public long CourseEnrolmentId { get; set; }

        [Display(Name = "Student ID")]
        public long StudentId { get; set; }

        [Display(Name = "Course ID")]
        public string CourseId { get; set; }

        [Display(Name = "Status")]
        public string CourseStatus { get; set; }

        // derived fields

        [Display(Name = "Course Title")]
        public string Title { get; set; }

        [Display(Name = "Student name")]
        public string StudentFullName { get; set; }

        // student
        public string StudentFirstName { get; set; }
        public string StudentLastName { get; set; }
    }

    public class CourseEnrolmentIndexViewModel : CourseEnrolmentBaseViewModel
    {
        public List<CourseEnrolmentIndexViewModel> CourseEnrolments = new List<CourseEnrolmentIndexViewModel>();
    }

    public class CourseEnrolmentDropDownListViewModel : CourseEnrolmentBaseViewModel
    {
        // stores lists of students/courses, will extract data for dropdownlists
        public List<CourseEnrolmentDropDownListViewModel> Students = new List<CourseEnrolmentDropDownListViewModel>();
        public List<CourseEnrolmentDropDownListViewModel> Courses = new List<CourseEnrolmentDropDownListViewModel>();
        // store derived items for dropdownlist
        public string StudentIdFullname { get; set; }
        public string CourseIdTitle { get; set; }

        // to be populated by controller
        public IEnumerable<SelectListItem> StudentDropDownList { get; set; }
        public IEnumerable<SelectListItem> CourseDropDownList { get; set; }

        // course status dropdownlist, TODO: may not allow user to edit state, to be deprecated
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
        // Uses own validator where status is required
    }

    public class CourseEnrolmentDeleteViewModel : CourseEnrolmentBaseViewModel
    {
        // No custom fields required
    }
}