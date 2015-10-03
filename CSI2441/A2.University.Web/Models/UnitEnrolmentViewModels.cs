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
    public class UnitEnrolmentIndexViewModel
    {
        public List<UnitEnrolment> UnitEnrolments { get; set; }
    }

    [Validator(typeof(UnitEnrolmentBaseViewModelValidator))]
    public class UnitEnrolmentBaseViewModel
    {
        // core fields

        // hidden from user
        public long unit_enrolment_id { get; set; }

        [Display(Name = "Student")]
        public long student_id { get; set; }

        [Display(Name = "Unit")]
        public string unit_id { get; set; }

        [Display(Name = "Year/Sem")]
        public string year_sem { get; set; }

        [Display(Name = "Mark")]
        public string mark { get; set; }

        // derived fields

        [Display(Name = "First name")]
        public string firstname { get; set; }

        [Display(Name = "Surname")]
        public string lastname { get; set; }

        [Display(Name = "Unit title")]
        public string title { get; set; }

        [Display(Name = "Student name")]
        public string fullname { get; set; }

        [Display(Name = "Grade")]
        public string grade { get; set; }
    }

    public class UnitEnrolmentDropDownListViewModel : UnitEnrolmentBaseViewModel
    {
        // to be populated by db
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
}