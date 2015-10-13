using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using FluentValidation.Attributes;

namespace A2.University.Web.Models.StaffPortal
{

    /// <summary>
    /// UnitEnrolment Base view model.
    /// </summary>
    [Validator(typeof(UnitEnrolmentBaseViewModelValidator))]
    public class UnitEnrolmentBaseViewModel
    {
        // core fields

        // hidden from user
        public long UnitEnrolmentId { get; set; }
        public long CourseEnrolmentId { get; set; }

        [Display(Name = "Student ID")]
        public long StudentId { get; set; }

        [Display(Name = "Unit ID")]
        public string UnitId { get; set; }

        [Display(Name = "Year/Sem")]
        public string YearSem { get; set; }

        [Display(Name = "Mark")]
        public string Mark { get; set; }

        // derived fields

        [Display(Name = "Unit title")]
        public string Title { get; set; }

        [Display(Name = "Student name")]
        public string StudentFullName { get; set; }

        [Display(Name = "Grade")]
        public string Grade { get; set; }

        // course
        public string CourseId { get; set; }

        // student
        public string StudentFirstName { get; set; }
        public string StudentLastName { get; set; }
    }

    /// <summary>
    /// UnitEnrolment Index view model. Includes list of UnitEnrolments displayed as CRUD grid.
    /// </summary>
    public class UnitEnrolmentIndexViewModel : UnitEnrolmentBaseViewModel
    {
        public List<UnitEnrolmentIndexViewModel> UnitEnrolments = new List<UnitEnrolmentIndexViewModel>();
    }

    /// <summary>
    /// Dropdownlist view model.
    /// </summary>
    public class UnitEnrolmentDropDownListViewModel : UnitEnrolmentBaseViewModel
    {
        // store lists of students/units, will extract data for dropdownlists
        public List<UnitEnrolmentDropDownListViewModel> Students = new List<UnitEnrolmentDropDownListViewModel>();
        public List<UnitEnrolmentDropDownListViewModel> Units = new List<UnitEnrolmentDropDownListViewModel>();
         
        // store derived items for dropdownlist
        public string StudentIdFullName { get; set; }
        public string UnitIdTitle { get; set; }

        // to be populated by controller
        public IEnumerable<SelectListItem> StudentDropDownList { get; set; }
        public IEnumerable<SelectListItem> UnitDropDownList { get; set; }
    }

    /// <summary>
    /// UnitEnrolment Details view model.
    /// </summary>
    public class UnitEnrolmentDetailsViewModel : UnitEnrolmentBaseViewModel
    {
        [Display(Name = "Course")]
        public string CourseIdTitle { get; set; }
    }

    /// <summary>
    /// UnitEnrolment Create view model.
    /// </summary>
    public class UnitEnrolmentCreateViewModel : UnitEnrolmentDropDownListViewModel
    {
        // Inherits UnitEnrolmentDropDownListViewModel, no custom fields required
    }

    /// <summary>
    /// UnitEnrolment Edit view model.
    /// </summary>
    public class UnitEnrolmentEditViewModel : UnitEnrolmentDropDownListViewModel
    {
        [Display(Name = "Student ID")]
        new public string StudentIdFullName { get; set; }
    }

    /// <summary>
    /// UnitEnrolment Delete view model.
    /// </summary>
    public class UnitEnrolmentDeleteViewModel : UnitEnrolmentBaseViewModel
    {
        // No custom fields required
    }
}