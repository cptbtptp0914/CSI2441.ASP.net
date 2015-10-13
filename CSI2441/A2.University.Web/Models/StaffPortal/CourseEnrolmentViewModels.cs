using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using A2.University.Web.Models.Business;
using FluentValidation.Attributes;

namespace A2.University.Web.Models.StaffPortal
{

    /// <summary>
    /// CourseEnrolment Base view model.
    /// </summary>
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

    /// <summary>
    /// CourseEnrolment Index view model. Includes list of CourseEnrolments displayed as CRUD grid.
    /// </summary>
    public class CourseEnrolmentIndexViewModel : CourseEnrolmentBaseViewModel
    {
        public List<CourseEnrolmentIndexViewModel> CourseEnrolments = new List<CourseEnrolmentIndexViewModel>();
    }

    /// <summary>
    /// Dropdownlist view model.
    /// </summary>
    public class CourseEnrolmentDropDownListViewModel : CourseEnrolmentBaseViewModel
    {
        // stores lists of students/courses, will extract data for dropdownlists
        public List<CourseEnrolmentDropDownListViewModel> Students = new List<CourseEnrolmentDropDownListViewModel>();
        public List<CourseEnrolmentDropDownListViewModel> Courses = new List<CourseEnrolmentDropDownListViewModel>();
        // store derived items for dropdownlist
        public string StudentIdFullName { get; set; }
        public string CourseIdTitle { get; set; }

        // to be populated by controller
        public IEnumerable<SelectListItem> StudentDropDownList { get; set; }
        public IEnumerable<SelectListItem> CourseDropDownList { get; set; }

        private static readonly CourseRules CourseRules = new CourseRules();

        // course status dropdownlist
        // deprecated, user cannot set course state, automatically set by application
        [Obsolete] 
        public IEnumerable<SelectListItem> CourseStatusDropDownList = new List<SelectListItem>
        {
            new SelectListItem { Value = CourseRules.CourseStates["Completed"], Text = CourseRules.CourseStates["Completed"] },
            new SelectListItem { Value = CourseRules.CourseStates["Enrolled"], Text = CourseRules.CourseStates["Enrolled"] }, // default value in db
            new SelectListItem { Value = CourseRules.CourseStates["Discontinued"], Text = CourseRules.CourseStates["Discontinued"] }, // make DISCONTIN when student enrols into new course but current course is ENROLLED
            new SelectListItem { Value = CourseRules.CourseStates["Excluded"], Text = CourseRules.CourseStates["Excluded"] }
        };
    }

    /// <summary>
    /// CourseEnrolment Details view model.
    /// </summary>
    public class CourseEnrolmentDetailsViewModel : CourseEnrolmentBaseViewModel
    {
        // No custom fields required
    }

    /// <summary>
    /// CourseEnrolment Create view model.
    /// </summary>
    public class CourseEnrolmentCreateViewModel : CourseEnrolmentDropDownListViewModel
    {
        // default value when creating new course enrolment
        public CourseEnrolmentCreateViewModel()
        {
            this.CourseStatus = "ENROLLED";
        }
    }

    /// <summary>
    /// CourseEnrolment Edit view model.
    /// </summary>
    public class CourseEnrolmentEditViewModel : CourseEnrolmentDropDownListViewModel
    {
        [Display(Name = "Student ID")]
        new public string StudentIdFullName { get; set; }
    }

    /// <summary>
    /// CourseEnrolment Delete view model.
    /// </summary>
    public class CourseEnrolmentDeleteViewModel : CourseEnrolmentBaseViewModel
    {
        // No custom fields required
    }
}