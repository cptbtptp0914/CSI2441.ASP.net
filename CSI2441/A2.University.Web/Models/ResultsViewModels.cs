﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using A2.University.Web.Models.Entities;

namespace A2.University.Web.Models
{
    public class ResultsBaseViewModel
    {
        [Display(Name = "Course average mark")]
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
        public int HighestMark { get; set; }

        [Display(Name = "Lowest mark")]
        public int LowestMark { get; set; }
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
        // pk
        public long UnitEnrolmentId { get; set; }
        // fk
        public long CourseEnrolmentId { get; set; }

        // displayed as static
        [Display(Name = "Student ID")]
        public long StudentId { get; set; }

        [Display(Name = "Name")]
        public string StudentFullName { get; set; }

        [Display(Name = "Course ID")]
        public string CourseId { get; set; }

        [Display(Name = "Course title")]
        public string CourseTitle { get; set; }

        // displayed as grid
        public string UnitId { get; set; }
        public string UnitTitle { get; set; }
        public int YearSem { get; set; }
        public int Mark { get; set; }
        public string Grade { get; set; }

        public TranscriptViewModel TranscriptView { get; set; }
    }

    public class TranscriptViewModel : ProgressViewModel
    {
        public List<ProgressViewModel> Transcript = new List<ProgressViewModel>();
    }
}