using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using A2.University.Web.Models.Business;
using A2.University.Web.Models.Entities;
using FluentValidation.Attributes;

namespace A2.University.Web.Models
{
    [Validator(typeof(UnitEnrolmentBaseViewModelValidator))]
    public class UnitEnrolmentBaseViewModel
    {
        // core fields

        // hidden from user
        public long unit_enrolment_id { get; set; }

        [Display(Name = "Student ID")]
        public long student_id { get; set; }

        [Display(Name = "Unit ID")]
        public string unit_id { get; set; }

        [Display(Name = "Year/Sem")]
        public string year_sem { get; set; }

        [Display(Name = "Mark")]
        public string mark { get; set; }

        // derived fields

        [Display(Name = "Unit title")]
        public string title { get; set; }

        [Display(Name = "Student name")]
        public string fullname { get; set; }

        [Display(Name = "Grade")]
        public string grade { get; set; }

        // student
        public string firstname { get; set; }
        public string lastname { get; set; }
    }

    public class UnitEnrolmentIndexViewModel : UnitEnrolmentBaseViewModel
    {
        public List<UnitEnrolmentIndexViewModel> UnitEnrolments = new List<UnitEnrolmentIndexViewModel>();
    }

    public class UnitEnrolmentDropDownListViewModel : UnitEnrolmentBaseViewModel
    {
        // store lists of students/units, will extract data for dropdownlists
        public List<UnitEnrolmentDropDownListViewModel> Students = new List<UnitEnrolmentDropDownListViewModel>();
        public List<UnitEnrolmentDropDownListViewModel> Units = new List<UnitEnrolmentDropDownListViewModel>(); 
        // store derived items for dropdownlist
        public string student_id_fullname { get; set; }
        public string unit_id_title { get; set; }

        // to be populated by controller
        public IEnumerable<SelectListItem> StudentDropDownList { get; set; }
        public IEnumerable<SelectListItem> UnitDropDownList { get; set; }
    }

    public class UnitEnrolmentDetailsViewModel : UnitEnrolmentBaseViewModel
    {
        // No custom fields required
    }

    public class UnitEnrolmentCreateViewModel : UnitEnrolmentDropDownListViewModel
    {
        // Inherits UnitEnrolmentDropDownListViewModel, no custom fields required
    }

    public class UnitEnrolmentEditViewModel : UnitEnrolmentDropDownListViewModel
    {
        // Inherits UnitEnrolmentDropDownListViewModel, no custom fields required
    }

    public class UnitEnrolmentDeleteViewModel : UnitEnrolmentBaseViewModel
    {
        // No custom fields required
    }
}