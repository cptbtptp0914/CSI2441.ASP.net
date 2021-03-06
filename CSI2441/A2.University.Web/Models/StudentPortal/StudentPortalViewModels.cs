﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.UI;
using A2.University.Web.Models.StaffPortal;

namespace A2.University.Web.Models.StudentPortal
{

    /// <summary>
    /// StudentPortal Base view model.
    /// </summary>
    public class StudentPortalBaseViewModel
    {
        // core fields

        // hidden from user
        public long CourseEnrolmentId { get; set; }

        [Display(Name = "Student ID")]
        public long StudentId { get; set; }

        [Display(Name = "Course ID")]
        public string CourseId { get; set; }

        [Display(Name = "Status")]
        public string CourseStatus { get; set; }

        // derived fields

        [Display(Name = "Course Title")]
        public string CourseTitle { get; set; }

        [Display(Name = "Student name")]
        public string StudentFullName { get; set; }

        // student details

        [Display(Name = "First name")]
        public string StudentFirstName { get; set; }

        [Display(Name = "Surname")]
        public string StudentLastName { get; set; }

        [Display(Name = "DOB")]
        public string Dob { get; set; }

        public string Gender { get; set; }
        public string Email { get; set; }
        public string Landline { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int Postcode { get; set; }
    }

    /// <summary>
    /// StudentCourses view model. Displays a list of Courses that Student has been enrolled in as grid.
    /// </summary>
    public class StudentCoursesViewModel : StudentPortalBaseViewModel
    {
        public StudentCourseEnrolmentListViewModel CoursesList { get; set; }
    }

    /// <summary>
    /// StudentCourseEnrolmentsList view model. Displayed as grid in partial view.
    /// </summary>
    public class StudentCourseEnrolmentListViewModel : StudentCoursesViewModel
    {
        // list of student's course enrolments
        public List<StudentCourseEnrolmentListViewModel> StudentCourseEnrolments = new List<StudentCourseEnrolmentListViewModel>();
    }

    /// <summary>
    /// StudentProgress view model. Displays Student's progress.
    /// </summary>
    public class StudentProgressViewModel : StudentCoursesViewModel
    {
        // pk
        public long UnitEnrolmentId { get; set; }

        // display as static, inheriting
        // StudentId
        // StudentFullName
        // CourseId
        // CourseTitle

        // summary items
        [Display(Name = "Course average mark")]
        public double CourseAverageMark { get; set; }

        [Display(Name = "Course average grade")]
        public string CourseAverageGrade { get; set; }

        [Display(Name = "Credit points achieved")]
        public int CpAchieved { get; set; }

        [Display(Name = "Credit points remaining")]
        public int CpRemaining { get; set; }

        [Display(Name = "Units attempted")]
        public int UnitsAttempted { get; set; }

        [Display(Name = "Highest mark")]
        public string HighestMark { get; set; }

        [Display(Name = "Lowest mark")]
        public string LowestMark { get; set; }

        // display as grid
        public string UnitId { get; set; }
        public string UnitTitle { get; set; }
        public int YearSem { get; set; }
        public int Mark { get; set; }
        public string Grade { get; set; }

        public StudentTranscriptViewModel TranscriptView { get; set; }
    }

    /// <summary>
    /// StudentTranscript view model. Displayed as grid in partial view.
    /// </summary>
    public class StudentTranscriptViewModel : StudentProgressViewModel
    {
        public List<StudentProgressViewModel> Transcript = new List<StudentProgressViewModel>();
    }
}