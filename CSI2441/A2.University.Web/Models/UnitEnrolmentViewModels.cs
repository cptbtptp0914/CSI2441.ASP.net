using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
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
    }
}