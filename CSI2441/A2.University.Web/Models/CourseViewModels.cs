using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using A2.University.Web.Models.Entities;
using FluentValidation.Attributes;

namespace A2.University.Web.Models
{
    [Validator(typeof(CourseBaseViewModelValidator))]
    public class CourseBaseViewModel
    {
        [Display(Name = "Course ID")]
        public string course_id { get; set; }

        [Display(Name = "Title")]
        public string title { get; set; }

        [Display(Name = "Coordinator")]
        public long coordinator_id { get; set; }

        [Display(Name = "Course Type")]
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

    public class CourseIndexViewModel : CourseBaseViewModel
    {
        public List<CourseIndexViewModel> Courses = new List<CourseIndexViewModel>();
    }

    public class CourseDropDownListViewModel : CourseBaseViewModel
    {
        // store lists of coordinators/course types, will extract data for dropdownlists
        public List<CourseDropDownListViewModel> Coordinators = new List<CourseDropDownListViewModel>();
        public List<CourseDropDownListViewModel> CourseTypes = new List<CourseDropDownListViewModel>();
        // store derived items for dropdownlist
        public string staff_id_fullname { get; set; }
             
        // to be populated by controller
        public IEnumerable<SelectListItem> CoordinatorDropDownList { get; set; }
        public IEnumerable<SelectListItem> CourseTypeTitleDropDownList { get; set; }
    }

    public class CourseDetailsViewModel : CourseBaseViewModel
    {
        // No custom fields required
    }

    public class CourseCreateViewModel : CourseDropDownListViewModel
    {
        // Inherits CourseDropDownListViewModel, no custom fields required
    }

    [Validator(typeof(CourseEditViewModelValidator))]
    public class CourseEditViewModel : CourseDropDownListViewModel
    {
        // Uses own validator, ignores course id for validation since user cannot edit
    }

    public class CourseDeleteViewModel : CourseBaseViewModel
    {
        // No custom fields required
    }
}