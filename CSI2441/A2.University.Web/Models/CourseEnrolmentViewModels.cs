using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using A2.University.Web.Models.Business;
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

        private static readonly CourseRules CourseRules = new CourseRules();

        // course status dropdownlist, TODO: may not allow user to edit state, to be deprecated
        public IEnumerable<SelectListItem> CourseStatusDropDownList = new List<SelectListItem>
        {
            new SelectListItem { Value = CourseRules.CourseStates["Completed"], Text = CourseRules.CourseStates["Completed"] },
            new SelectListItem { Value = CourseRules.CourseStates["Enrolled"], Text = CourseRules.CourseStates["Enrolled"] }, // default value in db
            new SelectListItem { Value = CourseRules.CourseStates["Discontinued"], Text = CourseRules.CourseStates["Discontinued"] }, // make DISCONTIN when student enrols into new course but current course is ENROLLED
            new SelectListItem { Value = CourseRules.CourseStates["Excluded"], Text = CourseRules.CourseStates["Excluded"] }
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