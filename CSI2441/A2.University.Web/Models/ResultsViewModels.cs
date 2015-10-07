using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using A2.University.Web.Models.Entities;

namespace A2.University.Web.Models
{
    public class ResultsBaseViewModel
    {
        [Display(Name = "Course average")]
        public double CourseAverage { get; set; }

        [Display(Name = "Credit points achieved")]
        public int CpAchieved { get; set; }

        [Display(Name = "Credit points remaining")]
        public int CpRemaining { get; set; }

        [Display(Name = "Course status")]
        public string CourseStatus { get; set; }

        [Display(Name = "Units attempted")]
        public int UnitsAttempted { get; set; }

        [Display(Name = "Highest mark")]
        public UnitEnrolment HighestMark { get; set; }

        [Display(Name = "Lowest mark")]
        public UnitEnrolment LowestMark { get; set; }
    }

    public class ResultsIndexViewModel : ResultsBaseViewModel
    {
        // display grid of course enrolments, replace View/Edit/Delete buttons with Results button
        public List<ResultsIndexViewModel> ResultsByCourse = new List<ResultsIndexViewModel>();

        public long CourseEnrolmentId { get; set; }
        public long StudentId { get; set; }
        public string StudentFirstName { get; set; }
        public string StudentLastName { get; set; }
        public string CourseId { get; set; }
        public string Title { get; set; }
    }

    public class ProgressViewModel : ResultsBaseViewModel
    {
        public List<ProgressViewModel> UnitResults = new List<ProgressViewModel>(); 

        // pk
        public long UnitEnrolmentId { get; set; }
        // fk
        public long CourseEnrolmentId { get; set; }

        public string UnitId { get; set; }
        public string UnitTitle { get; set; }

        public int Mark { get; set; }
        public string Grade { get; set; }
    }
}