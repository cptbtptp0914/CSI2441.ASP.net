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
    public class StudentIndexViewModel
    {
        public List<Student> Students { get; set; } 
    }

    [Validator(typeof(StudentBaseViewModelValidator))]
    public class StudentBaseViewModel
    {
        [Key]
        [Display(Name = "Student ID")]
        public long student_id { get; set; }

        [Display(Name = "First name")]
        public string firstname { get; set; }

        [Display(Name = "Surname")]
        public string lastname { get; set; }

        [Display(Name = "DOB")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime dob { get; set; }

        [Display(Name = "Gender")]
        public string gender { get; set; }

        [Display(Name = "Email")]
        public string email { get; set; }

        [Display(Name = "Landline")]
        public string ph_landline { get; set; }

        [Display(Name = "Mobile")]
        public string ph_mobile { get; set; }

        [Display(Name = "Address")]
        public string adrs { get; set; }

        [Display(Name = "City")]
        public string adrs_city { get; set; }

        [Display(Name = "State")]
        public string adrs_state { get; set; }

        // made string instead of int, else displays 0 as prefilled default for some reason
        // cast to int when updating db
        [Display(Name = "Postcode")]
        public string adrs_postcode { get; set; }
    }

    public class StudentDropDownListViewModel : StudentBaseViewModel
    {
        // gender dropdownlist
        // view sets selected value to model, see base class
        public IEnumerable<SelectListItem> GenderDropDownList = new List<SelectListItem>
        {
            new SelectListItem { Value = "M", Text = "Male" },
            new SelectListItem { Value = "F", Text = "Female" }
        };

        // state dropdownlist
        // view sets selected value to model, see base class
        public IEnumerable<SelectListItem> StateDropDownList = new List<SelectListItem>
        {
            new SelectListItem {Text = "ACT", Value = "ACT"},
            new SelectListItem {Text = "NSW", Value = "NSW"},
            new SelectListItem {Text = "NT", Value = "NT"},
            new SelectListItem {Text = "QLD", Value = "QLD"},
            new SelectListItem {Text = "SA", Value = "SA"},
            new SelectListItem {Text = "TAS", Value = "TAS"},
            new SelectListItem {Text = "VIC", Value = "VIC"},
            new SelectListItem {Text = "WA", Value = "WA"}
        };
    }

    public class StudentDetailsViewModel : StudentBaseViewModel
    {
        // No custom fields required
    }

    public class StudentCreateViewModel : StudentDropDownListViewModel
    {
        // Inherits StudentDropDownListViewModel, no custom fields required
    }

    public class StudentEditViewModel : StudentDropDownListViewModel
    {
        // Inherits StudentDropDownListViewModel, no custom fields required
    }

    public class StudentDeleteViewModel : StudentBaseViewModel
    {
        // No custom fields required
    }
}