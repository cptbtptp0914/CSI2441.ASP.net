using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using A2.University.Web.Models.Entities;

namespace A2.University.Web.Models
{
    public class UnitEnrolmentIndexViewModel
    {
        public List<UnitEnrolment> UnitEnrolments { get; set; }
    }

    public class UnitEnrolmentBaseViewModel
    {
        [Display(Name = "Student ID")]
        public long student_id { get; set; }

        [Display(Name = "Unit ID")]
        public string unit_id { get; set; }

        [Display(Name = "Year/Sem")]
        public int year_sem { get; set; }

        [Display(Name = "Mark")]
        public int mark { get; set; }

        [Display(Name = "Grade")]
        public string grade { get; set; }

        [Display(Name = "First name")]
        public string firstname { get; set; }

        [Display(Name = "Surname")]
        public string lastname { get; set; }

        [Display(Name = "Title")]
        public string title { get; set; }

        [Display(Name = "Student name")]
        public string fullname
        {
            get { return firstname + " " + lastname; }
        }

        [Display(Name = "Grade")]
        public string grade
        {
            get { return }
        }
    }

    public class UnitEnrolmentDropDownListViewModel : UnitEnrolmentBaseViewModel
    {
        // to be populated by db
        public IEnumerable<SelectListItem> StudentNameDropDownList { get; set; }
        public IEnumerable<SelectListItem> StudentIDDropDownList { get; set; }
        public IEnumerable<SelectListItem> UnitTitleDropDownList { get; set; }
        public IEnumerable<SelectListItem> UnitIDDropDownList { get; set; } 
    }

    public class UnitEnrolmentDetailsViewModel : UnitEnrolmentBaseViewModel
    {
        // No custom fields required
    }

    public class UnitEnrolmentCreateViewModel : UnitEnrolmentBaseViewModel
    {
        
    }
}