using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using FluentValidation.Attributes;

namespace A2.University.Web.Models.StaffPortal
{
    [Validator(typeof(CourseBaseViewModelValidator))]
    public class CourseBaseViewModel
    {
        [Display(Name = "Course ID")]
        public string CourseId { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Coordinator")]
        public long CoordinatorId { get; set; }

        [Display(Name = "Course Type")]
        public long CourseTypeId { get; set; }

        [Display(Name = "Coordinator")]
        public string StaffFullName { get; set; }

        [Display(Name = "Course Type")]
        public string CourseTypeTitle { get; set; }

        [Display(Name = "Credit Points")]
        public int CreditPoints { get; set; }

        [Display(Name = "Duration (months)")]
        public int Duration { get; set; }
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
        public string StaffIdFullName { get; set; }
             
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