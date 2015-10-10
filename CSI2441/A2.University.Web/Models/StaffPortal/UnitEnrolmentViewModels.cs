﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using FluentValidation.Attributes;

namespace A2.University.Web.Models.StaffPortal
{
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
        public string StudentIdFullName { get; set; }
        public string UnitIdTitle { get; set; }

        // to be populated by controller
        public IEnumerable<SelectListItem> StudentDropDownList { get; set; }
        public IEnumerable<SelectListItem> UnitDropDownList { get; set; }
    }

    public class UnitEnrolmentDetailsViewModel : UnitEnrolmentBaseViewModel
    {
        [Display(Name = "Course")]
        public string CourseIdTitle { get; set; }
    }

    public class UnitEnrolmentCreateViewModel : UnitEnrolmentDropDownListViewModel
    {
        // Inherits UnitEnrolmentDropDownListViewModel, no custom fields required
    }

    public class UnitEnrolmentEditViewModel : UnitEnrolmentDropDownListViewModel
    {
        [Display(Name = "Student ID")]
        new public string StudentIdFullName { get; set; }
    }

    public class UnitEnrolmentDeleteViewModel : UnitEnrolmentBaseViewModel
    {
        // No custom fields required
    }
}